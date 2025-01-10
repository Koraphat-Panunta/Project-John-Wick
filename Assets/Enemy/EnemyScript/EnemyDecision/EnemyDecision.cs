using UnityEngine;
[RequireComponent(typeof(EnemyCommandAPI))]
public abstract class EnemyDecision : MonoBehaviour
{
    public abstract EnemyCommandAPI enemyCommand { get; set; }
    public abstract Enemy enemy { get; set; }
    protected virtual void Start()
    {
        enemyCommand = GetComponent<EnemyCommandAPI>();
        this.enemy = enemyCommand._enemy;
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {

    }
}
