
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPDisplay : MonoBehaviour,IObserverPlayer,IGameplayUI
{
    [SerializeField] private RawImage HP_bar;
    private float maxHP_BAR_Lenght;
    [SerializeField] private Player playerInfo;

    private void Awake()
    {
        this.maxHP_BAR_Lenght = HP_bar.rectTransform.sizeDelta.y;
        Debug.Log("HP_bar.rectTransform.sizeDelta.x = " + HP_bar.rectTransform.sizeDelta.y);
        playerInfo.AddObserver(this);
        UpdateInfo();
    }
    private void OnValidate()
    {
        playerInfo = FindAnyObjectByType<Player>();
    }


    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.GetDamaged)
        {
           UpdateInfo();
        }
        if (playerAction == SubjectPlayer.PlayerAction.HealthRegen)
        {
            UpdateInfo();
        }
        if (playerAction == SubjectPlayer.PlayerAction.RecivedHp)
        {
            UpdateInfo();
        }
    }

    public void OnNotify(Player player)
    {
        
    }

    public void UpdateInfo()
    {
        HP_bar.rectTransform.sizeDelta = new Vector2(HP_bar.rectTransform.sizeDelta.x, this.maxHP_BAR_Lenght*(playerInfo.GetHP()/playerInfo.GetMaxHp()));
    }

    public void EnableUI() => this.HP_bar.enabled = true;
   

    public void DisableUI() => this.HP_bar.enabled = false;
    
}
