using UnityEngine;
[ExecuteInEditMode]
public class Armored_Protection : BodyPart,IDamageVisitor
{
    [SerializeField] public BodyPart syncBodyPart;
    [SerializeField] public float armorHP;

    [SerializeField] private Armored_ProtectionSCRP armored_ProtectionSCRP;

    [SerializeField] private GameObject meshRendererArmored;

    [SerializeField] private Collider armordCollider;

    public float hpDamage { get; protected set; }
    public float postureDamage { get; protected set; }
    public float staggerDamage { get; protected set; }
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

  
    public override void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        if(damageVisitor is Bullet bullet)
        {
            armorHP -= bullet.GetDestructionDamage;
            hpDamage = bullet.GetHpDamage * (_hpReciverMultiplyRate * syncBodyPart._hpReciverMultiplyRate);
            postureDamage = bullet.GetPostureDamage * (_postureReciverRate * syncBodyPart._postureReciverRate);
            staggerDamage = bullet.GetHpDamage * (_staggerReciverRate * syncBodyPart._staggerReciverRate);

            if (bullet.weapon.userWeapon != null && bullet.weapon.userWeapon is IFriendlyFirePreventing friendly && friendly.IsFriendlyCheck(enemy))
            {
                hpDamage *= 0.35f;
                postureDamage = 0;
                staggerDamage = 0;
            }
        }
       
        if (armorHP <= 0)
            ArmoredDestroyed();

        syncBodyPart.TakeDamageBullet(this, hitPart, hitDir, hitforce);
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
    public override void Notify<T>(Enemy enemy, T node)
    {
        if (node is SubjectEnemy.EnemyEvent enemyEvent 
            && enemyEvent == SubjectEnemy.EnemyEvent.OnEnable)
        {
            SetDefaultAttribute();
        }
        base.Notify(enemy, node);
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

}
