using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStatusInWorldUIManageNodeLeaf : InWorldUINodeLeaf
{
    private FieldOfView fieldOfView;
    private IGunFuAble gunFuAble;
    private ObjectPooling<InWorldUI> objectPooling;
    private Camera camera;
    private LayerMask enemyMask;
    public Dictionary<Enemy, InWorldUI> assignInWorldEnemy;
    public EnemyStatusInWorldUIManageNodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,IGunFuAble gunFuAble
        , InWorldUI enemyStatusInWorldUI) : base(preCondition)
    {
        this.fieldOfView = new FieldOfView(7.5f,90,camera.transform);
        this.gunFuAble = gunFuAble;
        objectPooling = new ObjectPooling<InWorldUI>(enemyStatusInWorldUI,10,5,Vector3.zero);
        assignInWorldEnemy = new Dictionary<Enemy, InWorldUI>();


        this.enemyMask.value = LayerMask.GetMask("Enemy");
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

        List<Enemy> enemyList = assignInWorldEnemy.Keys.ToList<Enemy>();

        for (int i = 0; i < enemyList.Count; i++) 
        {
            assignInWorldEnemy[enemyList[i]].SetAnchorPosition(enemyList[i].transform.position);
            if (enemyList[i].isDead)
            {
                objectPooling.ReturnToPool(assignInWorldEnemy[enemyList[i]]);
                assignInWorldEnemy.Remove(enemyList[i]);
                continue;
            }

            if (CheckExecuteTargetInAssinged(enemyList[i]))
                continue;

            if (CheckIsStaggerTargetInAssinged(enemyList[i]))
                continue;

            objectPooling.ReturnToPool(assignInWorldEnemy[enemyList[i]]);
            assignInWorldEnemy.Remove(enemyList[i]);
        }
       
    }
    private bool CheckExecuteTargetInAssinged(Enemy enemy)
    {
        if (gunFuAble.executedAbleGunFu == null)
            return false;

        if(gunFuAble.curGunFuNode is IGunFuExecuteNodeLeaf
                && (gunFuAble.executedAbleGunFu is BodyPart bodyPart)
                && bodyPart.enemy == enemy)
        {
            assignInWorldEnemy[enemy].PlayAnimation("ExecuteTrigger");
            return true;
        }
        else if(gunFuAble.executedAbleGunFu is BodyPart bodyPart2
            && bodyPart2.enemy == enemy)
        {
            assignInWorldEnemy[enemy].PlayAnimation("ExecuteAble");
            return true;
        }

        return false;

    }
    private bool CheckIsStaggerTargetInAssinged(Enemy enemy)
    {
        if (enemy.isStagger)
        {
            assignInWorldEnemy[enemy].PlayAnimation("Stagger");
            return true;
        }
        return false;

    }
    private void UpdateEnemyDetectStagger()
    {

        List<Enemy> enemyDected = new List<Enemy>();

        foreach (GameObject obj in fieldOfView.FindMultipleTargetsInView(this.enemyMask.value))
        {

            if (obj.TryGetComponent<BodyPart>(out BodyPart headBodyPart) == false)
                return;

            if (enemyDected.Contains(headBodyPart.enemy))
                return;

            enemyDected.Add(headBodyPart.enemy);

            if(headBodyPart.enemy.isDead
                ||headBodyPart.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<IGotGunFuExecuteNodeLeaf>())
                return;

            if (assignInWorldEnemy.ContainsKey(headBodyPart.enemy))
                return;

            if (headBodyPart.enemy.isStagger)
            {
                InWorldUI enemyStatusInWorldUI = objectPooling.Get();
                assignInWorldEnemy.Add(headBodyPart.enemy,enemyStatusInWorldUI);
            }
        }

        List<Enemy> assignEnemy = assignInWorldEnemy.Keys.ToList();

        for (int i = 0; i < assignEnemy.Count; i++) 
        {
            if (enemyDected.Contains(assignEnemy[i]) == false)
                assignInWorldEnemy.Remove(assignEnemy[i]);
        }
    }
}
