using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyRoleManager roleManager;
    public TacticManager tacticManager;
    public Enemy enemy;
    void Start()
    {
        roleManager = new EnemyRoleManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        roleManager.Update(this);
    }
}
