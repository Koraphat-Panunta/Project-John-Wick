using UnityEngine;

public class PlayerConstrainAnimationManager : AnimationConstrainManager, IObserverPlayer
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

    public Player player;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        RecoveryUpdateWeight();
    }
  

    public override INodeLeaf curNodeLeaf { get; set; }
    public override INodeSelector startNodeSelector { get; set; }

    public RestAnimationConstrainNodeLeaf restAnimationConstrainNodeLeaf { get;private set; }

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

        aimDownSightConstrainSelector = new AnimationConstrainNodeSelector(()=>player._currentWeapon != null && player.weaponAdvanceUser.weaponManuverManager.aimingWeight > 0);
        rifle_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player,this.StandSplineLookConstrain,standRifleAimSplineLookConstrainScriptableObject,()=> player._currentWeapon is PrimaryWeapon);
        rifle_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player, this.rifileLeaningConstrainScriptableObject, leaningRotation,player, () => player._currentWeapon is PrimaryWeapon);
        rifleADSConstrainCombineNode = new AnimationConstrainCombineNode(() => player._currentWeapon is PrimaryWeapon);

        pistol_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player, this.StandSplineLookConstrain, standPistolAimSplineLookConstrainScriptableObject, () => player._currentWeapon is SecondaryWeapon);
        pistol_leaningRotationConstrainNodeLeaf = new PlayerLeaningRotationConstrainNodeLeaf(this.player, this.pistolLeaningConstrainScriptableObject, leaningRotation, player, () => player._currentWeapon is SecondaryWeapon);
        pistolADSConstrainCombineNode = new AnimationConstrainCombineNode(() => player._currentWeapon is SecondaryWeapon);
        restAnimationConstrainNodeLeaf = new RestAnimationConstrainNodeLeaf(() => true);

        gunFuConstraintSelector = new AnimationConstrainNodeSelector(()=> player.curNodeLeaf is IGunFuNode);
        humanShieldConstrainSelector = new AnimationConstrainNodeSelector(() => player.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf || player.curNodeLeaf is HumanThrowGunFuInteractionNodeLeaf);
        humanShield_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,humanShieldRightHandConstrainLookAtScriptableObject_rifle,
            ()=> player._currentWeapon is PrimaryWeapon);
        humanShield_secondary_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager, humanShieldRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        restrictConstraintSelector = new AnimationConstrainNodeSelector(() => player.curNodeLeaf is RestrictGunFuStateNodeLeaf);
        restrict_rifle_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,restrictRightHandConstrainLookAtScriptableObject_rifle,
            ()=> player._currentWeapon is PrimaryWeapon);
        restrict_pistol_AnimationConstraintNodeLeaf = new RightHandLookControlAnimationConstraintNodeLeaf(RightHandConstrainLookAtManager,restrictRightHandConstrainLookAtScriptableObject_pistol,
            () => player._currentWeapon is SecondaryWeapon);

        startNodeSelector.AddtoChildNode(gunFuConstraintSelector);
        startNodeSelector.AddtoChildNode(aimDownSightConstrainSelector);
        startNodeSelector.AddtoChildNode(restAnimationConstrainNodeLeaf);

        aimDownSightConstrainSelector.AddtoChildNode(rifleADSConstrainCombineNode);
        aimDownSightConstrainSelector.AddtoChildNode(pistolADSConstrainCombineNode);

        gunFuConstraintSelector.AddtoChildNode(restrictConstraintSelector);
        gunFuConstraintSelector.AddtoChildNode(humanShieldConstrainSelector);
        gunFuConstraintSelector.AddtoChildNode(restAnimationConstrainNodeLeaf);

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
        else if (player.weaponManuverManager.curNodeLeaf is AimDownSightWeaponManuverNodeLeaf == false)
        {
            StandSplineLookConstrain.SetWeight(StandSplineLookConstrain.GetWeight() - Time.deltaTime);
            leaningRotation.SetWeight(leaningRotation.weight - Time.deltaTime);

        }
      
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {

    }

    public void OnNotify(Player player)
    {

    }
}
