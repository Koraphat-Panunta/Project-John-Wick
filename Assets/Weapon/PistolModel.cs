using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon",menuName ="PistolModel")]
public class PistolModel : ScriptableObject
{
    public int _magazineCapacity /*= 15*/;
    public float _rateOfFire /*= 260*/;
    public float _reloadSpeed;
    public float _accuracy /*= 50*/;
    public GameObject _bulletType;
    public float _recoilController /*= 18.56f*/;
    public float _aimDownSightSpeed /*= 3f*/;
    public float _recoilKickBack /*= 20*/;
    public float min_percision /*= 20*/;
    public float max_percision /*= 65*/;
    
}
