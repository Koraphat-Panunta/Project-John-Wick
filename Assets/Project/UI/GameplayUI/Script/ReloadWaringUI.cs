using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReloadWaringUI : GameplayUI, IObserverPlayer
{
    [SerializeField] Player player;
    [SerializeField] Image reloadWarningUIBG;
    [SerializeField] TextMeshProUGUI reloadWaringText;
    public override void Initialized()
    {
        this.player.AddObserver(this);
        reloadWarningUIBG.enabled = false;
        reloadWaringText.enabled = false;
    }

    public void OnNotify<T>(Player player, T node)
    {
        if(player._currentWeapon != null && player._currentWeapon.bulletStore[BulletStackType.Magazine] <= 0 && isEnable)
        {
            reloadWarningUIBG.enabled = true;
            reloadWaringText.enabled = true;
        }
        else
        {
            reloadWarningUIBG.enabled = false;
            reloadWaringText.enabled = false;
        }
    }
    public override void EnableUI()
    {
        this.player.AddObserver(this);
        //reloadWarningUIBG.enabled = true;
        //reloadWaringText.enabled = true;
        base.EnableUI();
    }
    public override void DisableUI()
    {
        this.player.RemoveObserver(this);
        reloadWarningUIBG.enabled = false;
        reloadWaringText.enabled = false;
        base.DisableUI();
    }
    private void OnValidate()
    {
        if(this.player == null)
            this.player = FindAnyObjectByType<Player>();
    }
}
