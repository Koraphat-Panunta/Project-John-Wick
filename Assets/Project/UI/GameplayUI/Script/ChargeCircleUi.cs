using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChargeCircleUi : GameplayUI, IObserverPlayer
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

    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
       
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        switch (node)
        {
            case GunFuExecute_OnGround_Single_NodeLeaf gunFuExecuteNodeLeaf:
                {
                    if(gunFuExecuteNodeLeaf.curExecutePhase == GunFuExecute_OnGround_Single_NodeLeaf.GunFuExecutePhase.Execute)

                    if (targetFill <= 1)
                        ammoIcon.color = Color.white;
                    StartFillingAsync();
                    break;
                }
        }
    }
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
    private void OnValidate()
    {
        player = FindAnyObjectByType<Player>();
    }

    public override void EnableUI()
    {
        cirCleUI.enabled = true;
        ammoIcon.enabled = true;
    }

    public override void DisableUI()
    {
        cirCleUI.enabled = false;
        ammoIcon.enabled = false;
    }

 
}
