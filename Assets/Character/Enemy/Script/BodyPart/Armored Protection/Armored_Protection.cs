using UnityEngine;
using UnityEngine.Animations;

public class Armored_Protection : BodyPart,IDamageVisitor
{
    [SerializeField] public BodyPart syncBodyPart;
    [SerializeField] public float armorHP;

    [SerializeField] private Armored_ProtectionSCRP armored_ProtectionSCRP;

    [SerializeField] private SkinnedMeshRenderer skinnedMeshRendererArmored;



    private float originBody_hpReciverMultiplyRate;
    private float originBody_postureReciverRate;
    private float originBody_staggerReciverRate;
    protected override void Awake()
    {
        base.bodyPartDamageRecivedSCRP = armored_ProtectionSCRP;
        armorHP = armored_ProtectionSCRP.armorHP;
        _hpReciverMultiplyRate = armored_ProtectionSCRP._hpReciverMultiplyRate;
        _postureReciverRate = armored_ProtectionSCRP._postureReciverRate;
        _staggerReciverRate = armored_ProtectionSCRP._staggerReciverRate;
    }
    protected override void Update()
    {

    }
    protected override void Start()
    {


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
            armorHP -= bullet._destructionDamage;
        }
        if (armorHP <= 0)
            ArmoredDestroyed();
        syncBodyPart.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
    
    protected virtual void ArmoredDestroyed()
    {
        skinnedMeshRendererArmored.gameObject.SetActive(false);
        Detach();
        Destroy(gameObject);
    }

    public void Attach(BodyPart attachable)
    {
        this.originBody_hpReciverMultiplyRate = attachable._hpReciverMultiplyRate;
        this.originBody_postureReciverRate = attachable._postureReciverRate;
        this.originBody_staggerReciverRate = attachable._staggerReciverRate;

        attachable._hpReciverMultiplyRate = this._hpReciverMultiplyRate * attachable._hpReciverMultiplyRate;
        attachable._postureReciverRate = this._postureReciverRate * attachable._postureReciverRate;
        attachable._staggerReciverRate = this._staggerReciverRate * attachable._staggerReciverRate;

    }

    public void Detach()
    {

        syncBodyPart._hpReciverMultiplyRate = originBody_hpReciverMultiplyRate;
        syncBodyPart._postureReciverRate = originBody_postureReciverRate;
        syncBodyPart._staggerReciverRate = originBody_staggerReciverRate;

    }
  
}
