using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyDebuger :MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] private string CurrentEnemyState;
    [SerializeField] private float posture;

    [SerializeField] private IPainStateAble.PainPart PainPart;

    [SerializeField,TextArea] 
    private string Debug;
    // Start is called before the first frame update
    void Start()
    {
        this.enemy = GetComponent<Enemy>();
    }

    // UpdateNode is called once per frame
    void Update()
    {
       CurrentEnemyState = this.enemy.enemyStateManagerNode.curNodeLeaf.ToString();
       posture = enemy._posture;
       PainPart = enemy._painPart;
    }
    private void OnDrawGizmos()
    {
       
    }
}
