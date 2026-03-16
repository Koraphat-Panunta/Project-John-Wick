using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPainStateNodeLeaf : EnemyStateLeafNode
    ,IObserverEnemy
    ,INodeLeafTransitionAble
{

    public INodeManager nodeManager { get => this.enemy.enemyStateManagerNode; set { } }
    public Dictionary<INode, bool> transitionAbleNode { get ; set ; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get ; set ; }

    public float painDuration { get; set; }
    public float time;

    public float painStateDuration { get; protected set; }
  

    public EnemyPainStateNodeLeaf(
        Enemy enemy
        ,Func<bool> preCondition
        , Animator animator
        ,float painStateDuration
        ) : base(enemy,preCondition)
    {

        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
        this.painStateDuration = painStateDuration;

        this.enemy.AddObserver(this);
    }
    public override void Enter()
    {

        time = 0f;
        this.painDuration = this.painStateDuration;

        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void UpdateNode()
    {
        time += Time.deltaTime;
        rotatePower = Mathf.Clamp(rotatePower - (Time.deltaTime * rotatePowerDecrease),0,maxRotatePower);
        this.moveSpeed = Mathf.Clamp(this.moveSpeed - (Time.deltaTime * this.moveSpeedDecrease),0,10);
        if (time >= painDuration)
        {
            this.nodeLeafTransitionBehavior.TransitionAbleAll(this);
            isComplete = true;
            this.TransitioningCheck();
        }

    }
    public override bool IsComplete()
    {
        return base.IsComplete();
    }
    public override bool IsReset()
    {
        if(IsComplete())
            return true; 

        if(enemy._isPainTrigger)
            return true;

        if(enemy._triggerHitedGunFu)
            return true;

        if(enemy.isDead)
            return true;

        else return false;
    }

    public override void FixedUpdateNode()
    {
        if(enemy._movementCompoent.curMoveVelocity_World.magnitude > enemy.StandMoveMaxSpeed)
        {
            this.enemy._movementCompoent.SetVelocityWorld(enemy._movementCompoent.curMoveVelocity_World.normalized * enemy.StandMoveMaxSpeed);
        }

        enemy._movementCompoent.UpdateMoveToDirWorld(moveDirWorldRandom.normalized * moveSpeed ,Mathf.Clamp(moveSpeed,1, moveSpeed), MoveMode.MaintainMomentumDirection);
        enemy._movementCompoent.SetRotateToDirWorld(this.rotateDir, this.rotatePower);
        base.FixedUpdateNode();
    }

    private float rotatePower;
    private float maxRotatePower = 10;
    private float rotatePowerDecrease = 0.05f;
    private Vector3 rotateDir;

    private Vector3 moveDirWorldRandom;
    private float moveSpeed;
    private float moveSpeedDecrease = 1.5f;

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if(node is EnemyBodyBulletDamageAbleBehavior.CharacterHitedEventDetail characterHitedEventDetail)
        {
            if (Vector3.Dot(characterHitedEventDetail.hitDir * -1, enemy.transform.forward) < .7f)
                this.rotateDir = characterHitedEventDetail.hitDir * -1;
            else
                this.rotateDir = (characterHitedEventDetail.hitPos - new Vector3(this.enemy._root.position.x, characterHitedEventDetail.hitPos.y, this.enemy._root.position.z)).normalized ;
            rotatePower = .5f * Vector3.Dot(enemy.transform.forward * -1,rotateDir);
            //Debug.DrawRay(enemy.transform.position, this.rotateDir, Color.blue, 2);

            this.moveDirWorldRandom = Quaternion.Euler(0, UnityEngine.Random.Range(-60, 60), 0) * characterHitedEventDetail.hitDir ;
            this.moveSpeed = UnityEngine.Random.Range(1, 2f);
        }
    }

    public void SetPainStateDuration(float painStateDuration) => this.painStateDuration = painStateDuration;

    public bool TransitioningCheck()
    {
        return this.nodeLeafTransitionBehavior.TransitioningCheck(this);
    }

    public void AddTransitionNode(INode node)
    {
        this.nodeLeafTransitionBehavior.AddTransistionNode(this,node);
    }
}
