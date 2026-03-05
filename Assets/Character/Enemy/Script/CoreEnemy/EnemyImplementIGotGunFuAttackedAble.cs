using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;


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
         
            if(stateManagerNode.TryGetCurNodeLeaf<EnemyDeadStateNode>())
                return false;
            if(stateManagerNode.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>())
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
            if (stateManagerNode.TryGetCurNodeLeaf<EnemyDeadStateNode>())
                return false;
            if (stateManagerNode.TryGetCurNodeLeaf<EnemyDodgeRollStateNodeLeaf>())
                return false;

            return true;
        }
        set { }
    }


    [SerializeField] public GotRestrictScriptableObject gotRestrictScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP humanShield_GotInteract_Exit_SCRP;
    [SerializeField] public AnimationTriggerEventSCRP primary_WeaponGotDisarmedScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP secondary_WeaponGotDisarmedScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuReloadScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP gotHitDown_ScriptableObject;

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

        if (gunFu_NodeLeaf is GunFuHitNodeLeaf gunFuHitNodeLeaf)
        {

            Vector3 gunFuAblePos = new Vector3
                (
                gunFuHitNodeLeaf.gunFuAble._character.transform.position.x
                , this.transform.position.y
                , gunFuHitNodeLeaf.gunFuAble._character.transform.position.z
                );

            Vector3 hitDir = (this.transform.position - gunFuAblePos).normalized;
            hitDir = Quaternion.LookRotation(hitDir, Vector3.up) * Quaternion.Euler(gunFuHitNodeLeaf.gunFuHitScriptableObject.gunFuHitDetail[gunFuHitNodeLeaf.hitCount].hitDirPoseAnimOffset) * Vector3.forward;

            Debug.DrawRay(this.transform.position, hitDir, Color.red, 3);

            CharacterHitedEventDetail characterHitedEventDetail = new CharacterHitedEventDetail
            {
                hitDir = hitDir,
                hitedPart = this.spline,
                hitforce = gunFuHitNodeLeaf.gunFuHitScriptableObject.gunFuHitDetail[gunFuHitNodeLeaf.hitCount].hitPushForce,
                hitPos = this.transform.position
            };

            this.NotifyObserver<CharacterHitedEventDetail>(this, characterHitedEventDetail);

        }

        _triggerHitedGunFu = true;
        this.curAttackerGunFuNode = gunFu_NodeLeaf;
        Debug.Log("this.curAttackerGunFuNode = "+ gunFu_NodeLeaf);
        gunFuAbleAttacker = attacker;
        TakeDamage(gunFu_NodeLeaf);
    }
    #endregion
}
