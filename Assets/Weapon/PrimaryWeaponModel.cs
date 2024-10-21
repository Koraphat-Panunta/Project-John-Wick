using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "PrimaryWeaponModel")]
public class PrimaryWeaponModel : ScriptableObject
{
    public int _magazineCapacity = 15;
    public float _rateOfFire = 360;
    public float _reloadSpeed = 2;
    public float _accuracy = 136;
    public GameObject _bulletType;
    public float _recoilController = 20;
    public float _aimDownSightSpeed = 3f;
    public float _recoilKickBack = 50;
    public float min_percision = 11.31f;
    public float max_percision = 65;
}
