using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandMoveStateNode : EnemyStateLeafNode
{
    
    RotateObjectToward objectToward;
    NavMeshAgent agent;
    WeaponInput weaponInput = new WeaponInput();
    IMovementCompoent enemyMovement;
  
    public EnemyStandMoveStateNode(Enemy enemy) : base(enemy)
    {
        this.objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
    }

    public EnemyStandMoveStateNode(Enemy enemy, Func<bool> preCondition, Func<bool> isReset) : base(enemy, preCondition, isReset)
    {
        this.objectToward = new RotateObjectToward();
        this.agent = enemy.agent;
        this.enemyMovement = enemy.enemyMovement;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        enemy.motionControlManager.ChangeMotionState(enemy.motionControlManager.codeDrivenMotionState);

        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.Move);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {

        //enemy.curMoveVelocity_World = Vector3.MoveTowards(enemy.curMoveVelocity_World, Vector3.ClampMagnitude(enemy.moveInputVelocity_WorldCommand, enemy._moveMaxSpeed), enemy._moveAccelerate * Time.deltaTime);
        //objectToward.RotateToward(enemy.lookRotationCommand, enemy.gameObject, enemy._rotateSpeed);
        //this.agent.Move(enemy.curMoveVelocity_World * Time.deltaTime);
        this.enemyMovement.MoveToDirWorld(enemy.moveInputVelocity_WorldCommand, enemy.moveAccelerate, enemy.moveMaxSpeed);

        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        return base.IsReset();
    }

    public override bool PreCondition()
    {
        return base.PreCondition();
    }

    public override void Update()
    {
        //Vector3 moveInputDirWorld = enemy.moveInputVelocity_WorldCommand;
        //Animator animator = enemy.animator;

        //Vector3 animDir = enemy.transform.InverseTransformDirection(moveInputDirWorld);
        //animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
        //animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);

        //_enemy.lookRotationCommand = (_enemy.agent.steeringTarget - _enemy.transform.position).normalized;
        weaponInput.InputWeaponUpdate(enemy);


        base.Update();
    }
}
