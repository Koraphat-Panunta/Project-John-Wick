using UnityEngine;
using System.Collections.Generic;
using static EnemyBodyBulletDamageAbleBehavior;
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public partial class EnemyAnimationManager : MonoBehaviour,IObserverEnemy,IInitializedAble
{
    // Start is called once before the first execution of UpdateNodeAndCheckFindingNode after the MonoBehaviour is created
    public Animator animator;
    public Enemy enemy;

    public AnimationPoseTimeNormalized basedAnimationPoseTimeNormalized;
    public AnimationPoseTimeNormalized upperAnimationPoseTimeNormalized;

    private Vector3 inputVelocity_World;
    private Vector3 inputVelocity_Local;
    private Vector3 curVelocity_Local;
    private Vector3 curVelocity_World;

    public float SholderSide;//-1 -> 1
    public float InputMoveMagnitude_Normalized;
    public float VelocityMoveMagnitude_Normalized;
    public float MoveInputLocalFoward_Normalized;
    public float MoveInputLocalSideWard_Normalized;
    public float MoveVelocityForward_Normalized;
    public float MoveVelocitySideward_Normalized;
    public float DotMoveInputWordl_VelocityWorld_Normalized;
    public float DotVectorLeftwardDir_MoveInputVelocity_Normallized;
    public float Rotating;
    public float AimDownSightWeight;
    public float DotVelocityWorld_Leftward_Normalized;
    public float RecoilWeight;
    public float CrouchWeight {
        get 
        { if (crouchWeightSoftCoverNodeLeaf != null)
                return crouchWeightSoftCoverNodeLeaf.GetCrouchWeight();
        else return 0;
        } 
    }
    public float PainStateHorizontal;
    public float PainStateVertical ;

    public bool isGround;
    public bool isSprint;

    public string AnimationStateName;

    public void Initialized()
    {
        this.basedAnimationPoseTimeNormalized = new AnimationPoseTimeNormalized();
        this.upperAnimationPoseTimeNormalized = new AnimationPoseTimeNormalized();
        
        enemy.AddObserver(this);
        _nodeManagerBehavior = new NodeManagerBehavior();
        _parallelNodeManahger = new List<INodeManager>();

        this.InitailizedNode();
    }
    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (node is IGotGunFuExecuteNodeLeaf gotExecute || node is IGotGunFuAttackNode)
            animator.SetLayerWeight(1, 0);

        if(node is CharacterHitedEventDetail hitedEventDetail)
        {

            Debug.Log("Implement hit Pose dir");

            this.PainStateHorizontal = Quaternion.FromToRotation(this.transform.forward * -1,new Vector3(hitedEventDetail.hitDir.x, this.transform.forward.y, hitedEventDetail.hitDir.z) ).eulerAngles.y;
            Debug.DrawRay(this.transform.position, new Vector3(hitedEventDetail.hitDir.x, this.transform.forward.y, hitedEventDetail.hitDir.z), Color.yellow, 3);
            //Debug.DrawRay(this.transform.position, this.transform.forward, Color.blue, 3);
            this.PainStateVertical = Vector3.Angle(
                this.transform.up
                , hitedEventDetail.hitDir);

        }

        if(node is GotGunFuHitNodeLeaf gunFuHitNodeLeaf
            && gunFuHitNodeLeaf.curGotHitPhase == GotGunFuHitNodeLeaf.GotHitPhase.Enter)
        {
            this.painStateAnimationNodeLeaf.TriggerReset();
        }
    }
  
  
    // UpdateNodeAndCheckFindingNode is called once per frame
    void Update()
    {
        UpdateNode();
        BackBoardUpdate();

    }
    private void FixedUpdate()
    {
        FixedUpdateNode();
    }
    [Range(0,50)]
    [SerializeField] private float lerpingTvelocityAnimation;
    private void BackBoardUpdate()
    {

        MovementCompoent movementComponent = enemy._movementCompoent;

        this.inputVelocity_World = Vector3.Lerp(this.inputVelocity_World,movementComponent.moveInputVelocity_World,Time.deltaTime*lerpingTvelocityAnimation);
        this.inputVelocity_Local = Vector3.Lerp(this.inputVelocity_Local, movementComponent.moveInputVelocity_Local, Time.deltaTime * lerpingTvelocityAnimation);
        this.curVelocity_Local = Vector3.Lerp(this.curVelocity_Local, movementComponent.curMoveVelocity_Local, Time.deltaTime * lerpingTvelocityAnimation);
        this.curVelocity_World = Vector3.Lerp(this.curVelocity_World, movementComponent.curMoveVelocity_World, Time.deltaTime * lerpingTvelocityAnimation);

        this.InputMoveMagnitude_Normalized = this.inputVelocity_World.normalized.magnitude;
        this.MoveInputLocalFoward_Normalized = this.inputVelocity_Local.normalized.z;
        this.MoveInputLocalSideWard_Normalized = this.inputVelocity_Local.normalized.x;


        this.DotMoveInputWordl_VelocityWorld_Normalized = Vector3.Dot(this.curVelocity_World.normalized
            , this.inputVelocity_World.normalized) * (this.curVelocity_World.magnitude / this.inputVelocity_World.magnitude);

        this.DotVectorLeftwardDir_MoveInputVelocity_Normallized = Mathf.Lerp(this.DotVectorLeftwardDir_MoveInputVelocity_Normallized,
                Vector3.Dot(this.inputVelocity_World.normalized,
                Vector3.Cross(enemy.transform.forward, Vector3.up))
            , 10 * Time.deltaTime);


        if (enemy.stateManagerNode.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>())
        {
            this.VelocityMoveMagnitude_Normalized = this.curVelocity_Local.magnitude / enemy.sprintMaxSpeed;
            this.MoveVelocityForward_Normalized = this.curVelocity_Local.z / enemy.sprintMaxSpeed;
            this.MoveVelocitySideward_Normalized = this.curVelocity_Local.x / enemy.sprintMaxSpeed;

            isSprint = true;
        }
        else
        {

            this.VelocityMoveMagnitude_Normalized = this.curVelocity_Local.magnitude / this.enemy.StandMoveMaxSpeed;
            this.MoveVelocityForward_Normalized = this.curVelocity_Local.z / this.enemy.StandMoveMaxSpeed;
            this.MoveVelocitySideward_Normalized = this.curVelocity_Local.x / this.enemy.StandMoveMaxSpeed;

            isSprint = false;
        }

        AimDownSightWeight = (enemy as IWeaponAdvanceUser)._weaponManuverManager.aimingWeight;

        this.DotVelocityWorld_Leftward_Normalized = Vector3.Dot(
            Vector3.Cross(enemy.transform.forward, Vector3.up).normalized
            , curVelocity_World.normalized);

        if (RecoilWeight > 0)
            RecoilWeight = Mathf.Clamp(RecoilWeight - 3 * Time.deltaTime, 0, 1);

        CalculateDeltaRotation();

        animator.SetFloat("SholderSide", SholderSide);
        animator.SetFloat("InputMoveMagnitude_Normalized", InputMoveMagnitude_Normalized);
        animator.SetFloat("VelocityMoveMagnitude_Normalized", VelocityMoveMagnitude_Normalized);
        animator.SetFloat("MoveInputLocalFoward_Normalized", MoveInputLocalFoward_Normalized);
        animator.SetFloat("MoveInputLocalSideWard_Normalized", MoveInputLocalSideWard_Normalized);
        animator.SetFloat("MoveVelocityForward_Normalized", MoveVelocityForward_Normalized);
        animator.SetFloat("MoveVelocitySideward_Normalized", MoveVelocitySideward_Normalized);
        animator.SetFloat("DotMoveInputWordl_VelocityWorld_Normalized", DotMoveInputWordl_VelocityWorld_Normalized);
        animator.SetFloat("Rotating", Rotating);
        animator.SetFloat("AimDownSightWeight", AimDownSightWeight);
        animator.SetFloat("DotVelocityWorld_Leftward_Normalized", DotVelocityWorld_Leftward_Normalized);
        animator.SetFloat("RecoilWeight", RecoilWeight);
        //animator.SetFloat("CAR_Weight", 0);
        animator.SetFloat("DotVectorLeftwardDir_MoveInputVelocity_Normallized", DotVectorLeftwardDir_MoveInputVelocity_Normallized);
        animator.SetFloat("CrouchWeight", CrouchWeight);
        this.animator.SetFloat("PainStateHorizontal",this.PainStateHorizontal);
        this.animator.SetFloat("PainStateVertical",this.PainStateVertical);

        animator.SetFloat("UpperLayerTimeNormalized", this.upperAnimationPoseTimeNormalized.timeNormal);
        this.animator.SetFloat("BasedLayerTimeNormalized", this.basedAnimationPoseTimeNormalized.timeNormal);
    }

    #region CalculateDeltaRotation
    private Vector3 previousDir;
    private void CalculateDeltaRotation()
    {
        Vector3 curDir = enemy.transform.forward;

        Rotating = Mathf.Clamp(Vector3.SignedAngle(previousDir, curDir, Vector3.up) * 10 * Time.deltaTime, -1, 1);
        previousDir = curDir;
    }

   
    #endregion
}
