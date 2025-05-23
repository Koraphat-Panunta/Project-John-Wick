using UnityEngine;

[CreateAssetMenu(fileName = "CameraAttribute", menuName = "ScriptableObjects/CameraScriptableObject")]
public class ScrpCameraViewAttribute : ScriptableObject
{
    public Vector3 StandView_Offset_Right;
    public float StandView_FOV;

    public Vector3 CrouchView_Offset_Right;
    public float CrouchView_FOV;

    public Vector3 AimDownSight_Offset_Right;
    public float AimDownSight_FOV;

    public Vector3 Sprint_Offset_Right;
    public float Sprint_FOV;

    public Vector3 GunFu_Offset_Right;
    public float GunFu_FOV;

    public Vector3 WeaponDisarmed_Offset_Right;
    public float WeaponDisarmed_FOV;

    public Vector3 GunFuExecute_Offset_Right;
    public float GunFuExecute_FOV;
}
