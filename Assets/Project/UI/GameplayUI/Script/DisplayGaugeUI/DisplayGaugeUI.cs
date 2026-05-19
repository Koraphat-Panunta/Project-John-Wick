
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class DisplayGaugeUI : GameplayUI, IObserverPlayer
{
    [SerializeField] protected Image bg_HP_bar_image;
    [SerializeField] protected Image front_HP_bar_image;
    [SerializeField] protected Image back_HP_bar_image;
    [SerializeField] protected Image iframe_HP_image;

    [SerializeField] protected Color positiveHP_Bar_Color;
    [SerializeField] protected Color negativeHP_Bar_Color;

    [SerializeField] protected Player playerInfo;

    [Range(0, 1)]
    [SerializeField] protected float changeVelocityBar;

    protected abstract float gaugeValueRefNormalized { get; }
    protected float curGaugeValue { get => this.gaugeValueRefNormalized * this.maxAmount; }
    protected float fillAmount;
    [SerializeField] protected float maxAmount;

    float alphaColorIframeUI = 0;
    float changeSpeed = 29f;
    SetAlphaColorUI setAlphaColorUI = new SetAlphaColorUI();
    public override void Initialized()
    {
        playerInfo.AddObserver(this);
        this.fillAmount = curGaugeValue;
    }
   
    private void Start()
    {
    }
    //private float iFrameTime = 0;   
    private void FixedUpdate()
    {
        this.UpdateHPBar();

        //if ((playerInfo.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>()
        //    || 
        //    ((playerInfo.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>(out RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf)
        //        && restrainGunFuStateNodeLeaf._timer < playerInfo.restrictShieldIFrame)
        //        || 
        //        ((playerInfo.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>(out HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction)
        //        && humanShield_GunFuInteraction.subject_GunFuAble.animationTriggerEventPlayer.timer < playerInfo.humanShiedlIFrame)
        //    || iFrameTime > 0)
        //{
        //    if(this.iframe_HP_image == null)
        //        return;

        //    setAlphaColorUI.SetColorAlpha(iframe_HP_image, alphaColorIframeUI);
        //    alphaColorIframeUI = Mathf.Clamp01(alphaColorIframeUI + (Time.deltaTime * changeSpeed));
        //    iFrameTime -= Time.deltaTime;   
        //}
        //else
        //{
        //    if (this.iframe_HP_image == null)
        //        return;

        //    alphaColorIframeUI = Mathf.Clamp01(alphaColorIframeUI - (Time.deltaTime * changeSpeed));
        //    setAlphaColorUI.SetColorAlpha(iframe_HP_image, alphaColorIframeUI);
        //}

       
    }
    private void OnValidate()
    {
        playerInfo = FindAnyObjectByType<Player>();
    }
    private void UpdateHPBar()
    {
        if (this.fillAmount < curGaugeValue)
        {
            this.UpdatePositiveHP();
        }
        else
        {
            this.UpdateNegativeHP();
        }

        this.fillAmount = Mathf.MoveTowards(this.fillAmount, this.curGaugeValue, this.changeVelocityBar * Time.deltaTime);
    }
    private void UpdatePositiveHP()
    {
        this.back_HP_bar_image.color = positiveHP_Bar_Color;
        this.back_HP_bar_image.fillAmount = this.curGaugeValue;
        this.front_HP_bar_image.fillAmount = this.fillAmount;
    }

    private void UpdateNegativeHP()
    {
        this.back_HP_bar_image.color = negativeHP_Bar_Color;
        this.front_HP_bar_image.fillAmount = this.curGaugeValue;
        this.back_HP_bar_image.fillAmount = this.fillAmount;


    }

    
    public override void EnableUI()
    {
        this.front_HP_bar_image.enabled = true;
        this.back_HP_bar_image.enabled = true;
        this.bg_HP_bar_image.enabled= true;
    }
    public override void DisableUI()
    {
        this.front_HP_bar_image.enabled = false;
        this.back_HP_bar_image.enabled = false;
        this.bg_HP_bar_image.enabled = false;
    }
    
    public virtual void OnNotify<T>(Player player, T node)
    {
        if (node is SubjectPlayer.NotifyEvent playerEvent)
        {
 
            //if(playerEvent == SubjectPlayer.NotifyEvent.TriggerIframe)
            //    iFrameTime = playerInfo.iFrameTime;
        }
    }

  
}
