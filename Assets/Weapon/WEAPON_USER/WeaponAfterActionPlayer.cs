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
    public override void AfterFiringSingleAction(Weapon weapon)
    {
    }

    public override void AimDownSight(Weapon weapon)
    {

        RotateObjectToward rotateObjectToward = new RotateObjectToward();
        rotateObjectToward.RotateToward(Camera.main.transform.forward, player.gameObject, 6);
        player.NotifyObserver(player, PlayerAction.Aim);
    }

    public override void Firing(Weapon weapon)
    {
        player.NotifyObserver(player, PlayerAction.Firing);

    }

    public override void HitDamageAble(IBulletDamageAble bulletDamageAble)
    {
        Enemy enemy;
        if(bulletDamageAble is BodyPart)
        {

            enemy = (bulletDamageAble as BodyPart).enemy;

            if(isKilleComfirm.ContainsKey(enemy) == false)
                isKilleComfirm.Add(enemy, false);

            if (isKilleComfirm[enemy])
                return;

            if (enemy.isDead)
            {
                player.NotifyObserver(player, PlayerAction.OpponentKilled);
                isKilleComfirm[enemy] = true;
                return;
            }

            if (enemy._posture <= 0)
            {
                player.NotifyObserver(player, PlayerAction.OppenentStagger);

            }
        }
    }

    public override void LowReady(Weapon weapon)
    {

        player.NotifyObserver(player, PlayerAction.LowReady);
    }
    public override void Reload(Weapon weapon, IReloadNodePhase reloadNodePhase)
    {
        if (reloadNodePhase is IReloadMagazineNodePhase reloadMagazineNodePhase)
        {
            switch (reloadMagazineNodePhase.curReloadPhase)
            {
                case IReloadMagazineNodePhase.ReloadMagazinePhase.Enter:
                    {
                        if (reloadNodePhase is ReloadMagazineFullStage)
                            player.NotifyObserver(player, PlayerAction.ReloadMagazineFullStage);

                        else if (reloadNodePhase is TacticalReloadMagazineFullStage)
                            player.NotifyObserver(player, PlayerAction.TacticalReloadMagazineFullStage);
                    }
                    break;

                case IReloadMagazineNodePhase.ReloadMagazinePhase.Exit:
                    {
                        if (reloadNodePhase is ReloadMagazineFullStage)
                            player.NotifyObserver(player, PlayerAction.ReloadMagazineFullStage);

                        else if (reloadNodePhase is TacticalReloadMagazineFullStage)
                            player.NotifyObserver(player, PlayerAction.TacticalReloadMagazineFullStage);
                    }
                    break;
            }
        }
    }

    public override void Resting(Weapon weapon)
    {
        
    }

    public override void SwitchingWeapon(Weapon weapon, IWeaponTransitionNodeLeaf weaponSwitchNodeLeaf)
    {
        player.NotifyObserver(player, PlayerAction.SwitchWeapon);
    }
    public void QuickDraw(Weapon weapon,QuickDrawWeaponManuverLeafNode.QuickDrawPhase quickDrawPhase)
    {
        player.NotifyObserver(player, PlayerAction.QuickDraw);
     
    }
}
