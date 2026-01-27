using UnityEngine;

public partial class PlayerConstrainAnimationManager : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
        if(node is GunFuExecute_Single_NodeLeaf gunFuExecuteNodeLeaf)
        {
            gunFuExecuteNodeLeaf.animationTriggerEventPlayer.SubscribeEvent("EnableIK", this.EnableIK);
            if(gunFuExecuteNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                isEnableIK = false;
        }
    }
}
