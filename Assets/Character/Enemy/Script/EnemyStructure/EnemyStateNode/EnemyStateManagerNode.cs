using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static IPainStateAble;

public partial class EnemyStateManagerNode : INodeManager
{
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager._curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior _nodeManagerBehavior { get; set; }
    public List<INodeManager> _parallelNodeManahger { get ; set ; }
    public NodeComponentManager enemyStateNodeComponentManager { get; set; }
    public Enemy enemy { get; protected set; }
    public EnemyStateManagerNode(Enemy enemy)
    {
        this.enemy = enemy;

        this._nodeManagerBehavior = new NodeManagerBehavior();
        this._parallelNodeManahger = new List<INodeManager>();
        enemyStateNodeComponentManager = new NodeComponentManager();

        InitailizedNode();
    }

    public void FixedUpdateNode()
    {
        this._nodeManagerBehavior.FixedUpdateNode(this);
        this.enemyStateNodeComponentManager.FixedUpdate();
    }
    public void UpdateNode() 
    {
        this._nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
        this.enemyStateNodeComponentManager.Update();
    }
    

    #region Initailized State Node

    public NodeSelector enemyStanceSelector { get; private set; }

    public NodeSelector crouchSelector { get; private set; }
    public EnemyCrouchMoveStateNodeLeaf enemyCrouchMoveStateNodeLeaf { get; private set; }
    public EnemyCrouchIdleStateNodeLeaf enemyCrouchIdleStateNodeLeaf { get; private set; }

    public NodeSelector standSelector { get; private set; }
    public EnemyStandIdleStateNodeLeaf enemyStandIdleStateNodeLeaf { get; private set; }
    public EnemyStandMoveStateNodeLeaf enemyStandMoveStateNodeLeaf { get; private set; }

    public EnemySprintStateNodeLeaf enemySprintStateNodeLeaf { get; private set; }
    public EnemyDodgeRollStateNodeLeaf enemyDodgeRollStateNodeLeaf { get; private set; }

    public NodeSelector zeroPostureSelector { get; private set; }
    public GotGunFuInteractingNodeLeaf gotHitDownNodeLeaf { get; private set; }
    public EnemyPainStateNodeLeaf painStateGotHitDownNodeLeaf { get; private set; }
    public NodeSelector gotExecuteOnGroundSelector { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_I_NodeLeaf { get; private set; }

    public FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf { get; private set; }
    public GetUpStateNodeLeaf enemyStandUpStateNodeLeaf { get; private set; }
    public GetUpStateNodeLeaf enemyPushUpStateNodeLeaf { get; private set; }
    public EnemyDeadStateNode enemtDeadState { get; private set; }



    public NodeSelector gunFuSelector { get; private set; }
    public EnemySpinKickGunFuNodeLeaf enemySpinKickGunFuNodeLeaf { get; private set; }

    public NodeSelector gotGunFuAttackSelector { get; private set; }
    public NodeSelector gotExecuteSelector { get; private set; }

    public GotGunFuExecuteNodeLeaf gotExecute_Dodge_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_II { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_III { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Primary_NodeLeaf_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Primary_NodeLeaf_II { get; private set; }
    public GotGunFuHitNodeLeaf gotGunFuHitNodeLeaf { get; private set; }

    public GotRestrictNodeLeaf gotRestrictNodeLeaf { get; private set; }
    public HumandShield_GotInteract_NodeLeaf gotHumandShielded_GunFuNodeLeaf { get; private set; }
    public HumanShield_Exit_GotInteract_NodeLeaf humanShield_Exit_GotInteract_NodeLeaf { get; private set; }
    public GotGunFuInteractingNodeLeaf gotGunFuReloadNodeLeaf { get; private set; }

    public EnemyPainStateNodeLeaf painStateNodeLeaf { get; private set; }

  
    #endregion
    public void InitailizedNode()
    {
        startNodeSelector = new EnemyStateSelectorNode(enemy, () => true);

        enemyStanceSelector = new NodeSelector(
            () => true
            , nameof(enemyStanceSelector));

        crouchSelector = new NodeSelector(
            () => enemy.stanceCommand == Stance.crouch
            , nameof(crouchSelector));
        enemyCrouchMoveStateNodeLeaf = new EnemyCrouchMoveStateNodeLeaf(enemy,
            () => 
            {
                return enemy.moveInputVelocity_WorldCommand.magnitude > 0;
            });
        enemyCrouchIdleStateNodeLeaf = new EnemyCrouchIdleStateNodeLeaf(enemy,
            () => true);

        standSelector = new NodeSelector(
            () =>enemy.stanceCommand == Stance.stand || true
            ,nameof(standSelector));
        enemyStandIdleStateNodeLeaf = new EnemyStandIdleStateNodeLeaf(this.enemy,
          () => true //Precondition
          );

        enemyStandMoveStateNodeLeaf = new EnemyStandMoveStateNodeLeaf(this.enemy,
            () =>
            {
                if (this.enemy.moveInputVelocity_WorldCommand.magnitude > 0)
                    return true;
                return false;
            }
            );

        enemySprintStateNodeLeaf = new EnemySprintStateNodeLeaf(this.enemy,
           () => this.enemy.isSprintCommand && this.enemy.moveInputVelocity_WorldCommand.magnitude > 0
           );
        enemyDodgeRollStateNodeLeaf = new EnemyDodgeRollStateNodeLeaf(this.enemy
            ,()=> enemy._triggerDodge && enemyDodgeRollStateNodeLeaf.dodgeRollCoolDown <=0
            );

        enemtDeadState = new EnemyDeadStateNode(this.enemy,
            () => this.enemy.isDead 
            );

        zeroPostureSelector = new NodeSelector(
            ()=> 
            {
                //Debug.Log("this.enemy._posture = " + this.enemy._posture);
                if (this.enemy._posture <= 0 && this.enemy.isNotFallAble == false)
                {
                    return true;
                }
                return false;
            }
            );

        this.gotHitDownNodeLeaf = new GotGunFuInteractingNodeLeaf(this.enemy,this.enemy.gotHitDown_ScriptableObject,
            ()=> this.enemy._triggerHitedGunFu 
            && this.enemy.curAttackerGunFuNode is GunFuHitDownNodeLeaf);
        this.painStateGotHitDownNodeLeaf = new EnemyPainStateNodeLeaf(this.enemy, 
            () => true
            , this.enemy.animator, 3);
        gotExecuteOnGroundSelector = new NodeSelector(
            () => this.enemy._triggerHitedGunFu 
            && enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf);
        gotExecute_OnGround_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy
            ,this.enemy.gotGunFu_Execute_OnGround_I
            ,this.enemy._root
            ,this.enemy._hipsBone
            ,this.enemy._bones
            ,GotExecutedStateName.GotExecuted_OnGround_I
            ,()=> enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
            &&( gunFuExecuteNodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Single_Execute_OnGround 
            || gunFuExecuteNodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Single_Execute_OnGround)
            && (enemy as IRagdollAble)._isFacingUp == false
            );
       
        fallDown_EnemyState_NodeLeaf = new FallDown_EnemyState_NodeLeaf(this.enemy, this.enemy,
            () => true
            );
        enemyStandUpStateNodeLeaf = new GetUpStateNodeLeaf(this.enemy,
        ()=> Vector3.Dot(this.enemy._hipsBone.forward, Vector3.up) > 0
        ,this.enemy
        ,this.enemy.standUpAnimationTriggerSCRP
        , "StandUp"
        );
        enemyPushUpStateNodeLeaf = new GetUpStateNodeLeaf(this.enemy,
        () => true
        , this.enemy
        , this.enemy.pushUpAnimationTriggerSCRP
        , "PushUp"
        );

        this.painStateNodeLeaf = new EnemyPainStateNodeLeaf(this.enemy
           , () => enemy._isPainTrigger && enemy.getPosturePainPhase >= Enemy.EnemyPosturePainStatePhase.MiniPainState
           , this.enemy.animator
           , this.enemy.miniPainStateDuration);

        gunFuSelector = new NodeSelector(
            () => enemy._triggerGunFu && enemy._isInPain == false);

        enemySpinKickGunFuNodeLeaf = new EnemySpinKickGunFuNodeLeaf(this.enemy.EnemySpinKickScriptable,this.enemy,()=>true);

        gotGunFuAttackSelector = new NodeSelector( 
            () => 
            {
                if (this.enemy._triggerHitedGunFu)
                {
                    Debug.Log("this.enemy._triggerHitedGunFu");
                    Debug.Log("this.enemy.curAttackNode = " + this.enemy.curAttackerGunFuNode);
                    return true;
                }

                return false;
                }
            );
        gotExecuteSelector = new NodeSelector(
            ()=> enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf);

        gotExecute_Dodge_I = new GotGunFuExecuteNodeLeaf(enemy,
            ()=> 
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
                && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Dodge_Secondary)
                    return true;
                return false;
            }
            ,this.enemy.gotGunFuExecute_Dodge_ScriptableObject_I
            , GotExecutedStateName.GotExecuted_Dodge_Secondary);

        gotExecute_Primary_NodeLeaf_I = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
                && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Single_Primary_I)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Primary_ScriptableObject_I
            ,GotExecutedStateName.GotExecuted_Single_Primary_I);
        gotExecute_Primary_NodeLeaf_II = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
                && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Single_Primary_II)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Primary_ScriptableObject_II
            ,GotExecutedStateName.GotExecuted_Single_Primary_II);
        gotExecute_Secondary_NodeLeaf_I = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
                && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Single_Secondary_I)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Secondary_ScriptableObject_I
            ,GotExecutedStateName.GotExecuted_Single_Secondary_I);
        gotExecute_Secondary_NodeLeaf_II = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
               && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Single_Secondary_II)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Secondary_ScriptableObject_II
            ,GotExecutedStateName.GotExecuted_Single_Secondary_II);
        gotExecute_Secondary_NodeLeaf_III = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
               && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Single_Secondary_III)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Secondary_ScriptableObject_III
            ,GotExecutedStateName.GotExecuted_Single_Secondary_III);
      
       
        this.gotGunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,this,
            () => 
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf)
                    return true;
                return false;
            });

        gotRestrictNodeLeaf = new GotRestrictNodeLeaf(this.enemy.gotRestrictScriptableObject, this.enemy,
            () => 
            {
                return enemy.curAttackerGunFuNode is RestrainGunFuStateNodeLeaf;
            }
            );
        gotHumandShielded_GunFuNodeLeaf = new HumandShield_GotInteract_NodeLeaf(this.enemy,
            () => 
            { 
                return enemy.curAttackerGunFuNode is HumanShield_GunFu_NodeLeaf; 
            }
            , this.enemy.animator);

        this.gotGunFuReloadNodeLeaf = new GotGunFuInteractingNodeLeaf(this.enemy
            , this.enemy.gotGunFuReloadScriptableObject
            , () => this.enemy.curAttackerGunFuNode is GunFuReloadNodeLeaf
            );

        this.humanShield_Exit_GotInteract_NodeLeaf = new HumanShield_Exit_GotInteract_NodeLeaf(this.enemy
            ,()=> enemy.curAttackerGunFuNode is HumanShieldExit_GunFu_NodeLeaf
            ,this.enemy.humanShield_GotInteract_Exit_SCRP
            ,this.enemy.animator);

        

        startNodeSelector.AddtoChildNode(enemtDeadState);
        startNodeSelector.AddtoChildNode(zeroPostureSelector);
        startNodeSelector.AddtoChildNode(gotGunFuAttackSelector);
        startNodeSelector.AddtoChildNode(painStateNodeLeaf);
        startNodeSelector.AddtoChildNode(gunFuSelector);
        startNodeSelector.AddtoChildNode(enemyStanceSelector);

        this.zeroPostureSelector.AddtoChildNode(this.gotHitDownNodeLeaf);
        this.zeroPostureSelector.AddtoChildNode(this.gotExecuteOnGroundSelector);
        this.zeroPostureSelector.AddtoChildNode(this.fallDown_EnemyState_NodeLeaf);

        this.gotHitDownNodeLeaf.AddTransitionNode(this.painStateGotHitDownNodeLeaf);

        this.gotExecuteOnGroundSelector.AddtoChildNode(this.gotExecute_OnGround_I_NodeLeaf);

        fallDown_EnemyState_NodeLeaf.AddTransitionNode(enemyStandUpStateNodeLeaf);
        fallDown_EnemyState_NodeLeaf.AddTransitionNode(enemyPushUpStateNodeLeaf);

        gunFuSelector.AddtoChildNode(enemySpinKickGunFuNodeLeaf);

        gotGunFuAttackSelector.AddtoChildNode(gotExecuteSelector);
        gotGunFuAttackSelector.AddtoChildNode(gotRestrictNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(humanShield_Exit_GotInteract_NodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHumandShielded_GunFuNodeLeaf);
        this.gotGunFuAttackSelector.AddtoChildNode(this.gotGunFuReloadNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(this.gotGunFuHitNodeLeaf);

        enemyStanceSelector.AddtoChildNode(enemyDodgeRollStateNodeLeaf);
        enemyStanceSelector.AddtoChildNode(enemySprintStateNodeLeaf);
        enemyStanceSelector.AddtoChildNode(crouchSelector);
        enemyStanceSelector.AddtoChildNode(standSelector);

        standSelector.AddtoChildNode(enemyStandMoveStateNodeLeaf);
        standSelector.AddtoChildNode(enemyStandIdleStateNodeLeaf);

        crouchSelector.AddtoChildNode(enemyCrouchMoveStateNodeLeaf);
        crouchSelector.AddtoChildNode(enemyCrouchIdleStateNodeLeaf);

        this.gotExecuteSelector.AddtoChildNode(gotExecute_Dodge_I);
        this.gotExecuteSelector.AddtoChildNode(gotExecute_Primary_NodeLeaf_I);
        this.gotExecuteSelector.AddtoChildNode(gotExecute_Primary_NodeLeaf_II);
        this.gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_I);
        this.gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_II);
        this.gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_III);

        _nodeManagerBehavior.SearchingNewNode(this);

        InitializedComponentNode();
    }

    #region Initialized ComponentNode

    public FindiAndTrackingTargetNodeLeaf findAndTrackTargetNodeLeaf;

    private void InitializedComponentNode()
    {
        this.findAndTrackTargetNodeLeaf = new FindiAndTrackingTargetNodeLeaf(this.enemy.findingTargetScriptableObject,this.enemy.rayCastPos
            ,()=> this.enemy.isDead == false);

        this.enemyStateNodeComponentManager.AddNode(this.findAndTrackTargetNodeLeaf);
    }
    #endregion
}
