using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private static bool _showCoreStats = true;
    private static bool _showCharController = true;
    private static bool _showSceneRefs = true;
    private static bool _showParkour = true;
    private static bool _showWeaponSockets = true;
    private static bool _showWeaponQuickSwitch = true;
    private static bool _showSprint = true;
    private static bool _showQuickShot = true;
    private static bool _showThrowObject = true;
    private static bool _showIFrame = true;
    private static bool _showGunFuSetup = true;
    private static bool _showGunFuHits = true;
    private static bool _showGunFuInteractions = true;
    private static bool _showGunFuExecutes = true;
    private static bool _showDebug = false;
    private static bool _showOthers = false;

    private static readonly string[] _organizedFields =
    {
        "m_Script",
        // Core Stats
        "playerStatsScriptableObject",
        "isImortal",
        // Character Controller
        "stand_CharacterControllerSCRP",
        "crouch_CharacterControllerSCRP",
        "parkour_CharacterControllerSCRP",
        "moveWarping",
        // Scene References
        "playerStateNodeManager",
        "RayCastPos",
        "cinemachineCamera",
        "centreTransform",
        "crosshairController",
        // Parkour
        "climbLowScrp",
        "climbHighScrp",
        "vaultingScrp",
        // Weapon - Sockets
        "MainHandSocket",
        "SecondHandSocket",
        "PrimaryWeaponSocket",
        "SecondaryWeaponSocket",
        // Weapon - Quick Switch
        "quickSwitchDrawSCRP",
        "quickSiwthcHolsterPrimarySCRP",
        "quickSwitchHoslterSecondarySCRP",
        "LeftHandHoldWeaponOffset",
        // Sprint
        "sprintChangeDirSCRP",
        // Quick Shot
        "quickShotAnimationTriggerEventSCRP",
        "castFindingScriptableObject",
        // Throw Object
        "throwObjectAnimationTriggerEventSCRP",
        // IFrame
        "humanShiedlIFrame",
        "restrictShieldIFrame",
        // GunFu - Setup
        "GunFuDetectTarget",
        "targetAdjustTranform",
        // GunFu - Hits
        "hit1",
        "hit2",
        "hit3",
        "dodgeSpinKick",
        // GunFu - Interactions
        "humanShieldSCRP",
        "humanShield_Exit_SCRP",
        "humanShieldTargetAdjustTransform",
        "restrictScriptableObject",
        "gunFuReloadScripatableObject",
        "gunFuHitDownScriptableObject",
        "parryPrimaryWeaponSCRP",
        "parrySecondaryWeaponSCRP",
        // GunFu - Executes
        "gunFuExecute_Single_Secondary_Dodge_ScriptableObject_I",
        "gunFuExecute_Single_Secondary_ScriptableObject_I",
        "gunFuExecute_Single_Primary_ScriptableObject_II",
        "gunFuExecute_Single_Primary_Dodge_ScriptableObject_I",
        "gunFu_Single_Execute_OnGround",
        // Debug
        "debugIsIFrame",
        "isPullTriggerCommand",
    };

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;

        EditorGUILayout.Space(2);

        Section(ref _showCoreStats, "Core Stats", () =>
        {
            Field("playerStatsScriptableObject");
            Field("isImortal");
        });

        Section(ref _showCharController, "Character Controller", () =>
        {
            Field("stand_CharacterControllerSCRP");
            Field("crouch_CharacterControllerSCRP");
            Field("parkour_CharacterControllerSCRP");
            Field("moveWarping");
        });

        Section(ref _showSceneRefs, "Scene References", () =>
        {
            Field("playerStateNodeManager");
            Field("RayCastPos");
            Field("cinemachineCamera");
            Field("centreTransform");
            Field("crosshairController");
        });

        Section(ref _showParkour, "Parkour", () =>
        {
            Field("climbLowScrp");
            Field("climbHighScrp");
            Field("vaultingScrp");
        });

        Section(ref _showWeaponSockets, "Weapon — Sockets", () =>
        {
            Field("MainHandSocket");
            Field("SecondHandSocket");
            Field("PrimaryWeaponSocket");
            Field("SecondaryWeaponSocket");
        });

        Section(ref _showWeaponQuickSwitch, "Weapon — Quick Switch", () =>
        {
            Field("quickSwitchDrawSCRP");
            Field("quickSiwthcHolsterPrimarySCRP");
            Field("quickSwitchHoslterSecondarySCRP");
            Field("LeftHandHoldWeaponOffset");
        });

        Section(ref _showSprint, "Sprint", () =>
        {
            Field("sprintChangeDirSCRP");
        });

        Section(ref _showQuickShot, "Quick Shot", () =>
        {
            Field("quickShotAnimationTriggerEventSCRP");
            Field("castFindingScriptableObject");
        });

        Section(ref _showThrowObject, "Throw Object", () =>
        {
            Field("throwObjectAnimationTriggerEventSCRP");
        });

        Section(ref _showIFrame, "I-Frame", () =>
        {
            Field("humanShiedlIFrame");
            Field("restrictShieldIFrame");
        });

        Section(ref _showGunFuSetup, "GunFu — Setup", () =>
        {
            Field("GunFuDetectTarget");
            Field("targetAdjustTranform");
        });

        Section(ref _showGunFuHits, "GunFu — Hits", () =>
        {
            Field("hit1");
            Field("hit2");
            Field("hit3");
            Field("dodgeSpinKick");
        });

        Section(ref _showGunFuInteractions, "GunFu — Interactions", () =>
        {
            Field("humanShieldSCRP");
            Field("humanShield_Exit_SCRP");
            Field("humanShieldTargetAdjustTransform");
            Field("restrictScriptableObject");
            Field("gunFuReloadScripatableObject");
            Field("gunFuHitDownScriptableObject");
            Field("parryPrimaryWeaponSCRP");
            Field("parrySecondaryWeaponSCRP");
        });

        Section(ref _showGunFuExecutes, "GunFu — Executes", () =>
        {
            Field("gunFuExecute_Single_Secondary_Dodge_ScriptableObject_I");
            Field("gunFuExecute_Single_Secondary_ScriptableObject_I");
            Field("gunFuExecute_Single_Primary_ScriptableObject_II");
            Field("gunFuExecute_Single_Primary_Dodge_ScriptableObject_I");
            Field("gunFu_Single_Execute_OnGround");
        });

        Section(ref _showDebug, "Debug", () =>
        {
            Field("debugIsIFrame");
            Field("isPullTriggerCommand");
        });

        EditorGUILayout.Space(4);

        Section(ref _showOthers, "Others (Base Class)", () =>
        {
            DrawPropertiesExcluding(serializedObject, _organizedFields);
        });

        serializedObject.ApplyModifiedProperties();
    }

    private void Section(ref bool foldout, string label, System.Action drawContent)
    {
        foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, label);
        if (foldout)
        {
            EditorGUI.indentLevel++;
            drawContent();
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(2);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void Field(string propertyName)
    {
        SerializedProperty prop = serializedObject.FindProperty(propertyName);
        if (prop != null)
            EditorGUILayout.PropertyField(prop, true);
    }
}
