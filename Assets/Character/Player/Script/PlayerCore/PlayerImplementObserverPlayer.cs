using System.Threading.Tasks;
using UnityEngine;

public partial class Player : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
        if(node is IGunFuExecuteNodeLeaf.GunFuExecutePhase gunFuExecutePhase)
        {
            switch (gunFuExecutePhase)
            {
                case IGunFuExecuteNodeLeaf.GunFuExecutePhase.Execute:
                    {
                        AddHP(25);
                        player.NotifyObserver<SubjectPlayer.NotifyEvent>(player, SubjectPlayer.NotifyEvent.HealthRegen);
                        break;
                    }
            }
        }
        
        switch (node)
        {
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if (gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Attacking)
                    {
                        PlayerImlementObserverBehavior.HitStop(gunFuHitNodeLeaf);
                    }
                    break;
                }
        }
        if((node is GunFuExecute_Single_NodeLeaf execute_Single_NodeLeaf
            && execute_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
            ||(node is GunFuExecute_OnGround_Single_NodeLeaf gunFuExecute_OnGround_Single_NodeLeaf
            && gunFuExecute_OnGround_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit))
        {
            TriggerIFrame(1);
        }
        if(node is SubjectPlayer.NotifyEvent.GetDamaged)
        {
            TriggerIFrame(0.45f);
        }
           
    }
    private static class PlayerImlementObserverBehavior
    {
        public static async void HitStop(GunFuHitNodeLeaf gunFuHitNodeLeaf)
        {
            await Task.Yield();
            try
            {
                TimeControlBehavior.TriggerTimeStop
                   (gunFuHitNodeLeaf.gunFuHitScriptableObject.hitStopDuration[gunFuHitNodeLeaf.hitCount - 1]
                   , gunFuHitNodeLeaf.gunFuHitScriptableObject.hitResetDuration[gunFuHitNodeLeaf.hitCount - 1]
                   , gunFuHitNodeLeaf.gunFuHitScriptableObject.hitSlowMotionCurve[gunFuHitNodeLeaf.hitCount - 1]
                   );
            }
            catch
            {
                TimeControlBehavior.TriggerTimeStop
                   (gunFuHitNodeLeaf.gunFuHitScriptableObject.hitStopDuration[gunFuHitNodeLeaf.hitCount - 1]
                   , gunFuHitNodeLeaf.gunFuHitScriptableObject.hitResetDuration[gunFuHitNodeLeaf.hitCount - 1]
                   );
            }
        }
    }
}
