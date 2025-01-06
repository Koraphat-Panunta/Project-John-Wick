using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Enemy))]
public class EnemyControllerAPI :MonoBehaviour, IEnemyGOAP,IEncounterGoal,IHoldingGoal,ITakeCoverGoal,IPatrolingGoal
{
    public Enemy enemy;
    private float time;
    private NormalFiringPattern NormalFiringPattern;
    private bool isShooting;
    //public enum Command 
    //{
        
    //}
    //public List<Command> commands;

    private void Start()
    {
        this.enemy = GetComponent<Enemy>();
        NormalFiringPattern = new NormalFiringPattern(enemy);
        //InitailizedGOAP();
    }


    public void Update()
    {
        time += Time.deltaTime;
        if (time < 3)
        {
            Freez();
        }
        else if (time < 6)
        {
            enemy.findingTargetComponent.FindTarget(out GameObject target);
            Vector3 moveDir = enemy.targetKnewPos - enemy.transform.position;
            Move(moveDir.normalized, 1);

            Vector3 lookdir = (enemy.targetKnewPos - enemy.transform.position).normalized;
            Rotate(lookdir, 6);
        }
        else if ((time < 9))
        {
            enemy.findingTargetComponent.FindTarget(out GameObject target);
            Move(enemy.targetKnewPos, 1);

            Vector3 lookdir = (enemy.targetKnewPos - enemy.transform.position).normalized;
            Rotate(lookdir, 6);
            AimDownSight();
        }
        else if (time < 12)
        {
            enemy.findingTargetComponent.FindTarget(out GameObject target);
            Vector3 lookdir = (enemy.targetKnewPos - enemy.transform.position).normalized;
            Rotate(lookdir, 6);

            Sprint();
        }
        else if(time < 15)
        {
            enemy.findingTargetComponent.FindTarget(out GameObject target);
            if (isShooting == false)
            {
                PullTrigger();
                isShooting = true;
            }
            Vector3 lookdir = (enemy.targetKnewPos - enemy.transform.position).normalized;
            Rotate(lookdir, 6);
            Freez();
        }
        else 
        {
            if (isShooting)
            {
                isShooting = false;
                Reload();
                CancleTrigger();
            }
                
            Freez();
        }
        //GOAP_Update();
    }
    public void FixedUpdate()
    {
        //GOAP_FixedUpdate();
    }

    public void Move(Vector3 MoveDirWorld, float velocity)
    {
        enemy.moveInputVelocity_World = MoveDirWorld.normalized*velocity;

    }
    public void RotateToPos(Vector3 pos, float rotSpeed)
    {
        Quaternion rotation = new RotateObjectToward().RotateToward(pos - enemy.transform.position, enemy.transform, rotSpeed);

        Rotate(rotation);
    }
    public void Rotate(Quaternion rotate)
    {
        //enemy.rotating = rotate;
    }

    public void Rotate(Vector3 dir,float rotSpeed)
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
        enemy.isInCover = true;
    }
    public void GetOffCover()
    {
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
    public EnemyControllerAPI _enemyController { get ; set ; }

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
