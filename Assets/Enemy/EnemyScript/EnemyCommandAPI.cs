using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Enemy))]
public class EnemyCommandAPI :MonoBehaviour
{

    //private float time;
    public NormalFiringPattern NormalFiringPattern;
    //private bool isShooting;

    //[SerializeField] Transform moveToPos;
    //[SerializeField] Transform sprintToPos;

    //int caseEvent = 0;
    //public float CoverTiming = 0;

    //public bool isKilled;

    //public bool TriigerPainLeg;
    //public bool TriggerPainBody;
    public Enemy _enemy;

    private void Start()
    {
        this._enemy = GetComponent<Enemy>();
        NormalFiringPattern = new NormalFiringPattern(this);
    }

    public void Update()
    {
       

    }

    #region Testing
    //private void TestCommand1()
    //{
    //    time += Time.deltaTime;
    //    if (time < 3)
    //    {
    //        Freez();
    //    }
    //    else if (time < 6)
    //    {
    //        MoveToPosition(moveToPos.position, 100, true);
    //    }
    //    else if ((time < 9))
    //    {
    //        _enemy.findingTargetComponent.FindTarget(out GameObject target);
    //        MoveToPosition(_enemy.targetKnewPos, 100);
    //        AimDownSight(_enemy.targetKnewPos, 6);
    //    }
    //    else if (time < 12)
    //    {
    //        if (SprintToPosition(sprintToPos.position, 5))
    //        {
    //            Freez(_enemy.targetKnewPos, 6);
    //        }
    //    }
    //    else if (time < 15)
    //    {
    //        if (_enemy.findingTargetComponent.FindTarget(out GameObject target))
    //        {
    //            AimDownSight(_enemy.targetKnewPos, 7);
    //            PullTrigger();
    //        }
    //        else
    //        {
    //            LowReady();
    //        }
    //        Freez();
    //    }
    //    else if (time < 18)
    //    {
    //        Reload();
    //    }
    //    else if (time < 20)
    //    {
    //        if (_enemy.findingTargetComponent.FindTarget(out GameObject target))
    //        {
    //            AimDownSight(_enemy.targetKnewPos, 7);
    //            PullTrigger();
    //        }
    //        else
    //        {
    //            LowReady();
    //        }
    //        MoveToPosition(_enemy.targetKnewPos, 100);
    //    }
    //    else if (time < 24)
    //    {
    //        LowReady();
    //        Freez();
    //    }
    //    if(time > 26)
    //    {
    //        time = 0;
    //    }
    //}
    //private void TestCommandTakeCover()
    //{
    //    switch (caseEvent)
    //    {
    //        case 0:
    //            {
    //                if (_enemy.findingCover.FindCoverInRaduis(8, out CoverPoint coverPoint))
    //                {
    //                    coverPoint.TakeThisCover(_enemy);
    //                    if (coverPoint == null)
    //                    {
    //                        Debug.Log("CoverPoint = null");
    //                    }
    //                    time = 0;
    //                    caseEvent = 1;
    //                }
    //            }
    //            break;
    //        case 1:
    //            {
    //                if (SprintToPosition(_enemy.coverPos, 6))
    //                {
    //                    caseEvent = 2;
    //                    TakeCover();
    //                    Freez();
    //                }
    //                //if (MoveToPosition(_enemy.coverPos, 1, true))
    //                //{

    //                //}
    //            }
    //            break;

    //        case 2:
    //            {
    //                CoverTiming += Time.deltaTime;
    //                time += Time.deltaTime;

    //                if (CoverTiming < 2)
    //                {
    //                    Debug.Log("Take Cover");
    //                    LowReady();
    //                }
    //                else if (CoverTiming < 5)
    //                {
    //                    Debug.Log("Take Aim");

    //                    if (_enemy.findingTargetComponent.FindTarget(out GameObject target))
    //                    {
    //                        NormalFiringPattern.Performing();
    //                        AimDownSight(_enemy.targetKnewPos, 6);
    //                    }
    //                    else
    //                        AimDownSight();
    //                }

    //                if (CoverTiming > 5)
    //                    CoverTiming = 0;

    //                if (time > 18)
    //                    caseEvent = 3;
    //            }
    //            break;

    //        case 3:
    //            {
    //                if (_enemy.isInCover)
    //                    GetOffCover();

    //                MoveToPosition(_enemy.targetKnewPos, 1);

    //                if (Vector3.Distance(_enemy.targetKnewPos, _enemy.transform.position) < 1.5f)
    //                    caseEvent = 0;
    //            }
    //            break;
    //    }
    //}
    #endregion

    public bool MoveToPosition(Vector3 DestinatePos, float velocity)
    {
        NavMeshAgent agent = _enemy.agent;
        if(agent.hasPath == false|| Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);
       
        Vector3 moveDir = agent.steeringTarget-_enemy.transform.position;
        Move(moveDir, velocity);

        return Vector3.Distance(DestinatePos, _enemy.transform.position) < 0.5f;
    }
    public bool MoveToPosition(Vector3 DestinatePos, float velocity,bool isRotateToMoveDir)
    {
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);

        Vector3 moveDir = agent.steeringTarget - _enemy.transform.position;
        Move(moveDir, velocity);

        if (isRotateToMoveDir)
            RotateToPosition(agent.steeringTarget,6);

        if (Vector3.Distance(DestinatePos, _enemy.transform.position) < 0.5f)
        {
            FreezRotation();
            return true;
        }
        else
            return false;
    }
    public bool MoveToPosition(Vector3 DestinatePos, float velocity,float reachDestinationDistance)
    {
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);

        Vector3 moveDir = agent.steeringTarget - _enemy.transform.position;
        Move(moveDir, velocity);

        return Vector3.Distance(DestinatePos, _enemy.transform.position) < reachDestinationDistance;
    }
    public bool MoveToPosition(Vector3 DestinatePos, float velocity, bool isRotateToMoveDir, float reachDestinationDistance)
    {
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);

        Vector3 moveDir = agent.steeringTarget - _enemy.transform.position;
        Move(moveDir, velocity);

        if (isRotateToMoveDir)
            RotateToPosition(agent.steeringTarget, 6);

        if (Vector3.Distance(DestinatePos, _enemy.transform.position) < reachDestinationDistance)
        {
            FreezRotation();
            return true;
        }
        else
            return false;
    }
    public bool RotateToPosition(Vector3 DestinatePos, float rotSpeed)
    {
        Vector3 rotateDir = DestinatePos - _enemy.transform.position;
        Rotate(rotateDir, rotSpeed);

        if(Mathf.Abs(Vector3.Dot(_enemy.transform.forward, rotateDir.normalized))>0.95f)
            return true;

        return false;

    }
    public bool SprintToPosition(Vector3 Destination,float rotSpeed)
    {
        _enemy.isSprintCommand = true;
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(Destination, agent.destination) > 0.1f)
            agent.SetDestination(Destination);

        RotateToPosition(Destination, rotSpeed);

        return Vector3.Distance(Destination, _enemy.transform.position) < 0.5f;

    }
    public bool SprintToPosition(Vector3 Destination, float rotSpeed,float reachDestinationDistance)
    {
        _enemy.isSprintCommand = true;
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(Destination, agent.destination) > 0.1f)
            agent.SetDestination(Destination);

        RotateToPosition(Destination, rotSpeed);

        return Vector3.Distance(Destination, _enemy.transform.position) < reachDestinationDistance;

    }
    public void Freez(Vector3 rotateToDes, float rotateSpeed)
    {
        _enemy.isSprintCommand = false;
        _enemy.moveInputVelocity_WorldCommand = Vector3.zero;
        RotateToPosition(rotateToDes, rotateSpeed);
    }
    public void Move(Vector3 MoveDirWorld, float velocity)
    {
        _enemy.moveInputVelocity_WorldCommand = MoveDirWorld.normalized * velocity;
    }
    public void FreezRotation()
    {
        _enemy.lookRotationCommand = _enemy.transform.forward;
        _enemy._rotateSpeed = 0;
    }
    public void Rotate(Vector3 dir, float rotSpeed)
    {
        _enemy.lookRotationCommand = dir;
        _enemy._rotateSpeed = rotSpeed;
    }
    public void Sprint()
    {
        _enemy.isSprintCommand = true;
    }
    public void Freez()
    {
        _enemy.isSprintCommand = false;
        _enemy.moveInputVelocity_WorldCommand = Vector3.zero;
    }
    public void Stand()
    {
        _enemy.curStance = IMovementCompoent.Stance.Stand;
    }
    public void Crouch()
    {
        _enemy.curStance = IMovementCompoent.Stance.Crouch;
        _enemy.isSprintCommand = false;
    }
    public void Dodge()
    {
        
    }
    public void TakeCover()
    {
        Freez();
        _enemy.isInCover = true;
    }
    public void MoveToTakeCover(CoverPoint coverPoint,float velocity,bool rotateMoveDir)
    {
        coverPoint.TakeThisCover(_enemy);

        if (MoveToPosition(coverPoint.coverPos.position, 1, rotateMoveDir,1)) 
        TakeCover();
    }
    public void MoveToTakeCover(CoverPoint coverPoint, float velocity)
    {
        if(coverPoint == null)
            return;

        coverPoint.TakeThisCover(_enemy);

        if (MoveToPosition(coverPoint.coverPos.position,1,1))
            TakeCover();
    }
    public void SprintToCover(CoverPoint coverPoint)
    {
        if(coverPoint == null)
            return ;
        coverPoint.TakeThisCover(_enemy);

        if (SprintToPosition(coverPoint.coverPos.position, 5, 1))
        {
            TakeCover();
        }
    }
    public void GetOffCover()
    {
        if(_enemy.coverPoint != null)
        _enemy.coverPoint.OffThisCover();

        _enemy.coverPoint = null;
        _enemy.isInCover = false;
    }

    public void LowReady()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;

        //weaponAdvanceUser.weaponCommand.LowReady();
        weaponAdvanceUser.isAiming = false;
        weaponAdvanceUser.isPullTrigger = false;
    }
    public void AimDownSight()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;

        //weaponAdvanceUser.weaponCommand.AimDownSight();
        weaponAdvanceUser.isAiming = true;
    }
    public void AimDownSight(Vector3 aimTargetPos,float rotateSpeed)
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        RotateToPosition(aimTargetPos,rotateSpeed);

        //weaponAdvanceUser.weaponCommand.AimDownSight();
        weaponAdvanceUser.isAiming = true;
    }
    public void PullTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        //weaponAdvanceUser.weaponCommand.PullTrigger();
        weaponAdvanceUser.isPullTrigger = true;
    }
    public void CancleTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        //weaponAdvanceUser.weaponCommand.CancleTrigger();
        weaponAdvanceUser.isPullTrigger = false;
    }
    public void Reload()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        //weaponAdvanceUser.weaponCommand.Reload(weaponAdvanceUser.weaponBelt.ammoProuch);
        weaponAdvanceUser.isReload = true;
    }



  
}
