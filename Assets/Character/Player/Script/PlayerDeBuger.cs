using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeBuger : MonoBehaviour,IInitializedAble
{
    public static List<Vector3> sphereCastPos = new List<Vector3>();
    public static Vector3 dirCast;
    public static float sphereRaduis;

    LayerMask layerMask ;

    public static Vector3 CoverPos;
    public static Vector3 AimPos;

    public bool isSwitchWeaponCommand;
    public bool isPullTriggerCommand;
    public bool isAimingCommand;
    public bool isReloadCommand;

    public bool isAimingManuver;
    public bool isPullTriggerManuver;
    public bool isReloadManuver;
    public bool isSwitchWeaponManuver;

    Player player;

    [SerializeField] private string PlayerCurNodeLeaf;
    [SerializeField] private Weapon curWeapon;
    [SerializeField] private Weapon myPrimaryWeapon;
    [SerializeField] private Weapon mySecondaryWeapon;
    // Start is called before the first frame update

    [SerializeField] private GunFuHitScriptableObject gunFuHitScriptableObject;
    public void Initialized()
    {
        player = GetComponent<Player>();
        layerMask = LayerMask.GetMask("Default");

    }
    

    // UpdateNode is called once per frame
    void Update()
    {

        isAimingCommand = player._isAimingCommand;
        isReloadCommand = player._isReloadCommand;
        isPullTriggerCommand = player._isPullTriggerCommand;
        isReloadCommand = player._isReloadCommand;

        isAimingManuver = player._weaponManuverManager.isAimingManuverAble;
        isPullTriggerManuver = player._weaponManuverManager.isPullTriggerManuverAble;
        isReloadManuver = player._weaponManuverManager.isReloadManuverAble;
        isSwitchWeaponManuver = player._weaponManuverManager.isSwitchWeaponManuverAble;

        PlayerCurNodeLeaf = (player.playerStateNodeManager as INodeManager).GetCurNodeLeaf().ToString();

        curWeapon = player._currentWeapon;
        myPrimaryWeapon = player._weaponBelt.myPrimaryWeapon as Weapon;
        mySecondaryWeapon = player._weaponBelt.mySecondaryWeapon as Weapon;
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red*0.4f;
        Vector3 shperePos = transform.position 
            + (transform.forward * gunFuHitScriptableObject.attackVolumeForward)
            + (transform.up * gunFuHitScriptableObject.attackVolumeUpward)
            + (transform.right * gunFuHitScriptableObject.attackVolumeRightward);
        Gizmos.DrawSphere(shperePos, gunFuHitScriptableObject.attackVolumeRaduis);
    }

   
}
