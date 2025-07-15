using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusInWorldUIManageNodeLeaf : InWorldUINodeLeaf
{
    private FieldOfView fieldOfView;
    private IGunFuAble gunFuAble;
    private ObjectPooling<InWorldUI> objectPooling;
    private Camera camera;
    private LayerMask enemyMask;
    private Dictionary<Enemy, InWorldUI> assignInWorldEnemy;
    public EnemyStatusInWorldUIManageNodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,IGunFuAble gunFuAble
        , InWorldUI enemyStatusInWorldUI) : base(preCondition)
    {
        this.fieldOfView = new FieldOfView(7.5f,camera.fieldOfView,camera.transform);
        this.gunFuAble = gunFuAble;
        objectPooling = new ObjectPooling<InWorldUI>(enemyStatusInWorldUI,10,5,Vector3.zero);
        assignInWorldEnemy = new Dictionary<Enemy, InWorldUI>();
        this.enemyMask = LayerMask.GetMask("Enemy");
    }
    public override void FixedUpdateNode()
    {
        UpdateUIActivate();
        UpdateEnemyDetectStagger();
        base.FixedUpdateNode();
    }
    private void UpdateUIActivate()
    {
        if(assignInWorldEnemy.Count <= 0)
            return;

        for (int i = 0; i < assignInWorldEnemy.Count; i++) 
        {
            //if (enemyStatusInWorldUIs[i].curEnemyStatusInWorldUIPhase == EnemyStatusInWorldUI.EnemyStatusInWorldUIPhase.none) 
            //{
            //    objectPooling.ReturnToPool(enemyStatusInWorldUIs[i]);
            //}
        }
       
    }
    private void UpdateEnemyDetectStagger()
    {

        List<Enemy> enemyDected = new List<Enemy>();

        foreach (GameObject obj in fieldOfView.FindMultipleTargetsInView(this.enemyMask))
        {

            if (obj.TryGetComponent<HeadBodyPart>(out HeadBodyPart headBodyPart) == false)
                return;

            if (enemyDected.Contains(headBodyPart.enemy))
                return;

            enemyDected.Add(headBodyPart.enemy);

            if (assignInWorldEnemy.ContainsKey(headBodyPart.enemy))
                return;

            if (headBodyPart.enemy.isStagger)
            {
                InWorldUI enemyStatusInWorldUI = objectPooling.Get();
                assignInWorldEnemy.Add(headBodyPart.enemy,enemyStatusInWorldUI);
            }
        }
    }
}
