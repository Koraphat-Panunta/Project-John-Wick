using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponObjectManager : MonoBehaviour , IInitializedAble
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Weapon weaponPrefab;
    protected ObjectPooling<Weapon> weaponObjPooling;
    public Dictionary<Weapon, float> clearWeaponList { get; protected set; }

    protected readonly int weaponDisapearTime = 10;
    protected readonly int weaponDisapearDistance = 6;
    public void Initialized()
    {
        weaponObjPooling = new ObjectPooling<Weapon>(this.weaponPrefab, 10, 2, Vector3.zero);
        clearWeaponList = new Dictionary<Weapon, float>();
    }
    public void SpawnWeapon(GameObject weaponAdvanceUser)
    {
        if (weaponAdvanceUser.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser weaponUser))
        {
            this.SpawnWeapon(weaponUser);
        }
        else
            throw new System.Exception("the parameter is not weaponAdvanceUser");
       
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
        clearWeaponList.Add(weapon, 0);
        return weapon;
    }
    private bool IsObjectInCameraView(Camera camera, Vector3 objectPos)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(objectPos);
        bool isInView = viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;

        return isInView;
    }

    float checkTimer = 0f;
    float checkInterval = 1f;
    private void LateUpdate()
    {
        checkTimer += Time.deltaTime;

        if (checkTimer < checkInterval)
            return;

        this.ClearWeaponUpdate();
        checkTimer = 0f;
    }
    
    private void ClearWeaponUpdate()
    {
        if (clearWeaponList.Count > 0)
        {
            List<Weapon> weapons = clearWeaponList.Keys.ToList();
            foreach (Weapon weapon in weapons)
            {
                if (weapon.userWeapon == null)
                {
                    clearWeaponList[weapon] += checkTimer;

                    if (clearWeaponList[weapon] <= weaponDisapearTime)
                        continue;
                    
                    if(IsObjectInCameraView(this.mainCamera,weapon.transform.position))
                        continue;
                    
                    if(Vector3.Distance(weapon.transform.position,this.mainCamera.transform.position) > weaponDisapearDistance == false)
                        continue;

                    clearWeaponList.Remove(weapon);
                    weaponObjPooling.ReturnToPool(weapon);
                }
                else
                    clearWeaponList[weapon] = 0;
            }
        }
    }

    private void OnValidate()
    {
        if(this.mainCamera == null)
            this.mainCamera = FindAnyObjectByType<Camera>();
    }
}
