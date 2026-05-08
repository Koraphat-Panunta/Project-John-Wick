using UnityEngine;
using static GunFuExecute_Single_NodeLeaf;

public interface IGunFuExecuteNodeLeaf : INodeLeaf,I_OCM_Node
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
    public BulletExecute(RangeWeapon weapon) : base(weapon)
    {
        myType = weapon.bullet.myType;
        _pureDestructionDamage = weapon.bullet.GetDestructionDamage * 3;
    }

    public override float GetHpDamage => _hPDamage;
    public override BulletType myType { get;protected set; }
    public override float _hPDamage { get => 10000; set => throw new System.NotImplementedException(); }
    public override float _postureDamageVisitor { get => 0; set => throw new System.NotImplementedException(); }
    public override float _pureDestructionDamage { get ; set ; }
}
public class ExecuteMethod : IDamageVisitor
{
    public IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf;
    public ExecuteMethod(IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf)
    {
        this.gunFuExecuteNodeLeaf = gunFuExecuteNodeLeaf;
    }
    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        this.gunFuExecuteNodeLeaf.OnNotifyFeedBackVisitor(damageAble);
    }
}