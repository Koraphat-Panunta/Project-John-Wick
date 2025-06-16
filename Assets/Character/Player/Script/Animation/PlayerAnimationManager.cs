
using UnityEngine;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour,IObserverPlayer
{
    public Animator animator;   
    public Player player;

    public string Sprint = "Sprint";
    public string Move_Idle = "Move/Idle";
    public string Crouch = "Crouch";
    

    public float CoverWeight;
    public float SholderSide;//-1 -> 1
    public float InputMoveMagnitude_Normalized;
    public float VelocityMoveMagnitude_Normalized;
    public float MoveInputLocalFoward_Normalized;
    public float MoveInputLocalSideWard_Normalized;
    public float MoveVelocityForward_Normalized;
    public float MoveVelocitySideward_Normalized;
    public float DotMoveInputWordl_VelocityWorld_Normalized;
    public float DotVectorLeftwardDir_MoveInputVelocity_Normallized;
    public float Rotating;
    public float AimDownSightWeight;
    public float DotVelocityWorld_Leftward_Normalized;
    public float RecoilWeight;
    public float CAR_Weight;
    public float WeaponSwayRate_Normalized;

    public bool isCover { 
        get { 
            if(player.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf || player.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf)
                return true;
            return false;
        }
    }

    public bool isLayer_1_Enable;

    private bool isIn_C_A_R_aim;
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        player.AddObserver(this);

        isLayer_1_Enable = false;
        isIn_C_A_R_aim = false;
    }

    // UpdateNode is called once per frame
    void Update()
    {
        BackBoardUpdate();

        if (isLayer_1_Enable == true && player._currentWeapon != null)
            animator.SetLayerWeight(1, Mathf.Clamp01(animator.GetLayerWeight(1) + 10 * Time.deltaTime));
        else
            animator.SetLayerWeight(1,Mathf.Clamp01(animator.GetLayerWeight(1) - 10 * Time.deltaTime));
   
    }
    private void FixedUpdate()
    {
        CalculateDeltaRotation();
    }
    private void BackBoardUpdate()
    {
       
        if (player.curShoulderSide == Player.ShoulderSide.Left)
            SholderSide = Mathf.Clamp(SholderSide - 100*Time.deltaTime, -1, 1);
        if (player.curShoulderSide == Player.ShoulderSide.Right)
            SholderSide = Mathf.Clamp(SholderSide + 100 * Time.deltaTime, -1, 1);

        if(player.playerStateNodeManager.curNodeLeaf is PlayerInCoverStandIdleNodeLeaf ||
            player.playerStateNodeManager.curNodeLeaf is PlayerInCoverStandMoveNodeLeaf)
            CoverWeight = Mathf.Clamp(CoverWeight + 2 * Time.deltaTime, 0, 1);
        else
            CoverWeight = Mathf.Clamp(CoverWeight - 2 * Time.deltaTime, 0, 1);

        PlayerMovement playerMovement = player.playerMovement;
        Vector3 inputVelocity_World = playerMovement.moveInputVelocity_World;
        Vector3 inputVelocity_Local = playerMovement.moveInputVelocity_Local;
        Vector3 curVelocity_Local = playerMovement.curMoveVelocity_Local;
        Vector3 curVelocity_World = playerMovement.curMoveVelocity_World;

        this.InputMoveMagnitude_Normalized = inputVelocity_World.normalized.magnitude;

        this.MoveInputLocalFoward_Normalized = inputVelocity_Local.normalized.z;
        this.MoveInputLocalSideWard_Normalized = inputVelocity_Local.normalized.x;

       
        this.DotMoveInputWordl_VelocityWorld_Normalized = Vector3.Dot(curVelocity_World.normalized
            , inputVelocity_World.normalized)*(curVelocity_World.magnitude/inputVelocity_World.magnitude);

        this.DotVectorLeftwardDir_MoveInputVelocity_Normallized = Mathf.Lerp(this.DotVectorLeftwardDir_MoveInputVelocity_Normallized, 
                Vector3.Dot(player.inputMoveDir_World, 
                Vector3.Cross(player.transform.forward, Vector3.up))
            ,3.5f*Time.deltaTime) ;

        if (player.playerStateNodeManager.curNodeLeaf is PlayerSprintNode)
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

        if (player._currentWeapon is PrimaryWeapon)
            isIn_C_A_R_aim = false;

        if ((player as IWeaponAdvanceUser)._currentWeapon != null)
        {
            if (isIn_C_A_R_aim)
            {
                CAR_Weight = Mathf.Lerp(CAR_Weight, 1, 10 * Time.deltaTime);
                if (Vector3.Distance((player as IWeaponAdvanceUser)._shootingPos
               , (player as IWeaponAdvanceUser)._currentWeapon.bulletSpawnerPos.position) > 24)
                    isIn_C_A_R_aim = false;
            }
            else if (isIn_C_A_R_aim == false)
            {
                CAR_Weight = Mathf.Lerp(CAR_Weight, 0, 10 * Time.deltaTime);
                if (Vector3.Distance((player as IWeaponAdvanceUser)._shootingPos
               , (player as IWeaponAdvanceUser)._currentWeapon.bulletSpawnerPos.position) < 3.5f)
                    isIn_C_A_R_aim = true;
            }
        }

        float changeSprintLowRate = 5;
        float changeSprintOutRate = 6;
        float changeSprintStayRate = 9;
        if (player.playerStateNodeManager.curNodeLeaf is PlayerSprintNode sprintNode)
        {
            if (sprintNode.sprintPhase == PlayerSprintNode.SprintPhase.Out)
                WeaponSwayRate_Normalized = Mathf.Lerp(WeaponSwayRate_Normalized, 0.5F, changeSprintOutRate * Time.deltaTime);
            else if (sprintNode.sprintPhase == PlayerSprintNode.SprintPhase.Stay)
                WeaponSwayRate_Normalized = Mathf.Lerp(WeaponSwayRate_Normalized,1, changeSprintStayRate * Time.deltaTime);
        }
        else
            WeaponSwayRate_Normalized = Mathf.Lerp(WeaponSwayRate_Normalized, 0, changeSprintLowRate * Time.deltaTime);


        animator.SetFloat("CoverWeight", CoverWeight);
        animator.SetFloat("SholderSide", SholderSide);
        animator.SetFloat("InputMoveMagnitude_Normalized", InputMoveMagnitude_Normalized);
        animator.SetFloat("VelocityMoveMagnitude_Normalized",VelocityMoveMagnitude_Normalized);
        animator.SetFloat("MoveInputLocalFoward_Normalized", MoveInputLocalFoward_Normalized);
        animator.SetFloat("MoveInputLocalSideWard_Normalized", MoveInputLocalSideWard_Normalized);
        animator.SetFloat("MoveVelocityForward_Normalized",MoveVelocityForward_Normalized);
        animator.SetFloat("MoveVelocitySideward_Normalized", MoveVelocitySideward_Normalized);
        animator.SetFloat("DotMoveInputWordl_VelocityWorld_Normalized",DotMoveInputWordl_VelocityWorld_Normalized);
        animator.SetFloat("Rotating",Rotating);
        animator.SetFloat("AimDownSightWeight",AimDownSightWeight);
        animator.SetFloat("DotVelocityWorld_Leftward_Normalized", DotVelocityWorld_Leftward_Normalized);
        animator.SetFloat("RecoilWeight", RecoilWeight);
        animator.SetFloat("CAR_Weight", CAR_Weight);
        animator.SetFloat("DotVectorLeftwardDir_MoveInputVelocity_Normallized", DotVectorLeftwardDir_MoveInputVelocity_Normallized);
        animator.SetFloat("WeaponSwayRate_Normalized", WeaponSwayRate_Normalized);

    }

    private bool isDead;
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent notifyEvent)
    {

        if(notifyEvent == SubjectPlayer.NotifyEvent.Firing)
            RecoilWeight = 1;
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if (node is PlayerStateNodeLeaf playerStateNode)
            this.PlayerStateNodeNotifyManager(playerStateNode);
        else if (node is WeaponManuverLeafNode weaponManuverLeaf)
            this.PlayerWeaponManuverStateStateNodeNotifyManager(weaponManuverLeaf);
    }

    #region CalculateRotateRate
    private Vector3 previousDir;
    private void CalculateDeltaRotation()
    {
        Vector3 curDir = player.transform.forward;

        Rotating = Mathf.Lerp(Rotating,Vector3.SignedAngle(previousDir, curDir,Vector3.up) * Time.deltaTime/0.5f,10*Time.deltaTime);
        previousDir = curDir;
    }

   


    #endregion

    private void PlayerStateNodeNotifyManager(PlayerStateNodeLeaf playerStateNode)
    {
        switch (playerStateNode)
        {
            case PlayerDodgeRollStateNodeLeaf:
                {
                    isLayer_1_Enable = false;
                    animator.CrossFade("DodgeRoll", 0.05f, 0, 0);
                    break;
                }
            case PlayerDeadNodeLeaf: 
                {
                    if (isDead)
                        return;
                    isLayer_1_Enable = false;
                    animator.CrossFade("Dead", 0.05f, 0, 0);
                    isDead = true;
                    break;
                }
            case PlayerSprintNode:
                {
                    animator.CrossFade(Sprint, 0.3f, 0, 0);
                    if(player.weaponAdvanceUser._weaponManuverManager.curNodeLeaf is LowReadyWeaponManuverNodeLeaf)
                        animator.CrossFade("SprintWeaponSway", 0.25f, 1);
                    isLayer_1_Enable = true;
                    break;
                }
            case PlayerStandMoveNodeLeaf:
            case PlayerStandIdleNodeLeaf:
                {
                    animator.CrossFade(Move_Idle, 0.3f, 0, 0);
                    isLayer_1_Enable = true;
                    if (player._weaponManuverManager.curNodeLeaf is LowReadyWeaponManuverNodeLeaf)
                        animator.CrossFade("StandWeaponHand LowReady/ADS", 0.35f, 1);
                    
                    break;
                }
            case PlayerCrouch_Idle_NodeLeaf:
            case PlayerCrouch_Move_NodeLeaf:
                {
                    animator.CrossFade(Crouch, 0.3f, 0, 0);
                    isLayer_1_Enable = true;
                    if (player._weaponManuverManager.curNodeLeaf is LowReadyWeaponManuverNodeLeaf)
                        animator.CrossFade("StandWeaponHand LowReady/ADS", 0.35f, 1);
                    
                    break;
                }
            case PlayerGetUpStateNodeLeaf: 
                {   
                    isLayer_1_Enable = false;   
                    animator.CrossFade("PlayerSpringGetUp", 0.1f, 0);
                    break;
                }
            case PlayerBrounceOffGotAttackGunFuNodeLeaf: 
                {
                    isLayer_1_Enable = false;    
                    animator.CrossFade("PlayerBounceOff", 0.05f, 0);
                    break;
                }
            case GunFuExecuteNodeLeaf:
                {
                    isLayer_1_Enable = false;
                    if (player._currentWeapon is PrimaryWeapon)
                        animator.CrossFade("GunFu_EX_stepOn_Rifle", 0.3f, 0, 0);
                    if (player._currentWeapon is SecondaryWeapon)
                        animator.CrossFade("GunFu_EX_Knee", 0.3f, 0, 0);
                    break;
                }
            case WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarm: 
                {
                    isLayer_1_Enable = false;
                    if (weaponDisarm.disarmedWeapon is PrimaryWeapon)
                        animator.CrossFade("GunFuPrimaryDisarm", 0f, 0, 0);
                    if (weaponDisarm.disarmedWeapon is SecondaryWeapon)
                        animator.CrossFade("GunFuSecondaryDisarm", 0f, 0, 0);
                    break;
                }
            case Hit1GunFuNode hit1: 
                {
                    isLayer_1_Enable = false;
                    animator.CrossFade("Hit", 0.2f, 0, 0);
                    break;
                }
            case Hit2GunFuNode hit2: 
                {
                    isLayer_1_Enable = false;
                    animator.CrossFade("Hit2", 0.2f, 0, 0);
                    break;
                }
            case KnockDown_GunFuNode knockDown: 
                {
                    isLayer_1_Enable = false;
                    animator.CrossFade("KnockDown", 0.2f, 0, 0);
                    break;
                }
            case RestrictGunFuStateNodeLeaf restrict:
                {
                    if (restrict.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
                    {
                        isLayer_1_Enable = false;
                        animator.CrossFade("Restrict_Enter", 0.05f, 0, 0);
                    }
                    else if (restrict.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                    {
                        animator.CrossFade(Move_Idle, 0.05f, 0, 0);
                        animator.CrossFade("Restrict_Stay", 0.05f, 1, 0);
                        isLayer_1_Enable = true;
                    }
                    else if (restrict.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                    {
                        animator.CrossFade("Restrict_Exit", 0.05f, 0, 0);
                        isLayer_1_Enable = false;
                    }

                    break;
                }
            case HumanShield_GunFuInteraction_NodeLeaf humandShield_NodeLeaf:
                {
                    if (humandShield_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Enter)
                    {
                        isLayer_1_Enable = false;
                        animator.CrossFade(humandShield_NodeLeaf.humandShieldEnter, 0.05f, 0, 0);
                    }
                    else if (humandShield_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay)
                    {
                        animator.CrossFade(Move_Idle, 0.05f, 0, 0);
                        animator.CrossFade(humandShield_NodeLeaf.humandShieldStay, 0.05f, 1, 0);
                        isLayer_1_Enable = true;
                    }
                    else if (humandShield_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Release)
                    {
                        animator.CrossFade("StandWeaponHand LowReady/ADS", 0.05f, 1, 0);
                        isLayer_1_Enable = true;
                    }
                    break;
                }
            case HumanThrowGunFuInteractionNodeLeaf humanThrowGunFuInteractionNodeLeaf: 
                {
                    isLayer_1_Enable = false;
                    animator.CrossFade("HumandThrow", 0.2f, 0, 0);
                    break; 
                }
            case DodgeSpinKicklGunFuNodeLeaf dodgeSpinKicklGunFuNodeLeaf: 
                {
                    isLayer_1_Enable = false;
                    animator.CrossFade("DodgeSpinKick", 0.2f, 0, 0);    
                    break;
                }
            

        }
       

       
    }
    private void PlayerWeaponManuverStateStateNodeNotifyManager(WeaponManuverLeafNode weaponManuverLeafNode)
    {
        switch (weaponManuverLeafNode)
        {
            case HolsterPrimaryWeaponManuverNodeLeaf: animator.CrossFade("HolsterPrimary", 0.1f, 1);    
                break;
            
            case HolsterSecondaryWeaponManuverNodeLeaf: animator.CrossFade("HolsterPrimary", 0.1f, 1);
                break;

            case DrawPrimaryWeaponManuverNodeLeaf: animator.CrossFade("DrawPrimary", 0.1f, 1);
                break;

            case DrawSecondaryWeaponManuverNodeLeaf: animator.CrossFade("DrawSecondary", 0.1f, 1);
                break;

            case PrimaryToSecondarySwitchWeaponManuverLeafNode PTS:
                {
                    if(PTS.curPhase == PrimaryToSecondarySwitchWeaponManuverLeafNode.TransitionPhase.Enter)
                        animator.CrossFade("SwitchWeaponPrimary -> Secondary", 0.1f, 1);
                    break;
                }
            case SecondaryToPrimarySwitchWeaponManuverLeafNode STP:
                {
                    if (STP.curPhase == SecondaryToPrimarySwitchWeaponManuverLeafNode.TransitionPhase.Enter)
                        animator.CrossFade("SwitchWeaponSecondary -> Primary", 0.1f, 1);
                    break;
                }
            case ReloadMagazineFullStageNodeLeaf reloadMagazineFullStageNodeLeaf:
                {    
                    animator.CrossFade("ReloadMagazineFullStage", 0.4f, 1);
                    break;
                }
            case TacticalReloadMagazineFullStageNodeLeaf tacticalReloadMagazineFullStageNodeLeaf:
                {
                    animator.CrossFade("TacticalReloadMagazineFullStage", 0.4f, 1);
                    break;
                }
            case QuickDrawWeaponManuverLeafNode QuickDrawWeaponManuverLeafNode: 
                {
                    QuickDrawWeaponManuverLeafNode.QuickDrawPhase quickDrawPhase = QuickDrawWeaponManuverLeafNode.quickDrawPhase;
                    switch (quickDrawPhase)
                    {
                        case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.Draw:
                            animator.CrossFade("QuickDraw", 0.1f, 1);
                            break;

                        case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.HolsterSecondary:
                            animator.CrossFade("QuickHolster", 0.1f, 1);
                            break;

                        case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.HolsterPrimary:
                            animator.CrossFade("StandWeaponHand LowReady/ADS", 0.1f, 1);
                            break;
                    }
                    break;
                }
            case LowReadyWeaponManuverNodeLeaf LowReadyWeaponManuverLeafNode: 
                {
                    if(player.curNodeLeaf is PlayerSprintNode)
                    {
                        animator.CrossFade("SprintWeaponSway", 0.25f, 1);
                        isLayer_1_Enable = true;
                    }
                    else
                    {
                        animator.CrossFade("StandWeaponHand LowReady/ADS", 0.05f, 1);
                    }
                    break;
                }

        }
    }
}
