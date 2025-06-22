
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
        if (playerAction == SubjectPlayer.NotifyEvent.RecivedAmmo)
        {
            UpdateInfo();
        }

        if(player.playerStateNodeManager != null &&
            (player.playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<WeaponDisarm_GunFuInteraction_NodeLeaf>())
            UpdateInfo();
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if(node is WeaponManuverLeafNode weaponManuverLeafNode)
        {
            switch (weaponManuverLeafNode)
            {
                case ReloadMagazineFullStageNodeLeaf:
                case TacticalReloadMagazineFullStageNodeLeaf:
                case PickUpWeaponNodeLeaf:
                case DropWeaponManuverNodeLeaf:
                case DrawPrimaryWeaponManuverNodeLeaf:
                case DrawSecondaryWeaponManuverNodeLeaf:
                case HolsterPrimaryWeaponManuverNodeLeaf:
                case HolsterSecondaryWeaponManuverNodeLeaf:
                case PrimaryToSecondarySwitchWeaponManuverLeafNode:
                case SecondaryToPrimarySwitchWeaponManuverLeafNode:
                case QuickDrawWeaponManuverLeafNodeLeaf:
                    {
                        UpdateInfo();
                        break;
                    }
            }
        }
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
    public override void EnableUI()=>this.AmmoDisplay.enabled = true;
  

    public override void DisableUI()=>this.AmmoDisplay.enabled=false;

   
}
