using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour,IObserverEnemy
{
    [SerializeField] private Enemy enemy;
    private NavMeshAgent agent;
    public Animator animator;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
       BackBoardUpdate();
    }
    private void FixedUpdate()
    {
        
    }

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        throw new System.NotImplementedException();
    }
    public float CoverWeight;
    public float SholderSide;//-1 -> 1
    public float InputMoveMagnitude_Normalized;
    public float VelocityMoveMagnitude_Normalized;
    public float MoveInputLocalFoward_Normalized;
    public float MoveInputLocalSideWard_Normalized;
    public float MoveVelocityForward_Normalized;
    public float MoveVelocitySideward_Normalized;
    public float DotMoveInputWordl_VelocityWorld_Normalized;
    public float Rotating;
    public float AimDownSightWeight;
    public float DotVelocityWorld_Leftward_Normalized;
    public float RecoilWeight;
    public float PointRange;

    public bool isSprint;
    public bool isAiming;
    public bool isTriggerMantle;
    public bool isTriggerGunFu;
    public Player.PlayerStance playerStance;
    public Player.ShoulderSide shoulderSide;
    public bool isGround;

    private bool isCover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private void BackBoardUpdate()
    {

        //if (playerAnimationManager.curShoulderSide == Player.ShoulderSide.Left)
        //    SholderSide = Mathf.Clamp(SholderSide - 100 * Time.deltaTime, -1, 1);
        //if (playerAnimationManager.curShoulderSide == Player.ShoulderSide.Right)
            SholderSide = Mathf.Clamp(SholderSide + 100 * Time.deltaTime, -1, 1);

        if (enemy.isInCover)
            CoverWeight = Mathf.Clamp(CoverWeight + 100 * Time.deltaTime, 0, 1);
        else
            CoverWeight = Mathf.Clamp(CoverWeight - 100 * Time.deltaTime, 0, 1);


        Vector3 inputVelocity_World = enemy.moveInputVelocity_World;
        Vector3 inputVelocity_Local = enemy.moveInputVelocity_Local;
        Vector3 curVelocity_Local = enemy.curMoveVelocity_Local;
        Vector3 curVelocity_World = enemy.curMoveVelocity_World;

        this.InputMoveMagnitude_Normalized = inputVelocity_World.normalized.magnitude;

        this.MoveInputLocalFoward_Normalized = inputVelocity_Local.normalized.z;
        this.MoveInputLocalSideWard_Normalized = inputVelocity_Local.normalized.x;


        this.DotMoveInputWordl_VelocityWorld_Normalized = Vector3.Dot(curVelocity_World.normalized
            , inputVelocity_World.normalized) * (curVelocity_World.magnitude / inputVelocity_World.magnitude);

        if (enemy.curStateLeaf == enemy.sprintState)
        {
            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / enemy._sprintMaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / enemy._sprintMaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / enemy._sprintMaxSpeed;
        }
        else
        {
            Debug.Log(curVelocity_Local);
            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / enemy._moveMaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / enemy._moveMaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / enemy._moveMaxSpeed;
        }

        CalculateDeltaRotation();

        AimDownSightWeight = (enemy as IWeaponAdvanceUser).currentWeapon.aimingWeight;


        this.DotVelocityWorld_Leftward_Normalized = Vector3.Dot(
            Vector3.Cross(enemy.transform.forward, Vector3.up).normalized
            , enemy.curMoveVelocity_World.normalized);

        if (RecoilWeight > 0)
            RecoilWeight = Mathf.Clamp(RecoilWeight - 3 * Time.deltaTime, 0, 1);

        if (enemy.currentWeapon != null)
        {
            if (Vector3.Distance((enemy as IWeaponAdvanceUser).shootingPos
            , (enemy as IWeaponAdvanceUser).currentWeapon.bulletSpawnerPos.position) < 2)

                PointRange = Mathf.Clamp(PointRange - 10 * Time.deltaTime, 0, 1);
            else
                PointRange = Mathf.Clamp(PointRange + 10 * Time.deltaTime, 0, 1);

        }


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
        animator.SetFloat("PointRange", PointRange);

    }

   

    private Vector3 previousDir;
    private void CalculateDeltaRotation()
    {
        Vector3 curDir = enemy.transform.forward;

        Rotating = Mathf.Clamp(Vector3.SignedAngle(previousDir, curDir, Vector3.up) * 10 * Time.deltaTime, -1, 1);
        previousDir = curDir;
    }
}
