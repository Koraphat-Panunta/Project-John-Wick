using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class HumanShieldStayGage : GameplayUI, IObserverPlayer
{
    [SerializeField] Player player;
    private bool isShowGage ;
    [SerializeField] RawImage humanShieldGage;
    private float maxWidthImage;
    HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf;

    public void GetNotify(Player player)
    {
        this.player = player;
        this.player.AddObserver(this);
    }

    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if(node is PlayerStateNodeLeaf playerStateNodeLeaf)
            switch (playerStateNodeLeaf)
            {
                case HumanShield_GunFuInteraction_NodeLeaf humanShieldNodeLeaf:
                    {
                        if (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay)
                        {
                            this.humanShield_GunFuInteraction_NodeLeaf = humanShieldNodeLeaf;
                            isShowGage = true;
                        }
                        else if (humanShieldNodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Exit)
                            isShowGage = false;
                        break;
                    }
            }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        player.AddObserver(this);
        this.maxWidthImage = humanShieldGage.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(isShowGage == true)
        {
            humanShieldGage.enabled = true;
            humanShieldGage.rectTransform.localScale = new Vector3(1-(humanShield_GunFuInteraction_NodeLeaf.elapesTimmerStay
              / humanShield_GunFuInteraction_NodeLeaf.StayDuration)
              , humanShieldGage.rectTransform.localScale.y
              , humanShieldGage.rectTransform.localScale.z);
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
