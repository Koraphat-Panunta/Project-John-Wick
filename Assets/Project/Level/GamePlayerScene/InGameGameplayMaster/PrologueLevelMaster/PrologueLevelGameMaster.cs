using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PrologueLevelGameMaster : InGameLevelGameMaster
{
    public override InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelMisstionCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public override InGameLevelRestGameMasterNodeLeaf levelRestGameMasterNodeLeaf { get; protected set; }
    public override PauseInGameGameMasterNodeLeaf pauseInGameGameMasterNodeLeaf { get ; protected set ; }
    public override InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get ; protected set ; }
    protected PrologueInGameLevelGameplayGameMasterNodeLeaf prologueInGameLevelGameplayGameMasterNodeLeaf;

    [SerializeField] private DoorKeyItem key;

    public Door door_A21;
    public Door door_A31;
    public Door door_A41_locked;
    public Door door_A42;
    public Door door_A43_locked;
    public Door door_A52;

    public EnemyDirector enemyDirectorA2;
    public List<EnemyDecision> enemyA2 = new List<EnemyDecision>();
    public EnemyDirector enemyDirectirA3 ;
    public List<EnemyDecision> enemyA3 = new List<EnemyDecision>();
    public EnemyDirector enemyDirectirA4;
    public List<EnemyDecision> enemyA4 = new List<EnemyDecision>();
    public EnemyDirector enemyDirectirA5;
    public List<EnemyDecision> enemyA5 = new List<EnemyDecision>();

    public GameObject movementHint;
    public GameObject pickUpWeaponHint;
    public GameObject openTheDoorHint;
    public GameObject shootHint;
    public GameObject reloadHint;
    public GameObject executeHint;
    public GameObject executeHP_refillHint;
    public GameObject showGunFuCommboHint;

    protected override void Start()
    {
        door_A43_locked.isLocked = true;
        door_A41_locked.isLocked = true;
        base.Start();
    }
    protected override void FixedUpdate()
    {
        try
        {
            if (Vector3.Distance(player.transform.position, key.transform.position) < 1f)
            {
                door_A43_locked.isLocked = false;
                door_A41_locked.isLocked = false;
                Destroy(key.gameObject);
            }
        }
        catch 
        {
            Debug.Log("LockedDoor is open");
        }
        base.FixedUpdate();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=>true,"PrologueStartNodeSelector");

        delayOpeningGameMasterNodeLeaf = new InGameLevelDelayOpeningLoad(this, () => base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        prologueInGameLevelGameplayGameMasterNodeLeaf = new PrologueInGameLevelGameplayGameMasterNodeLeaf(this,()=> prologueInGameLevelGameplayGameMasterNodeLeaf.IsComplete() == false);
        pauseInGameGameMasterNodeLeaf = new PauseInGameGameMasterNodeLeaf(this, pauseCanvasUI,
            () => pauseInGameGameMasterNodeLeaf.isPause);
        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelMisstionCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => true);

        startNodeSelector.AddtoChildNode(delayOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pauseInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(prologueInGameLevelGameplayGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);

    }

    private void OnValidate()
    {
        if(enemyDirectorA2 != null)
        {
            foreach(EnemyRoleBasedDecision enemyRoleBasedDecision in enemyDirectorA2.GetAllEnemyRoleBasedDecision())
            {
                if(enemyA2.Contains(enemyRoleBasedDecision) == false)
                    enemyA2.Add(enemyRoleBasedDecision);
            }
        }
        if (enemyDirectirA3 != null)
        {
            foreach (EnemyRoleBasedDecision enemyRoleBasedDecision in enemyDirectirA3.GetAllEnemyRoleBasedDecision())
            {
                if (enemyA3.Contains(enemyRoleBasedDecision) == false)
                    enemyA3.Add(enemyRoleBasedDecision);
            }
        }
        if (enemyDirectirA4 != null)
        {
            foreach (EnemyRoleBasedDecision enemyRoleBasedDecision in enemyDirectirA4.GetAllEnemyRoleBasedDecision())
            {
                if (enemyA4.Contains(enemyRoleBasedDecision) == false)
                    enemyA4.Add(enemyRoleBasedDecision);
            }
        }
        if (enemyDirectirA5 != null)
        {
            foreach (EnemyRoleBasedDecision enemyRoleBasedDecision in enemyDirectirA5.GetAllEnemyRoleBasedDecision())
            {
                if (enemyA5.Contains(enemyRoleBasedDecision) == false)
                    enemyA5.Add(enemyRoleBasedDecision);
            }
        }
    }

}

