using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStaggerStatusInWorldUIManageNodeLeaf : InWorldUINodeLeaf
{
    private FieldOfView fieldOfView;
    private IGunFuAble gunFuAble;
    private ObjectPooling<InWorldUI> objectPooling;
    private Camera camera;
    private LayerMask enemyMask;
    public Dictionary<Enemy, InWorldUI> assignInWorldEnemy;
    public EnemyStaggerStatusInWorldUIManageNodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,IGunFuAble gunFuAble
        , InWorldUI enemyStatusInWorldUI) : base(preCondition)
    {
        this.fieldOfView = new FieldOfView(19f,90,camera.transform);
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

        List<Enemy> enemyList = assignInWorldEnemy.Keys.ToList<Enemy>();

        for (int i = 0; i < enemyList.Count; i++) 
        {
            assignInWorldEnemy[enemyList[i]].SetAnchorPosition(enemyList[i].head.transform.position);
            if (enemyList[i].isDead){
                objectPooling.ReturnToPool(assignInWorldEnemy[enemyList[i]]);
                assignInWorldEnemy.Remove(enemyList[i]);
                //Debug.Log(enemyList[i] + "is dead");
                continue;
            }

            if (CheckExecuteTargetInAssinged(enemyList[i])){
                //Debug.Log(enemyList[i] + "is executeAble");
                continue;
            }

            if (CheckIsStaggerTargetInAssinged(enemyList[i])){
                //Debug.Log(enemyList[i] + "is Stagger");
                continue;
            }

            //Debug.Log(enemyList[i] + " last");
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
        else if(gunFuAble.executedAbleGunFu  == enemy as IGotGunFuAttackedAble)
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

        foreach (GameObject obj in fieldOfView.FindMultipleTargetsInView(this.enemyMask))
        {

            if (obj.TryGetComponent<BodyPart>(out BodyPart bodyPart) == false)
                continue;

            if (enemyDected.Contains(bodyPart.enemy))
                continue;

            enemyDected.Add(bodyPart.enemy);

            if(bodyPart.enemy.isDead
                ||bodyPart.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<IGotGunFuExecuteNodeLeaf>())
                continue;

            if (assignInWorldEnemy.ContainsKey(bodyPart.enemy))
                continue;

            if (bodyPart.enemy.isStagger)
            {
                InWorldUI enemyStatusInWorldUI = objectPooling.Get();
                assignInWorldEnemy.Add(bodyPart.enemy,enemyStatusInWorldUI);
            }
        }

        List<Enemy> assignEnemy = assignInWorldEnemy.Keys.ToList();

        for (int i = 0; i < assignEnemy.Count; i++) 
        {
            if (enemyDected.Contains(assignEnemy[i]) == false)
            {
                objectPooling.ReturnToPool(assignInWorldEnemy[assignEnemy[i]]);
                assignInWorldEnemy.Remove(assignEnemy[i]);
            }
        }
    }
}
