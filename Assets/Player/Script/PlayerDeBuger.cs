using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeBuger : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        layerMask = LayerMask.GetMask("Default");
    }

    // UpdateNode is called once per frame
    void Update()
    {

        isAimingCommand = player.isAimingCommand;
        isReloadCommand = player.isReloadCommand;
        isPullTriggerCommand = player.isPullTriggerCommand;
        isReloadCommand = player.isReloadCommand;

        isAimingManuver = player.weaponManuverManager.isAimingManuver;
        isPullTriggerManuver = player.weaponManuverManager.isPullTriggerManuver;
        isReloadManuver = player.weaponManuverManager.isReloadManuver;
        isSwitchWeaponManuver = player.weaponManuverManager.isSwitchWeaponManuver;

        
    }
   
    private void OnDrawGizmos()
    {
        if (sphereCastPos.Count > 0) 
        {
            foreach (Vector3 p in sphereCastPos)
            {
               
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(p,sphereRaduis);  
            } 
        }
        if(CoverPos != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(CoverPos, sphereRaduis*1.2f);
        }
        if(AimPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(AimPos, sphereRaduis * 1.2f);
        }
    }
}
