using UnityEngine;

public class StackGague
{
   public float amount { get; private set; }
   public float max { get; private set; }
   public StackGague(float maxAmount,float startAmount)
   {
        this.amount = startAmount;
        this.max = maxAmount;
   }

    public void SetAmout(float amount)=> this.amount = Mathf.Clamp(amount, 0, max);
    public void AddAmount(float amount)=> this.amount = Mathf.Clamp(this.amount+amount, 0, max);
    public void SetMaxAmount(float maxAmount) => this.max = maxAmount;
}
public class PlayerGunFuExecuteStackGauge : StackGague, IObserverPlayer
{
    private Player player;
    public PlayerGunFuExecuteStackGauge(Player player,float maxAmount, float startAmount) : base(maxAmount, startAmount)
    {
        this.player = player;
        player.AddObserver(this);
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.GunFuAttack && 
            player.playerStateNodeManager.curNodeLeaf is GunFuExecuteNodeLeaf gunFuExecute) 
        {
            if (this.amount >= this.max)
            {
                Debug.Log("SetAmout");
                SetAmout(0);
                return;
            }
            if (this.amount < this.max)
            {
                Debug.Log("AddAmount");
                AddAmount(this.max/3);
                return;
            }
           

        }
    }

    public void OnNotify(Player player)
    {
       
    }
}
