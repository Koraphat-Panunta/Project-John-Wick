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
        _pureDestructionDamage = weapon.bullet.GetDestructionDamage * 3;
    }


    public override float recoilKickBack { get => 0; set { } }
    public override BulletType myType { get; set; }
    public override float _pureHpDamage { get => 10000; set => throw new System.NotImplementedException(); }
    public override float _purePostureDamage { get => 0; set => throw new System.NotImplementedException(); }
    public override float _pureDestructionDamage { get ; set ; }
}