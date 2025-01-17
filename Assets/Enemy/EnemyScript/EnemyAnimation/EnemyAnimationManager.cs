using UnityEngine;
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class EnemyAnimationManager : MonoBehaviour,IObserverEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animator;
    public Enemy enemy;

    public float CoverWeight;
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

    public IMovementCompoent.Stance enemyStance;
    public bool isGround;
    public bool isSprint;

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BackBoardUpdate();
    }
    private void BackBoardUpdate()
    {
        this.enemyStance = enemy.curStance;

        if (enemy.isInCover)
            CoverWeight = Mathf.Clamp(CoverWeight + 100 * Time.deltaTime, 0, 1);
        else
            CoverWeight = Mathf.Clamp(CoverWeight - 100 * Time.deltaTime, 0, 1);

        IMovementCompoent movementComponent = enemy;
        Vector3 inputVelocity_World = movementComponent.moveInputVelocity_World;
        Vector3 inputVelocity_Local = movementComponent.moveInputVelocity_Local;
        Vector3 curVelocity_Local = movementComponent.curMoveVelocity_Local;
        Vector3 curVelocity_World = movementComponent.curMoveVelocity_World;

        this.InputMoveMagnitude_Normalized = inputVelocity_World.normalized.magnitude;

        this.MoveInputLocalFoward_Normalized = inputVelocity_Local.normalized.z;
        this.MoveInputLocalSideWard_Normalized = inputVelocity_Local.normalized.x;


        this.DotMoveInputWordl_VelocityWorld_Normalized = Vector3.Dot(curVelocity_World.normalized
            , inputVelocity_World.normalized) * (curVelocity_World.magnitude / inputVelocity_World.magnitude);

        this.DotVectorLeftwardDir_MoveInputVelocity_Normallized = Mathf.Lerp(this.DotVectorLeftwardDir_MoveInputVelocity_Normallized,
                Vector3.Dot(inputVelocity_World.normalized,
                Vector3.Cross(enemy.transform.forward, Vector3.up))
            , 10 * Time.deltaTime);


        if (enemy.curStateLeaf == enemy.sprintState)
        {
            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / enemy._sprintMaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / enemy._sprintMaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / enemy._sprintMaxSpeed;

            isSprint = true;
        }
        else
        {

            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / enemy._moveMaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / enemy._moveMaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / enemy._moveMaxSpeed;

            isSprint = false;
        }

        AimDownSightWeight = (enemy as IWeaponAdvanceUser).currentWeapon.aimingWeight;

        this.DotVelocityWorld_Leftward_Normalized = Vector3.Dot(
            Vector3.Cross(enemy.transform.forward, Vector3.up).normalized
            , curVelocity_World.normalized);

        if (RecoilWeight > 0)
            RecoilWeight = Mathf.Clamp(RecoilWeight - 3 * Time.deltaTime, 0, 1);

        CalculateDeltaRotation();

        animator.SetFloat("CoverWeight", CoverWeight);
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
        //animator.SetFloat("PointRange", 0);
        animator.SetFloat("DotVectorLeftwardDir_MoveInputVelocity_Normallized", DotVectorLeftwardDir_MoveInputVelocity_Normallized);

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
