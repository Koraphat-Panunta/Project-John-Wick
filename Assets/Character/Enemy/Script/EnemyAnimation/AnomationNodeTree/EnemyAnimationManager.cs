using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public partial class EnemyAnimationManager : MonoBehaviour,IObserverEnemy,IInitializedAble
{
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    public Animator animator;
    public Enemy enemy;

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

    public bool isGround;
    public bool isSprint;

    public string AnimationStateName;

    public void Initialized()
    {
        enemy.AddObserver(this);
        nodeManagerBehavior = new NodeManagerBehavior();
        parallelNodeManahger = new List<INodeManager>();

        this.InitailizedNode();
    }
    public void Notify<T>(Enemy enemy, T node)
    {
       
    }
  
  
    // UpdateNode is called once per frame
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


        if (enemy.enemyStateManagerNode.TryGetCurNodeLeaf<EnemySprintStateNodeLeaf>())
        {
            this.VelocityMoveMagnitude_Normalized = this.curVelocity_Local.magnitude / enemy.sprintMaxSpeed;
            this.MoveVelocityForward_Normalized = this.curVelocity_Local.z / enemy.sprintMaxSpeed;
            this.MoveVelocitySideward_Normalized = this.curVelocity_Local.x / enemy.sprintMaxSpeed;

            isSprint = true;
        }
        else
        {

            this.VelocityMoveMagnitude_Normalized = this.curVelocity_Local.magnitude / enemy.moveMaxSpeed;
            this.MoveVelocityForward_Normalized = this.curVelocity_Local.z / enemy.moveMaxSpeed;
            this.MoveVelocitySideward_Normalized = this.curVelocity_Local.x / enemy.moveMaxSpeed;

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
