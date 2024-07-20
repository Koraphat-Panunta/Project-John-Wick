using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerStateManager playerStateManager;
    public WeaponSocket WeaponSocket;
    public Weapon CurrentWeapon { get; private set; }
    public SecondaryWeapon secondaryWeapon { get; private set; }
    public PrimaryWeapon primaryWeapon { get; private set; }
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerStateManager = GetComponent<PlayerStateManager>();
       
    }
    private void Update()
    {
        if(CurrentWeapon == null)
        {
            CurrentWeapon = WeaponSocket.CurWeapon;
        }
    }
    private void FixedUpdate()
    {
        
    }
    public void Fire()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.weapon_stateManager.ChangeState(CurrentWeapon.weapon_stateManager.fireState);
        }
    }
    public void Aim(InputAction.CallbackContext Value)
    {
        if (Value.phase.IsInProgress())
        {
            Debug.Log("AimInput");
            CurrentWeapon.weapon_stateManager.ChangeState(CurrentWeapon.weapon_stateManager.stanceManager.aimDownSight);
        }
        else
        {
            CurrentWeapon.weapon_stateManager.ChangeState(CurrentWeapon.weapon_stateManager.stanceManager.lowReady);
        }
    }


}
