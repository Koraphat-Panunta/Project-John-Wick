
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponDisplay : GameplayUI, IObserverPlayer
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
 
    public  void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {


       if(playerAction == SubjectPlayer.NotifyEvent.Firing)
       {
            if (this.playerInfo._currentWeapon != null)
            {
                UpdateInfo();
            }
       }
       if(playerAction == SubjectPlayer.NotifyEvent.ReloadMagazineFullStage
            || playerAction == SubjectPlayer.NotifyEvent.TacticalReloadMagazineFullStage
            || playerAction == SubjectPlayer.NotifyEvent.InputMag_ReloadMagazineStage
            || playerAction == SubjectPlayer.NotifyEvent.ChamberLoad_ReloadMagazineStage)
       {
            UpdateInfo();
       }
       if(playerAction == SubjectPlayer.NotifyEvent.PickUpWeapon)
        {
            UpdateInfo();
        }
        if(playerAction == SubjectPlayer.NotifyEvent.SwitchWeapon)
        {
            UpdateInfo();
          
        }

        if (playerAction == SubjectPlayer.NotifyEvent.QuickDraw)
        {
           UpdateInfo();
        }

        if (playerAction == SubjectPlayer.NotifyEvent.RecivedAmmo)
        {
            UpdateInfo();
        }

        if(player.playerStateNodeManager != null &&
            player.playerStateNodeManager.curNodeLeaf is WeaponDisarm_GunFuInteraction_NodeLeaf)
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

        AmmoCount = playerInfo._weaponBelt.ammoProuch.amountOf_ammo[playerInfo._currentWeapon.bullet.myType];
        MagazineCount = this.playerInfo._currentWeapon.bulletStore[BulletStackType.Magazine] + this.playerInfo._currentWeapon.bulletStore[BulletStackType.Chamber];
        SetAmmoDisplay(AmmoDisplay, MagazineCount, AmmoCount);
    }

    public void OnNotify(Player player)
    {
        
    }

    public override void EnableUI()=>this.AmmoDisplay.enabled = true;
  

    public override void DisableUI()=>this.AmmoDisplay.enabled=false;
    
       
    
}
