using UnityEngine;

public partial class Player : IGunFuAble
{
    #region InitailizedGunFu
    public bool _triggerGunFu { get; set; }
    public bool _triggerExecuteGunFu { get; set; }
    public float triggerGunFuBufferTime { get; set; }
    public IWeaponAdvanceUser _weaponUser { get; set; }
    public Vector3 _gunFuAimDir { get; set; }
    public Transform _gunFuUserTransform { get; set; }
    public LayerMask _layerTarget { get; set; }
    [SerializeField] Transform targetAdjustTranform;
    public Transform _targetAdjustTranform { get; set; }

    [SerializeField] private GunFuDetectTarget GunFuDetectTarget;
    public GunFuDetectTarget _gunFuDetectTarget { get => this.GunFuDetectTarget; set => this.GunFuDetectTarget = value; }
    public IGotGunFuAttackedAble attackedAbleGunFu { get; set; }
    public IGotGunFuAttackedAble executedAbleGunFu { get; set; }
    public IGunFuNode curGunFuNode
    {
        get
        {
            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuNode>(out IGunFuNode gunFuNode))
                return gunFuNode;
            return null;
        }
        set { }
    }
    public StackGague gunFuExecuteStackGauge { get; set; }

    public Animator _gunFuAnimator => animator;

    Character IGunFuAble._character => this;

    [SerializeField] public GunFuHitScriptableObject hit1;
    [SerializeField] public GunFuHitScriptableObject hit2;
    [SerializeField] public GunFuHitScriptableObject hit3;
    [SerializeField] public GunFuHitScriptableObject dodgeSpinKick;
    [SerializeField] public GunFuInteraction_ScriptableObject humanShield;
    [SerializeField] public GunFuInteraction_ScriptableObject humanThrow;
    [SerializeField] public RestrictScriptableObject restrictScriptableObject;
    [SerializeField] public WeaponDisarmGunFuScriptableObject primaryWeaponDisarmGunFuScriptableObject;
    [SerializeField] public WeaponDisarmGunFuScriptableObject secondaryWeaponDisarmGunFuScriptableObject;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Pistol_Laydown_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Pistol_Layup_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Primary_Laydown_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Primary_Layup_I;

    public void InitailizedGunFuComponent()
    {
        gunFuExecuteStackGauge = new PlayerGunFuExecuteStackGauge(this, 4, 0);

        _weaponUser = this;
        _gunFuUserTransform = RayCastPos;
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(0));
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(7));

        _targetAdjustTranform = targetAdjustTranform;
        triggerGunFuBufferTime = 1;
    }
    public void UpdateDetectingTarget()
    {

        if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuNode>(out IGunFuNode gunFuNode))
            return;

        if (_gunFuDetectTarget.CastDetectExecuteAbleTarget(out IGotGunFuAttackedAble excecuteTarget))
            executedAbleGunFu = excecuteTarget;
        else
            executedAbleGunFu = null;

        if (_gunFuDetectTarget.CastDetect(out IGotGunFuAttackedAble target))
            attackedAbleGunFu = target;
        else
            attackedAbleGunFu = null;
    }
    #endregion
}
