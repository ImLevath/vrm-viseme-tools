using UnityEngine;
using UnityEditor;

public class ListBlendshapes : MonoBehaviour
{
    [MenuItem("Tools/VRM/List Blendshapes of Selected Mesh")]
    static void ListBlendshapeNames()
    {
        var smr = Selection.activeGameObject?.GetComponent<SkinnedMeshRenderer>();
        if (smr == null)
        {
            EditorUtility.DisplayDialog("List Blendshapes", "Select the mesh (SkinnedMeshRenderer) in the Hierarchy.", "OK");
            return;
        }
        Mesh mesh = smr.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found on SkinnedMeshRenderer.");
            return;
        }
        int count = mesh.blendShapeCount;
        string names = "";
        for (int i = 0; i < count; i++)
        {
            names += i + ": " + mesh.GetBlendShapeName(i) + "\n";
        }
        EditorUtility.DisplayDialog("Blendshapes", names, "OK");
    }
}