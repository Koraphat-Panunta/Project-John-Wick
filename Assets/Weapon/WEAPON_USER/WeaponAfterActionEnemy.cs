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
                        //No logic yet
                        break;
                    }
                case ReloadMagazineFullStageNodeLeaf:
                    {
                        enemy.NotifyObserver(enemy,SubjectEnemy.EnemyEvent.ReloadMagazineFullStage);
                        break;
                    }
                case TacticalReloadMagazineFullStageNodeLeaf:
                    {
                        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.TacticalReloadMagazineFullStage);
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
                case DropWeaponManuverNodeLeaf:
                case PickUpWeaponNodeLeaf:
                case HolsterPrimaryWeaponManuverNodeLeaf:
                case HolsterSecondaryWeaponManuverNodeLeaf:
                case DrawPrimaryWeaponManuverNodeLeaf:
                case DrawSecondaryWeaponManuverNodeLeaf:
                case PrimaryToSecondarySwitchWeaponManuverLeafNode:
                case SecondaryToPrimarySwitchWeaponManuverLeafNode:
                    {
                        enemy.NotifyObserver(enemy, SubjectEnemy.EnemyEvent.SwitchWeapon);
                        break;
                    }
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
        if (enemy.weaponBelt.ammoProuch.amountOf_ammo[enemy._currentWeapon.bullet.myType] <= 0)
        {
            enemy.weaponBelt.ammoProuch.AddAmmo(enemy._currentWeapon.bullet.myType, 100);
        }
    }
}
