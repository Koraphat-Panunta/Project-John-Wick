
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponDisplay : MonoBehaviour,IObserverPlayer
{
    private Image WeaponIcon;
    [SerializeField] private TextMeshProUGUI AmmoDisplay;
    [SerializeField] private Player playerInfo;
    private Weapon currentWeapon => playerInfo._currentWeapon;
    public int MagazineCount;
    public int AmmoCount;
    private void Awake()
    {
        this.playerInfo.AddObserver(this);

        if (this.playerInfo._currentWeapon != null)
            UpdateInfo();
        
    }
    private void OnValidate()
    {
        this.playerInfo = FindAnyObjectByType<Player>();
    }
 
    public  void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
       {
            if (this.playerInfo._currentWeapon != null)
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

    public void UpdateInfo()
    {
        if (currentWeapon == null)
        {
            AmmoDisplay.text = "- / -";
            return;
        }

        AmmoCount = playerInfo.weaponBelt.ammoProuch.amountOf_ammo[playerInfo._currentWeapon.bullet.myType];
        MagazineCount = this.playerInfo._currentWeapon.bulletStore[BulletStackType.Magazine] + this.playerInfo._currentWeapon.bulletStore[BulletStackType.Chamber];
        SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
    }

    public void OnNotify(Player player)
    {
        
    }
}
