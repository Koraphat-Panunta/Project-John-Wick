using UnityEngine;

public class GotHit1_GunFuGotHitNodeLeaf : GunFu_GotHit_NodeLeaf
{
    public GotHit1_GunFuGotHitNodeLeaf(Enemy enemy, GunFu_GotHit_ScriptableObject gunFu_GotHit_ScriptableObject) : base(enemy, gunFu_GotHit_ScriptableObject)
    {
    }

    public override void Enter()
    {
        animator.CrossFade(stateName,0.005f, 0);
        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.GunFuGotHit);

        
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
        if(_isExit)
            return true;

        return false;
    }

    public override void Update()
    {

        new RotateObjectToward().RotateTowardsObjectPos(enemy.attackedPos, enemy.gameObject, 20 * Time.deltaTime);
        base.Update();
    }
}
