using UnityEngine;
using System.Collections.Generic;

public class EnemyObjectPooling : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    private Queue<Enemy> enemiesQ = new Queue<Enemy>();

    private void Awake()
    {
        enemies.ForEach(enemy => enemiesQ.Enqueue(enemy));
    }
    public Enemy PullEnemy() 
    {  
        Enemy enemy = enemiesQ.Dequeue();
        enemy.gameObject.SetActive(true);
        return enemy;
    }
    public void ReturnEnemy(Enemy enemy) 
    {
        enemy.gameObject.SetActive(false);
        enemiesQ.Enqueue(enemy); 
    }
}
