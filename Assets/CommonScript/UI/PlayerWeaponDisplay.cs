using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponDisplay : PlayerInfoDisplay
{
    public Image WeaponIcon;
    public TextMeshProUGUI AmmoDisplay;
    private Weapon currentWeapon;
    public int MagazineCount;
    public int AmmoCount;
    public PlayerWeaponDisplay(Player player, HUD hud,TextMeshProUGUI textMeshProUGUI) : base(player, hud)
    {
        base.playerInfo = player;
        base.hud = hud;
        this.AmmoDisplay = textMeshProUGUI;
        if (base.playerInfo.currentWeapon != null)
        {
            MagazineCount = base.playerInfo.currentWeapon.bulletStore[BulletStackType.Magazine] + base.playerInfo.currentWeapon.bulletStore[BulletStackType.Chamber];
            AmmoCount = playerInfo.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.myType];
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }
    }
    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
       {
            if (base.playerInfo.currentWeapon != null)
            {
                MagazineCount = base.playerInfo.currentWeapon.bulletStore[BulletStackType.Magazine] + base.playerInfo.currentWeapon.bulletStore[BulletStackType.Chamber];
                SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
            }
       }
       if(playerAction == SubjectPlayer.PlayerAction.ReloadMagazineFullStage
            || playerAction == SubjectPlayer.PlayerAction.TacticalReloadMagazineFullStage
            || playerAction == SubjectPlayer.PlayerAction.InputMag_ReloadMagazineStage
            || playerAction == SubjectPlayer.PlayerAction.ChamberLoad_ReloadMagazineStage)
       {
            AmmoCount = player.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.myType];
            MagazineCount = base.playerInfo.currentWeapon.bulletStore[BulletStackType.Magazine] + base.playerInfo.currentWeapon.bulletStore[BulletStackType.Chamber];
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
       }
       if(playerAction == SubjectPlayer.PlayerAction.PickUpWeapon)
        {
            //
        }
        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            AmmoCount = player.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.myType];
            MagazineCount = base.playerInfo.currentWeapon.bulletStore[BulletStackType.Magazine] + base.playerInfo.currentWeapon.bulletStore[BulletStackType.Chamber];
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }
    } 
    private void SetAmmoDisplay(TextMeshProUGUI textGUI,float inLoad,float Ammoprouch)
    {
        textGUI.text = inLoad + " / " + Ammoprouch;
    }
   


    
}
