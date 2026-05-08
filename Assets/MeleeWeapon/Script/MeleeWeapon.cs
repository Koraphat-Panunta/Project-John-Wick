using UnityEngine;

public class MeleeWeapon : MonoBehaviour
    ,IObjectGrabbedAble
    ,IHPDamageVisitor
    ,IInitializedAble
{
    [SerializeField] MeleeWeaponScriptableObject meleeWeaponScriptableObject;
    [SerializeField] private Transform gripSocketTransform;
    [SerializeField] private MountComponent mountComponent;
    [SerializeField] private Rigidbody grabAbleRigidbody;
    [SerializeField] private Collider grabAbleCollider;


    private float hpDamage;
    public float _hPDamage => this.hpDamage;

    public Transform _grabAbleTransform => this.transform;

    public Transform _defaultGrabPoint => this.gripSocketTransform;

    public MountComponent _mountComponent => this.mountComponent;

    public Rigidbody _grabAbleRigidbody => this.grabAbleRigidbody;

    public Collider _grabAbleCollider => this.grabAbleCollider;

    private IGrabMeleeWeaponAble currentGrabbedAt ;
    public IGrabAbleObject _currentGrabbedAt => this.currentGrabbedAt;
    public IMeleeWeaponUserAble _meleeWeaponUserAble => this.currentGrabbedAt._meleeWeaponUser;
    public MeleeAttackingPhase attackingPhase 
    { 
        get 
        { 
            if(this._meleeWeaponUserAble == null)
                return MeleeAttackingPhase.None;
                
            return this._meleeWeaponUserAble._curMeleeAttackPhase;
        } 
    }

    public void Initialized()
    {
        this.hpDamage = this.meleeWeaponScriptableObject.damage;
    }

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        this._meleeWeaponUserAble.OnNotifyMeleeAttack<IDamageAble>(damageAble);
    }
    public void SetCurrentGrabbedAt(IGrabAbleObject socket)
    {
        this.currentGrabbedAt = socket as IGrabMeleeWeaponAble;

        if (this.currentGrabbedAt != null)
        {
            this.mountComponent.Attach(this.currentGrabbedAt._grabSocketTransform,IGrabMeleeWeaponAble.grabDuration);
            this._grabAbleRigidbody.isKinematic = true;
            this.grabAbleCollider.isTrigger = true;
        }
        else
        {
            this._mountComponent.Detach();
            this._grabAbleRigidbody.isKinematic = false;
            this.grabAbleCollider.isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(this.attackingPhase != MeleeAttackingPhase.Attacking)
            return;

        if (other.transform.gameObject.TryGetComponent<IDamageAble>(out IDamageAble damageAble) == false)
            return;
        
        if(other.transform.gameObject.TryGetComponent<IMeleeWeaponUserAble>(out IMeleeWeaponUserAble meleeWeaponUserAble)
            && meleeWeaponUserAble == this._meleeWeaponUserAble)
            return;

        damageAble.TakeDamage(this);
    }
}
