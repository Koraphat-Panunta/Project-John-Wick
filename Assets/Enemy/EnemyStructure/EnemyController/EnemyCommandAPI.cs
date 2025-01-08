using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Enemy))]
public class EnemyCommandAPI :MonoBehaviour, IEnemyGOAP,IEncounterGoal,IHoldingGoal,ITakeCoverGoal,IPatrolingGoal
{
    public Enemy enemy;
    private float time;
    private NormalFiringPattern NormalFiringPattern;
    private bool isShooting;

    [SerializeField] Transform moveToPos;
    [SerializeField] Transform sprintToPos;
    
    int caseEvent = 0;
    public float CoverTiming = 0;

    private void Start()
    {
        this.enemy = GetComponent<Enemy>();
        NormalFiringPattern = new NormalFiringPattern(enemy);
    }


    public void Update()
    {

        TestCommandTakeCover();

    }
    private void TestCommand1()
    {
        if (time < 3)
        {
            Freez();
        }
        else if (time < 6)
        {
            MoveToPosition(moveToPos.position, 1, true);
        }
        else if ((time < 9))
        {
            enemy.findingTargetComponent.FindTarget(out GameObject target);
            MoveToPosition(enemy.targetKnewPos, 1);
            AimDownSight(enemy.targetKnewPos, 6);
        }
        else if (time < 12)
        {
            if (SprintToPosition(sprintToPos.position, 5))
            {
                Freez(enemy.targetKnewPos, 6);
            }
        }
        else if (time < 15)
        {
            if (enemy.findingTargetComponent.FindTarget(out GameObject target))
            {
                AimDownSight(enemy.targetKnewPos, 7);
                PullTrigger();
            }
            else
            {
                LowReady();
            }
            Freez();
        }
        else if (time < 18)
        {
            Reload();
        }
        else if (time < 20)
        {
            if (enemy.findingTargetComponent.FindTarget(out GameObject target))
            {
                AimDownSight(enemy.targetKnewPos, 7);
                PullTrigger();
            }
            else
            {
                LowReady();
            }
            MoveToPosition(enemy.targetKnewPos, 1);
        }
        else if (time < 24)
        {
            LowReady();
            Freez();
        }
    }
    private void TestCommandTakeCover()
    {
        switch (caseEvent)
        {
            case 0: 
                {
                    if(enemy.findingCover.FindCoverInRaduis(8,out CoverPoint coverPoint))
                    {
                        coverPoint.TakeThisCover(enemy);
                        if(coverPoint == null)
                        {
                            Debug.Log("CoverPoint = null");
                        }
                        time = 0;
                        caseEvent = 1;
                    }
                }
                break;
            case 1: 
                {
                    if (MoveToPosition(enemy.coverPos, 1, true))
                    {
                        caseEvent= 2;
                        TakeCover();
                        Freez();
                    }
                }
                break;

            case 2: 
                {
                    CoverTiming += Time.deltaTime;
                    time += Time.deltaTime;

                    if (CoverTiming < 5)
                    {
                        Debug.Log("Take Cover");
                        LowReady();
                    }
                    else if(CoverTiming < 10)
                    {
                        Debug.Log("Take Aim");

                        if (enemy.findingTargetComponent.FindTarget(out GameObject target))
                            AimDownSight(enemy.targetKnewPos, 6);
                        else
                            AimDownSight(enemy.coverPoint.coverDir, 5);
                    }

                    if (CoverTiming > 10)
                        CoverTiming = 0;

                    if (time > 18)
                        caseEvent = 3;
                }
                break;
                
            case 3: 
                {
                    if (enemy.isInCover)
                        GetOffCover();

                    MoveToPosition(enemy.targetKnewPos, 1);

                    if (Vector3.Distance(enemy.targetKnewPos, enemy.transform.position) < 1.5f)
                        caseEvent = 0;
                }
                break;
        }
    }
    public void FixedUpdate()
    {

    }
    public bool MoveToPosition(Vector3 DestinatePos, float velocity)
    {
        NavMeshAgent agent = enemy.agent;
        if(agent.hasPath == false|| Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);
       
        Vector3 moveDir = agent.steeringTarget-enemy.transform.position;
        Move(moveDir, velocity);

        return Vector3.Distance(DestinatePos, enemy.transform.position) < 0.5f;
    }
    public bool MoveToPosition(Vector3 DestinatePos, float velocity,bool isRotateToMoveDir)
    {
        NavMeshAgent agent = enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);

        Vector3 moveDir = agent.steeringTarget - enemy.transform.position;
        Move(moveDir, velocity);

        if (isRotateToMoveDir)
            RotateToPosition(agent.steeringTarget,6);

        return Vector3.Distance(DestinatePos, enemy.transform.position) < 0.5f;
    }
    public bool RotateToPosition(Vector3 DestinatePos, float rotSpeed)
    {
        Vector3 rotateDir = DestinatePos - enemy.transform.position;
        Rotate(rotateDir, rotSpeed);

        if(Mathf.Abs(Vector3.Dot(enemy.transform.forward, rotateDir.normalized))>0.95f)
            return true;

        return false;

    }
    public bool SprintToPosition(Vector3 Destination,float rotSpeed)
    {
        enemy.isSprint = true;
        NavMeshAgent agent = enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(Destination, agent.destination) > 0.1f)
            agent.SetDestination(Destination);

        RotateToPosition(Destination, rotSpeed);

        return Vector3.Distance(Destination, enemy.transform.position) < 0.5f;

    }
    public void Freez(Vector3 rotateToDes, float rotateSpeed)
    {
        enemy.isSprint = false;
        enemy.moveInputVelocity_World = Vector3.zero;
        RotateToPosition(rotateToDes, rotateSpeed);
    }
    public void Move(Vector3 MoveDirWorld, float velocity)
    {
        enemy.moveInputVelocity_World = MoveDirWorld.normalized * velocity;
    }
    public void Rotate(Vector3 dir, float rotSpeed)
    {
        enemy.lookRotation = dir;
        enemy.rotateSpeed = rotSpeed;
    }
    public void Sprint()
    {
        enemy.isSprint = true;
    }
    public void Freez()
    {
        enemy.isSprint = false;
        enemy.moveInputVelocity_World = Vector3.zero;
    }
    public void Stand()
    {
        enemy.curStance = IMovementCompoent.Stance.Stand;
    }
    public void Crouch()
    {
        enemy.curStance = IMovementCompoent.Stance.Crouch;
        enemy.isSprint = false;
    }
    public void Dodge()
    {
        
    }
    public void TakeCover()
    {
        Freez();
        enemy.isInCover = true;
    }
    public void GetOffCover()
    {
        
        enemy.coverPoint.OffThisCover();
        enemy.coverPoint = null;
        enemy.isInCover = false;
    }

    public void LowReady()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.weaponCommand.LowReady();
        weaponAdvanceUser.isAiming = false;
        weaponAdvanceUser.isPullTrigger = false;
    }
    public void AimDownSight()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;

        weaponAdvanceUser.weaponCommand.AimDownSight();
        weaponAdvanceUser.isAiming = true;
    }
    public void AimDownSight(Vector3 aimTargetPos,float rotateSpeed)
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;
        RotateToPosition(aimTargetPos,rotateSpeed);

        weaponAdvanceUser.weaponCommand.AimDownSight();
        weaponAdvanceUser.isAiming = true;
    }
    public void PullTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;
        weaponAdvanceUser.weaponCommand.PullTrigger();
        weaponAdvanceUser.isPullTrigger = true;
    }
    public void CancleTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;
        weaponAdvanceUser.weaponCommand.CancleTrigger();
        weaponAdvanceUser.isPullTrigger = false;
    }
    public void Reload()
    {
        IWeaponAdvanceUser weaponAdvanceUser = enemy as IWeaponAdvanceUser;
        weaponAdvanceUser.weaponCommand.Reload(weaponAdvanceUser.weaponBelt.ammoProuch);
        weaponAdvanceUser.isReload = true;
    }


    public EnemyGoalLeaf curGoal { get ; set ; }
    public EnemyGoalSelector startSelecotr { get ; set ; }
    public Enemy _enemy { get ; set ; }
    public EnemyCommandAPI _enemyController { get ; set ; }

    #region InitailizedEncouter

    public IEnemyGOAP _enemyGOAP { get => this ; }
    public IFindingTarget _findingTarget { get => _enemy; }
    public EncouterGoal _encouterGoal { get; set; }

    #endregion

    #region InitailizedHolding
    public HoldingGoal _holdingGoal { get; set; }

    #endregion

    #region InitailizedTakeCover

    public ICoverUseable _coverUseable => _enemy ;
    public TakeCoverGoal _takeCoverGoal { get; set ; }

    #endregion

    #region InitializedSearchGoal

    private SearchingGoal _searchingGoal { get; set; }

    #endregion

    #region InitailizedPatrolingGoal
    public PatrolingGoal _patrolingGoal { get; set; }

    #endregion

    public void GOAP_Update()
    {
        if (curGoal.IsReset())
        {
            curGoal.Exit();
            startSelecotr.Transition(out EnemyGoalLeaf enemyGoalLeaf);
            curGoal = enemyGoalLeaf;
            Debug.Log("Goal =" + curGoal);
            curGoal.Enter();
        }

        if (curGoal != null) 
        curGoal.Update();
    }

    public void GOAP_FixedUpdate()
    {
        if (curGoal != null) 
        curGoal.FixedUpdate();
    }

    public void InitailizedGOAP()
    {
        _enemy = this.enemy;
        _enemyController = this;

        startSelecotr = new EnemyGoalSelector(this,this,() => true);

        _searchingGoal = new SearchingGoal(_enemyController,this, _findingTarget);
        _encouterGoal = new EncouterGoal(this, _enemyGOAP, _findingTarget);
        _holdingGoal = new HoldingGoal(this, _enemyGOAP, _findingTarget);
        _takeCoverGoal = new TakeCoverGoal(this, _enemyGOAP, _coverUseable);
        _patrolingGoal = new PatrolingGoal(this, this, enemy);

        startSelecotr.AddChildNode(_takeCoverGoal);
        startSelecotr.AddChildNode(_encouterGoal);
        startSelecotr.AddChildNode(_holdingGoal);
        startSelecotr.AddChildNode(_searchingGoal);
        startSelecotr.AddChildNode(_patrolingGoal);

        startSelecotr.Transition(out EnemyGoalLeaf enemyGoalLeaf);
        curGoal = enemyGoalLeaf;
        Debug.Log("Goal =" + curGoal);
        curGoal.Enter();
    }

  
}
