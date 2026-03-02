using UnityEngine;

public partial class PlayerAnimationManager
{
    public Animator animator;
    public Player player;

    public AnimationPoseTimeNormalized basedAnimationPoseTimeNormalzied;
    public AnimationPoseTimeNormalized upperAnimationPoseTimeNormalized;

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

    [SerializeField] string curUpperLayer;
    [SerializeField] string curBaseLayer;


    private INodeManager playerStateNodeMnager => player.playerStateNodeManager;
    private INodeManager playerWeaponManuverNodeManager => player.weaponAdvanceUser._weaponManuverManager;
    private bool isEnableUpperLayer { get 
        {
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<RestWeaponManuverLeafNode>())
                return false;

            if(playerStateNodeMnager.TryGetCurNodeLeaf<IParkourNodeLeaf>())
                return false;

            if(playerStateNodeMnager.TryGetCurNodeLeaf(out HumanShield_GunFu_NodeLeaf humanShiedl) 
                && humanShiedl.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay)
                return true;

            if (playerStateNodeMnager.TryGetCurNodeLeaf(out RestrainGunFuStateNodeLeaf restrict)
               && restrict.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                return true;

           if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<IReloadNode>())
                return true;

           if (playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawSecondaryWeaponManuverNodeLeaf>() 
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>()
                )
                return true;

            if (playerStateNodeMnager.TryGetCurNodeLeaf<GunFuHitNodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerDodgeRollStateNodeLeaf>()
                || (playerStateNodeMnager.TryGetCurNodeLeaf(out RestrainGunFuStateNodeLeaf nodeLeaf) && (nodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter || nodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit))
                || (playerStateNodeMnager.TryGetCurNodeLeaf(out HumanShield_GunFu_NodeLeaf humanShield) && (humanShield.curPhase == PlayerStateNodeLeaf.NodePhase.Enter ))
                || playerStateNodeMnager.TryGetCurNodeLeaf<HumanShieldExit_GunFu_NodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<WeaponDisarm_GunFuInteraction_NodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerPokePickUpWeaponNodeLeaf>()
                || playerStateNodeMnager.TryGetCurNodeLeaf<PlayerGetUpStateNodeLeaf>()
                || this.playerStateNodeMnager.TryGetCurNodeLeaf<GunFuHitDownNodeLeaf>()
                )
                return false;

            if (player.curNodeLeaf is PlayerDolphinDiveStateNodeLeaf dolphinDiveStateNodeLeaf)
                return false;

            if (player.curNodeLeaf is PlayerProneStateNodeLeaf)
                return false;

            return true;

            
        } 
    }
    private bool isPerformGunFu { get 
        {
            if(playerStateNodeMnager.TryGetCurNodeLeaf<IGunFuNode>())
                return true;
            return false;
        } 
    }
    private bool isDrawSwitchWeapon { get 
        { 
            if(playerWeaponManuverNodeManager.TryGetCurNodeLeaf<PrimaryToSecondarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<SecondaryToPrimarySwitchWeaponManuverLeafNode>()
                || playerWeaponManuverNodeManager.TryGetCurNodeLeaf<DrawPrimaryWeaponManuverNodeLeaf>()
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
