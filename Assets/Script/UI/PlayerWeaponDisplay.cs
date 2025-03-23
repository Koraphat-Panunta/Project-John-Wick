using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponDisplay : PlayerInfoDisplay
{
    public Image WeaponIcon;
    public TextMeshProUGUI AmmoDisplay;
    private Weapon currentWeapon => playerInfo.currentWeapon;
    public int MagazineCount;
    public int AmmoCount;
    public PlayerWeaponDisplay(Player player, HUD hud,TextMeshProUGUI textMeshProUGUI) : base(player, hud)
    {
        base.playerInfo = player;
        base.hud = hud;
        this.AmmoDisplay = textMeshProUGUI;
        if (base.playerInfo.currentWeapon != null)
        {
            UpdateInfo();
        }
    }
    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
       {
            if (base.playerInfo.currentWeapon != null)
            {
                UpdateInfo();
            }
       }
       if(playerAction == SubjectPlayer.PlayerAction.ReloadMagazineFullStage
            || playerAction == SubjectPlayer.PlayerAction.TacticalReloadMagazineFullStage
            || playerAction == SubjectPlayer.PlayerAction.InputMag_ReloadMagazineStage
            || playerAction == SubjectPlayer.PlayerAction.ChamberLoad_ReloadMagazineStage)
       {
            UpdateInfo();
       }
       if(playerAction == SubjectPlayer.PlayerAction.PickUpWeapon)
        {
            UpdateInfo();
        }
        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            PlayerWeaponManuver playerWeaponManuver = player.weaponManuverManager as PlayerWeaponManuver;
            if (playerWeaponManuver.curNodeLeaf is PrimaryToSecondarySwitchWeaponManuverLeafNode PTS)
            {
                if (PTS.curPhase == PrimaryToSecondarySwitchWeaponManuverLeafNode.TransitionPhase.Switch)
                {
                    UpdateInfo();
                }
            }
            else if (playerWeaponManuver.curNodeLeaf is SecondaryToPrimarySwitchWeaponManuverLeafNode STP)
            {
                if (STP.curPhase == SecondaryToPrimarySwitchWeaponManuverLeafNode.TransitionPhase.Switch)
                {
                    UpdateInfo();
                }
            }
        }

        if (playerAction == SubjectPlayer.PlayerAction.QuickDraw)
        {
           UpdateInfo();
        }

        if (playerAction == SubjectPlayer.PlayerAction.RecivedAmmo)
        {
            UpdateInfo();
        }

        if(player.playerStateNodeManager.curNodeLeaf is WeaponDisarm_GunFuInteraction_NodeLeaf)
            UpdateInfo();
    } 
    private void SetAmmoDisplay(TextMeshProUGUI textGUI,float inLoad,float Ammoprouch)
    {
        textGUI.text = inLoad + " / " + Ammoprouch;
    }

    public override void UpdateInfo()
    {
        if(currentWeapon == null)
            return;

        AmmoCount = playerInfo.weaponBelt.ammoProuch.amountOf_ammo[playerInfo.currentWeapon.bullet.myType];
        MagazineCount = base.playerInfo.currentWeapon.bulletStore[BulletStackType.Magazine] + base.playerInfo.currentWeapon.bulletStore[BulletStackType.Chamber];
        SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
    }
}
