using UnityEngine;
[ExecuteInEditMode]
public class Armored_Protection : BodyPart
{
    [SerializeField] public BodyPart syncBodyPart;
    [SerializeField] public float armorHP;

    [SerializeField] private Armored_ProtectionSCRP armored_ProtectionSCRP;

    [SerializeField] private GameObject meshRendererArmored;

    [SerializeField] private Collider armordCollider;

    public override float penatrateResistance { get => armored_ProtectionSCRP._penetrateResistRate; set { } }

    public override void Initialized()
    {
        base.bodyPartDamageRecivedSCRP = armored_ProtectionSCRP;
        armorHP = armored_ProtectionSCRP.armorHP;
        _hpReciverMultiplyRate = armored_ProtectionSCRP._hpReciverMultiplyRate;
        _postureReciverRate = armored_ProtectionSCRP._postureReciverRate;
        _staggerReciverRate = armored_ProtectionSCRP._staggerReciverRate;

        enemy.AddObserver(this);
        if (syncBodyPart != null)
        {
            meshRendererArmored.gameObject.SetActive(true);
            this.Attach(syncBodyPart);
        }
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
        meshRendererArmored.gameObject.SetActive(false);
        armordCollider.enabled = false;
        Detach();
    }

    public void Attach(BodyPart attachable)
    {


    }

    public void Detach()
    {

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
  
    private void OnValidate()
    {
        if (gameObject.activeSelf)
        {
            if (meshRendererArmored != null)
                meshRendererArmored.gameObject.SetActive(true);
        }
        else
        {
            if (meshRendererArmored != null)
                meshRendererArmored.gameObject.SetActive(false);
        }

    }
    private void OnDisable()
    {
        if(meshRendererArmored != null)
            meshRendererArmored.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (meshRendererArmored != null)
            meshRendererArmored.gameObject.SetActive(true);

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
        meshRendererArmored.gameObject.SetActive(true);
    }

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        
    }
}

public class VirtualBullet : Bullet
{
   

    public VirtualBullet(
        Bullet bullet
        
        ,RangeWeapon weapon) : base(weapon)
    {
        
    }

    public override float _hPDamage { get ; set ; }
    public override float _postureDamageVisitor { get ; set ; }
    public override float _pureDestructionDamage { get; set; }
    public override BulletType myType { get ; protected set ; }
}
