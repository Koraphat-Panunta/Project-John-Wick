using UnityEngine;

public class WeaponAttachingBehavior
{
    public void Attach(Weapon weapon,IWeaponAttachingAble weaponAttachingAble)
    {
        switch (weaponAttachingAble)
        {
            case MainHandSocket mainHandSocket:
                {

                    break;
                }
            case SecondHandSocket secondHandSocket: 
                {
                    
                    break; 
                }
            case PrimaryWeaponSocket primaryWeaponSocket: 
                { 

                    break; 
                }
            case SecondaryWeaponSocket secondaryWeaponSocket: 
                { 

                    break; 
                }
        }
    }
    
}
