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
    private void OnEnable()
    {
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Fire, VisualRecoil);
    }


}
