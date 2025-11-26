using UnityEngine;
using UnityEngine.UI;

public class HumanShieldStayGage : GameplayUI, IObserverPlayer
{
    [SerializeField] Player player;
    private bool isShowGage ;
    [SerializeField] Image humanShieldGage;
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
                case HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf:
                    {
                        if (humanShieldNodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay)
                        {
                            this.curGunFuInteraction_NodeLeaf = humanShieldNodeLeaf;
                            isShowGage = true;
                        }
                        else if (humanShieldNodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Exit)
                            isShowGage = false;
                        break;
                    }
                case RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf: 
                    {
                        if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                        {
                            this.curGunFuInteraction_NodeLeaf=restrictGunFuStateNodeLeaf;
                            isShowGage = true;

                        }
                        else if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
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
                case HumanShield_GunFu_NodeLeaf humanShieldNodeLeaf:
                    {
                        humanShieldGage.enabled = true;
                        humanShieldGage.rectTransform.localScale = new Vector3(1 - (humanShieldNodeLeaf.humanShield_Stay_Timer
                          / humanShieldNodeLeaf.humanShield_Stay_Duration)
                          , humanShieldGage.rectTransform.localScale.y
                          , humanShieldGage.rectTransform.localScale.z);
                        break;
                    }
                case RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
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
