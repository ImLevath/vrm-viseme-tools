using UnityEngine;
using UnityEditor;
using VRM;
using System.Collections.Generic;

public class AutoCreateVisemeBlendShapeClips : MonoBehaviour
{
    private static readonly string[] visemeNames = { "A", "I", "U", "E", "O" };
    private static readonly string[] japaneseVisemeNames = { "あ", "い", "う", "え", "お" };

    [MenuItem("Tools/VRM/Auto-Create Viseme Clips")]
    static void CreateVisemeClips()
    {
        GameObject avatarRoot = Selection.activeGameObject;
        if (avatarRoot == null)
        {
            Debug.LogError("Select the avatar root in the Hierarchy.");
            return;
        }

        var proxy = avatarRoot.GetComponent<VRMBlendShapeProxy>();
        if (proxy == null)
        {
            Debug.LogError("No VRMBlendShapeProxy found on selected object.");
            return;
        }

        // Find the mesh with blendshapes (usually named "Face" or similar)
        SkinnedMeshRenderer smr = avatarRoot.GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr == null)
        {
            Debug.LogError("No SkinnedMeshRenderer found in children.");
            return;
        }

        // Map visemes to blendshape indices
        int[] blendShapeIndices = new int[visemeNames.Length];
        bool foundEnglish = true;
        for (int i = 0; i < visemeNames.Length; i++)
        {
            int idx = smr.sharedMesh.GetBlendShapeIndex(visemeNames[i]);
            blendShapeIndices[i] = idx;
            if (idx < 0)
                foundEnglish = false;
        }

        // If any English viseme is missing, try Japanese
        if (!foundEnglish)
        {
            Debug.Log("English viseme names not found. Trying Japanese (あ, い, う, え, お)...");
            bool foundJapanese = true;
            for (int i = 0; i < japaneseVisemeNames.Length; i++)
            {
                int idx = smr.sharedMesh.GetBlendShapeIndex(japaneseVisemeNames[i]);
                blendShapeIndices[i] = idx;
                if (idx < 0)
                    foundJapanese = false;
            }
            if (!foundJapanese)
            {
                Debug.LogError("Could not find all Japanese viseme blendshapes ('あ', 'い', 'う', 'え', 'お'). Please check your mesh.");
                return;
            }
        }

        // Create folder for BlendShapeClips
        string folderPath = "Assets/" + avatarRoot.name + "/BlendShapes";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            string parentFolder = "Assets/" + avatarRoot.name;
            if (!AssetDatabase.IsValidFolder(parentFolder))
            {
                AssetDatabase.CreateFolder("Assets", avatarRoot.name);
            }
            AssetDatabase.CreateFolder(parentFolder, "BlendShapes");
        }

        // Create BlendShapeClips
        var clips = new BlendShapeClip[visemeNames.Length];
        for (int i = 0; i < visemeNames.Length; i++)
        {
            var clip = ScriptableObject.CreateInstance<BlendShapeClip>();
            clip.BlendShapeName = visemeNames[i];
            clip.Preset = (BlendShapePreset)System.Enum.Parse(typeof(BlendShapePreset), visemeNames[i]);
            clip.Values = new BlendShapeBinding[]
            {
                new BlendShapeBinding
                {
                    RelativePath = AnimationUtility.CalculateTransformPath(smr.transform, avatarRoot.transform),
                    Index = blendShapeIndices[i],
                    Weight = 100
                }
            };
            string assetPath = $"{folderPath}/{visemeNames[i]}.asset";
            AssetDatabase.CreateAsset(clip, assetPath);
            clips[i] = clip;
        }

        // Assign to proxy
        var blendShapeAvatar = proxy.BlendShapeAvatar;
        if (blendShapeAvatar == null)
        {
            // If no BlendShapeAvatar exists, create one
            blendShapeAvatar = ScriptableObject.CreateInstance<BlendShapeAvatar>();
            string avatarAssetPath = $"Assets/{avatarRoot.name}/{avatarRoot.name}_BlendShapeAvatar.asset";
            AssetDatabase.CreateAsset(blendShapeAvatar, avatarAssetPath);
            proxy.BlendShapeAvatar = blendShapeAvatar;
        }

        foreach (var clip in clips)
        {
            if (!blendShapeAvatar.Clips.Contains(clip))
            {
                blendShapeAvatar.Clips.Add(clip);
            }
        }
        EditorUtility.SetDirty(proxy);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Viseme BlendShapeClips created and assigned! If your blendshapes were in Japanese, they've been mapped to English visemes for VRM lip sync.");
    }
}