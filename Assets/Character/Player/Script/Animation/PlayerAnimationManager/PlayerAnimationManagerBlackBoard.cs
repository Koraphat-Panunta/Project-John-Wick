using UnityEngine;

public partial class PlayerAnimationManager
{
    public Animator animator;
    public Player player;

    public AnimationPoseTimeNormalized basedAnimationPoseTimeNormalzied;
    public AnimationPoseTimeNormalized upperAnimationPoseTimeNormalized;
    public AnimationPoseTimeNormalized upperBodyAnimationPoseTimeNormalized;

    public string Sprint = "Sprint";
    public string Move_Idle = "Move/Idle";
    public string Crouch = "Crouch";

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

    public float angleLookHorizontal;
    public float angleLookVertical;

    public bool isIn_C_A_R_aim { get; protected set; }
    [Range(0,10)]
    [SerializeField] private float CAR_ChangeRate;
    [SerializeField] private float CAR_Range = 2;
    [SerializeField] private float CAR_ChangeTime = .75f;
    private float CAR_ChangeTimer;

    [SerializeField] string curUpperBodyLayer;
    [SerializeField] string curUpperArmLayer;
    [SerializeField] string curBaseLayer;


    private INodeManager playerStateNodeMnager => player.playerStateNodeManager;
    private INodeManager playerWeaponManuverNodeManager => player._weaponManuverManager;
    private bool isEnableUpperBodyLayer
    {
        get
        {
            if(this.playerStateNodeMnager.TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>(out RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf)
                && restrainGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                return true;

            if (this.playerStateNodeMnager.TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>(out HumanShield_GunFu_NodeLeaf humanShield_GunFu_NodeLeaf)
                && humanShield_GunFu_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay)
                return true;

            if (this.isPerformReload)
                return true;

            if (this.isDrawSwitchWeapon)
                return true;

            return false;
        }
    }
    private bool isEnableUpperArmLayer { get 
        {
            if (this.player._currentWeapon == null)
                return false;

            if (this.playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IReloadNode>())
                return false;

            if (this.isDrawSwitchWeapon)
                return false;

            if(this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerStandIdleNodeLeaf>()
                || this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerStandMoveNodeLeaf>()
                || this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerCrouch_Idle_NodeLeaf>()
                || this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerCrouch_Move_NodeLeaf>()
                || this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintNode>()
                || this.playerStateNodeMnager.TryGetCurNodeLeaf<PlayerSprintChangeDirectionNode>()
                )
                return true;

            return false;

            
        } 
    }
    private bool isPerformGunFu { get 
        {
            if(playerStateNodeMnager.TryGetCurNodeLeaf<I_OCM_Node>())
                return true;
            return false;
        } 
    }
    private bool isDrawSwitchWeapon { get 
        { 
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_Draw_NodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_HolsterPrimaryWeapon_NodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<QuickSwitch_HolsterSecondaryWeapon_NodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterPrimaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<HolsterSecondaryWeaponManuverNodeLeaf>()
                )
                return true;
            return false;
        } 
    }
    private bool isPerformReload { get 
        {
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IReloadNode>())
                return true;
            return false;
        } 
    }

    [SerializeField] public PlayPoseAnimationScriptableObject dolphinDivePoseAnimationSCRP;
}
