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
                        if(_reloadMagFullStage.curReloadStage == ReloadMagazineFullStageNodeLeaf.ReloadStage.Enter)
                            player.NotifyObserver(player, NotifyEvent.ReloadMagazineFullStage);
                        break;
                    }
                case TacticalReloadMagazineFullStageNodeLeaf _tacticalReloadMagFullStage:
                    {

                        if (_tacticalReloadMagFullStage.curReloadStage == TacticalReloadMagazineFullStageNodeLeaf.TacticalReloadStage.Enter)
                        {
                            player.NotifyObserver(player, NotifyEvent.TacticalReloadMagazineFullStage);
                        }
                        break;
                    }
                case AimDownSightWeaponManuverNodeLeaf:
                    {
                        RotateObjectToward rotateObjectToward = new RotateObjectToward();
                        rotateObjectToward.RotateToward(Camera.main.transform.forward, player.gameObject, 6);
                        player.NotifyObserver(player, NotifyEvent.Aim);

                        break;
                    }
                case LowReadyWeaponManuverNodeLeaf:
                    {
                        player.NotifyObserver(player, NotifyEvent.LowReady);
                        break;
                    }
                case DropWeaponManuverNodeLeaf:
                case PickUpWeaponNodeLeaf:
                case HolsterPrimaryWeaponManuverNodeLeaf:
                case HolsterSecondaryWeaponManuverNodeLeaf:
                case DrawPrimaryWeaponManuverNodeLeaf:
                case DrawSecondaryWeaponManuverNodeLeaf:
                case PrimaryToSecondarySwitchWeaponManuverLeafNode:
                case SecondaryToPrimarySwitchWeaponManuverLeafNode:
                    {
                        player.NotifyObserver(player, NotifyEvent.SwitchWeapon);
                        break;
                    }
                case RestWeaponManuverLeafNode:
                    {
                        player.NotifyObserver(player, NotifyEvent.Resting);
                        break;
                    }
                case QuickDrawWeaponManuverLeafNode quickDrawNodeLeaf:
                    {
                        player.NotifyObserver(player, NotifyEvent.QuickDraw);
                        RotateObjectToward rotateObjectToward = new RotateObjectToward();
                        rotateObjectToward.RotateToward(Camera.main.transform.forward, player.gameObject, 6);
                        player.NotifyObserver(player, NotifyEvent.Aim);
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
