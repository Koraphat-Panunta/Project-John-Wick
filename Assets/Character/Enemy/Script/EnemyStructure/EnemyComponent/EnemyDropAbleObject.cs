using UnityEngine;

/// <summary>
/// Bridges enemy death events to the generic Dropper. The "what to drop" decision
/// has moved to the LootTableScriptableObject assigned on the sibling Dropper —
/// this component just listens for the right enemy state and triggers the drop.
/// </summary>
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Dropper))]
public class EnemyDropAbleObject : MonoBehaviour, IObserverEnemy, IInitializedAble
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] private Dropper dropper;
    [SerializeField] private LootTableScriptableObject executedLootTable;

    private bool isAlreadyDrop;
    private bool isBeenExecute;

    private void Reset()
    {
        if (enemy == null) enemy = GetComponent<Enemy>();
        if (dropper == null) dropper = GetComponent<Dropper>();
    }

    public void Initialized()
    {
        if (enemy == null) enemy = GetComponent<Enemy>();
        if (dropper == null) dropper = GetComponent<Dropper>();
        enemy.AddObserver(this);
    }

    public void OnNotify<T>(Enemy enemy, T node)
    {
        if (node is SubjectEnemy.EnemyEvent enemyEvent
            && enemyEvent == SubjectEnemy.EnemyEvent.OnEnable)
        {
            isBeenExecute = false;
            isAlreadyDrop = false;
        }

        if (node is IGotGunFuExecuteNodeLeaf)
            isBeenExecute = true;

        if (node is EnemyDeadStateNode deadState
            && deadState.curstate == EnemyStateLeafNode.Curstate.Enter
            && !isAlreadyDrop)
        {
            if (isBeenExecute)
                dropper.Drop(executedLootTable != null ? executedLootTable : null);

            isAlreadyDrop = true;
        }
    }
}
