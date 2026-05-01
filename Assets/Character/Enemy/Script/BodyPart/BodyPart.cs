using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;
using static SubjectEnemy;

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
    protected float forceSaveBufferTimeDuration = .15f;
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
        if (this.forceStay != null)
        {
            this.enemy.StopCoroutine(this.forceStay);
            this.forceStay = null;
        }

        this.forceStay = this.enemy.StartCoroutine(this.ForceStay());
        
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
            Debug.Log("Implement Force Save");
            bodyPartRigid.AddForceAtPosition(forceSave, hitForcePositionSave, ForceMode.Impulse);
            forceSave = Vector3.zero;
            hitForcePositionSave = Vector3.zero;
            isForceSave = false;
        }
    }

   
    public virtual void TakeDamage(IDamageVisitor damageVisitor)
    {
        this.enemy._isPainTrigger = true;

        if (this.enemy.NotifyGotAttack != null)
            this.enemy.NotifyGotAttack.Invoke(damageVisitor);

        switch (damageVisitor)
        {
            case Bullet bulletObj:
                {
                    float damage = bulletObj.GetHpDamage * this._hpReciverMultiplyRate;
                    float postureDamaged = bulletObj.GetPostureDamage * this._postureReciverRate;

                    if (bulletObj.weapon.userWeapon != null && bulletObj.weapon.userWeapon is IFriendlyFirePreventing friendly && friendly.IsFriendlyCheck(enemy))
                    {
                        damage *= 0.025f;
                        postureDamaged = 0;
                    }

                    bulletObj.weapon.userWeapon._weaponAfterAction.SendFeedBackWeaponAfterAction
                       <IBulletDamageAble>(WeaponAfterAction.WeaponAfterActionSending.HitConfirm, this);
                    this.enemy.TakePostureDamaged(postureDamaged);
                    this.enemy.TakeDamage(damage);
                    this.enemy.NotifyObserver(this.enemy, SubjectEnemy.EnemyEvent.GotBulletHit);

                    Debug.Log("Bullet OnNotifyFeedBackVisitor 1");
                    damageVisitor.OnNotifyFeedBackVisitor(this.enemy);
                    Debug.Log("Bullet OnNotifyFeedBackVisitor 2");

                    return;
                }
            case Armored_Protection armored_Protection:
                {
                    float damage = armored_Protection.hpDamage;
                    float postureDamaged = armored_Protection.postureDamage;

                    this.enemy.TakePostureDamaged(postureDamaged);
                    this.enemy.TakeDamage(damage);
                    this.enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GotBulletHit);
                    return;
                }

        }

        if(damageVisitor is IThrowAbleObject throwAbleObject)
        {
            this.enemy.NotifyObserver<CharacterHitedEventDetail>(this.enemy
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

  
        }

        if(damageVisitor is IHPDamageVisitor hPDamageVisitor)
            this.enemy.TakeDamage(hPDamageVisitor._hPDamage);

        if (damageVisitor is IPostureDamageVisitor postureDamageVisitor)
            this.enemy.TakePostureDamaged(postureDamageVisitor._postureDamageVisitor);

        damageVisitor.OnNotifyFeedBackVisitor(this.enemy);

    }

    public void SetBodyPartDamageRecivedSCRP(BodyPartDamageRecivedSCRP bodyPartDamageRecivedSCRP) => this.bodyPartDamageRecivedSCRP = bodyPartDamageRecivedSCRP;

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
    public IGotGunFuAttackNode gotGunFuAttackNode => this.enemy.gotGunFuAttackNode;

    public Character _character => this.enemy;

    public bool _isGotAttackedAble { get => this.enemy._isGotAttackedAble; set { } }
    public bool _isGotExecutedAble { get => this.enemy._isGotExecutedAble; set { } }
    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble attackerPos)
    {
        this.enemy.TakeGunFuAttacked(gunFu_NodeLeaf, attackerPos);
        
    }
    #endregion

    public virtual float penatrateResistance { get => bodyPartDamageRecivedSCRP._penetrateResistRate; set { } }

    public Vector3 _beenThrowObjectAtPosition { get => this.enemy.humanoidBone._headBone.position; set { } }

    public virtual void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce) => enemy.bulletDamageAbleBodyPartBehavior.TakeDamageBullet(damageVisitor, hitPart, hitDir, hitforce);

    public virtual void OnNotify<T>(Enemy enemy, T node) 
    {
        this.ForceCalulate();
    }

    private Coroutine forceStay;

    public void SetCharacterBodyOwner(Enemy character)
    {
        this.enemy = character;
    }
    public IEnumerator ForceStay()
    {
        yield return new WaitForSeconds(this.forceSaveBufferTimeDuration);
        this.isForceSave = false;
        this.forceStay = null;
    }

  
}
