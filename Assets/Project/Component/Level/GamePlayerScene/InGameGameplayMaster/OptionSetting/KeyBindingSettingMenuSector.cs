using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBindingSettingMenuSector : OptionMenuSector
{
    public struct RebindEntry
    {
        public string displayName;
        public InputAction action;
        public int bindingIndex;
    }

    public override OptionUIDisplayer optionUIDisplayer => _keyBindingDisplay;

    private readonly KeyBindingSettingOptionDisplay _keyBindingDisplay;
    private readonly UserInput _userInput;
    private readonly List<RebindEntry> _entries;
    private InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    public KeyBindingSettingMenuSector(
        OptionUICanvas optionUICanvas,
        KeyBindingSettingOptionDisplay optionUIDisplayer,
        UserInput userInput,
        GameMaster gameMaster)
        : base(optionUICanvas, gameMaster)
    {
        _keyBindingDisplay = optionUIDisplayer;
        _userInput = userInput;
        _entries = BuildRebindEntries();

        if (_keyBindingDisplay.resetToDefaultButton != null)
            _keyBindingDisplay.resetToDefaultButton.onClick.AddListener(ResetToDefault);
        else
            Debug.LogWarning("[KeyBindingSettingMenuSector] resetToDefaultButton is not assigned on KeyBindingSettingOptionDisplay.");

        if (_keyBindingDisplay.entryUIs == null || _keyBindingDisplay.entryUIs.Length == 0)
        {
            Debug.LogWarning("[KeyBindingSettingMenuSector] entryUIs is empty — run Tools → Setup Key Binding UI to build the prefab.");
            return;
        }

        for (int i = 0; i < _keyBindingDisplay.entryUIs.Length && i < _entries.Count; i++)
        {
            if (_keyBindingDisplay.entryUIs[i] == null || _keyBindingDisplay.entryUIs[i].rebindButton == null) continue;
            int capturedIndex = i;
            _keyBindingDisplay.entryUIs[i].rebindButton.onClick.AddListener(() => StartRebind(capturedIndex));
        }
    }

    public override void Enter()
    {
        _keyBindingDisplay.SetEntries(_entries);
        base.Enter();
    }

    public override void Exit()
    {
        _rebindOperation?.Cancel();
        SaveOverrides();
        base.Exit();
    }

    public override void ResetToDefault()
    {
        _rebindOperation?.Cancel();
        _userInput.RemoveAllBindingOverrides();
        _keyBindingDisplay.RefreshAllEntries();
    }

    private void StartRebind(int entryIndex)
    {
        if (entryIndex >= _entries.Count) return;

        var entry = _entries[entryIndex];
        var entryUI = _keyBindingDisplay.entryUIs[entryIndex];

        SetAllRebindButtonsEnabled(false);
        entryUI.SetWaiting(true);

        entry.action.Disable();

        _rebindOperation = entry.action
            .PerformInteractiveRebinding(entry.bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(_ => FinishRebind(entryIndex))
            .OnCancel(_ => FinishRebind(entryIndex))
            .Start();
    }

    private void FinishRebind(int entryIndex)
    {
        _rebindOperation?.Dispose();
        _rebindOperation = null;

        _entries[entryIndex].action.Enable();

        _keyBindingDisplay.entryUIs[entryIndex].SetWaiting(false);
        _keyBindingDisplay.RefreshEntry(entryIndex);
        SetAllRebindButtonsEnabled(true);
    }

    private void SetAllRebindButtonsEnabled(bool enabled)
    {
        foreach (var entryUI in _keyBindingDisplay.entryUIs)
            entryUI.rebindButton.interactable = enabled;
    }

    private void SaveOverrides()
    {
        DynamicDataBased.Instance.settingDataScriptableObject.keyBindingSetting.bindingOverridesJson =
            _userInput.SaveBindingOverridesAsJson();
    }

    private List<RebindEntry> BuildRebindEntries()
    {
        var entries = new List<RebindEntry>();
        AddKeyboardBindings(entries, _userInput.PlayerAction.Move, "Move");
        AddKeyboardBindings(entries, _userInput.PlayerAction.Sprint, "Sprint");
        AddKeyboardBindings(entries, _userInput.PlayerAction.Aim, "Aim");
        AddKeyboardBindings(entries, _userInput.PlayerAction.Attack, "Attack");
        AddKeyboardBindings(entries, _userInput.PlayerAction.Reload, "Reload");
        AddKeyboardBindings(entries, _userInput.PlayerAction.SwapShoulder, "Swap Shoulder");
        AddKeyboardBindings(entries, _userInput.PlayerAction.TrggerGunFuExecute, "Gun Fu Execute");
        AddKeyboardBindings(entries, _userInput.PlayerAction.ToggleChangeStance, "Crouch / Stand");
        AddKeyboardBindings(entries, _userInput.PlayerAction.TriggerDodgeRoll, "Dodge Roll");
        AddKeyboardBindings(entries, _userInput.PlayerAction.Interact, "Interact");
        AddKeyboardBindings(entries, _userInput.PlayerAction.TriggerDropWeapon, "Drop Weapon");
        AddKeyboardBindings(entries, _userInput.PlayerAction.TriggerSwitchDrawPrimary, "Draw Primary");
        AddKeyboardBindings(entries, _userInput.PlayerAction.TriggerSwitchDrawSecondary, "Draw Secondary");
        AddKeyboardBindings(entries, _userInput.PlayerAction.HolsterWeapon, "Holster");
        AddKeyboardBindings(entries, _userInput.PauseAction.PauseTrigger, "Pause");
        return entries;
    }

    private void AddKeyboardBindings(List<RebindEntry> entries, InputAction action, string displayPrefix)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite) continue;

            // No control schemes defined in this project — identify keyboard/mouse bindings by path prefix
            string path = binding.effectivePath;
            if (string.IsNullOrEmpty(path)) continue;
            if (!path.StartsWith("<Keyboard>") && !path.StartsWith("<Mouse>")) continue;

            string name = binding.isPartOfComposite
                ? $"{displayPrefix} {CapitalizeFirst(binding.name)}"
                : displayPrefix;

            entries.Add(new RebindEntry { displayName = name, action = action, bindingIndex = i });
        }
    }

    private string CapitalizeFirst(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}
