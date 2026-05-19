using System;
using System.Collections;
using UnityEngine;

public class GuardModeComponentNodeLeaf : NodeLeaf
{
    public float coolDownTime { get; protected set; }
    public float guardTime { get; protected set; }
    public float guardTimer { get; protected set; }
    public bool isGuardAble { get; protected set; }

    public Character character;

    public bool isTriggerGuard { get; protected set; }

    public Gauge guardGuage;
    public GuardModeComponentNodeLeaf(
        Func<bool> preCondition
        , Character character
        , Gauge guardGauge
        , float coolDownTime
        ,float guardTime
        ) : base(preCondition)
    {
        this.character = character;
        this.guardGuage = guardGauge;
        this.guardTime = guardTime;
        this.coolDownTime = coolDownTime;

        this.isGuardAble = true;
    }

    public override void Enter()
    {
        this.isTriggerGuard = false;
        this.isComplete = false;
        this.guardTimer = this.guardTime;
        this.isGuardAble = false;
        base.Enter();
    }
    public override void UpdateNode()
    {
        this.guardTimer -= Time.deltaTime;

        if(this.guardTimer <= 0)
            this.isComplete = true;

        base.UpdateNode();
    }
    public override bool IsReset()
    {
        if(this.guardGuage._gauge <= 0)
            return true;

        if(this.character.isDead)
            return true;

        if (this.IsComplete())
        {
            this.StartCoolDown();
            return true;
        }

        return false;
    }

    public override bool IsComplete()
    {
        return this.isComplete;
    }
    

    Coroutine coolDownCoroutine;
    public void StartCoolDown()
    {
        if(coolDownCoroutine != null)
            this.character.StopCoroutine(this.coolDownCoroutine);

        this.coolDownCoroutine = this.character.StartCoroutine(StartCoolDownTime());
    }

    public void TriggerGuard() => this.isTriggerGuard = true;
    protected IEnumerator StartCoolDownTime()
    {
        yield return new WaitForSeconds(this.coolDownTime);
        this.coolDownCoroutine = null;
        this.isGuardAble = true;
    }

}
