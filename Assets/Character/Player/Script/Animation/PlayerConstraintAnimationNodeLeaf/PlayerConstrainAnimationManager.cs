using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class PlayerConstrainAnimationManager : AnimationConstrainNodeManager
{
    public SplineLookConstrain StandSplineLookConstrain;
    public LeaningRotation leaningRotation;
    public RightHandConstrainLookAtManager RightHandConstrainLookAtManager;

    public AimSplineLookConstrainScriptableObject standPistolAimSplineLookConstrainScriptableObject;
    public AimSplineLookConstrainScriptableObject standRifleAimSplineLookConstrainScriptableObject;

    public LeaningRotaionScriptableObject pistolLeaningConstrainScriptableObject;
    public LeaningRotaionScriptableObject rifileLeaningConstrainScriptableObject;

    public RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject_rifle;
    public RightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject_pistol;

    public RightHandConstrainLookAtScriptableObject restrictRightHandConstrainLookAtScriptableObject_pistol;
    public RightHandConstrainLookAtScriptableObject restrictRightHandConstrainLookAtScriptableObject_rifle;

    [SerializeField] private Rig rig;

    [SerializeField] private string curState;

    public Player player;

    protected override void FixedUpdate()
    {
        curState = curNodeLeaf.ToString();
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        RecoveryUpdateWeight();
    }
  
    public override INodeSelector startNodeSelector { get; set; }
    public NodeCombine playerConstraintCombineNode { get; set; }
    public NodeSelector enableDisableConstraintWeightNodeSelector { get; set; }
    public SetWeightConstraintNodeLeaf enableConstraintWeight { get; set; }
    public SetWeightConstraintNodeLeaf disableConstraintWeight { get; set; }   
    
    public NodeSelector constraintNodeStateSelector { get; set; }
    public RestAnimationConstrainNodeLeaf restAnimationConstrainNodeLeaf { get;private set; }
    public RestAnimationConstrainNodeLeaf rest_gunfu_AnimationConstrainNodeLeaf { get; private set; }
    public AimDownSightAnimationConstrainNodeLeaf rifle_ADS_ConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf rifle_leaningRotationConstrainNodeLeaf { get; private set; }
    public AnimationConstrainCombineNode rifleADSConstrainCombineNode { get; private set; }

    public AimDownSightAnimationConstrainNodeLeaf pistol_ADS_ConstrainNodeLeaf { get; private set; }
    public PlayerLeaningRotationConstrainNodeLeaf pistol_leaningRotationConstrainNodeLeaf { get; private set; }
    public AnimationConstrainCombineNode pistolADSConstrainCombineNode { get; private set; }

    public AnimationConstrainNodeSelector gunFuConstraintSelector { get; private set; }
    public AnimationConstrainNodeSelector humanShieldConstrainSelector { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf humanShield_secondary_AnimationConstraintNodeLeaf { get; private set; }
    public AnimationConstrainNodeSelector restrictConstraintSelector { get; private set; }  
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_rifle_AnimationConstraintNodeLeaf { get; private set; }
    public RightHandLookControlAnimationConstraintNodeLeaf restrict_pistol_AnimationConstraintNodeLeaf { get; private set; }

    public AnimationConstrainNodeSelector aimDownSightConstrainSelector { get; private set; }
    public override void InitailizedNode()
    {
        startNodeSelector = new AnimationConstrainNodeSelector(()=>true);

        playerConstraintCombineNode = new NodeCombine(()=> true);

        enableDisableConstraintWeightNodeSelector = new NodeSelector(() => true, "enableDisableConstraintWeightNodeSelector");
        enableConstraintWeight = new SetWeightConstraintNodeLeaf(()=> isConstraintEnable,rig,4,1);
        disableConstraintWeight = new SetWeightConstraintNodeLeaf(() => true, rig, 5, 0);

        constraintNodeStateSelector = new NodeSelector(()=> isConstraintEnable);

        aimDownSightConstrainSelector = new AnimationConstrainNodeSelector(()=>player._currentWeapon != null && player.weaponAdvanceUser._weaponManuverManager.aimingWeight > 0);
        rifle_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player,this.StandSplineLookConstrain,standRifleAimSplineLookConstrainScriptableObject,()=> player._currentWeapon is PrimaryWeapon);
        rifle_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player, this.rifileLeaningConstrainScriptableObject, leaningRotation,player, () => player._currentWeapon is PrimaryWeapon);
        rifleADSConstrainCombineNode = new AnimationConstrainCombineNode(() => player._currentWeapon is PrimaryWeapon);

        pistol_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player, this.StandSplineLookConstrain, standPistolAimSplineLookConstrainScriptableObject, () => player._currentWeapon is SecondaryWeapon);
        pistol_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player, this.pistolLeaningConstrainScriptableObject, leaningRotation, player, () => player._currentWeapon is SecondaryWeapon);
        pistolADSConstrainCombineNode = new AnimationConstrainCombineNode(() => player._currentWeapon is SecondaryWeapon);
        restAnimationConstrainNodeLeaf = new RestAnimationConstrainNodeLeaf(rig,() => true);

        gunFuConstraintSelector = new AnimationConstrainNodeSelector(
            ()=> playerStateManaher.TryGetCurNodeLeaf<IGunFuNode>());
        humanShieldConstrainSelector = new AnimationConstrainNodeSelector(
            () => playerStateManaher.TryGetCurNodeLeaf<HumanShield_GunFuInteraction_NodeLeaf>() 
            || playerStateManaher.TryGetCurNodeLeaf<HumanThrowGunFuInteractionNodeLeaf>());
        humanShield_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,humanShieldRightHandConstrainLookAtScriptableObject_rifle,
            ()=> player._currentWeapon is PrimaryWeapon);
        humanShield_secondary_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, humanShieldRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        restrictConstraintSelector = new AnimationConstrainNodeSelector(
            () => playerStateManaher.TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>());
        restrict_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,restrictRightHandConstrainLookAtScriptableObject_rifle,
            ()=> player._currentWeapon is PrimaryWeapon);
        restrict_pistol_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,restrictRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        rest_gunfu_AnimationConstrainNodeLeaf = new RestAnimationConstrainNodeLeaf(rig, () => true);

        startNodeSelector.AddtoChildNode(playerConstraintCombineNode);

        playerConstraintCombineNode.AddCombineNode(enableDisableConstraintWeightNodeSelector);
        playerConstraintCombineNode.AddCombineNode(constraintNodeStateSelector);

        enableDisableConstraintWeightNodeSelector.AddtoChildNode(enableConstraintWeight);
        enableDisableConstraintWeightNodeSelector.AddtoChildNode(disableConstraintWeight);

        constraintNodeStateSelector.AddtoChildNode(gunFuConstraintSelector);
        constraintNodeStateSelector.AddtoChildNode(aimDownSightConstrainSelector);
        constraintNodeStateSelector.AddtoChildNode(restAnimationConstrainNodeLeaf);

        aimDownSightConstrainSelector.AddtoChildNode(rifleADSConstrainCombineNode);
        aimDownSightConstrainSelector.AddtoChildNode(pistolADSConstrainCombineNode);

        gunFuConstraintSelector.AddtoChildNode(restrictConstraintSelector);
        gunFuConstraintSelector.AddtoChildNode(humanShieldConstrainSelector);
        gunFuConstraintSelector.AddtoChildNode(rest_gunfu_AnimationConstrainNodeLeaf);

        restrictConstraintSelector.AddtoChildNode(restrict_rifle_AnimationConstraintNodeLeaf);
        restrictConstraintSelector.AddtoChildNode(restrict_pistol_AnimationConstraintNodeLeaf);

        humanShieldConstrainSelector.AddtoChildNode(humanShield_rifle_AnimationConstraintNodeLeaf);
        humanShieldConstrainSelector.AddtoChildNode(humanShield_secondary_AnimationConstraintNodeLeaf);

        rifleADSConstrainCombineNode.AddCombineNode(rifle_leaningRotationConstrainNodeLeaf);
        rifleADSConstrainCombineNode.AddCombineNode(rifle_ADS_ConstrainNodeLeaf);

        pistolADSConstrainCombineNode.AddCombineNode(pistol_leaningRotationConstrainNodeLeaf);
        pistolADSConstrainCombineNode.AddCombineNode(pistol_ADS_ConstrainNodeLeaf);

       

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();
    }
    private void RecoveryUpdateWeight()
    {

        if (curNodeLeaf is RightHandLookControlAnimationConstraintNodeLeaf == false)
        {
            RightHandConstrainLookAtManager.SetWeight(RightHandConstrainLookAtManager.GetWeight() - Time.deltaTime);
        }
        else if ((player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
        {
            StandSplineLookConstrain.SetWeight(StandSplineLookConstrain.GetWeight() - Time.deltaTime);
            leaningRotation.SetWeight(leaningRotation.weight - Time.deltaTime);
        }

      

    }
}
