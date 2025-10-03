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

                        break;
                    }
            }
        }
        
        switch (node)
        {
           
            case RestrictGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {

                     if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrictGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);

                    break;
                }
            case HumanShield_GunFuInteraction_NodeLeaf humanShield_GunFuInteraction_NodeLeaf:
                {

                     if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFuInteraction_NodeLeaf.HumanShieldInteractionPhase.Enter)
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);

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
            TriggerIFrame(0.25f);
        }

    }


}
