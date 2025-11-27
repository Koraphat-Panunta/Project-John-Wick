using UnityEngine;
using static GunFuExecute_Single_NodeLeaf;

public interface IGunFuExecuteNodeLeaf : INodeLeaf,IGunFuNode
{
    public enum GunFuExecutePhase
    {
        Warping,
        Interacting,
    }
    public GunFuExecutePhase _curGunFuPhase { get; protected set; }
    public AnimationInteractScriptableObject _gunFuExecuteInteractSCRP { get; }
    public bool _isExecuteAldready { get;protected set; }

    public GunFuExecuteStateName _executeStateName { get; protected set; }
}
public class BulletExecute : Bullet
{
    public BulletExecute(Weapon weapon) : base(weapon)
    {
        myType = weapon.bullet.myType;
        _pureDestructionDamage = weapon.bullet.GetDestructionDamage * 3;
    }

    public override float GetHpDamage => _pureHpDamage;
    public override BulletType myType { get; set; }
    public override float _pureHpDamage { get => 10000; set => throw new System.NotImplementedException(); }
    public override float _purePostureDamage { get => 0; set => throw new System.NotImplementedException(); }
    public override float _pureDestructionDamage { get ; set ; }
}