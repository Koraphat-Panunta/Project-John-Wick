using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusInWorldUIManageNodeLeaf : InWorldUINodeLeaf
{
    private FieldOfView fieldOfView;
    private IGunFuAble gunFuAble;
    private ObjectPooling<EnemyStatusInWorldUI> objectPooling;
    private Camera camera;
    private LayerMask enemyMask;
    private List<EnemyStatusInWorldUI> enemyStatusInWorldUIs;
    public EnemyStatusInWorldUIManageNodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,IGunFuAble gunFuAble
        ,EnemyStatusInWorldUI enemyStatusInWorldUI) : base(preCondition)
    {
        this.fieldOfView = new FieldOfView(7.5f,camera.fieldOfView,camera.transform);
        this.gunFuAble = gunFuAble;
        objectPooling = new ObjectPooling<EnemyStatusInWorldUI>(enemyStatusInWorldUI,10,5,Vector3.zero);
        enemyStatusInWorldUIs = new List<EnemyStatusInWorldUI>();
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
        if(enemyStatusInWorldUIs.Count <= 0)
            return;

        for (int i = 0; i < enemyStatusInWorldUIs.Count; i++) 
        {
            if (enemyStatusInWorldUIs[i].curEnemyStatusInWorldUIPhase == EnemyStatusInWorldUI.EnemyStatusInWorldUIPhase.none) 
            {
                objectPooling.ReturnToPool(enemyStatusInWorldUIs[i]);
            }
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

            if (headBodyPart.enemy.isStagger)
            {
                EnemyStatusInWorldUI enemyStatusInWorldUI = objectPooling.Get();
                enemyStatusInWorldUIs.Add(enemyStatusInWorldUI);
                enemyDected.Add(headBodyPart.enemy);
            }
        }
    }
}
