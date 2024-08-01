using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoleManager : MonoBehaviour
{
    public EnemyRole _currentRole;
    public Enemy _enemy;
    // Start is called before the first frame update
    public EnemyRoleManager()
    {

    }

   public void FixedUpdate()
    {
        _currentRole.RoleFixedUpdate(this);
    }
    public void Update()
    {
        _currentRole.RoleUpdate(this);
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
