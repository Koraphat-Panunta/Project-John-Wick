using UnityEngine;
[RequireComponent(typeof(EnemyCommandAPI))]
[RequireComponent(typeof(Enemy))]
public abstract class EnemyDecision : MonoBehaviour
{
    public abstract EnemyCommandAPI enemyCommand { get; set; }
    public Enemy enemy;
    protected virtual void Awake()
    {
        enemyCommand = GetComponent<EnemyCommandAPI>();
        this.enemy = GetComponent<Enemy>();
        this.enemy.NotifyGotHearing += OnNotifyHearding;
    }
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

    }
    protected abstract void OnNotifyHearding(INoiseMakingAble noiseMaker);
    protected abstract void OnNotifySpottingTarget(GameObject target);
     
    
}
