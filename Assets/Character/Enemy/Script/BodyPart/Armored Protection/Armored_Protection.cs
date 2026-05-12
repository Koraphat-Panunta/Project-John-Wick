using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class Armored_Protection : BodyPart
{
    [SerializeField] public BodyPart syncBodyPart;
    [SerializeField] public float armorHP;

    [SerializeField] private Armored_ProtectionSCRP armored_ProtectionSCRP;

    [SerializeField] protected Renderer armoredSkinMeshRenderer;

    [SerializeField] private Collider armordCollider;

    [SerializeField] private MountComponent mountComponent;

    public override float penatrateResistance { get => armored_ProtectionSCRP._penetrateResistRate; set { } }

    public bool isResizeBaseOnBody;

    public override void Initialized()
    {
        base.bodyPartDamageRecivedSCRP = armored_ProtectionSCRP;
        armorHP = armored_ProtectionSCRP.armorHP;
        _hpReciverMultiplyRate = armored_ProtectionSCRP._hpReciverMultiplyRate;
        _postureReciverRate = armored_ProtectionSCRP._postureReciverRate;
        _staggerReciverRate = armored_ProtectionSCRP._staggerReciverRate;

        enemy.AddObserver(this);
    }

    

    public override void TakeDamageBullet(Bullet damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        if(damageVisitor is Bullet bullet)
        {
            armorHP -= bullet.GetDestructionDamage;
        }
       
        if (armorHP <= 0)
            ArmoredDestroyed();

        VirtualBullet bulletDeformeAttribute = new VirtualBullet(damageVisitor, damageVisitor.weapon)
        {
            _hPDamage = damageVisitor.GetHpDamage * (_hpReciverMultiplyRate * syncBodyPart._hpReciverMultiplyRate),
            _postureDamageVisitor = damageVisitor.GetPostureDamage * (_postureReciverRate * syncBodyPart._postureReciverRate)
        };

        //Friendly Fire
        if (damageVisitor.weapon.userWeapon != null && damageVisitor.weapon.userWeapon is IFriendlyFirePreventing friendly && friendly.IsFriendlyCheck(enemy))
        {
            bulletDeformeAttribute._hPDamage *= 0.35f;
            bulletDeformeAttribute._postureDamageVisitor = 0;
        }

        syncBodyPart.TakeDamageBullet(bulletDeformeAttribute, hitPart, hitDir, hitforce);
    }
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        base.TakeDamage(damageVisitor);

    }
    protected virtual void ArmoredDestroyed()
    {
        armoredSkinMeshRenderer.gameObject.SetActive(false);
        armordCollider.enabled = false;
    }

    public virtual void Attach(BodyPart bodyPart)
    {
        this.enemy = bodyPart.enemy;
        this.syncBodyPart = bodyPart;

        this.mountComponent.Attach(bodyPart.transform,this.armored_ProtectionSCRP.attachOffsetPosition,Quaternion.identity);
        this.mountComponent.SetAttachRate(1);
        this.mountComponent.UpdatePosition();

        SaveEditorChanged.SaveEditorChangedObject(this.mountComponent);

        if(this.isResizeBaseOnBody == false)
            return;

        if (bodyPart.TryGetComponent<Collider>(out Collider collider))
        {
            switch (collider)
            {
                case BoxCollider boxCollider:
                    {
                        BoxCollider armoredBoxCollider = this.armordCollider as BoxCollider;

                        armoredBoxCollider.center = boxCollider.center;
                        armoredBoxCollider.size = new Vector3(
                            boxCollider.size.x + .025f
                            , boxCollider.size.y + .025f
                            , boxCollider.size.z + .025f
                            );
                    }
                    break;
                case CapsuleCollider capsuleCollider:
                    {
                        CapsuleCollider armoredCapsueCollider = this.armordCollider as CapsuleCollider;

                        armoredCapsueCollider.center = capsuleCollider.center;
                        armoredCapsueCollider.height = capsuleCollider.height + .025f;
                        armoredCapsueCollider.radius = capsuleCollider.radius + .025f;
                    }
                    break;
            }
        }
    }
   
    public virtual void Detach()
    {
        this.syncBodyPart = null;
    }
    public override void OnNotify<T>(Enemy enemy, T node)
    {
        if (node is SubjectEnemy.EnemyEvent enemyEvent 
            && enemyEvent == SubjectEnemy.EnemyEvent.OnEnable)
        {
            SetDefaultAttribute();
        }
        base.OnNotify(enemy, node);
    }
  
   
    private void OnDisable()
    {
        if(armoredSkinMeshRenderer != null)
            armoredSkinMeshRenderer.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (armoredSkinMeshRenderer != null)
            armoredSkinMeshRenderer.gameObject.SetActive(true);

        base.bodyPartDamageRecivedSCRP = armored_ProtectionSCRP;

        armorHP = armored_ProtectionSCRP.armorHP;
        _hpReciverMultiplyRate = armored_ProtectionSCRP._hpReciverMultiplyRate;
        _postureReciverRate = armored_ProtectionSCRP._postureReciverRate;
        _staggerReciverRate = armored_ProtectionSCRP._staggerReciverRate;
    }
    private void SetDefaultAttribute()
    {
        armorHP = armored_ProtectionSCRP.armorHP;
        _hpReciverMultiplyRate = armored_ProtectionSCRP._hpReciverMultiplyRate;
        _postureReciverRate = armored_ProtectionSCRP._postureReciverRate;
        _staggerReciverRate = armored_ProtectionSCRP._staggerReciverRate;

        armordCollider.enabled = true;
        armoredSkinMeshRenderer.gameObject.SetActive(true);
    }

   
}

public class VirtualBullet : Bullet
{

    public Bullet bullet;
    public VirtualBullet(
        Bullet bullet
        ,RangeWeapon weapon) : base(weapon)
    {
        this.bullet = bullet;
    }

    public override float _hPDamage { get ; set ; }
    public override float _postureDamageVisitor { get ; set ; }
    public override float _pureDestructionDamage { get; set; }
    public override BulletType myType { get ; protected set ; }
}
