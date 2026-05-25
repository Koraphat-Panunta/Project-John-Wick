using UnityEngine;

public interface I_Got_OCM_Attacked_Able
{
    public bool _triggerEnterGotAttacked_OCM { get; set; }
    //public Vector3 attackerPos { get; set; }
    public I_OCM_Node curAttackerGunFuNode { get; set; }
    //public INodeLeaf curNodeLeaf { get; set; }
    public I_OCM_Attack_Able gunFuAbleAttacker { get; set; }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get; set; }
    public IGotGunFuAttackNode gotGunFuAttackNode { get; }
    public IDamageAble _damageAble { get; set; }
    public Character _character { get; }
    public bool _isGotAttackedAble { get; set; }
    public bool _isGotExecutedAble { get; set; }
    public bool CanTakeAttack<T>(T attackNode) where T : I_OCM_Node;
    public void TakeGunFuAttacked(I_OCM_Node gunFu_NodeLeaf, I_OCM_Attack_Able gunFuAble,bool isTriggerEnterGotAttack_OCM);
}
