using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static SubjectPlayer;

public class WeaponAfterActionPlayer : WeaponAfterAction
{
    private Player player;
    private Dictionary<Enemy, bool> isKilleComfirm = new Dictionary<Enemy, bool>();
    public WeaponAfterActionPlayer(Player player)
    {
        this.player = player;
    }

    public override void SendFeedBackWeaponAfterAction<T>(WeaponAfterActionSending weaponAfterActionSending, T Var)
    {

        if (weaponAfterActionSending == WeaponAfterActionSending.WeaponStateNodeActive)
        {
            switch (Var)
            {
                case FiringNode firingNode:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, NotifyEvent.Firing);
                        break;
                    }
                case ReloadMagazineFullStageNodeLeaf _reloadMagFullStage:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, _reloadMagFullStage);
                        break;
                    }
                case TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagFullStage:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, _tacticalReloadMagFullStage);
                        break;
                    }
                case AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf:
                    {
                        if (player.stance != Stance.prone)
                        {
                            player._movementCompoent.SetRotation(Quaternion.Lerp(
                                this.player.playerMovement.characterController.rotation
                                , Quaternion.LookRotation(new Vector3(this.player.cinemachineCamera.targetDir.x, player.transform.forward.y, this.player.cinemachineCamera.targetDir.z))
                                , aimDownSightWeaponManuverNodeLeaf.weaponManuverManager.aimingWeight));
                        }

                        player.NotifyObserver(player, aimDownSightWeaponManuverNodeLeaf);

                        break;
                    }
                case QuickShootRangeWeaponNodeLeaf quickShootRangeWeaponNodeLeaf:
                    {
                        if (quickShootRangeWeaponNodeLeaf.target != null)
                        {
                            float rotateSPeed = SlowDownRotateSpeed.GetSlowDownRotateSpeedOnNearlyTargetRotation(this.player.cinemachineCamera.transform.forward, quickShootRangeWeaponNodeLeaf.shootDir, 45, 300);
                            this.player.cinemachineCamera.RotateCameraTowardsDirection(quickShootRangeWeaponNodeLeaf.shootDir, rotateSPeed);
                        }
                        player.NotifyObserver(player, quickShootRangeWeaponNodeLeaf);
                    }
                    break;
                case LowReadyWeaponManuverNodeLeaf lowReady:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, lowReady);
                        break;
                    }
                case DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, dropWeaponManuverNodeLeaf);
                    }
                    break;
                case PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, pickUpWeaponNodeLeaf);
                    }
                    break;
                case HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, holsterPrimaryWeaponManuverNodeLeaf);
                    }
                    break;
                case HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, holsterSecondaryWeaponManuverNodeLeaf);
                    }
                    break;
                case DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, drawPrimaryWeaponManuverNodeLeaf);
                    }
                    break;
                case DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, drawSecondaryWeaponManuverNodeLeaf);
                    }
                    break;
                case PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, primaryToSecondarySwitchWeaponManuverLeafNode);
                    }
                    break;
                case SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, secondaryToPrimarySwitchWeaponManuverLeafNode);
                    }
                    break;
                case RestWeaponManuverLeafNode restWeaponManuverLeafNode:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, restWeaponManuverLeafNode);
                        break;
                    }
                case IQuickSwitchNode quickSwitchNode:
                    {
                        player.commandBufferManager.RemoveCommand(nameof(player._isPullTriggerCommand));
                        player.NotifyObserver(player, quickSwitchNode);
                        break;
                    }

            }
        }
      
    }
   
}
