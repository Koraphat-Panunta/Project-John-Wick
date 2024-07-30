using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
    }

    void Update()
    {
        
    }
    private void VisualRecoil(Weapon weapon)
    {
        this.CinemachineFreeLook.m_XAxis.Value += weapon.Recoil;
        this.CinemachineFreeLook.m_YAxis.Value -= weapon.Recoil;
    }
    private void VisualRecoilUpdate()
    {
        
    }
    private void ADS(Weapon weapon)
    {
        if(weapon.weapon_StanceManager.Current_state == weapon.weapon_StanceManager.aimDownSight)
        {
            CinemachineFreeLook.m_Lens.FieldOfView = 65 - (weapon.weapon_StanceManager.AimingWeight * 25);
        }
        if (weapon.weapon_StanceManager.Current_state == weapon.weapon_StanceManager.lowReady)
        {
            CinemachineFreeLook.m_Lens.FieldOfView = 65 - weapon.weapon_StanceManager.AimingWeight * 25;  
        }
    }
    private void OnEnable()
    {
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Fire, VisualRecoil);
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Aim, ADS);
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.LowReady, ADS);

    }
    private void OnDisable()
    {
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.Fire, VisualRecoil);
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.Aim, ADS);
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.LowReady, ADS);
    }



}
