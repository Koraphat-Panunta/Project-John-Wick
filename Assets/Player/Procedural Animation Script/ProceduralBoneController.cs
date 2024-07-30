using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralBoneController : MonoBehaviour
{
    [SerializeField] MultiAimConstraint Spine;
    [SerializeField] MultiAimConstraint Head;
    [SerializeField] MultiAimConstraint RightArm;
    [SerializeField] MultiAimConstraint RightHand;
    // Start is called before the first frame update
    void Start()
    {
        Spine.weight = 0;
        Head.weight = 0;
        RightArm.weight = 0;
        RightHand.weight = 0;
    }

    // Update is called once per frame
    private void SetWeight(Weapon weapon)
    {
        float w = Mathf.Clamp(weapon.weapon_StanceManager.AimingWeight, 0.0f, 1.0f);
        Spine.weight = w;
        Head.weight = w;
        RightArm.weight = w;
        RightHand.weight = w;
    }
    private void OnEnable()
    {
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Aim, SetWeight);
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.LowReady, SetWeight);
    }
    private void OnDisable()
    {
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.Aim, SetWeight);
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.LowReady, SetWeight);
    }

}
