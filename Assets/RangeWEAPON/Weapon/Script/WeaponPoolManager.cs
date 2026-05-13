using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponPoolManager : MonoBehaviour, IInitializedAble
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private WeaponDataScriptableObject[] registeredWeaponTypes;
    [SerializeField] private int initialPoolSize = 2;
    [SerializeField] private int maxPoolSize = 10;

    private Dictionary<WeaponDataScriptableObject, ObjectPooling<Weapon>> _pools;
    private Dictionary<Weapon, WeaponDataScriptableObject> _weaponTypeMap;
    private Dictionary<Weapon, float> _clearWeaponList;

    private readonly int _weaponDisappearTime = 10;
    private readonly int _weaponDisappearDistance = 6;

    public void Initialized()
    {
        this._pools = new Dictionary<WeaponDataScriptableObject, ObjectPooling<Weapon>>();
        this._weaponTypeMap = new Dictionary<Weapon, WeaponDataScriptableObject>();
        this._clearWeaponList = new Dictionary<Weapon, float>();

        foreach (WeaponDataScriptableObject data in registeredWeaponTypes)
        {
            _pools[data] = new ObjectPooling<Weapon>(data._weaponPrefab, maxPoolSize, initialPoolSize, Vector3.zero);
        }
    }

    public Weapon SpawnWeapon(WeaponDataScriptableObject data, IGrabAbleObject grabAbleObject)
    {
        Weapon weapon = SpawnWeapon(data, Vector3.zero, Quaternion.identity);
        if (weapon == null) return null;
        WeaponAttachingBehavior.Attach(weapon, grabAbleObject, 0);
        return weapon;
    }

    public Weapon SpawnWeapon(WeaponDataScriptableObject data, Vector3 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(data, out ObjectPooling<Weapon> pool))
        {
            Debug.LogError("WeaponPoolManager: weapon type not registered — " + data.name);
            return null;
        }

        Weapon weapon = pool.Get(position, rotation);
        _weaponTypeMap[weapon] = data;
        _clearWeaponList[weapon] = 0;
        return weapon;
    }

    private float _checkTimer = 0f;
    private readonly float _checkInterval = 1f;

    private void LateUpdate()
    {
        _checkTimer += Time.deltaTime;
        if (_checkTimer < _checkInterval) return;
        ClearWeaponUpdate();
        _checkTimer = 0f;
    }

    private void ClearWeaponUpdate()
    {
        if (_clearWeaponList.Count == 0) return;

        List<Weapon> weapons = _clearWeaponList.Keys.ToList();
        foreach (Weapon weapon in weapons)
        {
            if (weapon._currentGrabbedAt == null)
            {
                _clearWeaponList[weapon] += _checkInterval;

                if (_clearWeaponList[weapon] <= _weaponDisappearTime) continue;
                if (IsObjectInCameraView(mainCamera, weapon.transform.position)) continue;
                if (Vector3.Distance(weapon.transform.position, mainCamera.transform.position) <= _weaponDisappearDistance) continue;

                ReturnToPool(weapon);
            }
            else
            {
                _clearWeaponList[weapon] = 0;
            }
        }
    }

    private void ReturnToPool(Weapon weapon)
    {
        if (_weaponTypeMap.TryGetValue(weapon, out WeaponDataScriptableObject data) && _pools.TryGetValue(data, out ObjectPooling<Weapon> pool))
            pool.ReturnToPool(weapon);

        _clearWeaponList.Remove(weapon);
        _weaponTypeMap.Remove(weapon);
    }

    private bool IsObjectInCameraView(Camera cam, Vector3 objectPos)
    {
        Vector3 viewPos = cam.WorldToViewportPoint(objectPos);
        return viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
    }

    private void OnValidate()
    {
        if (mainCamera == null)
            mainCamera = FindAnyObjectByType<Camera>();
    }
}
