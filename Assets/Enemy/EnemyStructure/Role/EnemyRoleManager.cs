using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoleManager 
{
    public EnemyRole _currentRole;
    public ChasingRole _chasing { get; private set; }
    public EnemyBrain _enemyBrain { get; private set; }
    // Start is called before the first frame update
    public EnemyRoleManager(EnemyBrain enemyBrain)
    {
        _enemyBrain = enemyBrain;
        _chasing = new ChasingRole();
    }

   public void FixedUpdate(EnemyBrain enemyBrain)
    {
        _currentRole.RoleFixedUpdate(enemyBrain);
    }
    public void Update(EnemyBrain enemyBrain)
    {
        _currentRole.RoleUpdate(enemyBrain);
    }
    public void ChangeRole(EnemyRole NextRole)
    {
        if(NextRole != null)
        {
            _currentRole.RoleExit(this);
        }
        _currentRole = NextRole;
        _currentRole.RoleEnter(this);
    }
}
