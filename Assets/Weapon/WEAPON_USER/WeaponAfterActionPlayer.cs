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
        Debug.Log("Player AimDownSight Call Back");

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

            Debug.Log("enemy._posture = "+ enemy._posture);
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
        Debug.Log("Player LowReady Call Back");

        player.NotifyObserver(player, PlayerAction.LowReady);
    }
    public override void Reload(Weapon weapon, ReloadType reloadType)
    {
        switch (reloadType)
        {
            case ReloadType.MAGAZINE_RELOAD
                :player.NotifyObserver(player, SubjectPlayer.PlayerAction.ReloadMagazineFullStage);
                break;
            case ReloadType.MAGAZINE_TACTICAL_RELOAD
                :player.NotifyObserver(player, SubjectPlayer.PlayerAction.TacticalReloadMagazineFullStage);
                break;
           
        }
    }

    public override void Resting(Weapon weapon)
    {
        Debug.Log("Player Resting Call Back");
    }

    public override void SwitchingWeapon(Weapon weapon, WeaponTransition switchingManuver)
    {
        player.NotifyObserver(player, PlayerAction.SwitchWeapon);
    }
    public void QuickDraw(Weapon weapon,QuickDrawWeaponManuverLeafNode.QuickDrawPhase quickDrawPhase)
    {
        player.NotifyObserver(player, PlayerAction.QuickDraw);
        //switch (quickDrawPhase)
        //{
        //    case QuickDrawWeaponManuverLeafNode.QuickDrawPhase.Draw: 
        //        {
                   
        //        }
        //        break;
        //}
    }
}
