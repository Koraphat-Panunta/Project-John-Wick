using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyRole 
{
    // Start is called before the first frame update
    public EnemyRole()
    {

    }

    public virtual void RoleEnter(EnemyRoleManager enemyRole)
    {

    }
    public virtual void RoleExit(EnemyRoleManager enemyRole)
    {

    }
    public virtual void RoleUpdate(EnemyBrain enemyBrain) 
    { 
    }
    public virtual void RoleFixedUpdate(EnemyBrain enemyBrain)
    {

    }
}
