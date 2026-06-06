using System.Threading.Tasks;
using UnityEngine;

public partial class Player : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
       
        
        switch (node)
        {
            case OCM_Hit_NodeLeaf gunFuHitNodeLeaf:
                {
                    break;
                }
            case OCM_Execute_Single_NodeLeaf gunFuExecute_Single_NodeLeaf:
                {
                    if(gunFuExecute_Single_NodeLeaf._curPhase == NodePhase.Enter)
                    {
                        this.executeGauge.SetGauge(0);
                    }

                    if(gunFuExecute_Single_NodeLeaf._curPhase == NodePhase.Exit)
                    {
                        secondaryExecuteGunFuRandomNumber.UpdateGunFuNumber();
                        primaryExecuteGunFuRandomNumber.UpdateGunFuNumber();
                        TriggerIFrame(.75f);
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }

                    break;
                }
            case RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf:
                {
                    if (restrictGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Enter)
                    {
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }

                    break;
                }
            case HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf:
                {
                    if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
                    {
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }

                    break;
                }
            case PlayerBrounceOffNodeLeaf playerBrounceOffGotAttackGunFuNodeLeaf:
                {    
                    break;
                }
            case PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf:
                {
                    break;
                }
            case PlayerGetUpStateNodeLeaf playerGetUpStateNodeLeaf: 
                {
                    break;
                }
            case PlayerSprintNode playerSprintNode: 
                {
                    break;
                }
            case PlayerDodgeRollStateNodeLeaf playerDodgeRollStateNodeLeaf:
                {
                    if(playerDodgeRollStateNodeLeaf._curPhase == NodePhase.Enter)
                    {
                        TriggerIFrame(0.45f);
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }
                    break;
                }

        }

        if(node is SubjectPlayer.NotifyEvent.GetDamaged)
        {
            this.playerStateNodeManager.regenarateHPNodeLeaf.SetDelay(3);
            TriggerIFrame(1);
        }

    }


}
