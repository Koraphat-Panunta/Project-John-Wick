using UnityEngine;


public partial class Enemy : IGotGunFuAttackedAble
{
    #region ImplementGunFuGotHitAble
    public bool _triggerHitedGunFu { get; set; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => this; set { } }
    public IDamageAble _damageAble { get => this; set { } }
    public bool _isGotAttackedAble
    {
        get
        {
            if (enemyStateManagerNode.TryGetCurNodeLeaf<GotKnockDown_GunFuGotHitNodeLeaf>())
                return false;
            if (enemyStateManagerNode.TryGetCurNodeLeaf<HumandThrow_GotInteract_NodeLeaf>())
                return false;
            if (enemyStateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>())
            {
                return false;
            }
            if (enemyStateManagerNode.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>())
                return false;

            return true;
        }
        set { }
    }
    public bool _isGotExecutedAble
    {
        get
        {
            if (isStagger)
                return true;

            if (enemyStateManagerNode.TryGetCurNodeLeaf<FallDown_EnemyState_NodeLeaf>())
                return true;

            return true;
            return false;
        }
        set { }
    }

    [SerializeField] public GotGunFuHitScriptableObject GotHit1_P;
    [SerializeField] public GotGunFuHitScriptableObject GotHit1_A;
    [SerializeField] public GotGunFuHitScriptableObject GotHit2_P;
    [SerializeField] public GotGunFuHitScriptableObject GotHit2_A;
    [SerializeField] public GotGunFuHitScriptableObject KnockDown;
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
