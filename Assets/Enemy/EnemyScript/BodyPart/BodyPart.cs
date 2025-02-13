using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour,IBulletDamageAble,IGunFuGotAttackedAble
{
    [SerializeField] public Enemy enemy;
    public abstract float hpReciverRate { get; set; }
    public abstract float postureReciverRate { get; set; }
    public bool _triggerHitedGunFu { get; set; }
    public Transform _gunFuHitedAble { get => enemy._gunFuHitedAble; set { } }
    public Vector3 attackedPos {get;set; }

    private Vector3 forceSave;
    private Vector3 hitForcePositionSave;

    private bool isForceSave;
    protected virtual void Start()
    {
        bodyPartRigid = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        ForceCalulate();

    }
    protected Rigidbody bodyPartRigid;
    public HumandShield_GotInteract_NodeLeaf _humandShield_GotInteract_NodeLeaf { get => enemy._humandShield_GotInteract_NodeLeaf; set => enemy._humandShield_GotInteract_NodeLeaf = value; }
    public IGunFuNode curGotAttackedGunFuNode { get => enemy.curGotAttackedGunFuNode; set => enemy.curGotAttackedGunFuNode = value; }

    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {

    }
    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        forceSave = hitDir * hitforce;
        hitForcePositionSave = hitPart;
        isForceSave = true;
    }

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
