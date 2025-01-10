using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    //[SerializeField] private PlayerController playerController;
    //[SerializeField] private PlayerStateManager playerStateManager;
    //[SerializeField] private WeaponSocket WeaponSocket;
    //public Weapon CurrentWeapon { get; private set; }
    //public SecondaryWeapon secondaryWeapon { get; private set; }
    //public PrimaryWeapon primaryWeapon { get; private set; }
    //private void Start()
    //{
       
       
    //}
    //private void Update()
    //{
    //    if(CurrentWeapon == null)
    //    {
    //        CurrentWeapon = WeaponSocket.CurWeapon;
    //    }
    //}
    //private void FixedUpdate()
    //{
        
    //}
    //public void Fire(InputAction.CallbackContext Value)
    //{
    //    if (CurrentWeapon != null && Value.phase == InputActionPhase.Started)
    //    {
    //        CurrentWeapon.weapon_stateManager.ChangeState(CurrentWeapon.weapon_stateManager.fireState);
    //    }
    //}
    //public void Aim(InputAction.CallbackContext Value)
    //{
    //    if (CurrentWeapon != null)
    //    {
    //        if (Value.phase.IsInProgress())
    //        {
    //            Debug.Log("AimInput");
    //            CurrentWeapon.weapon_StanceManager.ChangeStance(CurrentWeapon.weapon_StanceManager.aimDownSight);
    //        }
    //        else
    //        {
    //            CurrentWeapon.weapon_StanceManager.ChangeStance(CurrentWeapon.weapon_StanceManager.lowReady);
    //        }
    //    }
    //}


}
