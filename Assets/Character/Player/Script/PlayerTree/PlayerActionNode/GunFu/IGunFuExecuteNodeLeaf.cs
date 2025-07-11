using UnityEngine;

public interface IGunFuExecuteNodeLeaf : INodeLeaf,IGunFuNode
{
    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject { get; }
    protected bool _isExecuteAldready { get; set; }
}
public class BulletExecute : Bullet
{
    public BulletExecute(Weapon weapon) : base(weapon)
    {
        myType = weapon.bullet.myType;
    }

    public override float hpDamage { get => 10000; set { } }
    public override float impactDamage { get => 0; set { } }
    public override float recoilKickBack { get => 0; set { } }
    public override BulletType myType { get; set; }
}