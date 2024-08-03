using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingRole : EnemyRole
{
    public TacticManager tacticManager { get;private set; }
    public ChasingRole()
    {
        tacticManager = new TacticManager();
    }
    public override void RoleEnter(EnemyRoleManager enemyBrain)
    {
        
    }
    public override void RoleExit(EnemyRoleManager enemyBrain)
    {
       
    }

    public override void RoleFixedUpdate(EnemyBrain enemyBrain)
    {
    }
    public override void RoleUpdate(EnemyBrain enemyBrain)
    {
        
    }
}
