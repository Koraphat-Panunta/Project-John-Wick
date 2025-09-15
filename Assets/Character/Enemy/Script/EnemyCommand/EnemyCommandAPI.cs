
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Enemy))]
public class EnemyCommandAPI : MonoBehaviour,IInitializedAble
{
    public NormalFiringPattern NormalFiringPattern;

    public Enemy _enemy;
    private EnemyCommunicator enemyCommunicator;
    public EnemyAutoDefendCommand enemyAutoDefendCommand;

    public void Initialized()
    {
        NormalFiringPattern = new NormalFiringPattern(this);
        Debug.Log("NormalFiringPattern = "+ NormalFiringPattern);
        enemyCommunicator = new EnemyCommunicator(_enemy);
        enemyAutoDefendCommand = new EnemyAutoDefendCommand(this);
    }
   
    private void Update()
    {
        enemyAutoDefendCommand.UpdateDefendActionBlackBoard();
    }

    public bool MoveToPosition(Vector3 DestinatePos, float velocityScale)
    {
        return this.MoveToPosition(DestinatePos, velocityScale, 0.5f);
    }
    public bool MoveToPositionRotateToward(Vector3 DestinatePos, float velocityScale, float rotateTowardDirSpeedScale)
    {

        if (RotateToPosition(_enemy.agent.steeringTarget, rotateTowardDirSpeedScale))
            FreezRotation();


        if (MoveToPosition(DestinatePos, velocityScale))
        {
            FreezRotation();
            return true;
        }
        else
            return false;
    }
    public bool MoveToPosition(Vector3 DestinatePos, float velocityScale, float reachDestinationDistance)
    {
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(DestinatePos, agent.destination) > 0.1f)
            agent.SetDestination(DestinatePos);

        Vector3 moveDir = agent.steeringTarget - _enemy.transform.position;
        Move(moveDir, velocityScale);

        return Vector3.Distance(DestinatePos, _enemy.transform.position) < reachDestinationDistance;
    }
    public bool MoveToPositionRotateToward(Vector3 DestinatePos, float velocityScale, float rotateTowardDirSpeedScale, float reachDestinationDistance)
    {

        RotateToPosition(_enemy.agent.steeringTarget, rotateTowardDirSpeedScale);

        if (MoveToPosition(DestinatePos, velocityScale, reachDestinationDistance))
        {
            FreezRotation();
            return true;
        }
        else
            return false;

    }
    public bool RotateToPosition(Vector3 DestinatePos, float rotSpeedScale)
    {
        Vector3 rotateDir = DestinatePos - _enemy.transform.position;
        Rotate(rotateDir, rotSpeedScale);

        if (Mathf.Abs(Vector3.Dot(_enemy.transform.forward, rotateDir.normalized)) > 0.95f)
            return true;

        return false;

    }
    public bool SprintToPosition(Vector3 Destination, float rotSpeedScale)
    {
        return SprintToPosition(Destination, rotSpeedScale, 0.5f);
    }
    public bool SprintToPosition(Vector3 Destination, float rotSpeedScale, float reachDestinationDistance)
    {
        _enemy.isSprintCommand = true;
        NavMeshAgent agent = _enemy.agent;
        if (agent.hasPath == false || Vector3.Distance(Destination, agent.destination) > 0.1f)
            agent.SetDestination(Destination);

        RotateToPosition(agent.steeringTarget, rotSpeedScale);

        return Vector3.Distance(Destination, _enemy.transform.position) < reachDestinationDistance;
    }
    public void FreezPosition()
    {
        _enemy.isSprintCommand = false;
        _enemy.moveInputVelocity_WorldCommand = Vector3.zero;
    }
    public void Move(Vector3 MoveDirWorld, float velocityScale)
    {
        velocityScale = Mathf.Clamp01((float)velocityScale);
        _enemy.moveInputVelocity_WorldCommand = MoveDirWorld.normalized * velocityScale;
    }
    public void FreezRotation()
    {
        _enemy.lookRotationCommand = _enemy.transform.forward;
    }
    private void Rotate(Vector3 dir, float rotSpeedScale)
    {
        rotSpeedScale = Mathf.Clamp01((float)rotSpeedScale);
        _enemy.lookRotationCommand = Vector3.Lerp(_enemy.transform.forward, dir, rotSpeedScale);
    }
    public bool FindCoverAndBook(float raduis, out CoverPoint coverPoint)
    {
        coverPoint = null;

        if (_enemy.findingCover.FindCoverInRaduisInGunFight(raduis, out coverPoint))
        {
            coverPoint.TakeThisCover(_enemy);
            return true;
        }
        return false;
    }
    public bool FindCoverAndBook(float raduis, Vector3 targetPos, out CoverPoint coverPoint)
    {
        coverPoint = null;

        if (_enemy.findingCover.FindCoverInRaduisDirectionalBased(raduis, out coverPoint, targetPos))
        {
            coverPoint.TakeThisCover(_enemy);
            return true;
        }
        return false;
    }

    public void Dodge(Vector3 dodgeDir)
    {
        _enemy.moveInputVelocity_WorldCommand = dodgeDir;
        _enemy._triggerDodge = true;
    }
    public void Stand()
    {
        _enemy.enemyStance = Stance.stand;
    }
    public void Crouch()
    {
        _enemy.enemyStance = Stance.crouch;
    }

    public void AutoDetectSoftCover()
    {
        if(Physics.Raycast(_enemy.transform.position + Vector3.up*0.2f,_enemy.transform.forward,6,LayerMask.GetMask("Default"),QueryTriggerInteraction.Ignore))
        {
            this.Crouch();
        }
        else
            this.Stand();
    }

    
    public void LowReady()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        weaponAdvanceUser._isAimingCommand = false;
        weaponAdvanceUser._isPullTriggerCommand = false;
    }
    public void AimDownSight()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        weaponAdvanceUser._isAimingCommand = true;
        _enemy.enemyGetShootDirection.SetPointingPos(_enemy.transform.position + _enemy.transform.forward +Vector3.up);
    }
    public void AimDownSight(Vector3 aimTargetPos)
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        _enemy.enemyGetShootDirection.SetPointingPos(aimTargetPos);

        if (_enemy.enemyGetShootDirection.outOfHorizontalLimit)
        {
            RotateToPosition(aimTargetPos, .5f);
        }

        weaponAdvanceUser._isAimingCommand = true;
    }
    public void PullTrigger()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        weaponAdvanceUser._isPullTriggerCommand = true;
    }
    public void Reload()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        weaponAdvanceUser._isReloadCommand = true;
    }
    public void PickUpWeapon()
    {
        IWeaponAdvanceUser weaponAdvanceUser = _enemy as IWeaponAdvanceUser;
        weaponAdvanceUser._isPickingUpWeaponCommand = true;
    }
    public void HolsterWeapon()
    {
        (_enemy as IWeaponAdvanceUser)._isHolsterWeaponCommand = true;
    }
    public void DrawWeaponPrimary()
    {
        (_enemy as IWeaponAdvanceUser)._isDrawPrimaryWeaponCommand = true;
    }
    public void DrawWeaponSecondary()
    {
        (_enemy as IWeaponAdvanceUser)._isDrawSecondaryWeaponCommand = true;
    }
    public void DropWeapon()
    {
        (_enemy as IWeaponAdvanceUser)._isDropWeaponCommand = true;
    }
    public void SpinKick()
    {
        _enemy._triggerGunFu = true;
    }

    public LayerMask NotifyAbleMask;
    public void NotifyFriendly(float r, EnemyCommunicator.EnemyCommunicateMassage enemyCommunicateMassage) 
    {
        enemyCommunicator.SendCommunicate(this._enemy.transform.position, r, NotifyAbleMask, enemyCommunicateMassage); 
    }

   
}
