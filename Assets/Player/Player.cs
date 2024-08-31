using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Reload;

public class Player : SubjectPlayer
{
    //Unity Component
    public Animator animator;

    //C# Component
    public PlayerController playerController;
    public WeaponSocket weaponSocket;

    //Class coposition
    public PlayerWeaponCommand playerWeaponCommand;
    public PlayerMovement playerMovement;
    public PlayerStateManager playerStateManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = new PlayerMovement(this);
        playerWeaponCommand = new PlayerWeaponCommand(this);

        playerStateManager = new PlayerStateManager(this);
        playerStateManager.SetupState(this);
    }
    private void Update()
    {
        playerStateManager.Update();
    }
    private void FixedUpdate()
    {
        playerMovement.MovementUpdate();
        playerStateManager.FixedUpdate();
    }
    public override void Aiming(Weapon weapon)
    {
        RotateObjectToward rotateObjectToward = new RotateObjectToward();
        rotateObjectToward.RotateTowards(Camera.main.transform.forward,gameObject,6);
        animator.SetLayerWeight(1,weapon.weapon_StanceManager.AimingWeight);
        NotifyObserver(this, PlayerAction.Aim);
        base.Aiming(weapon);
    }

    public override void Firing(Weapon weapon)
    {
        NotifyObserver(this, PlayerAction.Firing);
        base.Firing(weapon);
    }

    public override void LowReadying(Weapon weapon)
    {
        animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
        NotifyObserver(this,PlayerAction.LowReady);
        base.LowReadying(weapon);
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        if (reloadType == ReloadType.TacticalReload)
        {
            animator.SetTrigger("TacticalReload");
            animator.SetLayerWeight(2, 1);
        }
        else if(reloadType == ReloadType.ReloadMagOut)
        {
            animator.SetTrigger("Reloading");
            animator.SetLayerWeight(2, 1);
        }
        else if(reloadType == ReloadType.ReloadFinished)
        {
            StartCoroutine(RecoveryReloadLayerWeight());
        }
        base.Reloading(weapon, reloadType);
    }
    IEnumerator RecoveryReloadLayerWeight()
    {
        float RecoveryWeight = 10;
        while(animator.GetLayerWeight(2) > 0)
        {
            animator.SetLayerWeight(2, animator.GetLayerWeight(2) - (RecoveryWeight * Time.deltaTime));
            yield return null;
        }
    }
}
