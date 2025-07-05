using UnityEngine;


public partial class Enemy : IGotGunFuAttackedAble
{
    #region ImplementGunFuGotHitAble
    public bool _triggerHitedGunFu { get; set; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public INodeLeaf curNodeLeaf { get => enemyStateManagerNode.GetCurNodeLeaf(); set { } }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => this; set { } }
    public IDamageAble _damageAble { get => this; set { } }
    public bool _isGotAttackedAble
    {
        get
        {
            if (curNodeLeaf is GotKnockDown_GunFuGotHitNodeLeaf)
                return false;
            if (curNodeLeaf is HumandThrow_GotInteract_NodeLeaf)
                return false;
            if (curNodeLeaf is FallDown_EnemyState_NodeLeaf fallNode)
            {
                return false;
            }
            if (curNodeLeaf is EnemySpinKickGunFuNodeLeaf)
                return false;

            return true;
        }
        set { }
    }
    public bool _isGotExecutedAble
    {
        get
        {
            if (posture < postureMedium)
                return true;

            if (curNodeLeaf is FallDown_EnemyState_NodeLeaf)
                return true;

            return false;
        }
        set { }
    }

    [SerializeField] public GunFu_GotHit_ScriptableObject GotHit1;
    [SerializeField] public GunFu_GotHit_ScriptableObject GotHit2;
    [SerializeField] public GunFu_GotHit_ScriptableObject KnockDown;
    [SerializeField] public GotRestrictScriptableObject gotRestrictScriptableObject;
    [SerializeField] public WeaponGotDisarmedScriptableObject primary_WeaponGotDisarmedScriptableObject;
    [SerializeField] public WeaponGotDisarmedScriptableObject secondary_WeaponGotDisarmedScriptableObject;

    [SerializeField] public AnimationClip layUpExecutedAnim;
    [SerializeField] public AnimationClip layDownExecutedAnim;
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attacker)
    {
        _triggerHitedGunFu = true;
        curAttackerGunFuNode = gunFu_NodeLeaf;
        gunFuAbleAttacker = attacker;
    }
    #endregion
}
