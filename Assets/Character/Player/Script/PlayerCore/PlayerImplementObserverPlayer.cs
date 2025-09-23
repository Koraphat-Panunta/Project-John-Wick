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
                        AddHP(10);
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
                        PlayerImlementObserverBehavior.HitStop(gunFuHitNodeLeaf,player.timeControlBehavior);
                    break;
                }
            case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {
                    if(restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
                        timeControlBehavior.TriggerBulletTime(this.restrictBulletTimeDuration, this.restrictBulletTimeCurve);
                    else if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    else if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Exit)
                        timeControlBehavior.TriggerCancelTimeControlBehavior();
                    break;
                }
            case HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf:
                {
                    if(humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Stay)
                        timeControlBehavior.TriggerBulletTime(this.humanShieldBulletTimeDuration, this.humanShiedlBulletTimeCurve);
                    else if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Enter)
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    else if(humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Exit)
                        timeControlBehavior.TriggerCancelTimeControlBehavior();
                    break;
                }

        }
        if((node is GunFuExecute_Single_NodeLeaf execute_Single_NodeLeaf
            && execute_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
            ||(node is GunFuExecute_OnGround_Single_NodeLeaf gunFuExecute_OnGround_Single_NodeLeaf
            && gunFuExecute_OnGround_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit))
        {
            TriggerIFrame(.75f);
            NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
        }

       
        if(node is PlayerDodgeRollStateNodeLeaf dodgeRollStateNodeLeaf && dodgeRollStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
        {
            TriggerIFrame(0.45f);
            NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
        }
        
        if(node is SubjectPlayer.NotifyEvent.GetDamaged)
        {
            regenHPDisableTimer = regenHPDisableTime;
            TriggerIFrame(0.5f);
        }

    }

    private static class PlayerImlementObserverBehavior
    {

        public static async void HitStop(GunFuHitNodeLeaf gunFuHitNodeLeaf,TimeControlBehavior timeControlBehavior)
        {
            await Task.Yield();
            try
            {
                timeControlBehavior.TriggerTimeStop
                   (gunFuHitNodeLeaf.gunFuHitScriptableObject.hitStopDuration[gunFuHitNodeLeaf.hitCount]
                   , gunFuHitNodeLeaf.gunFuHitScriptableObject.hitResetDuration[gunFuHitNodeLeaf.hitCount]
                   , gunFuHitNodeLeaf.gunFuHitScriptableObject.hitSlowMotionCurve[gunFuHitNodeLeaf.hitCount]
                   );
            }
            catch
            {
                timeControlBehavior.TriggerTimeStop
                   (gunFuHitNodeLeaf.gunFuHitScriptableObject.hitStopDuration[gunFuHitNodeLeaf.hitCount]
                   , gunFuHitNodeLeaf.gunFuHitScriptableObject.hitResetDuration[gunFuHitNodeLeaf.hitCount]
                   );
            }
        }
    }
}
