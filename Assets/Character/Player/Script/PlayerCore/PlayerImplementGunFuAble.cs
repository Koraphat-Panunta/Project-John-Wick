using UnityEngine;
using System.Collections.Generic;
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

    public Animator _gunFuAnimator => animator;

    Character IGunFuAble._character => this;

    [SerializeField] public GunFuHitScriptableObject hit1;
    [SerializeField] public GunFuHitScriptableObject hit2;
    [SerializeField] public GunFuHitScriptableObject hit3;
    [SerializeField] public GunFuHitScriptableObject dodgeSpinKick;
    [SerializeField] public GunFuInteraction_ScriptableObject humanShield;
    [SerializeField] public RestrictScriptableObject restrictScriptableObject;
    [SerializeField] public AnimationInteractScriptableObject primaryWeaponDisarmGunFuScriptableObject;
    [SerializeField] public AnimationInteractScriptableObject secondaryWeaponDisarmGunFuScriptableObject;

    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Secondary_Dodge_ScriptableObject_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Secondary_ScriptableObject_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Secondary_ScriptableObject_II;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Secondary_ScriptableObject_III;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Secondary_ScriptableObject_IV;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Primary_ScriptableObject_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Primary_ScriptableObject_II;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFuExecute_Single_Primary_Dodge_ScriptableObject_I;

    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Secondary_Laydown_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Secondary_Layup_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Primary_Laydown_I;
    [SerializeField] public GunFuExecute_Single_ScriptableObject gunFu_Single_Execute_OnGround_Primary_Layup_I;

    public RandomGunFuExecute secondaryExecuteGunFuRandomNumber;
    public RandomGunFuExecute primaryExecuteGunFuRandomNumber;
    public void InitailizedGunFuComponent()
    {

        _weaponUser = this;
        _gunFuUserTransform = RayCastPos;
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(0));
        _layerTarget += LayerMask.GetMask(LayerMask.LayerToName(7));

        _targetAdjustTranform = targetAdjustTranform;
        triggerGunFuBufferTime = 1;

        secondaryExecuteGunFuRandomNumber = new RandomGunFuExecute(4);
        primaryExecuteGunFuRandomNumber = new RandomGunFuExecute(2);
    }
    public void UpdateDetectingTarget()
    {
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

    #region RandomGunFuExecuteFactor
    public class RandomGunFuExecute
    {
        private int maxCount;
        private int seedNumber = 1;

        private int curIndex;

        private int minIndexPlus = 1;
        private int maxIndexPlus = 2;

        public RandomGunFuExecute(int maxGunFuCount)
        {
            this.maxCount = maxGunFuCount;   
            curIndex = Random.Range(seedNumber, maxGunFuCount);
;
        }

        public void UpdateGunFuNumber()
        {
            int lastIndex = curIndex;
            Debug.Log("1 curIndex = " + curIndex);
            curIndex += Random.Range(minIndexPlus, maxIndexPlus+1);

            Debug.Log("2 curIndex = " + curIndex);

            while (curIndex > maxCount)
                curIndex -= maxCount;

            Debug.Log("3 curIndex = " + curIndex);

            if (curIndex == lastIndex)
                curIndex++;

            Debug.Log("4 curIndex = " + curIndex);

        }

        public int GetGunExecuteGuNumber() 
        {
            while (curIndex > maxCount)
                curIndex -= maxCount;

            return curIndex;
        }
    }

    #endregion
}
