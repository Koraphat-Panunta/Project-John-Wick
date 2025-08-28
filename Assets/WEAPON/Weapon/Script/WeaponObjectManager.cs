using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponObjectManager 
{
    private Camera mainCamera;
    private Weapon weaponPrefab;
    protected ObjectPooling<Weapon> weaponObjPooling;
    public Dictionary<Weapon, float> clearWeaponList { get; protected set; }

    protected readonly int weaponDisapearTime = 10;
    protected readonly int weaponDisapearDistance = 20;

    public WeaponObjectManager(Weapon weapon, Camera mainCamera)
    {
        this.weaponPrefab = weapon;
        this.mainCamera = mainCamera;
        weaponObjPooling = new ObjectPooling<Weapon>(this.weaponPrefab, 10, 4, Vector3.zero);
        clearWeaponList = new Dictionary<Weapon, float>();
    }

    public Weapon SpawnWeapon(IWeaponAdvanceUser weaponAdvanceUser)
    {
        Weapon weapon = this.SpawnWeapon(Vector3.zero, Quaternion.identity);

        WeaponAttachingBehavior.Attach(weapon, weaponAdvanceUser._mainHandSocket);

        return weapon;
    }
    public Weapon SpawnWeapon(Vector3 position, Quaternion rotation)
    {
        Weapon weapon = this.weaponObjPooling.Get();
        weapon.transform.position = position;
        weapon.transform.rotation = rotation;
        AssignWeaponCleaner(weapon);
        return weapon;
    }
    public void ReturnWeapon(Weapon weapon)
    {
        weaponObjPooling.ReturnToPool(weapon);
    }
    public void AssignWeaponCleaner(Weapon weapon)
    {
        clearWeaponList.Add(weapon, 0);
        if (clearWeaponTask == null)
        {
            clearWeaponTask = ClearWeaponUpdate();
        }
    }
    private bool IsObjectInCameraView(Camera camera, Vector3 objectPos)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(objectPos);
        bool isInView = viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;

        return isInView;
    }
    private Task clearWeaponTask;
    private async Task ClearWeaponUpdate()
    {
        while (clearWeaponList.Count > 0)
        {
            List<Weapon> weapons = clearWeaponList.Keys.ToList();
            foreach (Weapon weapon in weapons)
            {
                if (weapon.userWeapon == null)
                {
                    clearWeaponList[weapon] += Time.deltaTime;

                    if (clearWeaponList[weapon] <= weaponDisapearTime)
                        continue;
                    
                    if(IsObjectInCameraView(this.mainCamera,weapon.transform.position))
                        continue;
                    
                    if(Vector3.Distance(weapon.transform.position,this.mainCamera.transform.position) > weaponDisapearDistance == false)
                        continue;

                    clearWeaponList.Remove(weapon);
                    ReturnWeapon(weapon);
                }
                else
                    clearWeaponList[weapon] = 0;

            }
            await Task.Yield();
        }
        clearWeaponTask = null;

    }
}
