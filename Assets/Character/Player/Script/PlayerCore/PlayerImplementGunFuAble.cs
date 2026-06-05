using UnityEngine;
using System.Collections.Generic;
public partial class Player : I_OCM_Attack_Able
{
    #region InitailizedGunFu
    public bool _triggerAttack { get; set; }
    public bool _triggerExecute { get; set; }
    public float triggerGunFuBufferTime { get; set; }
    public IRangeWeaponAdvanceUser _weaponUser { get; set; }
    public Vector3 _attackAimDir { get 
        {
            if (this.inputMoveDir_World.magnitude <= 0)
                return this.transform.forward;

            return new Vector3(this.inputMoveDir_World.x, 0, this.inputMoveDir_World.z).normalized;
        } set { } }
    public Transform _gunFuUserTransform { get; set; }

    [SerializeField] Transform targetAdjustTranform;
    public Transform _targetAdjustTranform { get; set; }

    [SerializeField] private OCM_Offendsive_DetectTarget GunFuDetectTarget;
    public OCM_Offendsive_DetectTarget _gunFuDetectTarget { get => this.GunFuDetectTarget; set => this.GunFuDetectTarget = value; }
    public I_Got_OCM_Attacked_Able attackedAbleGunFu { get; set; }
    public I_Got_OCM_Attacked_Able executedAbleGunFu { get; set; }
    public I_OCM_Node curGunFuNode
    {
        get
        {
            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<I_OCM_Node>(out I_OCM_Node gunFuNode))
                return gunFuNode;
            return null;
        }
        set { }
    }

    public Animator _gunFuAnimator => animator;

    Character IMeleeAttackerAble._character => this;
    Transform IMeleeAttackerAble._attackerTransform => this.transform;
    MeleeAttackingPhase IMeleeAttackerAble._curAttackPhase
    {
        get
        {
            if(this.curGunFuNode == null)
                return MeleeAttackingPhase.None;

            if(this.curGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf)
                return MeleeAttackingPhase.Attacking;

            return MeleeAttackingPhase.None;
            
        }
    }

    [SerializeField] public GunFuHitScriptableObject hit1;
    [SerializeField] public GunFuHitScriptableObject hit2;
    [SerializeField] public GunFuHitScriptableObject hit3;
    [SerializeField] public GunFuHitScriptableObject dodgeSpinKick;
    [SerializeField] public AnimationInteractScriptableObject humanShieldSCRP;
    [SerializeField] public AnimationInteractScriptableObject humanShield_Exit_SCRP;
    [SerializeField] public TransformOffsetSCRP humanShieldTargetAdjustTransform;
    [SerializeField] public RestrictScriptableObject restrictScriptableObject;
    [SerializeField] public AnimationInteractScriptableObject ocmKnockDownScripatableObject;
    [SerializeField] public AnimationInteractScriptableObject gunFuReloadScripatableObject;
    [SerializeField] public AnimationInteractScriptableObject gunFuHitDownScriptableObject;

    [SerializeField] public AnimationInteractScriptableObject gunFuExecute_Single_Secondary_Dodge_ScriptableObject_I;
    [SerializeField] public AnimationInteractScriptableObject gunFuExecute_Single_Secondary_ScriptableObject_I;
    [SerializeField] public AnimationInteractScriptableObject gunFuExecute_Single_Primary_ScriptableObject_II;
    [SerializeField] public AnimationInteractScriptableObject gunFuExecute_Single_Primary_Dodge_ScriptableObject_I;

    [SerializeField] public AnimationInteractScriptableObject gunFu_Single_Execute_OnGround;
    [SerializeField] public AnimationInteractScriptableObject parryPrimaryWeaponSCRP;
    [SerializeField] public AnimationInteractScriptableObject parrySecondaryWeaponSCRP;
    [SerializeField] public AnimationInteractScriptableObject meleeExecuteSCRP;

    public RandomGunFuExecute secondaryExecuteGunFuRandomNumber;
    public RandomGunFuExecute primaryExecuteGunFuRandomNumber;
    public void InitailizedGunFuComponent()
    {

        _weaponUser = this;
        _gunFuUserTransform = RayCastPos;

        _targetAdjustTranform = targetAdjustTranform;
        triggerGunFuBufferTime = 1;

        secondaryExecuteGunFuRandomNumber = new RandomGunFuExecute(3);
        primaryExecuteGunFuRandomNumber = new RandomGunFuExecute(2);
    }
    public void UpdateDetectingTarget()
    {
        if (_gunFuDetectTarget.CastDetectExecuteAbleTarget(out I_Got_OCM_Attacked_Able excecuteTarget))
            executedAbleGunFu = excecuteTarget;
        else
            executedAbleGunFu = null;

        if (_gunFuDetectTarget.CastDetect(out I_Got_OCM_Attacked_Able target))
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
            //Debug.Log("1 curIndex = " + curIndex);
            curIndex += Random.Range(minIndexPlus, maxIndexPlus+1);

            //Debug.Log("2 curIndex = " + curIndex);

            while (curIndex > maxCount)
                curIndex -= maxCount;

            //Debug.Log("3 curIndex = " + curIndex);

            if (curIndex == lastIndex)
                curIndex++;

            //Debug.Log("4 curIndex = " + curIndex);

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
