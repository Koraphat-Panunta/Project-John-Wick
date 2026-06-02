using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStaggerStatusInWorldUIManageNodeLeaf : InWorldUINodeLeaf
{
    private FieldOfView fieldOfView;
    private I_OCM_Attack_Able gunFuAble;
    private ObjectPooling<InWorldUI> objectPooling;
    private Camera camera;
    private LayerMask enemyMask;
    public Dictionary<Enemy, InWorldUI> assignInWorldEnemy;
    private readonly List<Enemy> _enemyListCache = new List<Enemy>();
    private readonly List<Enemy> _detectedCache = new List<Enemy>();
    public EnemyStaggerStatusInWorldUIManageNodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,I_OCM_Attack_Able gunFuAble
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

        _enemyListCache.Clear();
        _enemyListCache.AddRange(assignInWorldEnemy.Keys);

        for (int i = 0; i < _enemyListCache.Count; i++)
        {
            Enemy e = _enemyListCache[i];
            assignInWorldEnemy[e].SetAnchorPosition(e.humanoidBone._headBone.transform.position);
            if (e.isDead)
            {
                objectPooling.ReturnToPool(assignInWorldEnemy[e]);
                assignInWorldEnemy.Remove(e);
                continue;
            }

            if (CheckExecuteTargetInAssinged(e))
                continue;

            if (CheckIsStaggerTargetInAssinged(e))
                continue;

            objectPooling.ReturnToPool(assignInWorldEnemy[e]);
            assignInWorldEnemy.Remove(e);
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
        else if(gunFuAble.executedAbleGunFu  == enemy as I_Got_OCM_Attacked_Able)
        {
            assignInWorldEnemy[enemy].PlayAnimation("ExecuteAble");
            return true;
        }

        return false;

    }
    private bool CheckIsStaggerTargetInAssinged(Enemy enemy)
    {
        if (enemy._isGotExecutedAble)
        {
            assignInWorldEnemy[enemy].PlayAnimation("Stagger");
            return true;
        }
        return false;

    }
    private void UpdateEnemyDetectStagger()
    {
        _detectedCache.Clear();

        foreach (GameObject obj in fieldOfView.FindMultipleTargetsInView(this.enemyMask))
        {
            if (obj.TryGetComponent<BodyPart>(out BodyPart bodyPart) == false)
                continue;

            if (_detectedCache.Contains(bodyPart.enemy))
                continue;

            _detectedCache.Add(bodyPart.enemy);

            if (bodyPart.enemy.isDead
                || bodyPart.enemy.stateManagerNode.TryGetCurNodeLeaf<IGotGunFuExecuteNodeLeaf>())
                continue;

            if (assignInWorldEnemy.ContainsKey(bodyPart.enemy))
                continue;

            if (bodyPart.enemy.isStagger)
            {
                InWorldUI enemyStatusInWorldUI = objectPooling.Get();
                assignInWorldEnemy.Add(bodyPart.enemy, enemyStatusInWorldUI);
            }
        }

        _enemyListCache.Clear();
        _enemyListCache.AddRange(assignInWorldEnemy.Keys);

        for (int i = 0; i < _enemyListCache.Count; i++)
        {
            if (!_detectedCache.Contains(_enemyListCache[i]))
            {
                objectPooling.ReturnToPool(assignInWorldEnemy[_enemyListCache[i]]);
                assignInWorldEnemy.Remove(_enemyListCache[i]);
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        if (assignInWorldEnemy.Count > 0)
        {
            _enemyListCache.Clear();
            _enemyListCache.AddRange(assignInWorldEnemy.Keys);

            for (int i = 0; i < _enemyListCache.Count; i++)
            {
                objectPooling.ReturnToPool(assignInWorldEnemy[_enemyListCache[i]]);
                assignInWorldEnemy.Remove(_enemyListCache[i]);
            }
        }

        base.Exit();
    }
}
