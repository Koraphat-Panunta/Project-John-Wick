using UnityEngine;
using UnityEngine.UI;

public class HumanShieldStayGage : GameplayUI, IObserverPlayer
{
    [SerializeField] Player player;
    private bool isShowGage ;
    [SerializeField] RawImage humanShieldGage;
    private float maxWidthImage;
    IGunFuNode curGunFuInteraction_NodeLeaf;

    public override void Initialized()
    {
        player.AddObserver(this);
        this.maxWidthImage = humanShieldGage.rectTransform.rect.width;
    }
    public void GetNotify(Player player)
    {
        this.player = player;
        this.player.AddObserver(this);
    }

   
    public void OnNotify<T>(Player player, T node)
    {
        if(node is PlayerStateNodeLeaf playerStateNodeLeaf)
            switch (playerStateNodeLeaf)
            {
                case HumanShield_GunFuInteraction_NodeLeaf humanShieldNodeLeaf:
                    {
                        if (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay)
                        {
                            this.curGunFuInteraction_NodeLeaf = humanShieldNodeLeaf;
                            isShowGage = true;
                        }
                        else if (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Exit)
                            isShowGage = false;
                        break;
                    }
                case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf: 
                    {
                        if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                        {
                            this.curGunFuInteraction_NodeLeaf=restrictGunFuStateNodeLeaf;
                            isShowGage = true;

                        }
                        else if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                            isShowGage = false;
                        break;
                    }
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(isShowGage == true)
        {
            switch (curGunFuInteraction_NodeLeaf)
            {
                case HumanShield_GunFuInteraction_NodeLeaf humanShieldNodeLeaf:
                    {
                        humanShieldGage.enabled = true;
                        humanShieldGage.rectTransform.localScale = new Vector3(1 - (humanShieldNodeLeaf.elapesTimmerStay
                          / humanShieldNodeLeaf.StayDuration)
                          , humanShieldGage.rectTransform.localScale.y
                          , humanShieldGage.rectTransform.localScale.z);
                        break;
                    }
                case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                    {
                        humanShieldGage.enabled = true;
                        humanShieldGage.rectTransform.localScale = new Vector3(1 - (restrictGunFuStateNodeLeaf.phaseTimer
                          / restrictGunFuStateNodeLeaf.StayDuration)
                          , humanShieldGage.rectTransform.localScale.y
                          , humanShieldGage.rectTransform.localScale.z);

                        break;
                    }
            }
           
        }
        if(isShowGage == false)
        {
            humanShieldGage.enabled = false;
        }

    }
    private void OnValidate()
    {
        this.player = FindAnyObjectByType<Player>();
    }

    public override void EnableUI() => this.humanShieldGage.enabled = true;
    public override void DisableUI() => this.humanShieldGage.enabled=false;

   
}
