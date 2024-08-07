using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyRoleManager roleManager;
    public TacticManager tacticManager;
    public Enemy enemy;
    public GameObject Target;
    public EnemyAction _enemyAction;
    void Start()
    {
        roleManager = new EnemyRoleManager(this);
        tacticManager = new TacticManager(this);
        roleManager._currentRole = roleManager._chasing;
        roleManager._currentRole.RoleEnter(roleManager);
    }

    // Update is called once per frame
    void Update()
    {
        _enemyAction.Target = this.Target;
        roleManager.Update(this);
    }
}
