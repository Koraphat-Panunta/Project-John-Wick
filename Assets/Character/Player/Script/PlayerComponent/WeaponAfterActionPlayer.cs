using System.Collections.Generic;
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
                        player.NotifyObserver(player, NotifyEvent.Firing);
                        break;
                    }
                case ReloadMagazineFullStageNodeLeaf _reloadMagFullStage:
                    {    
                        player.NotifyObserver(player,_reloadMagFullStage);
                        break;
                    }
                case TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagFullStage:
                    {
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
                        player.NotifyObserver(player, lowReady);
                        break;
                    }
                case DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf: 
                    player.NotifyObserver(player, dropWeaponManuverNodeLeaf);
                    break ;
                case PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf:
                    player.NotifyObserver(player, pickUpWeaponNodeLeaf);
                    break;
                case HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf:
                    player.NotifyObserver(player, holsterPrimaryWeaponManuverNodeLeaf);
                    break;
                case HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf:
                    player.NotifyObserver(player, holsterSecondaryWeaponManuverNodeLeaf);
                    break;
                case DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf:
                    player.NotifyObserver(player, drawPrimaryWeaponManuverNodeLeaf);
                    break;
                case DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf:
                    player.NotifyObserver(player, drawSecondaryWeaponManuverNodeLeaf);
                    break;
                case PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode:
                    player.NotifyObserver(player, primaryToSecondarySwitchWeaponManuverLeafNode);
                    break;
                case SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode:
                    player.NotifyObserver(player, secondaryToPrimarySwitchWeaponManuverLeafNode);
                    break;
                case RestWeaponManuverLeafNode restWeaponManuverLeafNode:
                    {
                        player.NotifyObserver(player, restWeaponManuverLeafNode);
                        break;
                    }
                case QuickDrawWeaponManuverLeafNodeLeaf quickDrawNodeLeaf:
                    {
                        player.NotifyObserver(player, quickDrawNodeLeaf);
                        RotateObjectToward rotateObjectToward = new RotateObjectToward();
                        rotateObjectToward.RotateToward(Camera.main.transform.forward, player.gameObject, 6);
                        break;
                    }
            }
        }
        else
            this.NoneWeaponStateEvent<T>(weaponAfterActionSending, Var);
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
}
