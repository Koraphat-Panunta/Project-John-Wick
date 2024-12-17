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

    }
    public void Look(InputAction.CallbackContext context)
    {

    }
    public void Sprint(InputAction.CallbackContext context)
    {

    }
    public void Aim(InputAction.CallbackContext context)
    {

    }
    public void PullTrigger(InputAction.CallbackContext callback)
    {

    }
    public void Reload(InputAction.CallbackContext context)
    {

    }
    public void SwapShoulder(InputAction.CallbackContext context)
    {

    }
    public void SwitchWeapon(InputAction.CallbackContext context)
    {

    }
}
