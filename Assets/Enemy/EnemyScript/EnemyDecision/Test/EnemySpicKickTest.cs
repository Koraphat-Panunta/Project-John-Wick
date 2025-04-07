using UnityEngine;

public class EnemySpicKickTest : EnemyDecision
{
    public override EnemyCommandAPI enemyCommand { get ; set ; }
    [SerializeField] private Transform target;
    public enum EnemySpicKickTestState
    {
        moveToTarget,
        spinKicking,
        rest
    }
    public EnemySpicKickTestState curEnemySpicKickTestState;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        curEnemySpicKickTestState = EnemySpicKickTestState.moveToTarget;
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    float restTime;
    protected override void FixedUpdate()
    {
        if(curEnemySpicKickTestState == EnemySpicKickTestState.moveToTarget)
        {
            if(enemyCommand.MoveToPositionRotateToward(target.position, 1, 1, 2.5f))
            {
                curEnemySpicKickTestState = EnemySpicKickTestState.spinKicking;
            }
        }
        else if(curEnemySpicKickTestState == EnemySpicKickTestState.spinKicking)
        {
            enemyCommand.SpinKick();
            curEnemySpicKickTestState = EnemySpicKickTestState.rest;
        }
        else if(curEnemySpicKickTestState == EnemySpicKickTestState.rest)
        {
            restTime += Time.deltaTime;
            if(restTime > 1)
            {
                curEnemySpicKickTestState = EnemySpicKickTestState.moveToTarget;
                restTime = 0;
            }
        }
        base.FixedUpdate();
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker)
    {
        
    }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        
    }
}
