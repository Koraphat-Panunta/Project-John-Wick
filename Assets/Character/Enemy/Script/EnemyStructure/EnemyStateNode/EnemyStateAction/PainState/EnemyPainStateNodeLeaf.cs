using System;
using UnityEngine;

public class EnemyPainStateNodeLeaf : EnemyStateLeafNode,IObserverEnemy
{

    protected Animator animator;
    public float painDuration { get; set; }
    public float time;

    public float miniPainStateDuration { get; protected set; }
    public float mediumPainStateDuration { get; protected set; }
    public float heavyPainStateDuration { get; protected set; }
    public EnemyPainStateNodeLeaf(
        Enemy enemy
        ,Func<bool> preCondition
        , Animator animator
        ,float miniPainStateDuration
        ,float mediumPainStateDuration
        ,float heavyPainStateDuration) : base(enemy,preCondition)
    {
        this.animator = animator;

        this.enemy.AddObserver(this);

        this.miniPainStateDuration = miniPainStateDuration;
        this.mediumPainStateDuration = mediumPainStateDuration;
        this.heavyPainStateDuration = heavyPainStateDuration;
    }
    public override void Enter()
    {

        time = 0;

        switch (enemy.getPosturePainPhase)
        {
            case Enemy.EnemyPosturePainStatePhase.MiniPainState:
                {
                    this.painDuration = this.miniPainStateDuration;
                    break;
                }
            case Enemy.EnemyPosturePainStatePhase.MediumPainState:
                {
                    this.painDuration = this.mediumPainStateDuration;
                    break;
                }
            case Enemy.EnemyPosturePainStatePhase.HeavyPainState: 
                {
                    this.painDuration = this.heavyPainStateDuration;
                    break;
                }
        }

        (enemy._movementCompoent as EnemyMovement).AddForcePush(enemy.forceSave, IMotionImplusePushAble.PushMode.InstanlyMaintainMomentum);
        animator.CrossFade("PainState", 0.1f, 0,0);

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
        if(time >= painDuration)
            isComplete = true;

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
        enemy._movementCompoent.UpdateMoveToDirWorld(moveDirWorldRandom * moveSpeed ,Mathf.Clamp(moveSpeed,1, moveSpeed) , Mathf.Clamp(moveSpeed, 1, moveSpeed), MoveMode.MaintainMomentum);
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

    public void Notify<T>(Enemy enemy, T node)
    {
        if(node is EnemyBodyBulletDamageAbleBehavior.CharacterHitedEventDetail characterHitedEventDetail)
        {
            this.rotateDir = (characterHitedEventDetail.hitPos - new Vector3(this.enemy._root.position.x, characterHitedEventDetail.hitPos.y, this.enemy._root.position.z)).normalized ;
            rotatePower = .5f * Vector3.Dot(enemy.transform.forward * -1,rotateDir);
            Debug.DrawRay(enemy.transform.position, this.rotateDir, Color.blue, 2);

            this.moveDirWorldRandom = Quaternion.Euler(0, UnityEngine.Random.Range(-60, 60), 0) * characterHitedEventDetail.hitDir ;
            this.moveSpeed = UnityEngine.Random.Range(1, 2f);
        }
    }
}
