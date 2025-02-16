using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IBulletDamageAble,IGunFuGotAttackedAble,IFriendlyFirePreventing
{
    [SerializeField] public Enemy enemy;
    public abstract float hpReciverRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get => enemy._gunFuHitedAble; set { } }
    public Vector3 attackedPos {get;set; }

    public Vector3 forceSave;
    public Vector3 hitForcePositionSave;

    public bool isForceSave;

    protected virtual void Start()
    {
        enemy.bulletDamageAbleBodyPartBehavior = new EnemyBodyBulletDamageAbleBehavior(this);
        bodyPartRigid = GetComponent<Rigidbody>();

    }
    protected virtual void Update()
    {
        ForceCalulate();

    }
    protected Rigidbody bodyPartRigid;
    public IGunFuNode curGotAttackedGunFuNode { get => enemy.curGotAttackedGunFuNode; set => enemy.curGotAttackedGunFuNode = value; }
    public IFriendlyFirePreventing.FriendlyFirePreventingMode curFriendlyFireMode { get => enemy.curFriendlyFireMode; set => enemy.curFriendlyFireMode = value; }
    public int allieID { get => enemy.allieID; set => enemy.allieID = value; }
    public FriendlyFirePreventingBehavior friendlyFirePreventingBehavior { get => enemy.friendlyFirePreventingBehavior; set => enemy.friendlyFirePreventingBehavior = value; }
    public IGunFuAble gunFuAbleAttacker { get => enemy.gunFuAbleAttacker; set => enemy.gunFuAbleAttacker = value; }
    public bool _isDead { get => enemy.isDead; set { } }

    public bool _triggerGotThrowed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {

    }

    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce) => enemy.bulletDamageAbleBodyPartBehavior.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);



    private void ForceCalulate()
    {
        if (isForceSave == false)
            return;

        MotionControlManager motionControlManager = enemy.motionControlManager;

        if (motionControlManager.curMotionState == motionControlManager.ragdollMotionState)
        {
            Debug.Log("Force Implement = " + forceSave + " "+hitForcePositionSave);
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

   

    protected void HitsensingTarget(Vector3 hitPart)
    {
        if (enemy.fieldOfView.FindSingleObjectInView(enemy.targetMask, (new Vector3(hitPart.x,0,hitPart.z) - enemy.transform.position).normalized, 120, out GameObject targetObj))
        {
            enemy.targetKnewPos = targetObj.transform.position;
        }
    }

   
}
