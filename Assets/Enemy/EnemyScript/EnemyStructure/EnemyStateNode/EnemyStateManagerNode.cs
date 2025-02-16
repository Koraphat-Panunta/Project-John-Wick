using UnityEngine;
using static IPainStateAble;

public class EnemyStateManagerNode : INodeManager
{
    public INodeLeaf curNodeLeaf { get ; set; }
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
    public EnemyStateSelectorNode standSelector { get; private set; }
    public EnemyStateSelectorNode takeCoverSelector { get; private set; }
    public FallDown_EnemyState_NodeLeaf fallDown_EnemyState_NodeLeaf { get; private set; }
    public EnemyDeadStateNode enemtDeadState { get; private set; }
    public EnemySprintStateNode enemySprintState { get; private set; }
    public EnemyStandIdleStateNode enemyStandIdleState { get; private set; }
    public EnemyStandMoveStateNode enemyStandMoveState { get; private set; }
    public EnemyStandTakeCoverStateNode enemyStandTakeCoverState { get; private set; }
    public EnemyStandTakeAimStateNode enemyStandTakeAimState { get; private set; }

    public EnemyStateSelectorNode gunFuSelector { get; private set; }
    public GotHit1_GunFuGotHitNodeLeaf gotHit1_GunFuHitNodeLeaf { get; private set; }
    public GotHit2_GunFuGotHitNodeLeaf gotHit2_GunFuHitNodeLeaf { get; private set; }
    public GotKnockDown_GunFuGotHitNodeLeaf gotKnockDown_GunFuNodeLeaf { get; private set; }
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

                Debug.Log("this.enemy._tiggerThrowAbleObjectHit = " + this.enemy._tiggerThrowAbleObjectHit);

                if(this.enemy._tiggerThrowAbleObjectHit)
                    return true;

                return false;
            }
       );

        enemySprintState = new EnemySprintStateNode(this.enemy,
            () => this.enemy.isSprintCommand
            );

        enemyStandIdleState = new EnemyStandIdleStateNode(this.enemy,
            () => true //Precondition
            );

        enemyStandMoveState = new EnemyStandMoveStateNode(this.enemy,
            () => this.enemy.moveInputVelocity_WorldCommand.magnitude > 0
            );

        enemyStandTakeCoverState = new EnemyStandTakeCoverStateNode(this.enemy,
            () => this.enemy.isInCover
            , this.enemy);

        enemyStandTakeAimState = new EnemyStandTakeAimStateNode(this.enemy,
            () => this.enemy.isInCover && this.enemy.isAimingCommand
            , this.enemy);

        gunFuSelector = new EnemyStateSelectorNode(this.enemy, 
            () => enemy._triggerHitedGunFu);
        gotHit1_GunFuHitNodeLeaf = new GotHit1_GunFuGotHitNodeLeaf(this.enemy,
            () => 
            {
                return enemy.curGotAttackedGunFuNode is Hit1GunFuNode;
            }
            , this.enemy.GotHit1);

        gotHit2_GunFuHitNodeLeaf = new GotHit2_GunFuGotHitNodeLeaf(this.enemy,
            () => 
            {
                return enemy.curGotAttackedGunFuNode is Hit2GunFuNode;
            }
            , this.enemy.GotHit2);

        gotKnockDown_GunFuNodeLeaf = new GotKnockDown_GunFuGotHitNodeLeaf(this.enemy,
            () => 
            {
                return enemy.curGotAttackedGunFuNode is KnockDown_GunFuNode;
            }
            , this.enemy.KnockDown);

        gotHumandShielded_GunFuNodeLeaf = new HumandShield_GotInteract_NodeLeaf(this.enemy,
            () => 
            { 
                return enemy.curGotAttackedGunFuNode is HumanShield_GunFuInteraction_NodeLeaf; 
            }
            , this.enemy.animator);
        gotHumanThrow_GunFuNodeLeaf = new HumandThrow_GotInteract_NodeLeaf(this.enemy,
            () =>
            {
                Debug.Log("Precondition humanThrow = " + enemy.curGotAttackedGunFuNode);
                return enemy.curGotAttackedGunFuNode is HumanThrowGunFuInteractionNodeLeaf;
            }
            , enemy.animator);


        startNodeSelector.AddtoChildNode(enemtDeadState);
        startNodeSelector.AddtoChildNode(gunFuSelector);
        startNodeSelector.AddtoChildNode(fallDown_EnemyState_NodeLeaf);
        InitailizedPainStateNode();
        startNodeSelector.AddtoChildNode(painStateSelector);
        startNodeSelector.AddtoChildNode(standSelector);

        gunFuSelector.AddtoChildNode(gotHumandShielded_GunFuNodeLeaf);
        gunFuSelector.AddtoChildNode(gotKnockDown_GunFuNodeLeaf);
        gunFuSelector.AddtoChildNode(gotHit2_GunFuHitNodeLeaf);
        gunFuSelector.AddtoChildNode(gotHit1_GunFuHitNodeLeaf);

        gotHumandShielded_GunFuNodeLeaf.AddTransitionNode(gotHumanThrow_GunFuNodeLeaf);

        standSelector.AddtoChildNode(enemySprintState);
        standSelector.AddtoChildNode(takeCoverSelector);
        standSelector.AddtoChildNode(enemyStandMoveState);
        standSelector.AddtoChildNode(enemyStandIdleState);

        takeCoverSelector.AddtoChildNode(enemyStandTakeAimState);
        takeCoverSelector.AddtoChildNode(enemyStandTakeCoverState);

        startNodeSelector.FindingNode(out INodeLeaf enemyStateActionNode);
        curNodeLeaf = enemyStateActionNode as EnemyStateLeafNode;
        curNodeLeaf.Enter();
    }
}
