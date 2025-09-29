using UnityEngine;
using System.Collections.Generic;
public class TimeControlManager : MonoBehaviour, INodeManager,IInitializedAble,IGameLevelMasterObserver,IObserverPlayer
{
    public static readonly float fixDeltaTimeOnSlowMotion = 0.02f * 0.1f;
    public static readonly float fixDeltaTimeDefault = 0.02f ;
    public NodeManagerBehavior nodeManagerBehavior { get ; set; }

    private INodeLeaf curNodeLeaf;
    INodeLeaf INodeManager.curNodeLeaf { get => this.curNodeLeaf; set => curNodeLeaf = value; }
    private float returnTimeScale;
    protected bool isSystemStopTime;

    public void Initialized()
    {
        nodeManagerBehavior = new NodeManagerBehavior();
        this.observerTimeControlManagers = new List<IObserverTimeControlManager>();
        this.player.AddObserver(this);
        this.gameMaster.AddObserver(this);
        InitailizedNode();
    }
    private void Update()
    {
        if(isSystemStopTime == false)
            this.UpdateNode();
    }
    private void FixedUpdate()
    {
        if(isSystemStopTime == false)
            this.FixedUpdateNode();
    }
    public void UpdateNode() => nodeManagerBehavior.UpdateNode(this);
    public void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);

    public INodeSelector startNodeSelector { get; set; }
    public TimeScaleDefaultNodeLeaf timeDefaultNodeLeaf { get; set; }
    public TriggerTimeSlowCurveNodeLeaf triggerBulletTime { get; set; }

    public void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true, "startNodeSelector TimeControlManager");

        timeDefaultNodeLeaf = new TimeScaleDefaultNodeLeaf(()=> true,this,1,1);
        triggerBulletTime = new TriggerTimeSlowCurveNodeLeaf(
            () => triggerBulletTime.timer > 0
            , this);
        

        startNodeSelector.AddtoChildNode(triggerBulletTime);
        startNodeSelector.AddtoChildNode(timeDefaultNodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }

    public void StopTime()
    {
        if(this.isSystemStopTime)
            return;

        this.isSystemStopTime = true;
        this.returnTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void RunTime()
    {
        if(this.isSystemStopTime == false)
            return;

        this.isSystemStopTime = false;
        Time.timeScale = this.returnTimeScale;
    }

    #region ObserverPattern
    protected List<IObserverTimeControlManager> observerTimeControlManagers { get; set; }

    public void NotifyObserver<T>(TimeControlManager timeControlManager, T Var)
    {
        if (this.observerTimeControlManagers.Count <= 0)
            return;

        for (int i = 0; i < this.observerTimeControlManagers.Count; i++)
        {
            this.observerTimeControlManagers[i].OnNotifyObserver(timeControlManager, Var);
        }
    }
        
    public void AddObserver(IObserverTimeControlManager observerTimeControlManager)
    {
        this.observerTimeControlManagers.Add(observerTimeControlManager);
    }
    public void RemoveObsever(IObserverTimeControlManager observerTimeControlManager)
    {
        this.observerTimeControlManagers.Remove(observerTimeControlManager);    
    }

   
    #endregion

    #region timeCurve
    [Range(0, 10)]
    [SerializeField] private float restrict_HumanShield_BulletTimeDuration;
    [Range(0, 10)]
    [SerializeField] private float hit3SlowMotionDuration;

    [SerializeField] private AnimationCurve restrict_HS_BulletTimeCurve;
    [SerializeField] private AnimationCurve hit3_TimeCurve;
    #endregion

    public void OnNotify<T>(InGameLevelGameMaster inGameLevelGameMaster, T var)
    {
        if((inGameLevelGameMaster as INodeManager).TryGetCurNodeLeaf<MenuInGameGameMasterNodeLeaf>()
            || (inGameLevelGameMaster as INodeManager).TryGetCurNodeLeaf<OptionMenuSettingInGameGameMasterNodeLeaf>())
        {
            this.StopTime();
        }
        else
        this.RunTime();
            

    }

    public void OnNotify<T>(Player player, T node)
    {
        switch (node)
        {
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking
                        && gunFuHitNodeLeaf._stateName == "Hit3")
                        this.triggerBulletTime.TriggerSlowMotion(this.hit3_TimeCurve,this.hit3SlowMotionDuration);
                    break;
                }
            case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {
                    if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                        this.triggerBulletTime.TriggerSlowMotion(this.restrict_HS_BulletTimeCurve, this.restrict_HumanShield_BulletTimeDuration);
                    else if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                        this.triggerBulletTime.StopSlowMotion();
                    break;
                }
            case HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf:
                {
                    if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay)
                        this.triggerBulletTime.TriggerSlowMotion(this.restrict_HS_BulletTimeCurve, this.restrict_HumanShield_BulletTimeDuration);
                    else if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Exit)
                        this.triggerBulletTime.StopSlowMotion();
                    break;
                }
           

        }
    }

    [SerializeField] private InGameLevelGameMaster gameMaster;
    [SerializeField] private Player player;
    private void OnValidate()
    {
        this.gameMaster = FindAnyObjectByType<InGameLevelGameMaster>();
        this.player = FindAnyObjectByType<Player>();
    }
}
