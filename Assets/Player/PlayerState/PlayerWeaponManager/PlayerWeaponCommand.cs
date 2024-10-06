using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponCommand 
{
    private WeaponSocket WeaponSocket;
    private Player player;
    
    public Weapon CurrentWeapon { get; private set; }
    public SecondaryWeapon secondaryWeapon { get; private set; }
    public PrimaryWeapon primaryWeapon { get; private set; }
    public AmmoProuch ammoProuch { get; private set; }

    public CrosshairController crosshairController;//Set Bullet Direction When Player Shoot
    public LeanCover leanCover { get; private set; }

    public PlayerWeaponCommand(Player player)
    {
        this.player = player;
        ammoProuch = new AmmoProuch(120, 0, 0, 0);
        ammoProuch.prochReload = new AmmoProchReload(ammoProuch);
        this.WeaponSocket = player.weaponSocket;
        this.crosshairController = CrosshairController.FindAnyObjectByType<CrosshairController>();
        leanCover = new LeanCover(player.rotationConstraint, crosshairController);
        Start();
    }
    private void Start()
    {
        player.StartCoroutine(GetWeapon());
    }
    public void Pulltriger()
    {
        if (CurrentWeapon != null)
        {
            PullTriggerCommand pullTriggerCommand = new PullTriggerCommand(CurrentWeapon);
            pullTriggerCommand.Execute();
        }
    }
    public void CancelTrigger()
    {
        CancelTriggerCommand cancelTriggerCommand = new CancelTriggerCommand(CurrentWeapon);
        cancelTriggerCommand.Execute();
    }
    public void Aim()
    {
        if (CurrentWeapon != null)
        {
            AimDownSightCommand aimDownSightCommand = new AimDownSightCommand(CurrentWeapon);
            aimDownSightCommand.Execute();
            GameObject playerRayCastPost = GameObject.Find("RayCastPos");
            leanCover.LeaningUpdate(playerRayCastPost.transform);
        }
    }
    public void Reload()
    {
        if(CurrentWeapon != null)
        {
            ReloadCommand reloadCommand = new ReloadCommand(CurrentWeapon,ammoProuch);
            reloadCommand.Execute();
        }
    }
    public void LowWeapon()
    {
       if(CurrentWeapon != null)
        {
            LowReadyCommand lowReadyCommand = new LowReadyCommand(CurrentWeapon);
            lowReadyCommand.Execute();
        }
        else
        {
            player.LowReadying(CurrentWeapon);
        }
        leanCover.LeanRecovery();
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
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.PickUpWeapon);
    }
}
