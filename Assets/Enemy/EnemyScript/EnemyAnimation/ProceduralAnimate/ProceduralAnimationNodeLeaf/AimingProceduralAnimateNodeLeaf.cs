using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimingProceduralAnimateNodeLeaf : ProceduralAnimateNodeLeaf
{
    public MultiAimConstraint aimConstraint;
    protected float weight;

    protected Task fadeOutWeight;

    private float fadeOutWeightVelocity = 1;
    public AimingProceduralAnimateNodeLeaf(Func<bool> preCondition) : base(preCondition)
    {
    }

    public override void Enter()
    {
        if(fadeOutWeight == null || fadeOutWeight.IsCompleted)
            fadeOutWeight = FadeOutWeight();

        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }

    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    private async Task FadeOutWeight()
    {
        while(weight > 0)
        {
            weight = Mathf.MoveTowards(weight, 0, fadeOutWeightVelocity * Time.deltaTime);
            await Task.Yield();
        }
    }
}
