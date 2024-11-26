using UnityEngine;

public abstract class WeaponTreeManager 
{
   private WeaponBlackBoard WeaponBlackBoard;
   public WeaponTreeManager(WeaponBlackBoard weaponBlackBoard)
   {
        this.WeaponBlackBoard = weaponBlackBoard;
   }
}
