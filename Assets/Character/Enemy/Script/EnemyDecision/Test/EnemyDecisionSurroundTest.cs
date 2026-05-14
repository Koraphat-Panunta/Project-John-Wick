using UnityEngine;

public class EnemyDecisionSurroundTest : EnemyDecision
{
    private enum TestState { Idle, Surrounding, Insisting }

    private EnemyDecisionContext enemyDecisionContext;
    private SurroundPlayerPositioningActionNodeLeaf surroundNode;
    private InsistEnemyActionNodeLeaf insistNode;

    // Visible in Inspector for debugging
    [SerializeField] private TestState currentState = TestState.Idle;

    private bool hasSpottedOnce;
    private float timeSinceLastSpot = float.MaxValue;

    // OnNotifySpottingTarget fires every frame while spotting; 0.15s gives ~9 frames of buffer
    private const float LOST_SIGHT_THRESHOLD = 0.15f;

    public override void Initialized()
    {
        enemyDecisionContext = new EnemyDecisionContext( 5f, 5f);
        enemyDecisionContext.SetRoleCommand(EnemyRoleCommand.Support);
        enemyDecisionContext.SetCombatPhase(CombatPhase.Suspect);

        surroundNode = new SurroundPlayerPositioningActionNodeLeaf(
            enemy, enemyCommand, () => true, this, enemyDecisionContext);

        insistNode = new InsistEnemyActionNodeLeaf(
            enemy, enemyCommand, () => true, this, enemyDecisionContext);

        base.Initialized();
    }

    protected override void Update()
    {
        timeSinceLastSpot += Time.deltaTime;

        bool isSpotting = timeSinceLastSpot < LOST_SIGHT_THRESHOLD;

        TestState next;
        if (!hasSpottedOnce)
            next = TestState.Idle;
        else if (isSpotting)
            next = TestState.Surrounding;
        else
            next = TestState.Insisting;

        if (next != currentState)
            TransitionTo(next);

        switch (currentState)
        {
            case TestState.Surrounding:
                surroundNode.UpdateNode();
                break;
            case TestState.Insisting:
                insistNode.UpdateNode();
                break;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        switch (currentState)
        {
            case TestState.Surrounding:
                surroundNode.FixedUpdateNode();
                break;
            case TestState.Insisting:
                insistNode.FixedUpdateNode();
                break;
        }

        base.FixedUpdate();
    }

    private void TransitionTo(TestState next)
    {
        switch (currentState)
        {
            case TestState.Surrounding: surroundNode.Exit(); break;
            case TestState.Insisting:   insistNode.Exit();   break;
        }

        currentState = next;

        switch (currentState)
        {
            case TestState.Surrounding: surroundNode.Enter(); break;
            case TestState.Insisting:   insistNode.Enter();   break;
        }
    }

    protected override void OnNotifyHearding(INoiseMakingAble noiseMaker) { }

    protected override void OnNotifySpottingTarget(GameObject target)
    {
        timeSinceLastSpot = 0f;
        hasSpottedOnce = true;
        EnemyDecisionInjectionEvent.OnSpotingTarget(target.transform, enemyDecisionContext);
    }
}
