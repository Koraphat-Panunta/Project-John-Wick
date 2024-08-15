using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] public Animator animator;
    [SerializeField] public WeaponSocket weaponSocket;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public GameObject Target;
    [SerializeField] public EnemyStateManager enemyStateManager;
    [SerializeField] public EnemyWeaponCommand enemyWeaponCommand;
    [SerializeField] public EnemyPath enemyPath;
    public IEnemyTactic currentTactic;

    private RotateObjectToward rotateObjectToward;
    // Start is called before the first frame update
    void Start()
    {
        enemyStateManager.enemy = this;
        enemyWeaponCommand.enemy = this;
        enemyPath = new EnemyPath(agent);
        currentTactic = new Flanking(this);
       
    }

    // Update is called once per frame
    void Update()
    {
        currentTactic.Manufacturing();
    }
    private void FixedUpdate()
    {

    }

    public override void Aiming(Weapon weapon)
    {
        //Weapon Send Sensing
        rotateObjectToward.RotateTowards(Target, gameObject, 6);
    }

    public override void Firing(Weapon weapon)
    {
        
    }

    public override void LowReadying(Weapon weapon)
    {
        
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        
    }
}
