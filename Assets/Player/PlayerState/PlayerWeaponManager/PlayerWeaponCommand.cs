using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponCommand 
{
    private Player player;
    
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
        this.crosshairController = CrosshairController.FindAnyObjectByType<CrosshairController>();
        leanCover = new LeanCover(player.rotationConstraint, crosshairController);
        Start();
    }
    private void Start()
    {
        //player.StartCoroutine(GetWeapon());
    }
    public void Pulltriger()
    {
        if (player.curentWeapon != null)
        {
            PullTriggerCommand pullTriggerCommand = new PullTriggerCommand(player.curentWeapon);
            pullTriggerCommand.Execute();
        }
        
    }
    public void CancelTrigger()
    {
        if (player.curentWeapon != null)
        {
            CancelTriggerCommand cancelTriggerCommand = new CancelTriggerCommand(player.curentWeapon);
            cancelTriggerCommand.Execute();
        }
    }
    public void Aim()
    {
        if (player.curentWeapon != null)
        {
            AimDownSightCommand aimDownSightCommand = new AimDownSightCommand(player.curentWeapon);
            aimDownSightCommand.Execute();
            GameObject playerRayCastPost = player.RayCastPos.gameObject;
            leanCover.LeaningUpdate(playerRayCastPost.transform);
        }
        
    }
    public void Reload()
    {
        if (player.curentWeapon != null)
        {
            ReloadCommand reloadCommand = new ReloadCommand(player.curentWeapon, ammoProuch);
            reloadCommand.Execute();
        }
        
    }
    public void LowWeapon()
    {
        if (player.curentWeapon != null)
        {
            LowReadyCommand lowReadyCommand = new LowReadyCommand(player.curentWeapon);
            lowReadyCommand.Execute();
            leanCover.LeanRecovery();
        }
        else
        {
            leanCover.LeanRecovery();
        }
    }
    public void SwitchWeapon()
    {
       
    }
    //private IEnumerator GetWeapon()
    //{
    //    CurrentWeapon = null;
    //    while(CurrentWeapon == null)
    //    {
    //        CurrentWeapon = WeaponSocket.CurWeapon;
    //        yield return null;
    //    }
    //    player.NotifyObserver(player, SubjectPlayer.PlayerAction.PickUpWeapon);
    //}
}
