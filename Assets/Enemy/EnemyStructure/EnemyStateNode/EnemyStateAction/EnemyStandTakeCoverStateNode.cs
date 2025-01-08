using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStandTakeCoverStateNode : EnemyStateLeafNode
{
    ICoverUseable coverUseable;
    RotateObjectToward rotateObject;
    NavMeshAgent agent;
    public EnemyStandTakeCoverStateNode(Enemy enemy,ICoverUseable coverUseable) : base(enemy)
    {
        this.coverUseable = coverUseable;
        rotateObject = new RotateObjectToward();
        agent = enemy.agent;
    }

    public override List<EnemyStateNode> childNode { get => base.childNode; set => base.childNode = value; }
    protected override Func<bool> preCondidtion { get => base.preCondidtion; set => base.preCondidtion = value; }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsReset()
    {
        if(enemy.isInCover == false)
            return true;

        if(enemy.isAiming)
            return true;

        if(enemy.isPainTrigger)
            return true;

        return false;
    }

    public override bool PreCondition()
    {
        if(enemy.isInCover)
            return true;

        return false;
    }

    public override void Update()
    {
        
        Vector3 CoverPos = coverUseable.coverPos;

      
        enemy.weaponCommand.LowReady();

        Vector3 moveDir = (CoverPos - enemy.transform.position).normalized * Time.deltaTime * 2;
        agent.Move(moveDir);

        Vector3 moveInputDirWorld = enemy.moveInputVelocity_World;
        Animator animator = enemy.animator;

        Vector3 animDir = enemy.transform.InverseTransformDirection(moveInputDirWorld);
        animator.SetFloat("Vertical", animDir.z, 0.5f, Time.deltaTime);
        animator.SetFloat("Horizontal", animDir.x, 0.1f, Time.deltaTime);

        if (coverUseable.coverPoint == null)
        {
            Debug.Log("this Null");
        }
        new RotateObjectToward().RotateToward(coverUseable.coverPoint.coverDir, enemy.gameObject, 6);

        base.Update();
    }
}
