using UnityEngine;
public class EnemyStatusInWorldUI : InWorldUI
{
    public Enemy myEnemy;
    public IGunFuAble gunFuAble;
    public override Vector3 _anchorPos => myEnemy.transform.position;
    [SerializeField] private Vector3 offsetUIPos;
    public override Vector3 _offsetUIPos => this.offsetUIPos;
    [SerializeField] private Animator _animator;
    public enum EnemyStatusInWorldUIPhase
    {
        none,
        stagger,
        executeAble,
    }
    public EnemyStatusInWorldUIPhase curEnemyStatusInWorldUIPhase { 
        get
        {
            if (gunFuAble.executedAbleGunFu is BodyPart bodyPart
                && bodyPart.enemy == myEnemy)
            {
                return EnemyStatusInWorldUIPhase.executeAble;
            }
            else if (myEnemy.isStagger)
                return EnemyStatusInWorldUIPhase.stagger;
            else
                return EnemyStatusInWorldUIPhase.none;
        } }

    public void Activate(Enemy enemy,IGunFuAble gunFuAble)
    {
        this.myEnemy = enemy;
        this.gunFuAble = gunFuAble;
    }
    public void Deactivate()
    {
        this.myEnemy = null;
        this.gunFuAble = null;
    }
    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        base.Awake();
    }
    protected override void FixedUpdate()
    {
        if(curEnemyStatusInWorldUIPhase == EnemyStatusInWorldUIPhase.stagger)
        {
            _animator.CrossFade("staggerUI",0.1f,0);
            //playAnimatorStaggerUI
        }
        else if(curEnemyStatusInWorldUIPhase == EnemyStatusInWorldUIPhase.executeAble)
        {
            _animator.CrossFade("executeAbleUI", 0.1f, 0);
            //playAnimatorExecuteAbleUI
        }
        base.FixedUpdate();
    }

  
}
