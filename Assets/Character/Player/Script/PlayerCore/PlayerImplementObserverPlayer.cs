using System.Threading.Tasks;
using UnityEngine;

public partial class Player : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node) where T : INode
    {
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
    }

    public void OnNotify(Player player, NotifyEvent notifyEvent)
    {
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
