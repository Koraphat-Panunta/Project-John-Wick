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
    private List<InWorldUI> enemyStatusInWorldUIs;
    public EnemyStatusInWorldUIManageNodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,IGunFuAble gunFuAble
        , InWorldUI enemyStatusInWorldUI) : base(preCondition)
    {
        this.fieldOfView = new FieldOfView(7.5f,camera.fieldOfView,camera.transform);
        this.gunFuAble = gunFuAble;
        objectPooling = new ObjectPooling<InWorldUI>(enemyStatusInWorldUI,10,5,Vector3.zero);
        enemyStatusInWorldUIs = new List<InWorldUI>();
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

            if (headBodyPart.enemy.isStagger)
            {
                InWorldUI enemyStatusInWorldUI = objectPooling.Get();
                enemyStatusInWorldUIs.Add(enemyStatusInWorldUI);
                enemyDected.Add(headBodyPart.enemy);
            }
        }
    }
}
