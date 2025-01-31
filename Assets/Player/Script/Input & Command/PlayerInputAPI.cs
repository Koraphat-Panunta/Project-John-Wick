using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAPI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Move(InputAction.CallbackContext context)
    {
        player.inputMoveDir_Local = context.ReadValue<Vector2>();
    }
    public void Look(InputAction.CallbackContext context)
    {
        player.inputLookDir_Local = context.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isSprint = true;
        if(context.canceled)
            player.isSprint = false;
    }
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isAimingCommand = true;
        if (context.canceled)
            player.isAimingCommand = false;
    }
    public void PullTrigger(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isPullTriggerCommand = true;
        if (context.canceled)
            player.isPullTriggerCommand = false;
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isReloadCommand = true;
        if (context.canceled)
            player.isReloadCommand = false;
    }
    public void SwapShoulder(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isSwapShoulder = true;
        if (context.canceled)
            player.isSwapShoulder = false;
    }
    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
            player.isSwitchWeaponCommand = true;
        if (context.canceled)
            player.isSwitchWeaponCommand = false;
    }
    public void TriggerGunFu(InputAction.CallbackContext context)
    {
        if(context.performed)
            player._triggerGunFu = true;
        
        Debug.Log("_triggerGunFu = " + player._triggerGunFu);
    }
}
