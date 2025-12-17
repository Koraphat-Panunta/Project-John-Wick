using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputActor : Actor,IInitializedAble,UserInput.IPlayerActionActions
{
    public UserInput userInput { get; protected set; }

    public void Initialized()
    {
        userInput = new UserInput();
        userInput.PlayerAction.AddCallbacks(this);
        userInput.PlayerAction.Enable();
    }

   
    public void EnableInput()
    {
        if(userInput == null)
            userInput = new UserInput();

        userInput.Enable();
    }
    public void DisableInput()
    {
        userInput.Disable();
    }

  
    private void OnDisable()
    {
        DisableInput();
    }
   
    public void OnMove(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnSwapShoulder(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnTrggerGunFuExecute(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnToggleChangeStance(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnTriggerDodgeRoll(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnTriggerDropWeapon(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnTriggerSwitchDrawPrimary(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnTriggerSwitchDrawSecondary(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public void OnHolsterWeapon(InputAction.CallbackContext context)
    {
        base.NotifyObserver(context);
    }

    public enum InpuPhase
    {
        performed,
        canceled
    }
}
