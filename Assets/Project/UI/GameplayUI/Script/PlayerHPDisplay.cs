
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPDisplay : GameplayUI, IObserverPlayer
{
    [SerializeField] private Image bg_HP_bar_image;
    [SerializeField] private Image front_HP_bar_image;
    [SerializeField] private Image back_HP_bar_image;
    [SerializeField] private Image iframe_HP_image;

    [SerializeField] private Color positiveHP_Bar_Color;
    [SerializeField] private Color negativeHP_Bar_Color;

    [SerializeField] private Player playerInfo;

    [Range(0, 1)]
    [SerializeField] private float changeVelocityBar;

    private float curHP_OnBar => (playerInfo.GetHP() / playerInfo.GetMaxHp());
    private float saveHP;

    float alphaColorIframeUI = 0;
    float changeSpeed = 29f;
    SetAlphaColorUI setAlphaColorUI = new SetAlphaColorUI();
    public override void Initialized()
    {
        playerInfo.AddObserver(this);
        saveHP = curHP_OnBar;
    }
   
    private void Start()
    {
        UpdateInfo();
    }
    private float iFrameTime = 0;   
    private void FixedUpdate()
    {
        if ((playerInfo.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>()
            || 
            ((playerInfo.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<RestrictGunFuStateNodeLeaf>(out RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf)
                && restrictGunFuStateNodeLeaf._timer < playerInfo.restrictShieldIFrame)
                || 
                ((playerInfo.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>(out HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction)
                && humanShield_GunFuInteraction._timer < playerInfo.humanShiedlIFrame)
            || iFrameTime > 0)
        {
            setAlphaColorUI.SetColorAlpha(iframe_HP_image, alphaColorIframeUI);
            alphaColorIframeUI = Mathf.Clamp01(alphaColorIframeUI + (Time.deltaTime * changeSpeed));
            iFrameTime -= Time.deltaTime;   
        }
        else
        {
            alphaColorIframeUI = Mathf.Clamp01(alphaColorIframeUI - (Time.deltaTime * changeSpeed));
            setAlphaColorUI.SetColorAlpha(iframe_HP_image, alphaColorIframeUI);
        }
    }
    private void OnValidate()
    {
        playerInfo = FindAnyObjectByType<Player>();
    }

    public void UpdateInfo()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();

        }

        tokenSource = new CancellationTokenSource();

        if (saveHP < curHP_OnBar)
        {
            _ = UpdatePositiveHP(tokenSource.Token);
        }
        else
        {
            _ = UpdateNegativeHP(tokenSource.Token);
        }

        saveHP = curHP_OnBar;
    }

    private CancellationTokenSource tokenSource;
    private async Task UpdatePositiveHP(CancellationToken token)
    {
        this.back_HP_bar_image.color = positiveHP_Bar_Color;

        this.back_HP_bar_image.rectTransform.localScale
            = new Vector2(back_HP_bar_image.rectTransform.localScale.x, this.curHP_OnBar);

        try
        {
            while (this.front_HP_bar_image.rectTransform.localScale.y < this.curHP_OnBar)
            {
                token.ThrowIfCancellationRequested();

                this.front_HP_bar_image.rectTransform.localScale
                    = new Vector2(front_HP_bar_image.rectTransform.localScale.x, front_HP_bar_image.rectTransform.localScale.y + (Time.deltaTime * changeVelocityBar));

                await Task.Yield();
            }

            this.front_HP_bar_image.rectTransform.localScale
                   = new Vector2(front_HP_bar_image.rectTransform.localScale.x, this.curHP_OnBar);
        }
        catch
        {
            this.front_HP_bar_image.rectTransform.localScale
                   = new Vector2(front_HP_bar_image.rectTransform.localScale.x, this.curHP_OnBar);
            /*Task been Cancel*/
        }
    }
    private async Task UpdateNegativeHP(CancellationToken token)
    {

        this.back_HP_bar_image.color = negativeHP_Bar_Color;
        this.front_HP_bar_image.rectTransform.localScale
            = new Vector2(this.front_HP_bar_image.rectTransform.localScale.x, curHP_OnBar);

        try
        {
            while (this.back_HP_bar_image.rectTransform.localScale.y > this.curHP_OnBar)
            {
                token.ThrowIfCancellationRequested();

                this.back_HP_bar_image.rectTransform.localScale
                    = new Vector2(this.back_HP_bar_image.rectTransform.localScale.x
                    , Mathf.MoveTowards(this.back_HP_bar_image.rectTransform.localScale.y, this.curHP_OnBar, this.changeVelocityBar * Time.deltaTime));

                await Task.Yield();
            }

            //this.back_HP_bar_image.rectTransform.localScale
            //       = new Vector2(this.back_HP_bar_image.rectTransform.localScale.x, this.curHP_OnBar);


        }
        catch
        {
            //this.back_HP_bar_image.rectTransform.localScale
            //      = new Vector2(this.back_HP_bar_image.rectTransform.localScale.x, this.curHP_OnBar);
            /*Task been Cancel*/
        }
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
    private void OnDisable()
    {
        this.tokenSource.Cancel();
        this.tokenSource.Dispose();
        this.tokenSource = null;
    }
    public void OnNotify<T>(Player player, T node)
    {
        if (node is SubjectPlayer.NotifyEvent playerEvent)
        {
            if (playerEvent == SubjectPlayer.NotifyEvent.GetDamaged)
            {
                UpdateInfo();
            }
            if (playerEvent == SubjectPlayer.NotifyEvent.HealthRegen)
            {
                UpdateInfo();
            }
            if (playerEvent == SubjectPlayer.NotifyEvent.RecivedHp)
            {
                UpdateInfo();
            }
            if(playerEvent == SubjectPlayer.NotifyEvent.TriggerIframe)
                iFrameTime = playerInfo.iFrameTime;
        }
    }

  
}
