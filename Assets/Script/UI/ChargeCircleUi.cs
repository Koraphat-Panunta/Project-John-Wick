using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChargeCircleUi : MonoBehaviour,IObserverPlayer
{
    [SerializeField] private Image cirCleUI;
    [SerializeField] private float fillSpeed;
    [SerializeField] private Player player;
    [SerializeField] private Image ammoIcon;

    private Task updateCircleUI;

    private float fillAmount
    {
        get => cirCleUI.fillAmount;
        set => cirCleUI.fillAmount = value;
    }

    private float targetFill => playerGunFuExecuteStackGauge.amount / playerGunFuExecuteStackGauge.max;

    private StackGague playerGunFuExecuteStackGauge => player.gunFuExecuteStackGauge;

    private void Awake()
    {
        player.AddObserver(this);
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.GunFuAttack && player.playerStateNodeManager.curNodeLeaf is GunFuExecuteNodeLeaf gunFuExecute)
        {
            if(targetFill <= 1)
                ammoIcon.color = Color.white;
            StartFillingAsync();
        }
    }

    public void OnNotify(Player player) { }

    private async void StartFillingAsync()
    {
        if (updateCircleUI == null)
        {
            updateCircleUI = FillToTargetAsync();
            await updateCircleUI;
            updateCircleUI = null;
        }
    }

    private async Task FillToTargetAsync()
    {
        while (Mathf.Abs(fillAmount - targetFill) > 0.01f) // Continue until close enough
        { 
            fillAmount = Mathf.MoveTowards(fillAmount, targetFill, fillSpeed * Time.deltaTime);
            await Task.Yield(); // Wait until the next frame
        }

        if (targetFill >= 1)
            ammoIcon.color =Color.yellow;
        
        fillAmount = targetFill; // Ensure final value is accurate
    }


}
