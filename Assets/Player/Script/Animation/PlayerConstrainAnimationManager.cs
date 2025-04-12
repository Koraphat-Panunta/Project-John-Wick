using UnityEngine;

public class PlayerConstrainAnimationManager : AnimationConstrainManager, IObserverPlayer
{
    public SplineLookConstrain StandSplineLookConstrain;
    public LeaningRotation leaningRotation;

    public AimSplineLookConstrainScriptableObject standPistolAimSplineLookConstrainScriptableObject;
    public AimSplineLookConstrainScriptableObject standRifleAimSplineLookConstrainScriptableObject;

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
    public AimDownSightAnimationConstrainNodeLeaf pistol_ADS_ConstrainNodeLeaf { get; private set; }
    public AnimationConstrainNodeSelector aimDownSightConstrainSelector { get; private set; }
    public override void InitailizedNode()
    {
        startNodeSelector = new AnimationConstrainNodeSelector(()=>true);

        aimDownSightConstrainSelector = new AnimationConstrainNodeSelector(()=>player._currentWeapon != null && player.weaponAdvanceUser.weaponManuverManager.aimingWeight > 0);
        rifle_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player,this.StandSplineLookConstrain,standRifleAimSplineLookConstrainScriptableObject,()=> player._currentWeapon is PrimaryWeapon);
        pistol_ADS_ConstrainNodeLeaf = new AimDownSightAnimationConstrainNodeLeaf(this.player, this.StandSplineLookConstrain, standPistolAimSplineLookConstrainScriptableObject, () => player._currentWeapon is SecondaryWeapon);

        restAnimationConstrainNodeLeaf = new RestAnimationConstrainNodeLeaf(() => true);

        startNodeSelector.AddtoChildNode(aimDownSightConstrainSelector);
        startNodeSelector.AddtoChildNode(restAnimationConstrainNodeLeaf);

        aimDownSightConstrainSelector.AddtoChildNode(rifle_ADS_ConstrainNodeLeaf);
        aimDownSightConstrainSelector.AddtoChildNode(pistol_ADS_ConstrainNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();
    }
    private void RecoveryUpdateWeight()
    {
        if (player.curNodeLeaf is AimDownSightWeaponManuverNodeLeaf == false)
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
