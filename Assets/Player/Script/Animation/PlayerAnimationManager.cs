
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

    public bool isCover;

    public bool isLayer_1_Enable;

    private bool isIn_C_A_R_aim;
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        player.AddObserver(this);

        isLayer_1_Enable = true;
        isIn_C_A_R_aim = false;
    }

    // UpdateNode is called once per frame
    void Update()
    {
        BackBoardUpdate();

        if (isLayer_1_Enable == true)
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

        if(player.playerStateNodeManager.curNodeLeaf is PlayerInCoverStandIdleNode ||
            player.playerStateNodeManager.curNodeLeaf is PlayerInCoverStandMoveNode)
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
            ,6.5f*Time.deltaTime) ;

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



        AimDownSightWeight = (player as IWeaponAdvanceUser).weaponManuverManager.aimingWeight;
        


        this.DotVelocityWorld_Leftward_Normalized = Vector3.Dot(
            Vector3.Cross(player.transform.forward, Vector3.up).normalized
            , playerMovement.curMoveVelocity_World.normalized);

        if (RecoilWeight > 0)
            RecoilWeight = Mathf.Clamp(RecoilWeight - 3 * Time.deltaTime, 0, 1);

        if (player.currentWeapon is PrimaryWeapon)
            isIn_C_A_R_aim = false;

        if ((player as IWeaponAdvanceUser).currentWeapon != null)
        {
            if (isIn_C_A_R_aim)
            {
                CAR_Weight = Mathf.Lerp(CAR_Weight, 1, 10 * Time.deltaTime);
                if (Vector3.Distance((player as IWeaponAdvanceUser).shootingPos
               , (player as IWeaponAdvanceUser).currentWeapon.bulletSpawnerPos.position) > 24)
                    isIn_C_A_R_aim = false;
            }
            else if (isIn_C_A_R_aim == false)
            {
                CAR_Weight = Mathf.Lerp(CAR_Weight, 0, 10 * Time.deltaTime);
                if (Vector3.Distance((player as IWeaponAdvanceUser).shootingPos
               , (player as IWeaponAdvanceUser).currentWeapon.bulletSpawnerPos.position) < 3.5f)
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
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.Dodge)
        {
            isLayer_1_Enable = false;
            animator.CrossFade("DodgeRoll", 0.05f, 0, 0);
        }
        if (playerAction == SubjectPlayer.PlayerAction.Dead)
        {
            if(isDead)
                return;
            isLayer_1_Enable = false;
            animator.CrossFade("Dead", 0.05f, 0, 0);
            isDead = true;
        }

        if (playerAction == SubjectPlayer.PlayerAction.TakeCover)
            isCover = true;
        
        if(playerAction == SubjectPlayer.PlayerAction.GetOffCover)
            isCover = false;

        if(playerAction == SubjectPlayer.PlayerAction.Firing)
            RecoilWeight = 1;

        if (playerAction == SubjectPlayer.PlayerAction.Sprint)
        {
           
            animator.CrossFade(Sprint, 0.3f, 0, 0);
            isLayer_1_Enable = true;
        }
        else 
        {
            
        }
        if(playerAction == SubjectPlayer.PlayerAction.StandMove||
            playerAction == SubjectPlayer.PlayerAction.StandIdle)
        {

            animator.CrossFade(Move_Idle, 0.3f, 0, 0);
            isLayer_1_Enable = true;
        }

        if(playerAction == SubjectPlayer.PlayerAction.CrouchIdle||
            playerAction == SubjectPlayer.PlayerAction.CrouchMove)
        {
            animator.CrossFade(Crouch, 0.3f, 0, 0);
            isLayer_1_Enable = true;
        }

        if(playerAction == SubjectPlayer.PlayerAction.GunFuEnter)
        {

            isLayer_1_Enable = false;

            if(player.playerStateNodeManager.curNodeLeaf is GunFuExecuteNodeLeaf gunFuExecute)
            {
                if (player.currentWeapon is PrimaryWeapon)
                    animator.CrossFade("GunFu_EX_stepOn_Rifle", 0.2f,0,0);
                if (player.currentWeapon is SecondaryWeapon)
                    animator.CrossFade("GunFu_EX_Knee", 0.2f, 0, 0);
            }

            if(player.playerStateNodeManager.curNodeLeaf is WeaponDisarm_GunFuInteraction_NodeLeaf weaponDisarm)
            {
                if (weaponDisarm.disarmedWeapon is PrimaryWeapon)
                    animator.CrossFade("GunFuPrimaryDisarm", 0f,0,0);
                if (weaponDisarm.disarmedWeapon is SecondaryWeapon)
                    animator.CrossFade("GunFuSecondaryDisarm", 0f, 0, 0);
            }

            if(player.playerStateNodeManager.curNodeLeaf == (player.playerStateNodeManager as PlayerStateNodeManager).Hit1gunFuNode)
                animator.CrossFade("Hit", 0.2f, 0, 0);

            if (player.playerStateNodeManager.curNodeLeaf as PlayerStateNodeLeaf is Hit2GunFuNode)
                animator.CrossFade("Hit2", 0.2f, 0, 0);

            if (player.playerStateNodeManager.curNodeLeaf as PlayerStateNodeLeaf is KnockDown_GunFuNode)
                animator.CrossFade("KnockDown", 0.2f, 0, 0);

            if(player.playerStateNodeManager.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf humanShield)
                animator.CrossFade(humanShield.humandShieldEnter, 0.05f, 0, 0);

            if (player.playerStateNodeManager.curNodeLeaf is HumanThrowGunFuInteractionNodeLeaf humanThrow)
            {
                animator.CrossFade("HumandThrow", 0.2f, 0, 0);
            }

            if (player.playerStateNodeManager.curNodeLeaf is DodgeSpinKicklGunFuNodeLeaf)
                animator.CrossFade("DodgeSpinKick", 0.2f, 0, 0);
        }
        if(playerAction == SubjectPlayer.PlayerAction.GunFuInteract)
        {
            if (player.playerStateNodeManager.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf humanShield)
                if(humanShield.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.InteractionPhase.Stay) 
                {
                    animator.CrossFade(humanShield.humandShieldStay, 0.05f, 0, 0);
                }
                
        }

        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            PlayerWeaponManuver playerWeaponManuver = player.weaponManuverManager as PlayerWeaponManuver;
            if(playerWeaponManuver.curNodeLeaf is HolsterPrimaryWeaponManuverNodeLeaf)
                animator.CrossFade("HolsterPrimary", 0.1f, 1);
            if(playerWeaponManuver.curNodeLeaf is HolsterSecondaryWeaponManuverNodeLeaf)
                animator.CrossFade("HolsterSecondary", 0.1f, 1);
            if (playerWeaponManuver.curNodeLeaf is DrawPrimaryWeaponManuverNodeLeaf)
                animator.CrossFade("DrawPrimary", 0.1f, 1);
            if(playerWeaponManuver.curNodeLeaf is DrawSecondaryWeaponManuverNodeLeaf)
                animator.CrossFade("DrawSecondary", 0.1f, 1);

            if (playerWeaponManuver.curNodeLeaf is PrimaryToSecondarySwitchWeaponManuverLeafNode PTS)
            {
                if(PTS.curPhase == PrimaryToSecondarySwitchWeaponManuverLeafNode.TransitionPhase.Enter)
                    animator.CrossFade("SwitchWeaponPrimary -> Secondary", 0.1f, 1);
            }

            if (playerWeaponManuver.curNodeLeaf is SecondaryToPrimarySwitchWeaponManuverLeafNode STP)
            {
                if (STP.curPhase == SecondaryToPrimarySwitchWeaponManuverLeafNode.TransitionPhase.Enter)
                    animator.CrossFade("SwitchWeaponSecondary -> Primary", 0.1f, 1);
            }
               
        }

        if (playerAction == SubjectPlayer.PlayerAction.ReloadMagazineFullStage)
        {
            if((player.currentWeapon.currentEventNode as ReloadMagazineFullStage).curReloadPhase == IReloadMagazineNodePhase.ReloadMagazinePhase.Enter)
            animator.CrossFade("ReloadMagazineFullStage", 0.4f, 1);
        }

        if (playerAction == SubjectPlayer.PlayerAction.TacticalReloadMagazineFullStage)
        {
            if ((player.currentWeapon.currentEventNode as TacticalReloadMagazineFullStage).curReloadPhase == IReloadMagazineNodePhase.ReloadMagazinePhase.Enter)
                animator.CrossFade("TacticalReloadMagazineFullStage", 0.4f, 1);
        }

        if (playerAction == SubjectPlayer.PlayerAction.InputMag_ReloadMagazineStage)
            animator.CrossFade("MagIn_ReloadMagazineStage", 0.3f,1);

        if (playerAction == SubjectPlayer.PlayerAction.ChamberLoad_ReloadMagazineStage)
            animator.CrossFade("ChamberStage_ReloadMagazineStage", 0.3f, 1);

        if(playerAction == SubjectPlayer.PlayerAction.QuickDraw)
        {
            QuickDrawWeaponManuverLeafNode.QuickDrawPhase quickDrawPhase = (player.weaponManuverManager.curNodeLeaf as QuickDrawWeaponManuverLeafNode).quickDrawPhase;

            switch (quickDrawPhase)
            {
                case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.Draw:animator.CrossFade("QuickDraw",0.7f,1);
                    break;

                case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.HolsterSecondary: animator.CrossFade("QuickHolster", 0.1f, 1);
                    break;

                case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.HolsterPrimary: animator.CrossFade("StandWeaponHand LowReady/ADS", 0.1f, 1);
                    break;
            }
            
        }

    }

    public void OnNotify(Player player)
    {
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
    
}
