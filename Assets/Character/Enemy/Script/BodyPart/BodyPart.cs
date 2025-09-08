using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour, IBulletDamageAble, IGotGunFuAttackedAble, IFriendlyFirePreventing, 
     IObserverEnemy,IGotPointingAble
{
    [SerializeField] public Enemy enemy;
    [SerializeField] private EnemyHPbarDisplay enemyHPbarDisplay;
    public virtual float _hpReciverMultiplyRate { get; set; }
    public virtual float _postureReciverRate { get; set; }
    public virtual float _staggerReciverRate { get; set; }

    [SerializeField] protected BodyPartDamageRecivedSCRP bodyPartDamageRecivedSCRP;
    public bool _triggerHitedGunFu { get; set; }

    public Vector3 forceSave;
    public Vector3 hitForcePositionSave;

    public bool isForceSave;

    protected virtual void Awake()
    {
        _hpReciverMultiplyRate = bodyPartDamageRecivedSCRP._hpReciverMultiplyRate;
        _postureReciverRate = bodyPartDamageRecivedSCRP._postureReciverRate;
        _staggerReciverRate = bodyPartDamageRecivedSCRP._staggerReciverRate;
    }
    protected virtual void Start()
    {
        
        enemy.bulletDamageAbleBodyPartBehavior = new EnemyBodyBulletDamageAbleBehavior(this);
        bodyPartRigid = GetComponent<Rigidbody>();
        enemy.AddObserver(this);

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
    public Character _character => enemy;


    public virtual void StackingForce(Vector3 forceDir,Vector3 forcePos)
    {
        isForceSave = true;
        this.forceSave = forceDir;
        this.hitForcePositionSave = forcePos;
    }
    private void ForceCalulate()
    {
        if (isForceSave == false)
            return;

        MotionControlManager motionControlManager = enemy.motionControlManager;

        if (motionControlManager.curMotionState == motionControlManager.ragdollMotionState)
        {
            Debug.Log("AddForce");
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
    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {
        switch (damageVisitor)
        {
            case Bullet bulletObj:
                {
                    float damage = bulletObj._hpDamage * _hpReciverMultiplyRate;
                    float postureDamaged = bulletObj._postureDamage * _postureReciverRate;
                    float staggerDamaged = bulletObj._postureDamage * _staggerReciverRate;

                    if (bulletObj.weapon.userWeapon != null && bulletObj.weapon.userWeapon is IFriendlyFirePreventing friendly && friendly.IsFriendlyCheck(enemy))
                    {
                        damage *= 0.35f;
                        postureDamaged = 0;
                        staggerDamaged = 0;
                    }

                    enemy._isPainTrigger = true;

                    if (enemy._posture > 0)
                        enemy._posture -= postureDamaged;
                    if (enemy.staggerGauge > 0)
                        enemy.staggerGauge -= staggerDamaged;

                    enemy.TakeDamage(damage);
                    enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GotBulletHit);
                    break;
                }
            case Armored_Protection armored_Protection:
                {
                    float damage = armored_Protection.hpDamage;
                    float postureDamaged = armored_Protection.postureDamage;
                    float staggerDamaged = armored_Protection.staggerDamage;

                    enemy._isPainTrigger = true;

                    if (enemy._posture > 0)
                        enemy._posture -= postureDamaged;
                    if (enemy.staggerGauge > 0)
                        enemy.staggerGauge -= staggerDamaged;

                    enemy.TakeDamage(damage);
                    enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GotBulletHit);
                    break;
                }
        }

       
    }

    public virtual void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce) => enemy.bulletDamageAbleBodyPartBehavior.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);

    public virtual void Notify<T>(Enemy enemy, T node) 
    {
        this.ForceCalulate();
    }
    public void NotifyPointingAble(IPointerAble pointter) => enemyHPbarDisplay.NotifyPointingAble(pointter);

}
