using UnityEngine;


public partial class Enemy : IGotGunFuAttackedAble
{
    #region ImplementGunFuGotHitAble
    public bool _triggerHitedGunFu { get; set; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IGunFuNode curAttackerGunFuNode { get; set; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => this; set { } }
    public IDamageAble _damageAble { get => this; set { } }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get => this; set { } }
    public bool _isGotAttackedAble
    {
        get
        {
           

            if (enemyStateManagerNode.TryGetCurNodeLeaf<GotGunFuHitNodeLeaf>(out GotGunFuHitNodeLeaf gotGunFuHitNodeLeaf)
                && gotGunFuHitNodeLeaf.gotHitstateName == "Hit3")
                return false;
            if(_isFallDown)
                return false;
           
            if(enemyStateManagerNode.TryGetCurNodeLeaf<EnemyDeadStateNode>())
                return false;
            if(enemyStateManagerNode.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>())
                return false;

            return true;
        }
        set { }
    }
    public bool _isGotExecutedAble
    {
        get
        {
            //if (enemyStateManagerNode.TryGetCurNodeLeaf<EnemySpinKickGunFuNodeLeaf>())
            //    return false;
            if (enemyStateManagerNode.TryGetCurNodeLeaf<EnemyDeadStateNode>())
                return false;
            if (enemyStateManagerNode.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>())
                return false;

            if (isStagger)
                return true;

            return false;
        }
        set { }
    }

    [SerializeField] public GotGunFuHitScriptableObject GotHit1_P;
    [SerializeField] public GotGunFuHitScriptableObject GotHit1_A;
    [SerializeField] public GotGunFuHitScriptableObject GotHit2_P;
    [SerializeField] public GotGunFuHitScriptableObject GotHit2_A;
    [SerializeField] public GotGunFuHitScriptableObject GotHit3;
    [SerializeField] public GotRestrictScriptableObject gotRestrictScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP humanShield_GotInteract_Exit_SCRP;
    [SerializeField] public AnimationTriggerEventSCRP primary_WeaponGotDisarmedScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP secondary_WeaponGotDisarmedScriptableObject;

    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_Dodge_ScriptableObject_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_II;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_III;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_IV;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Primary_ScriptableObject_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Primary_ScriptableObject_II;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Primary_Dodge_ScriptableObject_I;

    [SerializeField] public AnimationTriggerEventSCRP gotGunFu_Single_Execute_OnGround_LayUp_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFu_Single_Execute_OnGround_LayDown_I;
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attacker)
    {
        _triggerHitedGunFu = true;
        curAttackerGunFuNode = gunFu_NodeLeaf;
        gunFuAbleAttacker = attacker;
        TakeDamage(gunFu_NodeLeaf);
    }
    #endregion
}
