#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerConstrainAnimationManager))]
public class PlayerConstrainAnimationManagerEditor : Editor
{
    private bool showGeneral = true;
    private bool showConstraintComponents = true;
    private bool showBodyADS = true;
    private bool showLean = true;
    private bool showRightHandIK = true;
    private bool showLeftHandIK = true;
    private bool showLegs = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showGeneral = DrawSection("General", showGeneral, DrawGeneral);
        EditorGUILayout.Space(4);
        showConstraintComponents = DrawSection("Constraint Components", showConstraintComponents, DrawConstraintComponents);
        EditorGUILayout.Space(4);
        showBodyADS = DrawSection("Body ADS ScriptableObjects", showBodyADS, DrawBodyADS);
        EditorGUILayout.Space(4);
        showLean = DrawSection("Lean ScriptableObjects", showLean, DrawLean);
        EditorGUILayout.Space(4);
        showRightHandIK = DrawSection("Right Hand IK ScriptableObjects", showRightHandIK, DrawRightHandIK);
        EditorGUILayout.Space(4);
        showLeftHandIK = DrawSection("Left Hand IK ScriptableObjects", showLeftHandIK, DrawLeftHandIK);
        EditorGUILayout.Space(4);
        showLegs = DrawSection("Legs ScriptableObjects", showLegs, DrawLegs);

        serializedObject.ApplyModifiedProperties();
    }

    private bool DrawSection(string label, bool foldout, System.Action drawContent)
    {
        foldout = EditorGUILayout.Foldout(foldout, label, true, EditorStyles.foldoutHeader);
        if (foldout)
        {
            EditorGUI.indentLevel++;
            drawContent();
            EditorGUI.indentLevel--;
        }
        return foldout;
    }

    private void DrawProperty(string fieldName)
    {
        SerializedProperty prop = serializedObject.FindProperty(fieldName);
        if (prop != null)
            EditorGUILayout.PropertyField(prop, true);
    }

    private void DrawGeneral()
    {
        DrawProperty("rig");
        DrawProperty("rigBuilder");
        DrawProperty("player");
        DrawProperty("playerAnimationManager");
        DrawProperty("aimConstrainPositionReference");
        DrawProperty("maxCastDistacne");
        DrawProperty("minCastDisTance");
        DrawProperty("castCollideMask");
    }

    private void DrawConstraintComponents()
    {
        DrawProperty("bodyRotateConstraintManager");
        DrawProperty("leaningRotation");
        DrawProperty("leftHandConstraintManager");
        DrawProperty("rightHandIKConstriantManager");
        DrawProperty("legsConstraintManager");
        DrawProperty("headLookConstraintManager");
    }

    private void DrawBodyADS()
    {
        DrawProperty("body_ADS_Prone_Constrain_SCRP");
        DrawProperty("quickSwitchAimSplineLookConstrainScriptableObject");
        DrawProperty("standPistolAimSplineLookConstrainScriptableObject");
        DrawProperty("standPistolAim_CAR_SplineLookConstrainScriptableObject");
        DrawProperty("standRifleAimSplineLookConstrainScriptableObject");
        DrawProperty("standRifleAim_CAR_SplineLookConstrainScriptableObject");
    }

    private void DrawLean()
    {
        DrawProperty("quickSwitchlLeaningConstrainScriptableObject");
        DrawProperty("pistolLeaningConstrainScriptableObject");
        DrawProperty("pistolLeaning_CAR_ConstrainScriptableObject");
        DrawProperty("rifileLeaningConstrainScriptableObject");
        DrawProperty("rifileLeaning_CAR_ConstrainScriptableObject");
    }

    private void DrawRightHandIK()
    {
        DrawProperty("rightHand_AimDownSight_HumanShield_Primary_SCRP");
        DrawProperty("rightHand_AimDownSight_HumanShield_Secondary_SCRP");
        DrawProperty("rightHand_AimDownSight_Restrain_Primary_SCRP");
        DrawProperty("rightHand_AimDownSight_Restrain_Secondary_SCRP");
        DrawProperty("rightHand_AimDownSight_ProneUp_PrimaryWeapon_SCRP");
        DrawProperty("rightHand_AimDownSight_ProneUp_SecondaryWeapon_SCRP");
        DrawProperty("rightHand_AimDownSight_ProneDown_PrimaryWeapon_SCRP");
        DrawProperty("rightHand_AimDownSight_ProneDown_SecondaryWeapon_SCRP");
        DrawProperty("rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP");
        DrawProperty("rightHand_Target_AimDownSight_PrimaryWeapon_SCRP");
        DrawProperty("rightHand_AimDownSight_QuickSwitch_SCRP");
        DrawProperty("rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP");
        DrawProperty("rightHand_Target_AimDownSight_SecondaryWeapon_SCRP");
    }

    private void DrawLeftHandIK()
    {
        DrawProperty("lowReadyProne_LeftHand_IK_ConstrainSCRP");
        DrawProperty("leftHandIK_QuickSwitch_SCRP");
        DrawProperty("primaryWeaponGripLeftHandScrp");
        DrawProperty("secondaryWeaponGripLeftHandScrp");
        DrawProperty("lowReadyWeaponGripLeftHandScrp");
    }

    private void DrawLegs()
    {
        DrawProperty("proneLegsBlendingConstrainSCRP");
        DrawProperty("diveStallLegsBlendingConstrainSCRP");
    }
}
#endif
