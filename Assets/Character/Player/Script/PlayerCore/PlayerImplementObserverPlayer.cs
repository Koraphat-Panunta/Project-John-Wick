using System.Threading.Tasks;
using UnityEngine;

public partial class Player : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
       
        
        switch (node)
        {
            case GunFuHitNodeLeaf gunFuHitNodeLeaf:
                {
                    if(gunFuHitNodeLeaf.curPhaseGunFuHit == GunFuHitNodeLeaf.GunFuPhaseHit.Enter)
                    this.DrainStamina(this.playerStatsScriptableObject.HitStaminaDrain);
                    break;
                }
            case GunFuExecute_Single_NodeLeaf gunFuExecute_Single_NodeLeaf:
                {
                    if(gunFuExecute_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                    {
                        this.executeGauge.SetGauge(0);
                    }

                    if(gunFuExecute_Single_NodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
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
                        this.DrainStamina(this.playerStatsScriptableObject.restrainHumanShieldStaminaDrain);
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }

                    break;
                }
            case HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction_NodeLeaf:
                {

                    if (humanShield_GunFuInteraction_NodeLeaf.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Enter)
                    {
                        this.DrainStamina(this.playerStatsScriptableObject.restrainHumanShieldStaminaDrain);
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }

                    break;
                }
            case PlayerBrounceOffGotAttackGunFuNodeLeaf playerBrounceOffGotAttackGunFuNodeLeaf:
                {    
                    player.stanceCommand = Stance.prone;
                    break;
                }
            case PlayerDolphinDiveStateNodeLeaf playerDolphinDiveStateNodeLeaf:
                {
                    if(playerDolphinDiveStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                    {
                        this.DrainStamina(this.playerStatsScriptableObject.dolphinDiveStaminaDrain);
                    }
                    player.stanceCommand = Stance.prone;
                    break;
                }
            case PlayerGetUpStateNodeLeaf playerGetUpStateNodeLeaf: 
                {
                    player.stanceCommand = Stance.stand;
                    break;
                }
            case PlayerSprintNode playerSprintNode: 
                {
                    player.stanceCommand = Stance.stand;
                    break;
                }
            case PlayerDodgeRollStateNodeLeaf playerDodgeRollStateNodeLeaf: 
                {
                    if(playerDodgeRollStateNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Enter)
                    {
                        this.DrainStamina(this.playerStatsScriptableObject.dodgeStaminaDrain);
                        TriggerIFrame(0.45f);
                        NotifyObserver(player, SubjectPlayer.NotifyEvent.TriggerIframe);
                    }
                    player.stanceCommand = Stance.stand;
                    break;
                }

        }

        if(node is SubjectPlayer.NotifyEvent.GetDamaged)
        {
            this.playerStateNodeManager.regenarateHPNodeLeaf.SetDelay(3);
            TriggerIFrame(1);
        }

    }

    private void DrainStamina(float value)
    {
        Debug.Log("Drain Stamina");
        this.staminaGauge.AddGauge(-value);
        this.playerStateNodeManager.regenarateStaminaNodeLeaf.SetDelay(this.playerStatsScriptableObject.delayStaminaDrain);
    }


}
