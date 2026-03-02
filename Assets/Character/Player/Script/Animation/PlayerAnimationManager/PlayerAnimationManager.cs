
using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public partial class PlayerAnimationManager : MonoBehaviour, IObserverPlayer,IInitializedAble
{

    // Start is called once before the first execution of UpdateNodeAndCheckFindingNode after the MonoBehaviour is created
    public void Initialized()
    {
        player.AddObserver(this);

        isIn_C_A_R_aim = false;

        this.InitailizedNode();
    }
   
    void Update()
    {
        BackBoardUpdate();
        UpdateNode();

    }
    private void FixedUpdate()
    {
        CalculateDeltaRotation();
        FixedUpdateNode();
    }
    
    private void BackBoardUpdate()
    {

        MovementCompoent playerMovement = player._movementCompoent;
        Vector3 inputVelocity_World = playerMovement.moveInputVelocity_World;
        Vector3 inputVelocity_Local = playerMovement.moveInputVelocity_Local;
        Vector3 curVelocity_Local = playerMovement.curMoveVelocity_Local;
        Vector3 curVelocity_World = playerMovement.curMoveVelocity_World;

        this.InputMoveMagnitude_Normalized = inputVelocity_World.normalized.magnitude;

        this.MoveInputLocalFoward_Normalized = inputVelocity_Local.normalized.z;
        this.MoveInputLocalSideWard_Normalized = inputVelocity_Local.normalized.x;


        this.DotMoveInputWordl_VelocityWorld_Normalized = Vector3.Dot(curVelocity_World.normalized
            , inputVelocity_World.normalized) * (curVelocity_World.magnitude / inputVelocity_World.magnitude);

        this.DotVectorLeftwardDir_MoveInputVelocity_Normallized = Mathf.MoveTowards(this.DotVectorLeftwardDir_MoveInputVelocity_Normallized,
                Vector3.Dot(player.inputMoveDir_World,
                Vector3.Cross(player.transform.forward, Vector3.up))
            , 3.5f * Time.deltaTime);

        if ((player.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<PlayerSprintNode>())
        {
            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / player.sprintMaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / player.sprintMaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / player.sprintMaxSpeed;
        }
        else
        {
            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / player.StandMoveMaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / player.StandMoveMaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / player.StandMoveMaxSpeed;
        }



        AimDownSightWeight = (player as IWeaponAdvanceUser)._weaponManuverManager.aimingWeight;



        this.DotVelocityWorld_Leftward_Normalized = Vector3.Dot(
            Vector3.Cross(player.transform.forward, Vector3.up).normalized
            , playerMovement.curMoveVelocity_World.normalized);

        if (RecoilWeight > 0)
            RecoilWeight = Mathf.Clamp(RecoilWeight - 3 * Time.deltaTime, 0, 1);


        if ((player as IWeaponAdvanceUser)._currentWeapon != null)
        {
            if (isIn_C_A_R_aim)
            {
                if (Vector3.Distance((player as IWeaponAdvanceUser)._pointingPos
               , (player as IWeaponAdvanceUser)._currentWeapon.bulletSpawner.transform.position) >= CAR_Range)
                {
                    CAR_ChangeTimer -= Time.deltaTime;

                    if(CAR_ChangeTimer <= 0)
                        isIn_C_A_R_aim = false;
                }
                else
                    CAR_ChangeTimer = CAR_ChangeTime;

                    CAR_Weight = Mathf.MoveTowards(CAR_Weight, 1, CAR_ChangeRate * Time.deltaTime);

            }
            else if (isIn_C_A_R_aim == false)
            {
                if (Vector3.Distance((player as IWeaponAdvanceUser)._pointingPos
              , (player as IWeaponAdvanceUser)._currentWeapon.bulletSpawner.transform.position) <= CAR_Range)
                {
                   
                    isIn_C_A_R_aim = true;

                }
                else
                    CAR_ChangeTimer = CAR_ChangeTime;
                CAR_Weight = Mathf.MoveTowards(CAR_Weight, 0, CAR_ChangeRate * Time.deltaTime);
            }
        }

        float changeSprintLowRate = 5;
        float changeSprintOutRate = 6;
        float changeSprintStayRate = 9;
        if ((player.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<PlayerSprintNode>(out PlayerSprintNode sprintNode))
        {
            if (sprintNode.sprintPhase == PlayerSprintNode.SprintManuver.Out)
                WeaponSwayRate_Normalized = Mathf.MoveTowards(WeaponSwayRate_Normalized, 0.5F, changeSprintOutRate * Time.deltaTime);
            else if (sprintNode.sprintPhase == PlayerSprintNode.SprintManuver.Stay)
                WeaponSwayRate_Normalized = Mathf.MoveTowards(WeaponSwayRate_Normalized, 1, changeSprintStayRate * Time.deltaTime);
        }
        else
            WeaponSwayRate_Normalized = Mathf.MoveTowards(WeaponSwayRate_Normalized, 0, changeSprintLowRate * Time.deltaTime);

        Vector3 lookDir = (this.player._lookingPos - this.player.transform.position).normalized;

        lookDir = new Vector3(lookDir.x, this.player.transform.forward.y, lookDir.z).normalized;

        float angleLookHorizontal = Quaternion.FromToRotation(this.player.transform.forward, lookDir).eulerAngles.y;

        Debug.DrawRay(this.player.transform.position, lookDir, Color.red);


        if (Vector3.Dot(this.player.transform.forward, lookDir) <= -.99f)
            angleLookHorizontal = 180;

         if (Vector3.Dot(this.player.transform.forward, lookDir) >= .99f)
            angleLookHorizontal = 1;


        if ((angleLookHorizontal > 0 && angleLookHorizontal <= 180))
            {

                if (this.angleLookHorizontal > 270 && this.angleLookHorizontal < 360)
                {
                    this.angleLookHorizontal = Mathf.Lerp(this.angleLookHorizontal, 361, Time.deltaTime * 50);
                    if (this.angleLookHorizontal >= 360)
                    {
                        this.angleLookHorizontal = 0;
                    }
                }
                else
                {
                    this.angleLookHorizontal = Mathf.Lerp(this.angleLookHorizontal, angleLookHorizontal, Time.deltaTime * 50);
                }
            }
            else
            {
                if (this.angleLookHorizontal > 0 && this.angleLookHorizontal < 90)
                {
                    this.angleLookHorizontal = Mathf.Lerp(this.angleLookHorizontal, -1, Time.deltaTime * 50);
                    if (this.angleLookHorizontal <= 0)
                        this.angleLookHorizontal = 360;
                }
                else
                {
                    this.angleLookHorizontal = Mathf.Lerp(this.angleLookHorizontal, angleLookHorizontal, Time.deltaTime * 50);
                }
            }


        this.angleLookVertical = Vector3.Angle(Vector3.up, lookDir);

        animator.SetFloat("InputMoveMagnitude_Normalized", InputMoveMagnitude_Normalized);
        animator.SetFloat("VelocityMoveMagnitude_Normalized", VelocityMoveMagnitude_Normalized);
        animator.SetFloat("MoveInputLocalFoward_Normalized", MoveInputLocalFoward_Normalized);
        animator.SetFloat("MoveInputLocalSideWard_Normalized", MoveInputLocalSideWard_Normalized);
        animator.SetFloat("MoveVelocityForward_Normalized", MoveVelocityForward_Normalized);
        animator.SetFloat("MoveVelocitySideward_Normalized", MoveVelocitySideward_Normalized);
        animator.SetFloat("DotMoveInputWordl_VelocityWorld_Normalized", DotMoveInputWordl_VelocityWorld_Normalized);
        animator.SetFloat("Rotating", Rotating);
        animator.SetFloat("AimDownSightWeight", AimDownSightWeight);
        animator.SetFloat("DotVelocityWorld_Leftward_Normalized", DotVelocityWorld_Leftward_Normalized);
        animator.SetFloat("RecoilWeight", RecoilWeight);
        animator.SetFloat("CAR_Weight", CAR_Weight);
        animator.SetFloat("DotVectorLeftwardDir_MoveInputVelocity_Normallized", DotVectorLeftwardDir_MoveInputVelocity_Normallized);
        animator.SetFloat("WeaponSwayRate_Normalized", WeaponSwayRate_Normalized);
        animator.SetFloat("CrouchWeight", crouchWeight);

        this.animator.SetFloat("UpperLayerTimeNormalized", this.upperAnimationPoseTimeNormalized.timeNormal);
        this.animator.SetFloat("BasedLayerTimeNormalized",this.basedAnimationPoseTimeNormalzied.timeNormal);
        this.animator.SetFloat("AngleLookHorizontal", this.angleLookHorizontal);
        this.animator.SetFloat("AngleLookVertical", this.angleLookVertical);

        try
        {
            curBaseLayer = (basedLayerNodeSelector.curNodeLeaf as PlayAnimationNodeLeaf).stateName;
            curUpperLayer = (basedLayerNodeSelector.curNodeLeaf as PlayAnimationNodeLeaf).stateName;
        }
        catch
        {

        }
    }


  
    public void OnNotify<T>(Player player, T node)
    {
        if (node is AimDownSightWeaponManuverNodeLeaf downSightWeaponManuverNodeLeaf && downSightWeaponManuverNodeLeaf.curPhase == AimDownSightWeaponManuverNodeLeaf.AimDownSightPhase.Enter)
        {
            if (Vector3.Distance((player as IWeaponAdvanceUser)._shootingPos
               , (player as IWeaponAdvanceUser)._currentWeapon.bulletSpawner.transform.position) < 3.5f)
                isIn_C_A_R_aim = true;
            else
                isIn_C_A_R_aim = false;
        }
        if(node is IReloadMagazineNode reloadMagazineNode)
        {
            //Debug.Log(reloadMagazineNode._reloadTime);

            this.rifleReloadNodeLeaf.SetDuration(reloadMagazineNode._reloadTime);
            this.rifleReloadNodeLeaf.SetStartNormalized(reloadMagazineNode._startReloadStageNormalizedTime);
            this.rifleTacticalReloadNodeLeaf.SetDuration(reloadMagazineNode._reloadTime);
            this.rifleTacticalReloadNodeLeaf.SetStartNormalized(reloadMagazineNode._startReloadStageNormalizedTime);

            this.pistolReloadNodeLeaf.SetDuration(reloadMagazineNode._reloadTime);
            this.pistolReloadNodeLeaf.SetStartNormalized(reloadMagazineNode._startReloadStageNormalizedTime);
            this.pistolTacticalReloadNodeLeaf.SetDuration(reloadMagazineNode._reloadTime);
            this.pistolTacticalReloadNodeLeaf.SetStartNormalized(reloadMagazineNode._startReloadStageNormalizedTime);
        }

        if(node is GunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Enter)
        {
            this.hit1NodeLeaf.TriggerReset();
            this.hit2NodeLeaf.TriggerReset();
            this.hit3NodeLeaf.TriggerReset();
        }

        if(node is PlayerDodgeRollStateNodeLeaf dodgeRollStateNodeLeaf
            && dodgeRollStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
            this.dodgeNodeLeaf.TriggerReset();
    }

    #region CalculateRotateRate
    private Vector3 previousDir;
    private void CalculateDeltaRotation()
    {
        Vector3 curDir = player.transform.forward;
        if (this.VelocityMoveMagnitude_Normalized <= 0)
        {
            Rotating = Mathf.MoveTowards(Rotating, Vector3.SignedAngle(previousDir, curDir, Vector3.up) * Time.deltaTime / 0.5f, 10 * Time.deltaTime);
        }
        else
        {
            Rotating = Mathf.MoveTowards(Rotating, 0, 10 * Time.deltaTime);
        }
        previousDir = curDir;
    }




    #endregion
    #region CalculateCrouchWeight
    private float crouchWeightChange = 5;
    private float crouchUpdateTimeInterval = 0.25f;
    private float crouchUpdateTimer;

    private Vector3 crouchCastPosStart => player.transform.position + (Vector3.up * .5f) + (player.transform.forward*-0.1f);
    private Vector3 crouchCastPosEnd => player.transform.position + Vector3.up * 2f;
    private Vector3 crouchCastDir => player._pointingPos -  new Vector3(player.transform.position.x,player._pointingPos.y, player.transform.position.z);
    private float crouchSphereRaduis = .2f;
    private float crouchWeight 
    { 
        get 
        {
            if(this.crouchWeightSoftCoverNodeLeaf!=null)
                return this.crouchWeightSoftCoverNodeLeaf.GetCrouchWeight();
            else
                return 0;
        } 
    }
    [Range(0, 1)]
    [SerializeField] private float crouchWeightOffset;
    private List<Vector3> crouchSphereSurface;
    //private void CalculateCrouchWeight()
    //{
    //    crouchUpdateTimer += Time.deltaTime;

    //    if (crouchUpdateTimer < crouchUpdateTimeInterval)
    //        return;

    //    if ((playerStateNodeMnager.TryGetCurNodeLeaf<PlayerCrouch_Idle_NodeLeaf>()
    //        || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerCrouch_Move_NodeLeaf>())
    //        && playerWeaponManuverNodeManager.TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
    //    {
    //        if (EdgeObstacleDetection.GetEdgeObstaclePos(crouchSphereRaduis, 2.3f, crouchCastDir, this.crouchCastPosStart, crouchCastPosEnd, .5f, true, out Vector3 edgePos, out List<Vector3> sphereSurface))
    //        {
    //            float targetCrouchWeight = Mathf.Clamp01(Mathf.Abs(player.transform.position.y - edgePos.y) - crouchWeightOffset);
    //            if(player._currentWeapon != null && player._weaponManuverManager.aimingWeight >=1 && Vector3.Dot(Vector3.up,player._currentWeapon.bulletSpawner.transform.forward) <= 0)
    //            {
    //                float angleCrouchPeekOffset = Mathf.Clamp01(
    //                    Mathf.Abs(
    //                        Vector3.Dot(Vector3.up, player._currentWeapon.bulletSpawner.transform.forward)/ 0.5f
    //                        )
    //                    );
    //                targetCrouchWeight += angleCrouchPeekOffset;
    //            }
    //            crouchWeight = Mathf.Lerp(crouchWeight, targetCrouchWeight, crouchWeightChange * Time.deltaTime);
    //        }
    //        else
    //        {
    //            crouchWeight = Mathf.Lerp(crouchWeight, 0, crouchWeightChange * Time.deltaTime);
    //        }
    //        crouchSphereSurface = sphereSurface;
    //    }
    //    else
    //    {
    //        crouchWeight = Mathf.Lerp(crouchWeight, 0, crouchWeightChange * Time.deltaTime);
    //    }

    //}
    #endregion


    private void OnDrawGizmos()
    {
        if (crouchSphereSurface != null)
        {
            for (int i = 0; i < crouchSphereSurface.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(this.crouchSphereSurface[i],crouchSphereRaduis);
            }
        }

    }

   
}
