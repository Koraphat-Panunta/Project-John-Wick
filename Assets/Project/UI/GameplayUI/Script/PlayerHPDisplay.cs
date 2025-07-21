
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPDisplay : GameplayUI, IObserverPlayer
{
    [SerializeField] private RawImage HP_bar;
    private float maxHP_BAR_Lenght;
    [SerializeField] private Player playerInfo;

    private void Awake()
    {
        this.maxHP_BAR_Lenght = HP_bar.rectTransform.sizeDelta.y;
        playerInfo.AddObserver(this);
    }
    private void Start()
    {
        UpdateInfo();
    }
    private void OnValidate()
    {
        playerInfo = FindAnyObjectByType<Player>();
    }

    public void UpdateInfo()
    {
        HP_bar.rectTransform.sizeDelta = new Vector2(HP_bar.rectTransform.sizeDelta.x, this.maxHP_BAR_Lenght*(playerInfo.GetHP()/playerInfo.GetMaxHp()));
    }

    public override void EnableUI() => this.HP_bar.enabled = true;
   

    public override void DisableUI() => this.HP_bar.enabled = false;

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
        }
    }
}
