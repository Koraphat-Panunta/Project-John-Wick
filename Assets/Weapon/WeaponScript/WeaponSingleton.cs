using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSingleton : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Character UserWeapon { get; private set; }
    public WeaponSocket weaponSocket { get; private set; }
    public WeaponStance CurStance { get; private set; }
    public WeaponState CurState { get; private set; }
    [SerializeField] private CrosshairController CrosshairController;
    [SerializeField] private Weapon MyWeapon;
    [SerializeField] private WeaponStateManager _weaponStateManager;
    [SerializeField] private WeaponStanceManager _weaponStanceManager;
    [SerializeField] AnimatorOverrideController _weaponOverrideControllerPlayer;
    [SerializeField] AnimatorOverrideController _weaponOverrideControllerEnemy;
    public Action<Weapon> FireEvent;
    public Action<Weapon> Aim;
    public Action<Weapon> Reload;
    public Action<Weapon> LowReady;
    private void Start()
    {
        weaponSocket = GetComponentInParent<WeaponSocket>();
        UserWeapon = weaponSocket.GetWeaponUser();
        animator = weaponSocket.GetAnimator();
    }
    private void Update()
    {
        CurStance = _weaponStanceManager._currentStance;
        CurState = _weaponStateManager._currentState;
    }
    public WeaponStanceManager GetStanceManager()
    {
        return _weaponStanceManager;
    }
    public WeaponStateManager GetStateManager() 
    { 
        return _weaponStateManager; 
    }
    public Weapon GetWeapon()
    {
        return MyWeapon;
    }
    public CrosshairController GetCrosshair()
    {
        return CrosshairController;
    }
    public AnimatorOverrideController GetOverride_Player_Controller()
    {
        return _weaponOverrideControllerPlayer;
    }
    public AnimatorOverrideController GetOverride_Enemy_Controller()
    {
        return _weaponOverrideControllerEnemy;
    }
    public void UnsubAllEvent()
    {

    }
}
