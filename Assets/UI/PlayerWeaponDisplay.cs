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
            MagazineCount = base.playerInfo.currentWeapon.Magazine_count + base.playerInfo.currentWeapon.Chamber_Count;
            AmmoCount = playerInfo.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.GetComponent<Bullet>().type];
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }
    }
    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
       {
            if (base.playerInfo.currentWeapon != null)
            {
                MagazineCount = base.playerInfo.currentWeapon.Magazine_count + base.playerInfo.currentWeapon.Chamber_Count;
                SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
            }
       }
       if(playerAction == SubjectPlayer.PlayerAction.Reloading)
       {
            AmmoCount = player.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.GetComponent<Bullet>().type];
            MagazineCount = base.playerInfo.currentWeapon.Magazine_count + base.playerInfo.currentWeapon.Chamber_Count;
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
       }
       if(playerAction == SubjectPlayer.PlayerAction.PickUpWeapon)
        {
            //
        }
        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            AmmoCount = player.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.GetComponent<Bullet>().type];
            MagazineCount = base.playerInfo.currentWeapon.Magazine_count + base.playerInfo.currentWeapon.Chamber_Count;
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }
    } 
    private void SetAmmoDisplay(TextMeshProUGUI textGUI,float inLoad,float Ammoprouch)
    {
        textGUI.text = inLoad + " / " + Ammoprouch;
    }
   


    
}
