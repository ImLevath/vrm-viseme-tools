# Usage Guide: VRM Viseme and BlendShape Scripts

---

## Prerequisites

- **Unity Editor** (recommended version: 2019.4 or later)
- **VRM Addon for Unity** (https://github.com/vrm-c/UniVRM)
- A VRM-compatible avatar imported into your Unity project

---

## Script Overview

### 1. **Auto-Create Viseme Clips**

**Purpose:**  
Automatically generates VRM BlendShapeClips for English visemes (A, I, U, E, O) and maps them to your mesh's Japanese viseme blendshapes (あ, い, う, え, お) if necessary.

**How to Use:**
1. In the Hierarchy, select the **root GameObject** of your VRM avatar (typically named `YourAvatarName VRM`).
2. In the Unity menu bar, go to:  
   `Tools > VRM > Auto-Create Viseme Clips`
3. The tool will:
   - Check for English viseme blendshapes.
   - If not found, map the corresponding Japanese blendshapes.
   - Create new BlendShapeClip assets (A, I, U, E, O) under a `BlendShapes` folder.
   - Assign these clips in your avatar's VRMBlendShapeProxy/BlendShapeAvatar for proper lip sync.

**After running:**  
- You should see new assets in `Assets/YourAvatarName VRM/BlendShapes/`.
- In the Inspector, your VRMBlendShapeProxy should reference a BlendShapeAvatar asset with the visemes set.

---

### 2. **List Blendshapes Script**

**Purpose:**  
Lists all blendshapes present on the selected SkinnedMeshRenderer in your scene, along with their indices and names.

**How to Use:**
1. Select the mesh object (usually the face mesh) in the Hierarchy.
2. In the menu bar, go to:  
   `Tools > VRM > List Blendshapes of Selected Mesh`
3. A popup window will appear showing all blendshapes and their indices.
4. Use this information to confirm which blendshapes are present and their names for troubleshooting and manual mapping.

---

### 3. **General Workflow**

1. **Import your VRM model** into Unity.
2. **Check your blendshapes** using the List Blendshapes script.
3. **Run the Auto-Create Viseme Clips script** with your VRM root selected.
4. **Verify and, if needed, manually edit** the BlendShapeAvatar asset to ensure visemes map to the correct blendshapes.
5. **Fill out the VRM Meta Object** on your VRM root for export.
6. **Export your VRM** and test in VSeeFace or other VRM-compatible software.

---

## Troubleshooting

- **No VRMBlendShapeProxy found:**  
  Make sure you have selected the VRM root GameObject with the VRMBlendShapeProxy component. If you don't have one add one.
- **Meta required error on export:**  
  Assign and fill out a VRM Meta Object in the VRM Meta component before exporting.

---

## See Also

- [UniVRM Documentation](https://vrm.dev/en/)
- [VSeeFace Setup Guide](https://www.vseeface.icu/) -- This was created to make japanese made avatars compatible with vseeface

---
