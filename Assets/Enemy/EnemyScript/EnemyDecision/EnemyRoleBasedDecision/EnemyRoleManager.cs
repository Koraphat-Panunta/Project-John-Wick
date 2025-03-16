using UnityEngine;
using System.Collections.Generic;

public class EnemyRoleManager : MonoBehaviour, IObserverEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected List<Enemy> enemies = new List<Enemy>();


    [SerializeField] private int MAX_ChaserCount;
    [SerializeField] private int chaserCount;
    [SerializeField] private int overwatchCount;
    public int allEnemiesAliveCount => enemies.Count;
    private void Awake()
    {
        foreach (Enemy enemy in enemies) 
        {
            enemy.AddObserver(this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AssignEnemy(Enemy enemy) => enemies.Add(enemy);
    public void RemoveEnemy(Enemy enemy) => enemies.Remove(enemy);

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        if(enemyEvent == SubjectEnemy.EnemyEvent.Dead) 
        {
            RemoveEnemy(enemy);
            enemy.RemoveObserver(this);
        }

    }
}
