using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour, IBulletDamageAble, IGotGunFuAttackedAble, IFriendlyFirePreventing, 
     IThrowAbleObjectVisitor,IObserverEnemy,IGotPointingAble
{
    [SerializeField] public Enemy enemy;
    [SerializeField] private EnemyHPbarDisplay enemyHPbarDisplay;
    public abstract float hpReciverMultiplyRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public bool _triggerHitedGunFu { get; set; }

    public Vector3 forceSave;
    public Vector3 hitForcePositionSave;

    public bool isForceSave;

    protected virtual void Start()
    {
        enemy.bulletDamageAbleBodyPartBehavior = new EnemyBodyBulletDamageAbleBehavior(this);
        bodyPartRigid = GetComponent<Rigidbody>();
        enemy.AddObserver(this);

    }
    protected virtual void Update()
    {
        ForceCalulate();

    }
    protected Rigidbody bodyPartRigid;
    public IGunFuNode curAttackerGunFuNode { get => enemy.curAttackerGunFuNode; set => enemy.curAttackerGunFuNode = value; }
    public bool _isGotAttackedAble { get => enemy._isGotAttackedAble; set => enemy._isGotAttackedAble = value ; }
    public bool _isGotExecutedAble { get => enemy._isGotExecutedAble; set => enemy._isGotExecutedAble = value; }
    public IFriendlyFirePreventing.FriendlyFirePreventingMode curFriendlyFireMode { get => enemy.curFriendlyFireMode; set => enemy.curFriendlyFireMode = value; }
    public int allieID { get => enemy.allieID; set => enemy.allieID = value; }
    public FriendlyFirePreventingBehavior friendlyFirePreventingBehavior { get => enemy.friendlyFirePreventingBehavior; set => enemy.friendlyFirePreventingBehavior = value; }
    public IGunFuAble gunFuAbleAttacker { get => enemy.gunFuAbleAttacker; set => enemy.gunFuAbleAttacker = value; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get => enemy._weaponAdvanceUser; set => enemy._weaponAdvanceUser = value; }
    public IDamageAble _damageAble { get => enemy._damageAble; set => enemy._damageAble = value; }

    public bool _triggerGotThrowed { get => enemy._tiggerThrowAbleObjectHit; set { } }


    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {

    }

    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce) => enemy.bulletDamageAbleBodyPartBehavior.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);



    protected virtual void ForceCalulate()
    {
        if (isForceSave == false)
            return;

        MotionControlManager motionControlManager = enemy.motionControlManager;

        if (motionControlManager.curMotionState == motionControlManager.ragdollMotionState)
        {
            bodyPartRigid.AddForceAtPosition(forceSave, hitForcePositionSave, ForceMode.Impulse);

            forceSave = Vector3.zero;
            hitForcePositionSave = Vector3.zero;
            isForceSave = false;
        }
    }

    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attackerPos)
    {

        enemy.TakeGunFuAttacked(gunFu_NodeLeaf, attackerPos);
    }

    public Vector3 velocity { get => bodyPartRigid.linearVelocity; set { } }
    public Vector3 position { get => enemy.transform.position; set => enemy.transform.position = value; }
    public INodeLeaf curNodeLeaf { get => ((IGotGunFuAttackedAble)enemy).curNodeLeaf; set => ((IGotGunFuAttackedAble)enemy).curNodeLeaf = value; }

    public Character _character => enemy;

    //public void GotVisit(IThrowAbleObjectVisitor throwAbleObjectVisitor)
    //{
    //    switch (throwAbleObjectVisitor)
    //    {
    //        case BodyPart bodyPart: 
    //            {
    //                if (bodyPart.enemy == enemy)
    //                {
    //                    return;
    //                }

    //                if(bodyPart.enemy.enemyStateManagerNode.curNodeLeaf is HumanThrowFallDown_GotInteract_NodeLeaf)
    //                {
    //                    isForceSave = true;
    //                    Vector3 hitDir = (this.bodyPartRigid.position - bodyPart.bodyPartRigid.position).normalized;
    //                    float dot = Vector3.Dot(bodyPart.bodyPartRigid.linearVelocity.normalized, hitDir);
    //                    forceSave = bodyPart.bodyPartRigid.linearVelocity.magnitude*dot*hitDir;
    //                    enemy.GotVisit(throwAbleObjectVisitor);
    //                }
    //            }
    //            break;
    //    }
    //}

    public virtual void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IThrowAbleObjectVisitable>(out IThrowAbleObjectVisitable throwAbleObjectVisitable))
        {
            throwAbleObjectVisitable.GotVisit(this);
        }
    }

    public void NotifyPointingAble(IPointerAble pointter) => enemyHPbarDisplay.NotifyPointingAble(pointter);

    public virtual void Notify<T>(Enemy enemy, T node) where T : INode
    {
        
    }
}
