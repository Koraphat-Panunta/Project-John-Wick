using UnityEngine;
using UnityEngine.Animations;
using static SubjectEnemy;
[ExecuteInEditMode]
public class Armored_Protection : BodyPart,IDamageVisitor
{
    [SerializeField] public BodyPart syncBodyPart;
    [SerializeField] public float armorHP;

    [SerializeField] private Armored_ProtectionSCRP armored_ProtectionSCRP;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRendererArmored;

    [SerializeField] private Collider collider;

    public float hpDamage { get; protected set; }
    public float postureDamage { get; protected set; }
    public float staggerDamage { get; protected set; }
    public override float penatrateResistance { get => armored_ProtectionSCRP._penetrateResistRate; set { } }


    protected override void Awake()
    {
        base.bodyPartDamageRecivedSCRP = armored_ProtectionSCRP;
        armorHP = armored_ProtectionSCRP.armorHP;
        _hpReciverMultiplyRate = armored_ProtectionSCRP._hpReciverMultiplyRate;
        _postureReciverRate = armored_ProtectionSCRP._postureReciverRate;
        _staggerReciverRate = armored_ProtectionSCRP._staggerReciverRate;
    }
   
    protected override void Start()
    {
        enemy.AddObserver(this);
        if (syncBodyPart != null)
        {
            skinnedMeshRendererArmored.gameObject.SetActive(true);
            this.Attach(syncBodyPart);
        }
    }
  
    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
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

        syncBodyPart.TakeDamage(this, hitPart, hitDir, hitforce);
    }
    
    protected virtual void ArmoredDestroyed()
    {
        skinnedMeshRendererArmored.gameObject.SetActive(false);
        collider.enabled = false;
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
            if (skinnedMeshRendererArmored != null)
                skinnedMeshRendererArmored.gameObject.SetActive(true);
        }
        else
        {
            if (skinnedMeshRendererArmored != null)
                skinnedMeshRendererArmored.gameObject.SetActive(false);
        }

    }
    private void OnDisable()
    {
        if(skinnedMeshRendererArmored != null)
            skinnedMeshRendererArmored.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (skinnedMeshRendererArmored != null)
            skinnedMeshRendererArmored.gameObject.SetActive(true);

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
        collider.enabled = true;
        skinnedMeshRendererArmored.gameObject.SetActive(true);
    }

}
