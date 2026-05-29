using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Presets;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Run via: Tools → Setup Key Binding UI
/// Creates the KeyBindingEntryUI prefab and adds the Key Bindings panel to OptionSetting.prefab.
/// Safe to re-run: skips steps that are already done.
/// </summary>
public static class KeyBindingSetupEditor
{
    private const string OPTION_SETTING_PREFAB_PATH = "Assets/Project/UI/GameplayUI/Prefab/OptionSetting.prefab";
    private const string ENTRY_UI_PREFAB_PATH       = "Assets/Project/UI/GameplayUI/Prefab/KeyBindingEntryUI.prefab";
    private const int    REBIND_ENTRY_COUNT          = 18;

    [MenuItem("Tools/Setup Key Binding UI")]
    public static void Run()
    {
        // ── 1. Create KeyBindingEntryUI prefab ──────────────────────────────────
        GameObject entryUIPrefab = CreateOrGetEntryUIPrefab();

        // ── 2. Patch OptionSetting.prefab ───────────────────────────────────────
        PatchOptionSettingPrefab(entryUIPrefab);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[KeyBindingSetup] Done. Check OptionSetting.prefab for the Key Bindings panel.");
    }

    // ──────────────────────────────────────────────────────────────────────────────
    // KeyBindingEntryUI prefab
    // ──────────────────────────────────────────────────────────────────────────────

    private static GameObject CreateOrGetEntryUIPrefab()
    {
        // Skip if already exists
        var existing = AssetDatabase.LoadAssetAtPath<GameObject>(ENTRY_UI_PREFAB_PATH);
        if (existing != null)
        {
            Debug.Log("[KeyBindingSetup] KeyBindingEntryUI prefab already exists — skipping.");
            return existing;
        }

        // Root row (Horizontal Layout Group)
        var root = new GameObject("KeyBindingEntryUI");
        root.AddComponent<RectTransform>();
        root.AddComponent<CanvasRenderer>();

        var hlg = root.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlHeight  = true;
        hlg.childControlWidth   = true;
        hlg.childForceExpandHeight = true;
        hlg.childForceExpandWidth  = false;
        hlg.spacing = 8;

        var rootLE = root.AddComponent<LayoutElement>();
        rootLE.preferredHeight = 40;
        rootLE.flexibleWidth   = 1;

        // Action name label
        var actionNameObj = CreateTMPLabel("ActionName", root.transform, "Action Name", 14, TextAlignmentOptions.Left);
        actionNameObj.GetComponent<LayoutElement>().flexibleWidth = 1;

        // Binding text label
        var bindingTextObj = CreateTMPLabel("BindingText", root.transform, "Key", 14, TextAlignmentOptions.Center);
        bindingTextObj.GetComponent<LayoutElement>().preferredWidth = 120;

        // Binding icon image (hidden by default — activated when iconData is assigned)
        var iconObj = new GameObject("BindingIconImage");
        iconObj.transform.SetParent(root.transform, false);
        iconObj.AddComponent<RectTransform>();
        iconObj.AddComponent<CanvasRenderer>();
        var iconImg = iconObj.AddComponent<Image>();
        iconImg.preserveAspect = true;
        var iconLE = iconObj.AddComponent<LayoutElement>();
        iconLE.preferredWidth  = 40;
        iconLE.preferredHeight = 40;
        iconObj.SetActive(false); // shown only when iconData provides a sprite

        // "Press any key…" waiting text (hidden by default)
        var waitingObj = CreateTMPLabel("WaitingText", root.transform, "Press any key…", 12, TextAlignmentOptions.Center);
        waitingObj.GetComponent<LayoutElement>().preferredWidth = 120;
        waitingObj.SetActive(false);

        // Rebind button
        var buttonObj = new GameObject("RebindButton");
        buttonObj.transform.SetParent(root.transform, false);
        buttonObj.AddComponent<RectTransform>();
        buttonObj.AddComponent<CanvasRenderer>();
        buttonObj.AddComponent<Image>(); // button background
        var button = buttonObj.AddComponent<Button>();

        var buttonLE = buttonObj.AddComponent<LayoutElement>();
        buttonLE.preferredWidth  = 80;
        buttonLE.preferredHeight = 30;

        var buttonLabel = CreateTMPLabel("Label", buttonObj.transform, "Rebind", 12, TextAlignmentOptions.Center);
        buttonLabel.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        buttonLabel.GetComponent<RectTransform>().anchorMax = Vector2.one;
        buttonLabel.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        buttonLabel.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        // Wire KeyBindingEntryUI component
        var entryUI = root.AddComponent<KeyBindingEntryUI>();
        entryUI.actionNameText  = actionNameObj.GetComponent<TextMeshProUGUI>();
        entryUI.bindingText     = bindingTextObj.GetComponent<TextMeshProUGUI>();
        entryUI.bindingIconImage = iconImg;
        entryUI.rebindButton    = button;
        entryUI.waitingText     = waitingObj.GetComponent<TextMeshProUGUI>();

        // Save as prefab
        var prefab = PrefabUtility.SaveAsPrefabAsset(root, ENTRY_UI_PREFAB_PATH);
        Object.DestroyImmediate(root);

        Debug.Log("[KeyBindingSetup] Created KeyBindingEntryUI prefab at " + ENTRY_UI_PREFAB_PATH);
        return prefab;
    }

    // ──────────────────────────────────────────────────────────────────────────────
    // Patch OptionSetting.prefab
    // ──────────────────────────────────────────────────────────────────────────────

    private static void PatchOptionSettingPrefab(GameObject entryUIPrefab)
    {
        var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(OPTION_SETTING_PREFAB_PATH);
        if (prefabAsset == null)
        {
            Debug.LogError("[KeyBindingSetup] Could not find OptionSetting.prefab at " + OPTION_SETTING_PREFAB_PATH);
            return;
        }

        // Open prefab for editing
        var prefabRoot = PrefabUtility.LoadPrefabContents(OPTION_SETTING_PREFAB_PATH);

        var optionCanvas = prefabRoot.GetComponentInChildren<OptionUICanvas>(true);
        if (optionCanvas == null)
        {
            Debug.LogError("[KeyBindingSetup] OptionUICanvas not found inside OptionSetting.prefab.");
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            return;
        }

        // Skip if KeyBindingSettingOptionDisplay already exists
        if (prefabRoot.GetComponentInChildren<KeyBindingSettingOptionDisplay>(true) != null)
        {
            Debug.Log("[KeyBindingSetup] KeyBindingSettingOptionDisplay already present — skipping prefab patch.");
            PrefabUtility.UnloadPrefabContents(prefabRoot);
            return;
        }

        // Find an existing displayer to use as a sibling reference for layout
        OptionUIDisplayer existingDisplayer = prefabRoot.GetComponentInChildren<ControlSettingOptionDisplay>(true);
        if (existingDisplayer == null)
            existingDisplayer = prefabRoot.GetComponentInChildren<AudioSettingOptionDisplay>(true);

        Transform contentParent = existingDisplayer != null
            ? existingDisplayer.transform.parent
            : optionCanvas.transform;

        // ── Build Key Binding panel ────────────────────────────────────────────

        var kbPanel = new GameObject("KeyBindingSettingPanel");
        kbPanel.transform.SetParent(contentParent, false);
        var kbPanelRT = kbPanel.AddComponent<RectTransform>();
        CopyRectTransform(existingDisplayer?.GetComponent<RectTransform>(), kbPanelRT);

        var kbDisplayer = kbPanel.AddComponent<KeyBindingSettingOptionDisplay>();

        // optionCanvasSector references the panel root
        kbDisplayer.optionCanvasSector = kbPanel;
        kbPanel.SetActive(false); // hidden by default, shown when tab is selected

        // ── Scroll View ────────────────────────────────────────────────────────

        var scrollViewObj = new GameObject("ScrollView");
        scrollViewObj.transform.SetParent(kbPanel.transform, false);
        var scrollRT = scrollViewObj.AddComponent<RectTransform>();
        scrollRT.anchorMin = Vector2.zero;
        scrollRT.anchorMax = Vector2.one;
        scrollRT.offsetMin = Vector2.zero;
        scrollRT.offsetMax = Vector2.zero;

        var scrollRect = scrollViewObj.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical   = true;

        // Viewport
        var viewportObj = new GameObject("Viewport");
        viewportObj.transform.SetParent(scrollViewObj.transform, false);
        var viewportRT = viewportObj.AddComponent<RectTransform>();
        viewportRT.anchorMin = Vector2.zero;
        viewportRT.anchorMax = Vector2.one;
        viewportRT.offsetMin = Vector2.zero;
        viewportRT.offsetMax = Vector2.zero;
        viewportObj.AddComponent<RectMask2D>();
        scrollRect.viewport = viewportRT;

        // Content
        var contentObj = new GameObject("Content");
        contentObj.transform.SetParent(viewportObj.transform, false);
        var contentRT = contentObj.AddComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0, 1);
        contentRT.anchorMax = new Vector2(1, 1);
        contentRT.pivot     = new Vector2(0.5f, 1f);
        contentRT.offsetMin = Vector2.zero;
        contentRT.offsetMax = Vector2.zero;
        scrollRect.content  = contentRT;

        var vlg = contentObj.AddComponent<VerticalLayoutGroup>();
        vlg.childControlHeight     = false;
        vlg.childControlWidth      = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth  = true;
        vlg.spacing = 4;
        vlg.padding = new RectOffset(8, 8, 8, 8);

        var csf = contentObj.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // ── Spawn 18 entry rows ────────────────────────────────────────────────

        var entryUIs = new List<KeyBindingEntryUI>();
        for (int i = 0; i < REBIND_ENTRY_COUNT; i++)
        {
            var entryInstance = (GameObject)PrefabUtility.InstantiatePrefab(entryUIPrefab, contentObj.transform);
            entryInstance.name = $"KeyBindingEntry_{i:D2}";
            entryUIs.Add(entryInstance.GetComponent<KeyBindingEntryUI>());
        }

        kbDisplayer.entryUIs = entryUIs.ToArray();

        // ── Reset to Default button ────────────────────────────────────────────

        var resetBtnObj = new GameObject("ResetToDefaultButton");
        resetBtnObj.transform.SetParent(kbPanel.transform, false);
        var resetRT = resetBtnObj.AddComponent<RectTransform>();
        resetRT.anchorMin = new Vector2(0.5f, 0f);
        resetRT.anchorMax = new Vector2(0.5f, 0f);
        resetRT.pivot     = new Vector2(0.5f, 0f);
        resetRT.sizeDelta = new Vector2(160, 36);
        resetRT.anchoredPosition = new Vector2(0, 8);
        resetBtnObj.AddComponent<CanvasRenderer>();
        resetBtnObj.AddComponent<Image>();
        var resetBtn = resetBtnObj.AddComponent<Button>();

        var resetLabel = CreateTMPLabel("Label", resetBtnObj.transform, "Reset to Default", 13, TextAlignmentOptions.Center);
        resetLabel.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        resetLabel.GetComponent<RectTransform>().anchorMax = Vector2.one;
        resetLabel.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        resetLabel.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        kbDisplayer.resetToDefaultButton = resetBtn;

        // ── Tab button (clone style from an existing selector button) ──────────

        Button tabButton = null;
        if (optionCanvas.selectOptionSectors != null && optionCanvas.selectOptionSectors.Length > 0)
        {
            var sampleBtn = optionCanvas.selectOptionSectors[0].settingSelectionButton;
            if (sampleBtn != null)
            {
                var tabBtnObj = Object.Instantiate(sampleBtn.gameObject, sampleBtn.transform.parent);
                tabBtnObj.name = "KeyBindingTabButton";
                var tabTMP = tabBtnObj.GetComponentInChildren<TextMeshProUGUI>();
                if (tabTMP != null) tabTMP.text = "Key Bindings";
                tabButton = tabBtnObj.GetComponent<Button>();
            }
        }

        if (tabButton == null)
        {
            // Fallback: create a plain button next to the canvas
            var tabBtnObj = new GameObject("KeyBindingTabButton");
            tabBtnObj.transform.SetParent(optionCanvas.transform, false);
            tabBtnObj.AddComponent<RectTransform>();
            tabBtnObj.AddComponent<CanvasRenderer>();
            tabBtnObj.AddComponent<Image>();
            tabButton = tabBtnObj.AddComponent<Button>();
            CreateTMPLabel("Label", tabBtnObj.transform, "Key Bindings", 14, TextAlignmentOptions.Center);
        }

        // ── Register new sector in OptionUICanvas.selectOptionSectors ─────────

        var oldSectors = optionCanvas.selectOptionSectors ?? new OptionUICanvas.SelectOptionSector[0];
        var newSectors = new OptionUICanvas.SelectOptionSector[oldSectors.Length + 1];
        for (int i = 0; i < oldSectors.Length; i++)
            newSectors[i] = oldSectors[i];

        newSectors[oldSectors.Length] = new OptionUICanvas.SelectOptionSector
        {
            optionUIDisplayer    = kbDisplayer,
            settingSelectionButton = tabButton
        };

        optionCanvas.selectOptionSectors = newSectors;

        // ── Save prefab ────────────────────────────────────────────────────────

        PrefabUtility.SaveAsPrefabAsset(prefabRoot, OPTION_SETTING_PREFAB_PATH);
        PrefabUtility.UnloadPrefabContents(prefabRoot);

        Debug.Log("[KeyBindingSetup] OptionSetting.prefab patched — Key Bindings panel added.");
    }

    // ──────────────────────────────────────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────────────────────────────────────

    private static GameObject CreateTMPLabel(string name, Transform parent, string text, float fontSize, TextAlignmentOptions alignment)
    {
        var obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        obj.AddComponent<RectTransform>();
        obj.AddComponent<CanvasRenderer>();

        var tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text      = text;
        tmp.fontSize  = fontSize;
        tmp.alignment = alignment;

        obj.AddComponent<LayoutElement>();
        return obj;
    }

    private static void CopyRectTransform(RectTransform src, RectTransform dst)
    {
        if (src == null) return;
        dst.anchorMin        = src.anchorMin;
        dst.anchorMax        = src.anchorMax;
        dst.pivot            = src.pivot;
        dst.anchoredPosition = src.anchoredPosition;
        dst.sizeDelta        = src.sizeDelta;
    }
}
