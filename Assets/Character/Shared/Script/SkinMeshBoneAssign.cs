using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public static class SkinMeshBoneAssign 
{

    public static void AssignBonesByIndex(SkinnedMeshRenderer newCloth, SkinnedMeshRenderer alreadyCharacterCloth)
    {
        if (newCloth == null || alreadyCharacterCloth == null)
        {
            Debug.LogError("Missing SkinnedMeshRenderer reference!");
            return;
        }

        Transform[] sourceBones = alreadyCharacterCloth.bones;
        Transform[] targetBones = newCloth.bones;

        // Safety check
        if (sourceBones.Length != targetBones.Length)
        {
            Debug.LogError("Bone count mismatch! Cannot assign by index.");
            return;
        }

        Transform[] newBones = new Transform[targetBones.Length];

        for (int i = 0; i < newBones.Length; i++)
        {
            newBones[i] = sourceBones[i];
        }

        newCloth.bones = newBones;
        newCloth.rootBone = alreadyCharacterCloth.rootBone;

        Debug.Log("Bone assignment (by index) complete!");
    }
}


