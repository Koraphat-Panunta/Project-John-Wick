using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearingSensing : IEnemySensing,IEnvironmentAware
{
    private Enemy enemy;
    private Player player;
    public EnemyHearingSensing(Enemy enemy) 
    {
        this.enemy = enemy;
        enemy.StartCoroutine(ListeningEnvironment());
    }

    public void OnAware(GameObject sourceFrom, EnvironmentType environmentType)
    {
        if(sourceFrom.TryGetComponent<Player>(out Player player)&&environmentType == EnvironmentType.Sound)
        {
            if (Vector3.Distance(sourceFrom.transform.position, enemy.transform.position) <= 30)
            {
                enemy.Target.transform.position = new Vector3(sourceFrom.transform.position.x,sourceFrom.transform.position.y,sourceFrom.transform.position.z);
                if(enemy.isIncombat == false)
                {
                    enemy.currentTactic = new FlankingTactic(enemy);
                }
            }
        }
    }

    public void Recived()
    {
        
    }
    IEnumerator ListeningEnvironment()
    {
        yield return new WaitForEndOfFrame();
        if(this.enemy.My_environment == null)
        {
            Debug.Log("Environment Null");
        }
        if(this == null)
        {
            Debug.Log("This is Null");
        }
        this.enemy.My_environment.Add_Listener(this);
    }
    


  
}
