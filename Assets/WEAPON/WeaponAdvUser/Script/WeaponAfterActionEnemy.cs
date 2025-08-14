using UnityEngine;
using static SubjectPlayer;

public class WeaponAfterActionEnemy : WeaponAfterAction
{
    private Enemy enemy;
    public WeaponAfterActionEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void SendFeedBackWeaponAfterAction<T>(WeaponAfterActionSending weaponAfterActionSending, T Var)
    {
        if (weaponAfterActionSending == WeaponAfterActionSending.WeaponStateNodeActive)
            switch (Var)
            {
                case FiringNode firingNode:
                    {
                        enemy.NotifyObserver(enemy, firingNode);
                        break;
                    }
                case ReloadMagazineFullStageNodeLeaf reloadMagazineFullStageNodeLeaf:
                    {
                        enemy.NotifyObserver(enemy,reloadMagazineFullStageNodeLeaf);
                        break;
                    }
                case TacticalReloadMagazineFullStageNodeLeaf tacticalReloadMagazineFullStageNodeLeaf:
                    {
                        enemy.NotifyObserver(enemy,tacticalReloadMagazineFullStageNodeLeaf);
                        break;
                    }
                case AimDownSightWeaponManuverNodeLeaf:
                    {
                        //No logic yet
                        break;
                    }
                case LowReadyWeaponManuverNodeLeaf:
                    {
                        //No logic yet
                        break;
                    }
                case DropWeaponManuverNodeLeaf dropWeaponManuverNodeLeaf:
                    {
                        enemy.NotifyObserver(enemy, dropWeaponManuverNodeLeaf);
                        break;
                    }
                case PickUpWeaponNodeLeaf pickUpWeaponNodeLeaf: enemy.NotifyObserver(enemy, pickUpWeaponNodeLeaf);
                    break;
                case HolsterPrimaryWeaponManuverNodeLeaf holsterPrimaryWeaponManuverNodeLeaf: enemy.NotifyObserver(enemy, holsterPrimaryWeaponManuverNodeLeaf);
                    break;
                case HolsterSecondaryWeaponManuverNodeLeaf holsterSecondaryWeaponManuverNodeLeaf: enemy.NotifyObserver(enemy, holsterSecondaryWeaponManuverNodeLeaf);
                    break;
                case DrawPrimaryWeaponManuverNodeLeaf drawPrimaryWeaponManuverNodeLeaf: enemy.NotifyObserver(enemy, drawPrimaryWeaponManuverNodeLeaf);
                    break;
                case DrawSecondaryWeaponManuverNodeLeaf drawSecondaryWeaponManuverNodeLeaf: enemy.NotifyObserver(enemy, drawSecondaryWeaponManuverNodeLeaf);
                    break;
                case PrimaryToSecondarySwitchWeaponManuverLeafNode primaryToSecondarySwitchWeaponManuverLeafNode: enemy.NotifyObserver(enemy, primaryToSecondarySwitchWeaponManuverLeafNode);
                    break;
                case SecondaryToPrimarySwitchWeaponManuverLeafNode secondaryToPrimarySwitchWeaponManuverLeafNode: enemy.NotifyObserver(enemy, secondaryToPrimarySwitchWeaponManuverLeafNode);
                    break;
                  
                case RestWeaponManuverLeafNode:
                    {
                        //No logic yet
                        break;
                    }
            }
        
        else
            this.NoneWeaponStateEvent<T>(weaponAfterActionSending, Var);
    }
    private void NoneWeaponStateEvent<T>(WeaponAfterActionSending weaponAfterActionSending, T Var)
    {

    }
    private void AutoRegenAmmo()
    {
        if (enemy._weaponBelt.ammoProuch.amountOf_ammo[enemy._currentWeapon.bullet.myType] <= 0)
        {
            enemy._weaponBelt.ammoProuch.AddAmmo(enemy._currentWeapon.bullet.myType, 100);
        }
    }
}
