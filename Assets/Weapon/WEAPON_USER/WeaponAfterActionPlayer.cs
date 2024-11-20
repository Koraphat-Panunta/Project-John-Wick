using UnityEngine;
using static SubjectPlayer;

public class WeaponAfterActionPlayer : WeaponAfterAction
{
    private Player player;
    public WeaponAfterActionPlayer(Player player)
    {
        this.player = player;
    }
    public override void AfterFiringSingleAction(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void AimDownSight(Weapon weapon)
    {
        RotateObjectToward rotateObjectToward = new RotateObjectToward();
        rotateObjectToward.RotateTowards(Camera.main.transform.forward, player.gameObject, 6);
        player.NotifyObserver(player, PlayerAction.Aim);
        throw new System.NotImplementedException();
    }

    public override void Firing(Weapon weapon)
    {
        player.NotifyObserver(player, PlayerAction.Firing);
        throw new System.NotImplementedException();

    }

    public override void LowReady(Weapon weapon)
    {
        player.NotifyObserver(player, PlayerAction.LowReady);
        throw new System.NotImplementedException();
    }

    public override void PreLoad(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void Reload(Weapon weapon, ReloadType reloadType)
    {
        switch (reloadType)
        {
            case ReloadType.MAGAZINE_RELOAD
                :player.NotifyObserver(player, SubjectPlayer.PlayerAction.Reloading);
                break;
            case ReloadType.MAGAZINE_TACTICAL_RELOAD
                :player.NotifyObserver(player, SubjectPlayer.PlayerAction.Reloading);
                break;
            case ReloadType.MAGAZINE_RELOAD_SUCCESS
                :new AmmoProchReload(player.weaponBelt.ammoProuch).Performed(weapon);
                break;
        }
    }

    public override void ReloadingMagazine(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void Reload_ChamberAction(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void Reload_SingleAction(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void Tactical_ReloadMagazine(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }
}
