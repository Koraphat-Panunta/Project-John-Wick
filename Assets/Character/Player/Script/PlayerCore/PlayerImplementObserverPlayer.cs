using System.Threading.Tasks;
using UnityEngine;

public partial class Player : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
        if(node is GunFuExecute_Single_NodeLeaf gunFuExecute 
            && gunFuExecute.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
        {
            secondaryExecuteGunFuRandomNumber.UpdateGunFuNumber();
            primaryExecuteGunFuRandomNumber.UpdateGunFuNumber();
        }
        
        switch (node)
        {
           
            case RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {

                     if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);

                    break;
                }
            case HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf:
                {

                     if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);

                    break;
                }

        }
        if((node is GunFuExecute_Single_NodeLeaf execute_Single_NodeLeaf
            && execute_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
            )
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
