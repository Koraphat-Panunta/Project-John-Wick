using UnityEngine;

public partial class Player: I_Got_OCM_Attacked_Able
{
    #region InitializedGotAttackedGunFu
    public bool _triggerHitedGunFu { get; set; }
    public I_OCM_Node curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get => (playerStateNodeManager as INodeManager).GetCurNodeLeaf(); set => (playerStateNodeManager as INodeManager).SetCurNodeLeaf(value); }
    public I_OCM_Attack_Able gunFuAbleAttacker { get; set; }
    public IDamageAble _damageAble { get => this; set { } }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get => this; set { } }
    public bool _isGotAttackedAble
    {
        get
        {
            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<PlayerBrounceOffNodeLeaf>())
                return false;
            return true;
        }
        set { }
    }
    public bool _isGotExecutedAble { get; set; }

    public IGotGunFuAttackNode gotGunFuAttackNode => ReturnGotAttackNodeFromStateMachine.GetAttackNode(this.playerStateNodeManager);

    Character I_Got_OCM_Attacked_Able._character => this;

    public bool CanTakeAttack<T>(T attackNode) where T : I_OCM_Node
    {
        return true;
    }



    //public Character _character { get => this; }
    public void TakeGunFuAttacked(I_OCM_Node gunFu_NodeLeaf, I_OCM_Attack_Able gunFuAble)
    {
        _triggerHitedGunFu = true;
        gunFuAbleAttacker = gunFuAble;
        curAttackerGunFuNode = gunFu_NodeLeaf;
    }
    #endregion
}
