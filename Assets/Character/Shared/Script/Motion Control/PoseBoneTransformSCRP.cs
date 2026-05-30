using UnityEngine;

[CreateAssetMenu(fileName = "PoseBoneTransformSCRP", menuName = "ScriptableObjects/Pose/PoseBoneTransformSCRP")]
public class PoseBoneTransformSCRP : ScriptableObject
{
    public AnimationClip sourceClip;
    [Range(0, 1)] public float sampleNormalizedTime = 0f;

    // Aligned to hips.GetComponentsInChildren<Transform>() order (depth-first), same as
    // IRagdollAble._bones at runtime. boneNames is kept for a runtime sanity check.
    public string[] boneNames;
    public BoneTransform[] boneLocalTransforms;
}
