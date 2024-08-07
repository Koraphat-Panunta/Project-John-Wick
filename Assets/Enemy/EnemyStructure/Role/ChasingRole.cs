using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingRole : EnemyRole
{
    public TacticManager tacticManager { get;private set; }
    public ChasingRole()
    {
    }
    public override void RoleEnter(EnemyRoleManager enemyRoleManager)
    {
        if (tacticManager == null)
        {
            tacticManager = enemyRoleManager._enemyBrain.tacticManager;
        }
        tacticManager.ChangeTactic(tacticManager._flanking);
    }
    public override void RoleExit(EnemyRoleManager enemyRoleManager)
    {
       
    }

    public override void RoleFixedUpdate(EnemyBrain enemyRoleManager)
    {
        tacticManager.FixedUpdate(enemyRoleManager);
    }
    public override void RoleUpdate(EnemyBrain enemyRoleManager)
    {
        tacticManager.Update(enemyRoleManager);
    }
}
