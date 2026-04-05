using System.Collections.Generic;
using UnityEngine;


public class HumanoidBone : MonoBehaviour 
{
    [SerializeField] public Transform hips;
    [SerializeField] public Transform _leftUpperLegBone;
    [SerializeField] public Transform _leftLowerLegBone;
    [SerializeField] public Transform _leftFootBone;
    [SerializeField] public Transform _rightUpperLegBone;
    [SerializeField] public Transform _rightLowerLegBone;
    [SerializeField] public Transform _rightFootBone;
    [SerializeField] public Transform _spine_0_Bone;
    [SerializeField] public Transform _spine_1_Bone;
    [SerializeField] public Transform _spine_2_Bone;
    [SerializeField] public Transform _leftShoulderBone;
    [SerializeField] public Transform _leftArmBone;
    [SerializeField] public Transform _leftForeArmBone;
    [SerializeField] public Transform _leftHandBone;

    [SerializeField] public Transform _neckBone;
    [SerializeField] public Transform _headBone;

    [SerializeField] public Transform _rightShoulderBone;
    [SerializeField] public Transform _rightArmBone;
    [SerializeField] public Transform _rightForeArmBone;
    [SerializeField] public Transform _rightHandBone;

    public void PopulateBone()
    {
        if (hips == null)
        {
            Debug.LogError("Hips not assigned!");
            return;
        }

        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();

        // Collect all children
        foreach (Transform t in hips.GetComponentsInChildren<Transform>())
        {
            boneMap[t.name] = t;
        }

        // Mixamo standard names
        _leftUpperLegBone = Find(boneMap, "mixamorig:LeftUpLeg");
        _leftLowerLegBone = Find(boneMap, "mixamorig:LeftLeg");
        _leftFootBone = Find(boneMap, "mixamorig:LeftFoot");

        _rightUpperLegBone = Find(boneMap, "mixamorig:RightUpLeg");
        _rightLowerLegBone = Find(boneMap, "mixamorig:RightLeg");
        _rightFootBone = Find(boneMap, "mixamorig:RightFoot");

        _spine_0_Bone = Find(boneMap, "mixamorig:Spine");
        _spine_1_Bone = Find(boneMap, "mixamorig:Spine1");
        _spine_2_Bone = Find(boneMap, "mixamorig:Spine2");

        _leftShoulderBone = Find(boneMap, "mixamorig:LeftShoulder");
        _leftArmBone = Find(boneMap, "mixamorig:LeftArm");
        _leftForeArmBone = Find(boneMap, "mixamorig:LeftForeArm");
        _leftHandBone = Find(boneMap, "mixamorig:LeftHand");

        _rightShoulderBone = Find(boneMap, "mixamorig:RightShoulder");
        _rightArmBone = Find(boneMap, "mixamorig:RightArm");
        _rightForeArmBone = Find(boneMap, "mixamorig:RightForeArm");
        _rightHandBone = Find(boneMap, "mixamorig:RightHand");

        _neckBone = Find(boneMap, "mixamorig:Neck");
        _headBone = Find(boneMap, "mixamorig:Head");

        Debug.Log("Humanoid bones populated!");
    }

    private Transform Find(Dictionary<string, Transform> map, string name)
    {
        if (map.TryGetValue(name, out Transform t))
            return t;

        Debug.LogWarning($"Bone not found: {name}");
        return null;
    }

}
