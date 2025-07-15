using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

public class EnemyStatusInWorldUI : InWorldUI
{
    public InputActionReference interactAction; // Assign "Interact" InputAction
    public TextMeshProUGUI promptText; // For text like "Press [E]"

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        UpdatePrompt();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected ||
            change == InputDeviceChange.Enabled)
        {
            UpdatePrompt();
        }
    }

    void UpdatePrompt()
    {
        var controlScheme = GetCurrentControlScheme();

        switch (controlScheme)
        {
            case "Keyboard&Mouse":
                promptText.text = "Press [E]";
                break;

            case "Gamepad":
                var gamepad = Gamepad.current;

                if (gamepad != null)
                {
                    string buttonName = gamepad is XInputController ? "[X]" : "[□]";
                    promptText.text = $"Press {buttonName}";
                }
                break;
        }
    }

    string GetCurrentControlScheme()
    {
        var playerInput = PlayerInput.all.Count > 0 ? PlayerInput.all[0] : null;
        return playerInput != null ? playerInput.currentControlScheme : "Keyboard&Mouse";
    }
}
