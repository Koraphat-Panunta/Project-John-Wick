using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponCommand 
{
    private Player player;
    public AmmoProuch ammoProuch { get; private set; }

    public CrosshairController crosshairController;//Set Bullet Direction When Player Shoot
    public LeanCover leanCover { get; private set; }
    public bool isSwitchingWeapon;

    public PlayerWeaponCommand(Player player)
    {
        this.player = player;
        ammoProuch = new AmmoProuch(60, 120, 150, 120);
        ammoProuch.prochReload = new AmmoProchReload(ammoProuch);
        this.crosshairController = CrosshairController.FindAnyObjectByType<CrosshairController>();
        leanCover = new LeanCover(player.rotationConstraint, crosshairController);
        isSwitchingWeapon = false;
        Start();
    }
    private void Start()
    {
        //player.StartCoroutine(GetWeapon());
    }
    public void Pulltriger()
    {
        if (player.curentWeapon != null&& isSwitchingWeapon == false)
        {
            PullTriggerCommand pullTriggerCommand = new PullTriggerCommand(player.curentWeapon);
            pullTriggerCommand.Execute();
        }
        
    }
    public void CancelTrigger()
    {
        if (player.curentWeapon != null )
        {
            CancelTriggerCommand cancelTriggerCommand = new CancelTriggerCommand(player.curentWeapon);
            cancelTriggerCommand.Execute();
        }
    }
    public void Aim()
    {
        if (player.curentWeapon != null && isSwitchingWeapon == false)
        {
            AimDownSightCommand aimDownSightCommand = new AimDownSightCommand(player.curentWeapon);
            aimDownSightCommand.Execute();
            GameObject playerRayCastPost = player.RayCastPos.gameObject;
            leanCover.LeaningUpdate(playerRayCastPost.transform);
        }
        
    }
    public void Reload()
    {
        if (player.curentWeapon != null && isSwitchingWeapon == false)
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
    Coroutine switchCoroutine;
    public void SwitchWeapon()
    {
        if (isSwitchingWeapon == false)
        {
            if (player.curentWeapon == player.primaryWeapon)
            {
                switchCoroutine = player.StartCoroutine(SwitchPTS());
            }
            else if (player.curentWeapon == player.secondaryWeapon)
            {
                switchCoroutine = player.StartCoroutine(SwitchSTP());
            }
        }

    }
    IEnumerator SwitchPTS()
    {
        isSwitchingWeapon = true;
        WeaponStateManager weaponStateManager = player.curentWeapon.weapon_stateManager;
        PrimaryWeapon primaryWeapon = player.primaryWeapon;
        SecondaryWeapon secondaryWeapon = player.secondaryWeapon;
        Weapon curWeapon = player.curentWeapon;
        if (weaponStateManager._currentState == weaponStateManager.reloadState)
        {
            player.curentWeapon.weapon_stateManager.ChangeState(weaponStateManager.none);
        }
        Animator animator = player.animator;
        animator.SetTrigger("HolsterPrimary");
        yield return new WaitForSeconds(0.3f);
        curWeapon.AttachWeaponTo(player.primaryHolster);
        animator.SetTrigger("DrawSecondary");
        yield return new WaitForSeconds(0.13f);
        secondaryWeapon.AttatchWeaponTo(player);
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwitchWeapon);
        isSwitchingWeapon = false;
        

    }
    IEnumerator SwitchSTP()
    {
        isSwitchingWeapon = true;
        WeaponStateManager weaponStateManager = player.curentWeapon.weapon_stateManager;
        PrimaryWeapon primaryWeapon = player.primaryWeapon;
        SecondaryWeapon secondaryWeapon = player.secondaryWeapon;
        Weapon curWeapon = player.curentWeapon;
        if (weaponStateManager._currentState == weaponStateManager.reloadState)
        {
            player.curentWeapon.weapon_stateManager.ChangeState(weaponStateManager.none);
        }
        Animator animator = player.animator;
        animator.SetTrigger("HolsterSecondary");
        yield return new WaitForSeconds(0.4f);
        curWeapon.AttachWeaponTo(player.secondaryHolster);
        animator.SetTrigger("DrawPrimary");
        yield return new WaitForSeconds(0.5f);
        primaryWeapon.AttatchWeaponTo(player);
        player.NotifyObserver(player, SubjectPlayer.PlayerAction.SwitchWeapon);
        isSwitchingWeapon = false;
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
