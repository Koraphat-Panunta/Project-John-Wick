using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class HumanShieldStayGage :MonoBehaviour, IObserverPlayer
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

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.GunFuInteract)
        {
            if (player.playerStateNodeManager.curNodeLeaf is HumanShield_GunFuInteraction_NodeLeaf humanShieldNode)
                if (humanShieldNode.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.InteractionPhase.Stay)
                {
                    this.humanShield_GunFuInteraction_NodeLeaf = humanShieldNode;
                    isShowGage = true;
                }
        }
        if(playerAction == SubjectPlayer.PlayerAction.GunFuExit)
            isShowGage = false;
    }

    public void OnNotify(Player player)
    {
        
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

   
}
