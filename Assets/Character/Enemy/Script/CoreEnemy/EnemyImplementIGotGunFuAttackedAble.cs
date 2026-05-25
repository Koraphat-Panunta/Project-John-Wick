using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;


public partial class Enemy : I_Got_OCM_Attacked_Able
{
    #region ImplementGunFuGotHitAble
    public bool _triggerEnterGotAttacked_OCM { get; set; }
    public I_OCM_Attack_Able gunFuAbleAttacker { get; set; }
    public I_OCM_Node curAttackerGunFuNode { get; set; }
    public IDamageAble _damageAble { get => this; set { } }
    public I_Got_OCM_Attacked_Able gotGunFuAttackedAble { get => this; set { } }
    public IGotGunFuAttackNode gotGunFuAttackNode => ReturnGotAttackNodeFromStateMachine.GetAttackNode(this.enemyStateManagerNode);
    public bool _isGotAttackedAble
    {
        get
        {
         
            if(stateManagerNode.TryGetCurNodeLeaf<EnemyDeadStateNode>())
                return false;
            if(stateManagerNode.TryGetCurNodeLeaf<EnemyDodgeStateNodeLeaf>())
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
            if (stateManagerNode.TryGetCurNodeLeaf<EnemyDodgeStateNodeLeaf>())
                return false;

            return true;
        }
        set { }
    }



    [SerializeField] public GotRestrictScriptableObject gotRestrictScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP humanShield_GotInteract_Exit_SCRP;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuReloadScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP gotKnockDownScriptableObject;
    [SerializeField] public AnimationTriggerEventSCRP gotHitDown_ScriptableObject;

    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Dodge_ScriptableObject_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_II;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Secondary_ScriptableObject_III;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Primary_ScriptableObject_I;
    [SerializeField] public AnimationTriggerEventSCRP gotGunFuExecute_Single_Primary_ScriptableObject_II;

    [SerializeField] public AnimationTriggerEventSCRP gotGunFu_Execute_OnGround_I;
    [SerializeField] public AnimationTriggerEventSCRP gotMeleeExecute_SCRP;
    public void TakeGunFuAttacked(I_OCM_Node gunFu_NodeLeaf, I_OCM_Attack_Able attacker,bool isTriggerEnterGotAttack_OCM)
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

            //Debug.DrawRay(this.transform.position, hitDir, Color.red, 3);

            CharacterHitedEventDetail characterHitedEventDetail = new CharacterHitedEventDetail
            {
                hitDir = hitDir,
                hitedPart = this.localServiceLocator.Get<FullBodyCharacterPart>().spline_0BodyPart,
                hitforce = gunFuHitNodeLeaf.gunFuHitScriptableObject.gunFuHitDetail[gunFuHitNodeLeaf.hitCount].hitPushForce,
                hitPos = this.transform.position
            };

            this.NotifyObserver<CharacterHitedEventDetail>(this, characterHitedEventDetail);

        }
        this.curAttackerGunFuNode = gunFu_NodeLeaf;
        Debug.Log("this.curAttackerGunFuNode = "+ gunFu_NodeLeaf);
        gunFuAbleAttacker = attacker;

        this._triggerEnterGotAttacked_OCM = isTriggerEnterGotAttack_OCM;


        TakeDamage(gunFu_NodeLeaf);
    }

    public bool CanTakeAttack<T>(T attackNode) where T : I_OCM_Node
    {
        switch (attackNode)
        {
            case RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf:
            case HumanShield_GunFu_NodeLeaf humanShield_GunFu_NodeLeaf:
                {
                    if (this._isInPain == false)
                        return false;
                    break;
                }
            case OCM_KnockDown_NodeLeaf oCM_KnockDown_NodeLeaf:
                {
                    if(this._isFallDown)
                        return false;
                    break;
                }
        }

        if(this.isDead)
            return false;

        return true;
    }
    #endregion
}
