
using UnityEngine;

public class Player : SubjectPlayer
{
    //C# Component
    public PlayerController playerController;
    public WeaponSocket weaponSocket;
    public PlayerAnimation playerAnimation;

    //Class coposition
    public PlayerWeaponCommand playerWeaponCommand;
    public PlayerMovement playerMovement;
    public PlayerStateManager playerStateManager;

    private void Start()
    {
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
        playerStateManager.FixedUpdate();
        playerMovement.MovementUpdate();
    }
    public override void Aiming(Weapon weapon)
    {
        RotateObjectToward rotateObjectToward = new RotateObjectToward();
        rotateObjectToward.RotateTowards(Camera.main.transform.forward,gameObject,6);
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
        if (weapon != null)
        {
            NotifyObserver(this, PlayerAction.LowReady);
            base.LowReadying(weapon);
        }
        else
        {
            //animator.SetLayerWeight(1,Mathf.Lerp(animator.GetLayerWeight\));
            NotifyObserver(this, PlayerAction.LowReady);
        }
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        NotifyObserver(this, PlayerAction.Reloading);
        base.Reloading(weapon, reloadType);
    }
   
}
