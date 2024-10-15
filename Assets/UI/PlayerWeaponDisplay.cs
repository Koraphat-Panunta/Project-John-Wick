using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponDisplay : PlayerInfoDisplay
{
    public Image WeaponIcon;
    public TextMeshProUGUI AmmoDisplay;
    public int MagazineCount;
    public int AmmoCount;
    public PlayerWeaponDisplay(Player player, HUD hud,TextMeshProUGUI textMeshProUGUI) : base(player, hud)
    {
        base.playerInfo = player;
        base.hud = hud;
        this.AmmoDisplay = textMeshProUGUI;
        if (base.playerInfo.curentWeapon != null)
        {
            MagazineCount = base.playerInfo.curentWeapon.Magazine_count + base.playerInfo.curentWeapon.Chamber_Count;
            AmmoCount = playerInfo.playerWeaponCommand.ammoProuch.amountOf_ammo[playerInfo.curentWeapon.bullet.GetComponent<Bullet>().type];
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }
    }
    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
       {
            if (base.playerInfo.curentWeapon != null)
            {
                MagazineCount = base.playerInfo.curentWeapon.Magazine_count + base.playerInfo.curentWeapon.Chamber_Count;
                SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
            }
       }
       if(playerAction == SubjectPlayer.PlayerAction.Reloading)
       {
            AmmoCount = player.playerWeaponCommand.ammoProuch.amountOf_ammo[playerInfo.curentWeapon.bullet.GetComponent<Bullet>().type];
            MagazineCount = base.playerInfo.curentWeapon.Magazine_count + base.playerInfo.curentWeapon.Chamber_Count;
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
       }
       if(playerAction == SubjectPlayer.PlayerAction.PickUpWeapon)
        {
            //
        }
    } 
    private void SetAmmoDisplay(TextMeshProUGUI textGUI,float inLoad,float Ammoprouch)
    {
        textGUI.text = inLoad + " / " + Ammoprouch;
    }
   


    
}
