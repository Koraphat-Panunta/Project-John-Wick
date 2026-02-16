using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;

public abstract class BodyPart : MonoBehaviour
    , IBulletDamageAble
    , IFriendlyFirePreventing
    , IObserverEnemy
    , IInitializedAble
    , IBeenThrewObjectAt
    , IGotGunFuAttackedAble

{
    [SerializeField] public Enemy enemy;
    public virtual float _hpReciverMultiplyRate { get; set; }
    public virtual float _postureReciverRate { get; set; }
    public virtual float _staggerReciverRate { get; set; }

    [SerializeField] protected BodyPartDamageRecivedSCRP bodyPartDamageRecivedSCRP;

    public Vector3 forceSave;
    public Vector3 hitForcePositionSave;

    public bool isForceSave;
    public virtual void Initialized()
    {
        _hpReciverMultiplyRate = bodyPartDamageRecivedSCRP._hpReciverMultiplyRate;
        _postureReciverRate = bodyPartDamageRecivedSCRP._postureReciverRate;
        _staggerReciverRate = bodyPartDamageRecivedSCRP._staggerReciverRate;

        enemy.bulletDamageAbleBodyPartBehavior = new EnemyBodyBulletDamageAbleBehavior(this);
        bodyPartRigid = GetComponent<Rigidbody>();
        enemy.AddObserver(this);
    }
    
    protected Rigidbody bodyPartRigid;
   
    public IFriendlyFirePreventing.FriendlyFirePreventingMode curFriendlyFireMode { get => enemy.curFriendlyFireMode; set => enemy.curFriendlyFireMode = value; }
    public int allieID { get => enemy.allieID; set => enemy.allieID = value; }
    public FriendlyFirePreventingBehavior friendlyFirePreventingBehavior { get => enemy.friendlyFirePreventingBehavior; set => enemy.friendlyFirePreventingBehavior = value; }



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
            bodyPartRigid.AddForceAtPosition(forceSave, hitForcePositionSave, ForceMode.Impulse);
            forceSave = Vector3.zero;
            hitForcePositionSave = Vector3.zero;
            isForceSave = false;
        }
    }

   
    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {

        switch (damageVisitor)
        {
            case Bullet bulletObj:
                {
                    float damage = bulletObj.GetHpDamage * _hpReciverMultiplyRate;
                    float postureDamaged = bulletObj.GetPostureDamage * _postureReciverRate;
                    float staggerDamaged = bulletObj.GetPostureDamage * _staggerReciverRate;

                    if (bulletObj.weapon.userWeapon != null && bulletObj.weapon.userWeapon is IFriendlyFirePreventing friendly && friendly.IsFriendlyCheck(enemy))
                    {
                        damage *= 0.025f;
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
            case IThrowAbleObject throwAbleObject: 
                {

                    if (enemy._posture > 0)
                        enemy._posture -= 40;

                    enemy._isPainTrigger = true;

                    enemy.NotifyObserver<CharacterHitedEventDetail>(this.enemy
                        , new CharacterHitedEventDetail
                        {
                            hitedPart = this
                            ,
                            hitDir = (this.bodyPartRigid.transform.position - throwAbleObject._throwAbleObjectTransform.position).normalized
                            ,
                            hitforce = throwAbleObject._throwVelocity
                            ,
                            hitPos = throwAbleObject._throwAbleObjectTransform.position
                        });
                    break;
                }

        }

       
    }

    #region ImplementIGotGunFuAttackedAble
    public bool _triggerHitedGunFu
    {
        get => this.enemy._triggerHitedGunFu;
        set => this.enemy._triggerHitedGunFu = value;
    }

    public IGunFuNode curAttackerGunFuNode
    {
        get => this.enemy.curAttackerGunFuNode;
        set => this.enemy.curAttackerGunFuNode = value;
    }
    public IGunFuAble gunFuAbleAttacker
    {
        get => this.enemy.gunFuAbleAttacker;
        set => this.enemy.gunFuAbleAttacker = value;
    }
    public IWeaponAdvanceUser _weaponAdvanceUser
    {
        get => this.enemy._weaponAdvanceUser;
        set => this.enemy._weaponAdvanceUser = value;
    }
    public IGotGunFuAttackedAble gotGunFuAttackedAble
    {
        get => this.enemy;
        set { }
    }
    public IDamageAble _damageAble
    {
        get => this.enemy;
        set { }
    }

    public Character _character => this.enemy;

    public bool _isGotAttackedAble { get => this.enemy._isGotAttackedAble; set { } }
    public bool _isGotExecutedAble { get => this.enemy._isGotExecutedAble; set { } }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attackerPos)
    {

        this.enemy.TakeGunFuAttacked(gunFu_NodeLeaf, attackerPos);
        
    }
    #endregion

    public virtual float penatrateResistance { get => bodyPartDamageRecivedSCRP._penetrateResistRate; set { } }

    public Vector3 _beenThrowObjectAtPosition { get => enemy.head.transform.position; set { } }

    public virtual void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce) => enemy.bulletDamageAbleBodyPartBehavior.TakeDamageBullet(damageVisitor, hitPart, hitDir, hitforce);

    public virtual void OnNotify<T>(Enemy enemy, T node) 
    {
        this.ForceCalulate();
    }

  
}
