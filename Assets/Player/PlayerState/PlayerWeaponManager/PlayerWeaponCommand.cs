using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponCommand 
{
    private WeaponSocket WeaponSocket;
    private Animator playerAnimator;

    private Player player;

    public event Action<Weapon> weaponAim;
    public event Action<Weapon> weaponLow;
    public event Action<Weapon> weaponShoot;
    public Weapon CurrentWeapon { get; private set; }
    public SecondaryWeapon secondaryWeapon { get; private set; }
    public PrimaryWeapon primaryWeapon { get; private set; }
    public CameraZoom cameraZoom { get; private set; }
    public AmmoProuch ammoProuch { get; private set; }

    public PlayerWeaponCommand(Player player)
    {
        this.player = player;
        ammoProuch = new AmmoProuch(120, 0, 0, 0);
        ammoProuch.prochReload = new AmmoProchReload(ammoProuch);
        this.WeaponSocket = player.weaponSocket;
        this.playerAnimator = player.animator;
        Start();
    }
    private void Start()
    {
        player.StartCoroutine(GetWeapon());
    }
    public void Pulltriger(PlayerStateManager playerstate,InputAction.CallbackContext Value)
    {
        if(CurrentWeapon.weapon_StanceManager._currentStance == CurrentWeapon.weapon_StanceManager.aimDownSight
            &&playerstate.Current_state != playerstate.sprint)
        {
            PullTriggerCommand pullTrigger = new PullTriggerCommand(CurrentWeapon);
            if (Value.phase == InputActionPhase.Started || Value.phase == InputActionPhase.Performed)
            {
                pullTrigger.Execute();
            }
            else if(Value.phase == InputActionPhase.Canceled)
            {
                pullTrigger.TriggerCancel();
            }
        }
    }
    public void Aim(PlayerStateManager playerstate)
    {
        if(playerstate.Current_state != playerstate.sprint)
        {
            AimDownSightCommand Aim = new AimDownSightCommand(CurrentWeapon);
            Aim.Execute();
        }
        else
        {
            LowReadyCommand lowReady = new LowReadyCommand(CurrentWeapon);
            lowReady.Execute();
        }
    }
    public void Reload(CharacterState playerstate)
    {
        if (ammoProuch.amountOf_ammo[CurrentWeapon.bullet.GetComponent<Bullet>().type] > 0) 
        {
            ReloadCommand reload = new ReloadCommand(CurrentWeapon,ammoProuch);
            reload.Execute();
        }
    }
    public void LowWeapon(CharacterState playerstate)
    {
        LowReadyCommand lowReady = new LowReadyCommand(CurrentWeapon);
        lowReady.Execute();
        
    }
    public void SwitchWeapon()
    {
       
    }
    private IEnumerator GetWeapon()
    {
        CurrentWeapon = null;
        while(CurrentWeapon == null)
        {
            CurrentWeapon = WeaponSocket.CurWeapon;
            yield return null;
        }
        playerAnimator.runtimeAnimatorController = WeaponSocket.weaponSingleton.GetOverride_Player_Controller();
    }
}
