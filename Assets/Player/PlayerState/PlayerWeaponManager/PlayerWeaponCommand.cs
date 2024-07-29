using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponCommand : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponSocket WeaponSocket;
    public Weapon CurrentWeapon { get; private set; }
    public SecondaryWeapon secondaryWeapon { get; private set; }
    public PrimaryWeapon primaryWeapon { get; private set; }
    private void Start()
    {
        InvokeRepeating("GetWeapon", 0.1f, 1f);
    }
    private void Update()
    {
        //if(CurrentWeapon == null)
        //{
        //    CurrentWeapon = WeaponSocket.CurWeapon;
        //}
    }
    public void Pulltriger(PlayerStateManager playerstate)
    {
        if(CurrentWeapon.weapon_StanceManager.Current_state == CurrentWeapon.weapon_StanceManager.aimDownSight
            &&playerstate.Current_state != playerstate.sprint)
        {
            CurrentWeapon.weapon_stateManager.ChangeState(CurrentWeapon.weapon_stateManager.fireState);
            Debug.Log("PullTriger");
        }
    }
    public void Aim(PlayerStateManager playerstate)
    {
        if(playerstate.Current_state != playerstate.sprint)
        {
            CurrentWeapon.weapon_StanceManager.ChangeState(CurrentWeapon.weapon_StanceManager.aimDownSight);
        }
        else
        {
            CurrentWeapon.weapon_StanceManager.ChangeState(CurrentWeapon.weapon_StanceManager.lowReady);
        }
    }
    public void Reload(State playerstate)
    {

    }
    public void LowWeapon(State playerstate)
    {
        CurrentWeapon.weapon_StanceManager.ChangeState(CurrentWeapon.weapon_StanceManager.lowReady);
    }
    private void GetWeapon()
    {
        CurrentWeapon = WeaponSocket.CurWeapon;
       if(CurrentWeapon == null)
        {
            CancelInvoke("GetWeapon");
        }
    }


}
