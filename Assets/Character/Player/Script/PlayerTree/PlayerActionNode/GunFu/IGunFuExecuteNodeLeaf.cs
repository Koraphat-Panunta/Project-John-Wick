using UnityEngine;
using static GunFuExecute_Single_NodeLeaf;

public interface IGunFuExecuteNodeLeaf : INodeLeaf,IGunFuNode
{
    public enum GunFuExecutePhase
    {
        Warping,
        Interacting,
        Execute,
    }
    public GunFuExecutePhase _curGunFuPhase { get; protected set; }
    public GunFuExecuteScriptableObject _gunFuExecuteScriptableObject { get; }
    protected bool _isExecuteAldready { get; set; }
}
public class BulletExecute : Bullet
{
    public BulletExecute(Weapon weapon) : base(weapon)
    {
        myType = weapon.bullet.myType;
        _destructionDamage = weapon.bullet._destructionDamage * 3;
    }

    public override float _hpDamage { get => 10000; set { } }
    public override float _postureDamage { get => 0; set { } }
    public override float recoilKickBack { get => 0; set { } }
    public override BulletType myType { get; set; }
    public override float _destructionDamage { get ; set ; }
}