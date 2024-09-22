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
    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
       {
            if (base.playerInfo.playerWeaponCommand.CurrentWeapon != null)
            {
                MagazineCount = base.playerInfo.playerWeaponCommand.CurrentWeapon.Magazine_count + base.playerInfo.playerWeaponCommand.CurrentWeapon.Chamber_Count;
                SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
            }
       }
       if(playerAction == SubjectPlayer.PlayerAction.Reloading)
        {
            AmmoCount = player.playerWeaponCommand.ammoProuch.amountOf_ammo[playerInfo.playerWeaponCommand.CurrentWeapon.bullet.GetComponent<Bullet>().type];
            MagazineCount = base.playerInfo.playerWeaponCommand.CurrentWeapon.Magazine_count + base.playerInfo.playerWeaponCommand.CurrentWeapon.Chamber_Count;
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }
    }

    protected override void Start()
    {
        base.Start();
        if(base.playerInfo.playerWeaponCommand.CurrentWeapon != null)
        {
            MagazineCount = base.playerInfo.playerWeaponCommand.CurrentWeapon.Magazine_count + base.playerInfo.playerWeaponCommand.CurrentWeapon.Chamber_Count;
            AmmoCount =  playerInfo.playerWeaponCommand.ammoProuch.amountOf_ammo[playerInfo.playerWeaponCommand.CurrentWeapon.bullet.GetComponent<Bullet>().type];
            SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
        }

    }
    private void SetAmmoDisplay(TextMeshProUGUI textGUI,float inLoad,float Ammoprouch)
    {
        textGUI.text = inLoad + " / " + Ammoprouch;
    }

    
}
