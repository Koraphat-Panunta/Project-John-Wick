using System;
using System.Collections.Generic;
using UnityEngine;

public class CrouchWeightSoftCoverNodeLeaf : AnimationNodeLeaf
{
    IWeaponAdvanceUser weaponAdvanceUser;
    private float crouchWeight;
    private float crouchWeightOffset;


    private float crouchWeightChange = 5;
    private float crouchUpdateTimeInterval = 0.25f;
    private float crouchUpdateTimer;
    public float checkDistance { get; }
    public CrouchWeightSoftCoverNodeLeaf(IWeaponAdvanceUser weaponAdvanceUser,
        float crouchWeightOffset,float checkDistance, Func<bool> preCondition) : base(preCondition)
    {
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.crouchWeightOffset = crouchWeightOffset;
        this.checkDistance = checkDistance;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override bool IsComplete()
    {
        return base.IsComplete();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool Precondition()
    {
        return base.Precondition();
    }

    public override void UpdateNode()
    {
        this.CalculateCrouchWeight();
        base.UpdateNode();
    }

    public float GetCrouchWeight() => this.crouchWeight;
    private Vector3 crouchCastPosStart => weaponAdvanceUser._userWeapon.transform.position + (Vector3.up * .5f) + (weaponAdvanceUser._userWeapon.transform.forward * -0.1f);
    private Vector3 crouchCastPosEnd => weaponAdvanceUser._userWeapon.transform.position + Vector3.up * 2f;
    private Vector3 crouchCastDir => weaponAdvanceUser._pointingPos - new Vector3(weaponAdvanceUser._userWeapon.transform.position.x, weaponAdvanceUser._pointingPos.y, weaponAdvanceUser._userWeapon.transform.position.z);
    private float crouchSphereRaduis = .2f;

    private List<Vector3> crouchSphereSurface;
    private void CalculateCrouchWeight()
    {
        crouchUpdateTimer += Time.deltaTime;

        if (crouchUpdateTimer < crouchUpdateTimeInterval)
            return;

        if ((this.weaponAdvanceUser._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
        {
            if (EdgeObstacleDetection.GetEdgeObstaclePos(crouchSphereRaduis, this.checkDistance, crouchCastDir, this.crouchCastPosStart, crouchCastPosEnd, .5f, true, out Vector3 edgePos, out List<Vector3> sphereSurface))
            {
                float targetCrouchWeight = Mathf.Clamp01(Mathf.Abs(weaponAdvanceUser._userWeapon.transform.position.y - edgePos.y) - crouchWeightOffset);
                if (weaponAdvanceUser._currentWeapon != null && weaponAdvanceUser._weaponManuverManager.aimingWeight >= 1 && Vector3.Dot(Vector3.up, weaponAdvanceUser._currentWeapon.bulletSpawner.transform.forward) <= 0)
                {
                    float angleCrouchPeekOffset = Mathf.Clamp01(
                        Mathf.Abs(
                            Vector3.Dot(Vector3.up, weaponAdvanceUser._currentWeapon.bulletSpawner.transform.forward) / 0.5f
                            )
                        );
                    targetCrouchWeight += angleCrouchPeekOffset;
                }
                crouchWeight = Mathf.Lerp(crouchWeight, targetCrouchWeight, crouchWeightChange * Time.deltaTime);
            }
            else
            {
                crouchWeight = Mathf.Lerp(crouchWeight, 0, crouchWeightChange * Time.deltaTime);
            }
            crouchSphereSurface = sphereSurface;
        }
        else
        {
            crouchWeight = Mathf.Lerp(crouchWeight, 0, crouchWeightChange * Time.deltaTime);
        }

    }
}
