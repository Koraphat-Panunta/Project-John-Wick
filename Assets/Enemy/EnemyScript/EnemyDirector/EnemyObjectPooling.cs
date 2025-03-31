using UnityEngine;
using System.Collections.Generic;

public class EnemyObjectPooling : MonoBehaviour
{
    [SerializeField] private Queue<Enemy> enemies = new Queue<Enemy>();
  

    public Enemy PullEnemy() 
    {  
        Enemy enemy = enemies.Dequeue();
        enemy.gameObject.SetActive(true);
        return enemy;
    }
    public void ReturnEnemy(Enemy enemy) 
    {
        enemy.gameObject.SetActive(false);
        enemies.Enqueue(enemy); 
    }
}
