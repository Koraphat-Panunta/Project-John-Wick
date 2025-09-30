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
                        RotateObjectToward rotateObjectToward = new RotateObjectToward();
                        rotateObjectToward.RotateToward(Camera.main.transform.forward, player.gameObject, 6);
                        player.NotifyObserver(player, aimDownSightWeaponManuverNodeLeaf);

                        break;
                    }
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
        else
        {
            this.NoneWeaponStateEvent<T>(weaponAfterActionSending, Var);

            if(this.curTask == null)
                this.curTask = ClearKilledConfirmEnemy();
        }
    }
    private void NoneWeaponStateEvent<T>(WeaponAfterActionSending weaponAfterActionSending, T Var)
    {
        switch (weaponAfterActionSending)
        {
            case WeaponAfterActionSending.HitConfirm:
                {
                    Enemy enemy;
                    IBulletDamageAble bulletDamageAble = Var as IBulletDamageAble;
                    if (bulletDamageAble is BodyPart)
                    {
                        enemy = (bulletDamageAble as BodyPart).enemy;

                        if (isKilleComfirm.ContainsKey(enemy) == false)
                            isKilleComfirm.Add(enemy, false);

                        if (isKilleComfirm[enemy])
                            return;

                        if (enemy.isDead)
                        {
                            player.NotifyObserver(player, NotifyEvent.OpponentKilled);
                            isKilleComfirm[enemy] = true;
                            return;
                        }

                        if (enemy._posture <= 0)
                        {
                            player.NotifyObserver(player, NotifyEvent.OppenentStagger);

                        }
                    }
                    if (bulletDamageAble is Enemy thisenemy)
                    {
                        enemy = thisenemy;

                        if (isKilleComfirm.ContainsKey(enemy) == false)
                            isKilleComfirm.Add(enemy, false);

                        if (isKilleComfirm[enemy])
                            return;

                        if (enemy.isDead)
                        {
                            player.NotifyObserver(player, NotifyEvent.OpponentKilled);
                            isKilleComfirm[enemy] = true;
                            return;
                        }

                        if (enemy._posture <= 0)
                        {
                            player.NotifyObserver(player, NotifyEvent.OppenentStagger);

                        }
                    }
                    break;
                }
        }
            
    }
    private Task curTask;
    private async Task ClearKilledConfirmEnemy()
    {
        try
        {
            while (isKilleComfirm.Count > 0)
            {
                Debug.Log("isKilleComfirm.Count = " + isKilleComfirm.Count);
                List<Enemy> enemies = isKilleComfirm.Keys.ToList();
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] == null || enemies[i].gameObject.activeSelf == false)
                        isKilleComfirm.Remove(enemies[i]);
                }
                await Task.Yield();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in ClearKilledConfirmEnemy: {ex}");
        }
        finally
        {
            curTask = null;
        }
    }
}
