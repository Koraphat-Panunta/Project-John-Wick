using UnityEditor;
using UnityEngine;
using static IPainStateAble;

public class EnemyStateManagerNode : INodeManager
{
    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get => curNodeLeaf; set => curNodeLeaf = value; }
    public INodeSelector startNodeSelector { get ; set ; }
    public NodeManagerBehavior nodeManagerBehavior { get; set; }
    public Enemy enemy { get; protected set; }
    public EnemyStateManagerNode(Enemy enemy)
    {
        this.enemy = enemy;

        nodeManagerBehavior = new NodeManagerBehavior();

        InitailizedNode();
    }

    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);
    public void UpdateNode() 
    {
        if (curNodeLeaf.IsReset()) 
        {
            startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);
            curNodeLeaf.Exit();
            curNodeLeaf = nodeLeaf;
            curNodeLeaf.Enter();
        }

        curNodeLeaf.UpdateNode();
    }
    

    #region Initailized State Node
    public NodeCombine enemyCombineNode { get;private set; }
    public NodeSelector enemyStateSelector { get; private set; }


    public EnemyStateSelectorNode standSelector { get; private set; }
    public EnemyStateSelectorNode takeCoverSelector { get; private set; }
    public FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf { get; private set; }
    public EnemyDeadStateNode enemtDeadState { get; private set; }
    public EnemySprintStateNodeLeaf enemySprintState { get; private set; }
    public EnemyStandIdleStateNodeLeaf enemyStandIdleState { get; private set; }
    public EnemyStandMoveStateNodeLeaf enemyStandMoveState { get; private set; }
    public EnemyStandTakeCoverStateNodeLeaf enemyStandTakeCoverState { get; private set; }
    public EnemyStandTakeAimStateNodeLeaf enemyStandTakeAimState { get; private set; }

    public EnemyStateSelectorNode gunFuSelector { get; private set; }
    public EnemySpinKickGunFuNodeLeaf enemySpinKickGunFuNodeLeaf { get; private set; }

    public EnemyStateSelectorNode gotGunFuAttackSelector { get; private set; }
    public NodeSelector gotExecuteSelector { get; private set; }
    public NodeSelector gotExecuteOnGroundSelector { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_I { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_Secondary_LayUp_I_NodeLeaf { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_Secondary_LayDown_I_NodeLeaf { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_Primary_LayUp_I_NodeLeaf { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_Primary_LayDown_I_NodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit1_P_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit1_A_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit2_P_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit2_A_GunFuHitNodeLeaf { get; private set; }
    public NodeSequence gotHit3_KnockDown_SequenceNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit3_GunFuNodeLeaf { get; private set; }
    public EnemyStateSelectorNode weaponGotDisarmSelector { get; private set; }
    public WeaponGotDisarmedGunFuGotInteractNodeLeaf primaryWeaponDisarmedGunFuGotInteractNodeLeaf { get; private set; }
    public WeaponGotDisarmedGunFuGotInteractNodeLeaf secondaryWeaponDisarmGunFuGotInteractNodeLeaf { get; private set; }
    public GotRestrictNodeLeaf gotRestrictNodeLeaf { get; private set; }
    public HumandShield_GotInteract_NodeLeaf gotHumandShielded_GunFuNodeLeaf { get; private set; }
    public HumandThrow_GotInteract_NodeLeaf gotHumanThrow_GunFuNodeLeaf { get; private set; }

    #region PainState Node
    public EnemyStateSelectorNode painStateSelector { get; private set; }
    public EnemyStateSelectorNode head_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Body_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Arm_PainState_Selector { get; private set; }
    public EnemyStateSelectorNode Leg_PainState_Selector { get; private set; }

    //Head PainState LeafNode
    //public HeavyPainStateHeadNode enemy_Head_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateHeadNode enemy_Head_PainState_Light_NodeLeaf { get; private set; }

    //BodyFront PainSate LeafNode
    public HeavyPainStateFrontBody enemy_BodyFront_PainState_Heavy_NodeLeaf { get; private set; }
    public MeduimPainStateFrontBody enemy_BodyFront_PainState_Medium_NodeLeaf { get; private set; }
    public LightPainStateFrontBody enemy_BodyFront_PainState_Light_NodeLeaf { get; private set; }

    //BodyBack PainState LeafNode
    public HeavyPainStateBackBody enemy_BodyBack_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateBackBody enemy_BodyBack_PainState_Light_NodeLeaf { get; private set; }

    //ArmLeft PainState LeafNode
    public HeavyPainStateLeftArmNode enemy_LeftArm_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateLeftArmNode enemy_LeftArm_PainState_Light_NodeLeaf { get; private set; }

    //ArmRight PainState LeafNode
    public HeavyPainStateRightArmNode enemy_RightArm_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateRightArmNode enemy_RightArm_PainState_Light_NodeLeaf { get; private set; }

    //LegLeft PainState LeafNode
    public HeavyPainStateLeftLeg enemy_LeftLeg_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateLeftLeg enemy_LeftLeg_PainState_Light_NodeLeaf { get; private set; }

    //LegRight PainState LeafNode
    public HeavyPainStateRightLeg enemy_RightLeg_PainState_Heavy_NodeLeaf { get; private set; }
    public LightPainStateRightLeg enemy_RightLeg_PainState_Light_NodeLeaf { get; private set; }

    private void InitailizedPainStateNode()
    {
        painStateSelector = new EnemyStateSelectorNode(this.enemy,
            () =>
            {
                if (this.enemy._isPainTrigger)
                {
                    return true;
                }
                return false;
            }
            );

        head_PainState_Selector = new EnemyStateSelectorNode(this.enemy, () =>
        {
            if (this.enemy._painPart == IPainStateAble.PainPart.Head)
                return true;
            return false;
        });

        Body_PainState_Selector = new EnemyStateSelectorNode(this.enemy, () =>
        {
            if (this.enemy._painPart == IPainStateAble.PainPart.BodyBack
            || this.enemy._painPart == IPainStateAble.PainPart.BodyFornt)
                return true;
            return false;
        });

        Arm_PainState_Selector = new EnemyStateSelectorNode(this.enemy, () =>
        {
            if (this.enemy._painPart == IPainStateAble.PainPart.ArmLeft
            || this.enemy._painPart == IPainStateAble.PainPart.ArmRight)
                return true;
            return false;
        });

        Leg_PainState_Selector = new EnemyStateSelectorNode(this.enemy, () =>
        {
            if (this.enemy._painPart == IPainStateAble.PainPart.LegLeft
            || this.enemy._painPart == IPainStateAble.PainPart.LegRight)
                return true;
            return false;
        });

        painStateSelector.AddtoChildNode(head_PainState_Selector);
        painStateSelector.AddtoChildNode(Body_PainState_Selector);
        painStateSelector.AddtoChildNode(Arm_PainState_Selector);
        painStateSelector.AddtoChildNode(Leg_PainState_Selector);

        enemy_Head_PainState_Light_NodeLeaf = new LightPainStateHeadNode(this.enemy,
            () =>this.enemy._posture <=this.enemy._postureLight
            ,this.enemy.animator);

        head_PainState_Selector.AddtoChildNode(enemy_Head_PainState_Light_NodeLeaf);

        enemy_BodyFront_PainState_Heavy_NodeLeaf = new HeavyPainStateFrontBody(this.enemy,
            () =>this.enemy._posture <= this.enemy._postureHeavy
            ,this.enemy.animator);

        enemy_BodyFront_PainState_Medium_NodeLeaf = new MeduimPainStateFrontBody(this.enemy,
            () => this.enemy._posture <= this.enemy._postureMedium
            , this.enemy.animator);

        enemy_BodyFront_PainState_Light_NodeLeaf = new LightPainStateFrontBody(this.enemy,
            () => this.enemy._posture <= this.enemy._postureLight
            , this.enemy.animator);

        enemy_BodyBack_PainState_Heavy_NodeLeaf = new HeavyPainStateBackBody(this.enemy,
            () => this.enemy._posture <= this.enemy._postureHeavy
            , this.enemy.animator);
        enemy_BodyBack_PainState_Light_NodeLeaf = new LightPainStateBackBody(this.enemy,
            () => this.enemy._posture <= this.enemy._postureLight
            , this.enemy.animator);

        Body_PainState_Selector.AddtoChildNode(enemy_BodyFront_PainState_Heavy_NodeLeaf);
        Body_PainState_Selector.AddtoChildNode(enemy_BodyBack_PainState_Heavy_NodeLeaf);
        Body_PainState_Selector.AddtoChildNode(enemy_BodyFront_PainState_Medium_NodeLeaf);
        Body_PainState_Selector.AddtoChildNode(enemy_BodyFront_PainState_Light_NodeLeaf);
        Body_PainState_Selector.AddtoChildNode(enemy_BodyBack_PainState_Light_NodeLeaf);

        enemy_LeftArm_PainState_Heavy_NodeLeaf = new HeavyPainStateLeftArmNode(this.enemy,
            () => this.enemy._posture <= this.enemy._postureHeavy
            , this.enemy.animator);
        enemy_LeftArm_PainState_Light_NodeLeaf = new LightPainStateLeftArmNode(this.enemy,
            () => this.enemy._posture <= this.enemy._postureLight
            , this.enemy.animator);
        enemy_RightArm_PainState_Heavy_NodeLeaf = new HeavyPainStateRightArmNode(this.enemy,
            () => this.enemy._posture <= this.enemy._postureHeavy
            , this.enemy.animator);
        enemy_RightArm_PainState_Light_NodeLeaf = new LightPainStateRightArmNode(this.enemy,
            () => this.enemy._posture <= this.enemy._postureLight
            , this.enemy.animator);

        Arm_PainState_Selector.AddtoChildNode(enemy_LeftArm_PainState_Heavy_NodeLeaf);
        Arm_PainState_Selector.AddtoChildNode(enemy_RightArm_PainState_Heavy_NodeLeaf);
        Arm_PainState_Selector.AddtoChildNode(enemy_LeftArm_PainState_Light_NodeLeaf);
        Arm_PainState_Selector.AddtoChildNode(enemy_RightArm_PainState_Light_NodeLeaf);


        enemy_LeftLeg_PainState_Heavy_NodeLeaf = new HeavyPainStateLeftLeg(this.enemy,
            () => this.enemy._posture <= this.enemy._postureHeavy
            , this.enemy.animator);
        enemy_LeftLeg_PainState_Light_NodeLeaf = new LightPainStateLeftLeg(this.enemy,
            () => this.enemy._posture <= this.enemy._postureLight, this.enemy.animator);
        enemy_RightLeg_PainState_Light_NodeLeaf = new LightPainStateRightLeg(this.enemy,
            () => this.enemy._posture <= this.enemy._postureLight, this.enemy.animator);
        enemy_RightLeg_PainState_Heavy_NodeLeaf = new HeavyPainStateRightLeg(this.enemy,
            () => this.enemy._posture <= this.enemy._postureHeavy
            , this.enemy.animator);

        Leg_PainState_Selector.AddtoChildNode(enemy_LeftLeg_PainState_Heavy_NodeLeaf);
        Leg_PainState_Selector.AddtoChildNode(enemy_RightLeg_PainState_Heavy_NodeLeaf);
        Leg_PainState_Selector.AddtoChildNode(enemy_LeftLeg_PainState_Light_NodeLeaf);
        Leg_PainState_Selector.AddtoChildNode(enemy_RightLeg_PainState_Light_NodeLeaf);

    }
    #endregion


    #endregion
    public void InitailizedNode()
    {
        startNodeSelector = new EnemyStateSelectorNode(enemy, () => true);
        enemyCombineNode = new NodeCombine(()=>true,()=>false);
        enemyStateSelector = new NodeSelector(() => true);

        standSelector = new EnemyStateSelectorNode(this.enemy,
            () => true
            );

        takeCoverSelector = new EnemyStateSelectorNode(this.enemy,
            () => this.enemy.isInCover
            );

        

        enemtDeadState = new EnemyDeadStateNode(this.enemy,
            () => this.enemy.GetHP() <= 0
            );

        fallDown_EnemyState_NodeLeaf = new FallDown_EnemyState_NodeLeaf(this.enemy, this.enemy,
            () => //Precondition
            {
                if (this.enemy._isPainTrigger && this.enemy._posture <= 0)
                    return true;
                return false;
            }
       );

        enemySprintState = new EnemySprintStateNodeLeaf(this.enemy,
            () => this.enemy.isSprintCommand
            );

        enemyStandIdleState = new EnemyStandIdleStateNodeLeaf(this.enemy,
            () => true //Precondition
            );

        enemyStandMoveState = new EnemyStandMoveStateNodeLeaf(this.enemy,
            () => this.enemy.moveInputVelocity_WorldCommand.magnitude > 0
            );

        enemyStandTakeCoverState = new EnemyStandTakeCoverStateNodeLeaf(this.enemy,
            () => this.enemy.isInCover
            , this.enemy);

        enemyStandTakeAimState = new EnemyStandTakeAimStateNodeLeaf(this.enemy,
            () => this.enemy.isInCover && this.enemy._isAimingCommand
            , this.enemy);

        gunFuSelector = new EnemyStateSelectorNode(this.enemy, () 
            => enemy._triggerGunFu);
        enemySpinKickGunFuNodeLeaf = new EnemySpinKickGunFuNodeLeaf(this.enemy.EnemySpinKickScriptable,this.enemy,()=>true);

        gotGunFuAttackSelector = new EnemyStateSelectorNode(this.enemy, 
            () => enemy._triggerHitedGunFu);
        gotExecuteSelector = new NodeSelector(
            ()=> enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf);
        gotExecuteOnGroundSelector = new NodeSelector(
            ()=> enemy.curAttackerGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf);
        gotExecute_I = new GotGunFuExecuteNodeLeaf(enemy,()=> true, "GotGunFuDodgeExecuteSecondary");
        gotExecute_OnGround_Secondary_LayUp_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy,this.enemy.layUpExecutedAnim,enemy.transform.root,enemy._hipsBone,enemy._bones, "GotGunFu_Single_Execute_OnGround_Pistol_Layup_I",
            () => 
            {
                if(enemy.curAttackerGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf execute_OnGround_Single_NodeLeaf
                && execute_OnGround_Single_NodeLeaf.gunFuExecute_OnGround_Single_ScriptableObject.gotGunFuStateName == gotExecute_OnGround_Secondary_LayUp_I_NodeLeaf.gotExecuteStateName)
                    return true;
                return false;
            }
            );
        gotExecute_OnGround_Secondary_LayDown_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy, this.enemy.layUpExecutedAnim, enemy.transform.root, enemy._hipsBone, enemy._bones, "GotGunFu_Single_Execute_OnGround_Pistol_Laydown_I",
            () =>
            {
                if (enemy.curAttackerGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf execute_OnGround_Single_NodeLeaf
                && execute_OnGround_Single_NodeLeaf.gunFuExecute_OnGround_Single_ScriptableObject.gotGunFuStateName == gotExecute_OnGround_Secondary_LayDown_I_NodeLeaf.gotExecuteStateName)
                    return true;
                return false;
            }
            );
        gotExecute_OnGround_Primary_LayUp_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy, this.enemy.layUpExecutedAnim, enemy.transform.root, enemy._hipsBone, enemy._bones, "GotGunFu_Single_Execute_OnGround_Primary_Layup_I",
            () =>
            {
                if (enemy.curAttackerGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf execute_OnGround_Single_NodeLeaf
                && execute_OnGround_Single_NodeLeaf.gunFuExecute_OnGround_Single_ScriptableObject.gotGunFuStateName == gotExecute_OnGround_Primary_LayUp_I_NodeLeaf.gotExecuteStateName)
                    return true;
                return false;
            }
            );
        gotExecute_OnGround_Primary_LayDown_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy, this.enemy.layUpExecutedAnim, enemy.transform.root, enemy._hipsBone, enemy._bones, "GotGunFu_Single_Execute_OnGround_Primary_Laydown_I",
            () =>
            {
                if (enemy.curAttackerGunFuNode is GunFuExecute_OnGround_Single_NodeLeaf execute_OnGround_Single_NodeLeaf
                && execute_OnGround_Single_NodeLeaf.gunFuExecute_OnGround_Single_ScriptableObject.gotGunFuStateName == gotExecute_OnGround_Primary_LayDown_I_NodeLeaf.gotExecuteStateName)
                    return true;
                return false;
            }
            );
        gotHit1_P_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,
            () => 
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
                && gunFuHitNodeLeaf._stateName == "Hit1"
                && gunFuHitNodeLeaf.hitCount == 1)
                    return true;
                
                return false;
            }
            , this.enemy.GotHit1_P);

        gotHit1_A_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,
            () => 
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
                && gunFuHitNodeLeaf._stateName == "Hit1"
                && gunFuHitNodeLeaf.hitCount == 2)
                    return true;

                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitDodgeSpinNodeLeaf
                && gunFuHitDodgeSpinNodeLeaf._stateName == "DodgeSpinKick"
                && gunFuHitDodgeSpinNodeLeaf.hitCount == 1)
                    return true;

                return false;
            }
            , this.enemy.GotHit1_A);

        gotHit2_P_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
                && gunFuHitNodeLeaf._stateName == "Hit2"
                && gunFuHitNodeLeaf.hitCount == 1)
                    return true;
                return false;
            }
            , this.enemy.GotHit2_P);

        gotHit2_A_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,
           () =>
           {
               if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
               && gunFuHitNodeLeaf._stateName == "Hit2"
               && gunFuHitNodeLeaf.hitCount == 2)
                   return true;
               return false;
           }
           , this.enemy.GotHit2_A);

        gotHit3_KnockDown_SequenceNodeLeaf = new NodeSequence(() => enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
               && gunFuHitNodeLeaf._stateName == "Hit3");

        gotHit3_GunFuNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,
            () => 
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
               && gunFuHitNodeLeaf._stateName == "Hit3"
               )
                    return true;
                return false;
            }
            , this.enemy.KnockDown);

        weaponGotDisarmSelector = new EnemyStateSelectorNode(this.enemy,
            () => enemy.curAttackerGunFuNode is WeaponDisarm_GunFuInteraction_NodeLeaf);

        primaryWeaponDisarmedGunFuGotInteractNodeLeaf = new WeaponGotDisarmedGunFuGotInteractNodeLeaf(this.enemy.primary_WeaponGotDisarmedScriptableObject,
            this.enemy,
            () => enemy._currentWeapon is PrimaryWeapon);

        secondaryWeaponDisarmGunFuGotInteractNodeLeaf = new WeaponGotDisarmedGunFuGotInteractNodeLeaf(this.enemy.secondary_WeaponGotDisarmedScriptableObject,
            this.enemy,
            () => enemy._currentWeapon is SecondaryWeapon);

        gotRestrictNodeLeaf = new GotRestrictNodeLeaf(this.enemy.gotRestrictScriptableObject, this.enemy,
            () => 
            {
                return enemy.curAttackerGunFuNode is RestrictGunFuStateNodeLeaf;
            }
            );
        gotHumandShielded_GunFuNodeLeaf = new HumandShield_GotInteract_NodeLeaf(this.enemy,
            () => 
            { 
                return enemy.curAttackerGunFuNode is HumanShield_GunFuInteraction_NodeLeaf; 
            }
            , this.enemy.animator);
        gotHumanThrow_GunFuNodeLeaf = new HumandThrow_GotInteract_NodeLeaf(this.enemy,
            () =>
            {
                Debug.Log("Precondition humanThrow = " + enemy.curAttackerGunFuNode);
                return enemy.curAttackerGunFuNode is HumanThrowGunFuInteractionNodeLeaf;
            }
            , enemy.animator);

        startNodeSelector.AddtoChildNode(enemyCombineNode);
        enemyCombineNode.AddCombineNode(enemyStateSelector);

        enemyStateSelector.AddtoChildNode(enemtDeadState);
        enemyStateSelector.AddtoChildNode(gotGunFuAttackSelector);
        enemyStateSelector.AddtoChildNode(fallDown_EnemyState_NodeLeaf);
        InitailizedPainStateNode();
        enemyStateSelector.AddtoChildNode(painStateSelector);
        enemyStateSelector.AddtoChildNode(gunFuSelector);
        enemyStateSelector.AddtoChildNode(standSelector);

        gunFuSelector.AddtoChildNode(enemySpinKickGunFuNodeLeaf);

        weaponGotDisarmSelector.AddtoChildNode(primaryWeaponDisarmedGunFuGotInteractNodeLeaf);
        weaponGotDisarmSelector.AddtoChildNode(secondaryWeaponDisarmGunFuGotInteractNodeLeaf);

        gotGunFuAttackSelector.AddtoChildNode(gotExecuteSelector);
        gotGunFuAttackSelector.AddtoChildNode(weaponGotDisarmSelector);
        gotGunFuAttackSelector.AddtoChildNode(gotRestrictNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHumandShielded_GunFuNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit3_KnockDown_SequenceNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit1_P_GunFuHitNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit1_A_GunFuHitNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit2_P_GunFuHitNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit2_A_GunFuHitNodeLeaf);

        gotHit3_KnockDown_SequenceNodeLeaf.AddChildNode(gotHit3_GunFuNodeLeaf);
        gotHit3_KnockDown_SequenceNodeLeaf.AddChildNode(fallDown_EnemyState_NodeLeaf);

        gotHumandShielded_GunFuNodeLeaf.AddTransitionNode(gotHumanThrow_GunFuNodeLeaf);
        
        standSelector.AddtoChildNode(enemySprintState);
        standSelector.AddtoChildNode(takeCoverSelector);
        standSelector.AddtoChildNode(enemyStandMoveState);
        standSelector.AddtoChildNode(enemyStandIdleState);

        takeCoverSelector.AddtoChildNode(enemyStandTakeAimState);
        takeCoverSelector.AddtoChildNode(enemyStandTakeCoverState);

        gotExecuteSelector.AddtoChildNode(gotExecuteOnGroundSelector);
        gotExecuteSelector.AddtoChildNode(gotExecute_I);

        gotExecuteOnGroundSelector.AddtoChildNode(gotExecute_OnGround_Secondary_LayUp_I_NodeLeaf);
        gotExecuteOnGroundSelector.AddtoChildNode(gotExecute_OnGround_Secondary_LayDown_I_NodeLeaf);
        gotExecuteOnGroundSelector.AddtoChildNode(gotExecute_OnGround_Primary_LayUp_I_NodeLeaf);
        gotExecuteOnGroundSelector.AddtoChildNode(gotExecute_OnGround_Primary_LayDown_I_NodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
}
