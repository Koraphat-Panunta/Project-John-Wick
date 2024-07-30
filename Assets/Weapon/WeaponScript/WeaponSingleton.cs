using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSingleton : MonoBehaviour
{
    public Animator animator { get; private set; }
    public GameObject UserWeapon { get; private set; }
    public WeaponSocket weaponSocket { get; private set; }
    public State CurStance { get; private set; }
    public State CurState { get; private set; }
    public Camera Camera { get; private set; }
    [SerializeField] private CrosshairController CrosshairController;
    [SerializeField] private Weapon MyWeapon;
    [SerializeField] private WeaponStateManager stateManager;
    [SerializeField] private WeaponStanceManager stanceManager;
    public Action<Weapon> FireEvent;
    public Action<Weapon> Aim;
    public Action<Weapon> Reload;
    public Action<Weapon> LowReady;
    private void Start()
    {
        weaponSocket = GetComponentInParent<WeaponSocket>();
        UserWeapon = weaponSocket.GetWeaponUser();
        animator = weaponSocket.GetAnimator();
        Camera = weaponSocket.GetCamera();
    }
    private void Update()
    {
        CurStance = stanceManager.Current_state;
        CurState = stateManager.Current_state;
    }
    public WeaponStanceManager GetStanceManager()
    {
        return stanceManager;
    }
    public WeaponStateManager GetStateManager() 
    { 
        return stateManager; 
    }
    public Weapon GetWeapon()
    {
        return MyWeapon;
    }
    public CrosshairController GetCrosshair()
    {
        return CrosshairController;
    }
    public void UnsubAllEvent()
    {

    }
}
