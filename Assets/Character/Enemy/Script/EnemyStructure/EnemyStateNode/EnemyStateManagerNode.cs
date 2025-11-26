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

        _nodeManagerBehavior = new NodeManagerBehavior();
        _parallelNodeManahger = new List<INodeManager>();
        enemyStateNodeComponentManager = new NodeComponentManager();

        InitailizedNode();
    }

    public void FixedUpdateNode()
    {
        _nodeManagerBehavior.FixedUpdateNode(this);
        this.enemyStateNodeComponentManager.FixedUpdate();
    }
    public void UpdateNode() 
    {
        _nodeManagerBehavior.UpdateNode(this);
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
    public NodeSelector gunFuZeroPostureSelector { get; private set; }
    public NodeSelector gotExecuteOnGroundSelector { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_LayUp_I_NodeLeaf { get; private set; }
    public GotExecuteOnGround_NodeLeaf gotExecute_OnGround_LayDown_I_NodeLeaf { get; private set; }

    public FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf { get; private set; }
    public GetUpStateNodeLeaf enemyStandUpStateNodeLeaf { get; private set; }
    public GetUpStateNodeLeaf enemyPushUpStateNodeLeaf { get; private set; }
    public EnemyDeadStateNode enemtDeadState { get; private set; }



    public NodeSelector gunFuSelector { get; private set; }
    public EnemySpinKickGunFuNodeLeaf enemySpinKickGunFuNodeLeaf { get; private set; }

    public NodeSelector gotGunFuAttackSelector { get; private set; }
    public NodeSelector gotExecuteSelector { get; private set; }

    public GotGunFuExecuteNodeLeaf gotExecute_Dodge_Primary_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Dodge_Secondary_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_II { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_III { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Secondary_NodeLeaf_IV { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Primary_NodeLeaf_I { get; private set; }
    public GotGunFuExecuteNodeLeaf gotExecute_Primary_NodeLeaf_II { get; private set; }

    public GotGunFuHitNodeLeaf gotHit1_P_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit1_A_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit2_P_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit2_A_GunFuHitNodeLeaf { get; private set; }
    public GotGunFuHitNodeLeaf gotHit3_GunFuNodeLeaf { get; private set; }
    public NodeSelector weaponGotDisarmSelector { get; private set; }
    public WeaponGotDisarmedGunFuGotInteractNodeLeaf primaryWeaponDisarmedGunFuGotInteractNodeLeaf { get; private set; }
    public WeaponGotDisarmedGunFuGotInteractNodeLeaf secondaryWeaponDisarmGunFuGotInteractNodeLeaf { get; private set; }
    public GotRestrictNodeLeaf gotRestrictNodeLeaf { get; private set; }
    public HumandShield_GotInteract_NodeLeaf gotHumandShielded_GunFuNodeLeaf { get; private set; }
    public HumanShield_Exit_GotInteract_NodeLeaf humanShield_Exit_GotInteract_NodeLeaf { get; private set; }

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

        enemyStanceSelector = new NodeSelector(
            () => true
            , nameof(enemyStanceSelector));

        crouchSelector = new NodeSelector(
            () => enemy.enemyStance == Stance.crouch
            , nameof(crouchSelector));
        enemyCrouchMoveStateNodeLeaf = new EnemyCrouchMoveStateNodeLeaf(enemy,
            () => 
            {
                return enemy.moveInputVelocity_WorldCommand.magnitude > 0;
            });
        enemyCrouchIdleStateNodeLeaf = new EnemyCrouchIdleStateNodeLeaf(enemy,
            () => true);

        standSelector = new NodeSelector(
            () =>enemy.enemyStance == Stance.stand || true
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
           () => this.enemy.isSprintCommand
           );
        enemyDodgeRollStateNodeLeaf = new EnemyDodgeRollStateNodeLeaf(this.enemy
            ,()=> enemy._triggerDodge && enemyDodgeRollStateNodeLeaf.dodgeRollCoolDown <=0
            );

        enemtDeadState = new EnemyDeadStateNode(this.enemy,
            () => this.enemy.isDead
            );

        zeroPostureSelector = new NodeSelector(
            ()=> this.enemy._posture <= 0);
        gunFuZeroPostureSelector = new NodeSelector(
            () => enemy._triggerHitedGunFu);
        gotExecuteOnGroundSelector = new NodeSelector(
            () => enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf);
        gotExecute_OnGround_LayDown_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy
            ,this.enemy.gotGunFu_Single_Execute_OnGround_LayDown_I
            ,this.enemy._root
            ,this.enemy._hipsBone
            ,this.enemy._bones
            ,GotExecutedStateName.GotExecuted_OnGround_LayDown_I
            ,()=> enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
            &&( gunFuExecuteNodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Single_Execute_OnGround_Primary_I 
            || gunFuExecuteNodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Single_Execute_OnGround_Secondary_I)
            && (enemy as IRagdollAble)._isFacingUp == false
            );
        gotExecute_OnGround_LayUp_I_NodeLeaf = new GotExecuteOnGround_NodeLeaf(this.enemy
           , this.enemy.gotGunFu_Single_Execute_OnGround_LayUp_I
           , this.enemy._root
           , this.enemy._hipsBone
           , this.enemy._bones
           , GotExecutedStateName.GotExecuted_OnGround_LayUp_I
           , () => enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecuteNodeLeaf
           && (gunFuExecuteNodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Single_Execute_OnGround_Primary_I
           || gunFuExecuteNodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Single_Execute_OnGround_Secondary_I)
           && (enemy as IRagdollAble)._isFacingUp 
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

        gunFuSelector = new NodeSelector(
            () => enemy._triggerGunFu && enemy._isInPain == false);

        enemySpinKickGunFuNodeLeaf = new EnemySpinKickGunFuNodeLeaf(this.enemy.EnemySpinKickScriptable,this.enemy,()=>true);

        gotGunFuAttackSelector = new NodeSelector( 
            () => 
            {
                if (enemy._triggerHitedGunFu)
                {
                    return true;
                }

                return false;
                }
            );
        gotExecuteSelector = new NodeSelector(
            ()=> enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf);

        gotExecute_Dodge_Primary_I = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
                && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Dodge_Primary)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Primary_Dodge_ScriptableObject_I
            ,GotExecutedStateName.GotExecuted_Dodge_Primary);

        gotExecute_Dodge_Secondary_I = new GotGunFuExecuteNodeLeaf(enemy,
            ()=> 
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
                && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Dodge_Secondary)
                    return true;
                return false;
            }
            ,this.enemy.gotGunFuExecute_Single_Secondary_Dodge_ScriptableObject_I
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
        gotExecute_Secondary_NodeLeaf_IV = new GotGunFuExecuteNodeLeaf(enemy,
            () =>
            {
                if (enemy.curAttackerGunFuNode is IGunFuExecuteNodeLeaf gunFuExecute_Single_NodeLeaf
               && gunFuExecute_Single_NodeLeaf._executeStateName == GunFuExecuteStateName.GunFu_Execute_Single_Secondary_IV)
                    return true;
                return false;
            }
            , this.enemy.gotGunFuExecute_Single_Secondary_ScriptableObject_IV
            ,GotExecutedStateName.GotExecuted_Single_Secondary_IV);
       
        gotHit1_P_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,this,
            () => 
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
                && gunFuHitNodeLeaf._stateName == "Hit1"
                && gunFuHitNodeLeaf.hitCount == 0)
                    return true;
                return false;
            }
            , this.enemy.GotHit1_P);

        gotHit1_A_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,this,
            () => 
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
                && gunFuHitNodeLeaf._stateName == "Hit1"
                && gunFuHitNodeLeaf.hitCount == 1)
                    return true;

                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitDodgeSpinNodeLeaf
                && gunFuHitDodgeSpinNodeLeaf._stateName == "DodgeSpinKick"
                && gunFuHitDodgeSpinNodeLeaf.hitCount == 0)
                    return true;

                return false;
            }
            , this.enemy.GotHit1_A);

        gotHit2_P_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,this,
            () =>
            {
                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
                && gunFuHitNodeLeaf._stateName == "Hit2"
                && gunFuHitNodeLeaf.hitCount == 0)
                    return true;
                return false;
            }
            , this.enemy.GotHit2_P);

        gotHit2_A_GunFuHitNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,this,
           () =>
           {
               if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
               && gunFuHitNodeLeaf._stateName == "Hit2"
               && gunFuHitNodeLeaf.hitCount == 1)
                   return true;
               return false;
           }
           , this.enemy.GotHit2_A);

        gotHit3_GunFuNodeLeaf = new GotGunFuHitNodeLeaf(this.enemy,this,
            () => 
            {

                if (enemy.curAttackerGunFuNode is GunFuHitNodeLeaf gunFuHitNodeLeaf
               && gunFuHitNodeLeaf._stateName == "Hit3")
                    return true;

                if (enemy.curAttackerGunFuNode is EnemySpinKickGunFuNodeLeaf enemySpinKickGunFuNodeLeaf
                && enemySpinKickGunFuNodeLeaf.curPhase == EnemySpinKickGunFuNodeLeaf.SpinKickPhase.Hit)
                    return true;

                return false;
            }
            , this.enemy.GotHit3);

        weaponGotDisarmSelector = new NodeSelector(
            () => enemy.curAttackerGunFuNode is WeaponDisarm_GunFuInteraction_NodeLeaf);

        primaryWeaponDisarmedGunFuGotInteractNodeLeaf = new WeaponGotDisarmedGunFuGotInteractNodeLeaf(this.enemy.primary_WeaponGotDisarmedScriptableObject
            , GotGunFuManuverStateName.GotWeaponDisarmPrimary.ToString(),
            this.enemy,
            () => enemy._currentWeapon is PrimaryWeapon);

        secondaryWeaponDisarmGunFuGotInteractNodeLeaf = new WeaponGotDisarmedGunFuGotInteractNodeLeaf(this.enemy.secondary_WeaponGotDisarmedScriptableObject
            , GotGunFuManuverStateName.GotWeaponDisarmSecondary.ToString(),
            this.enemy,
            () => enemy._currentWeapon is SecondaryWeapon);

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

        this.humanShield_Exit_GotInteract_NodeLeaf = new HumanShield_Exit_GotInteract_NodeLeaf(this.enemy
            ,()=> enemy.curAttackerGunFuNode is HumanShieldExit_GunFu_NodeLeaf
            ,this.enemy.humanShield_GotInteract_Exit_SCRP
            ,this.enemy.animator);

        startNodeSelector.AddtoChildNode(enemtDeadState);
        startNodeSelector.AddtoChildNode(zeroPostureSelector);
        startNodeSelector.AddtoChildNode(gotGunFuAttackSelector);

        zeroPostureSelector.AddtoChildNode(gunFuZeroPostureSelector);
        zeroPostureSelector.AddtoChildNode(fallDown_EnemyState_NodeLeaf);

        gunFuZeroPostureSelector.AddtoChildNode(gotExecuteOnGroundSelector);

        gotExecuteOnGroundSelector.AddtoChildNode(gotExecute_OnGround_LayDown_I_NodeLeaf);
        gotExecuteOnGroundSelector.AddtoChildNode(gotExecute_OnGround_LayUp_I_NodeLeaf);

        fallDown_EnemyState_NodeLeaf.AddTransitionNode(enemyStandUpStateNodeLeaf);
        fallDown_EnemyState_NodeLeaf.AddTransitionNode(enemyPushUpStateNodeLeaf);

        InitailizedPainStateNode();
        startNodeSelector.AddtoChildNode(painStateSelector);
        startNodeSelector.AddtoChildNode(gunFuSelector);
        startNodeSelector.AddtoChildNode(enemyStanceSelector);

        gunFuSelector.AddtoChildNode(enemySpinKickGunFuNodeLeaf);

        weaponGotDisarmSelector.AddtoChildNode(primaryWeaponDisarmedGunFuGotInteractNodeLeaf);
        weaponGotDisarmSelector.AddtoChildNode(secondaryWeaponDisarmGunFuGotInteractNodeLeaf);

        gotGunFuAttackSelector.AddtoChildNode(gotExecuteSelector);
        gotGunFuAttackSelector.AddtoChildNode(weaponGotDisarmSelector);
        gotGunFuAttackSelector.AddtoChildNode(gotRestrictNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(humanShield_Exit_GotInteract_NodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHumandShielded_GunFuNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit3_GunFuNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit1_P_GunFuHitNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit1_A_GunFuHitNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit2_P_GunFuHitNodeLeaf);
        gotGunFuAttackSelector.AddtoChildNode(gotHit2_A_GunFuHitNodeLeaf);

        gotHit3_GunFuNodeLeaf.AddTransitionNode(fallDown_EnemyState_NodeLeaf);

        enemyStanceSelector.AddtoChildNode(enemyDodgeRollStateNodeLeaf);
        enemyStanceSelector.AddtoChildNode(enemySprintStateNodeLeaf);
        enemyStanceSelector.AddtoChildNode(crouchSelector);
        enemyStanceSelector.AddtoChildNode(standSelector);

        standSelector.AddtoChildNode(enemyStandMoveStateNodeLeaf);
        standSelector.AddtoChildNode(enemyStandIdleStateNodeLeaf);

        crouchSelector.AddtoChildNode(enemyCrouchMoveStateNodeLeaf);
        crouchSelector.AddtoChildNode(enemyCrouchIdleStateNodeLeaf);

        gotExecuteSelector.AddtoChildNode(gotExecute_Dodge_Primary_I);
        gotExecuteSelector.AddtoChildNode(gotExecute_Dodge_Secondary_I);
        gotExecuteSelector.AddtoChildNode(gotExecute_Primary_NodeLeaf_I);
        gotExecuteSelector.AddtoChildNode(gotExecute_Primary_NodeLeaf_II);
        gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_I);
        gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_II);
        gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_III);
        gotExecuteSelector.AddtoChildNode(gotExecute_Secondary_NodeLeaf_IV);

        _nodeManagerBehavior.SearchingNewNode(this);

        InitializedComponentNode();
    }

    private void InitializedComponentNode()
    {
        enemy.recoveryStaggerNodeLeaf = new RecoveryStaggerNodeLeaf(
            () => enemy.staggerGauge <= 0 && enemy._isInPain == false, enemy, 9);

        this.enemyStateNodeComponentManager.AddNode(enemy.recoveryStaggerNodeLeaf);
    }
}
