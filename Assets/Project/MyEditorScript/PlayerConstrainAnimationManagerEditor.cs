#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerConstrainAnimationManager))]
public class PlayerConstrainAnimationManagerEditor : Editor
{
    private List<string> _sectionOrder;
    private Dictionary<string, List<string>> _sections;
    private Dictionary<string, bool> _foldouts;

    private void OnEnable()
    {
        BuildSections();
        _foldouts = new Dictionary<string, bool>();
        foreach (var section in _sectionOrder)
            _foldouts[section] = true;
    }

    private void BuildSections()
    {
        _sections = new Dictionary<string, List<string>>();
        _sectionOrder = new List<string>();

        const string GENERAL = "General";
        string currentSection = GENERAL;
        AddSection(GENERAL);

        var type = typeof(PlayerConstrainAnimationManager);
        var iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // moves to m_Script, skip it

        while (iterator.NextVisible(false))
        {
            var fieldInfo = GetFieldRecursive(type, iterator.name);
            if (fieldInfo != null)
            {
                var header = fieldInfo.GetCustomAttribute<HeaderAttribute>();
                if (header != null)
                {
                    currentSection = ExtractSectionName(header.header);
                    AddSection(currentSection);
                }
            }
            _sections[currentSection].Add(iterator.name);
        }
    }

    private void AddSection(string name)
    {
        if (!_sections.ContainsKey(name))
        {
            _sections[name] = new List<string>();
            _sectionOrder.Add(name);
        }
    }

    private static FieldInfo GetFieldRecursive(System.Type type, string fieldName)
    {
        while (type != null)
        {
            var fi = type.GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            if (fi != null) return fi;
            type = type.BaseType;
        }
        return null;
    }

    private static string ExtractSectionName(string header)
    {
        var match = Regex.Match(header, @"^─+\s*(.*?)\s*─*$");
        return match.Success ? match.Groups[1].Value.Trim() : header.Trim();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < _sectionOrder.Count; i++)
        {
            var sectionName = _sectionOrder[i];
            var fields = _sections[sectionName];

            if (fields.Count == 0) continue;

            if (i > 0) EditorGUILayout.Space(4);

            _foldouts[sectionName] = EditorGUILayout.Foldout(
                _foldouts[sectionName], sectionName, true, EditorStyles.foldoutHeader);

            if (_foldouts[sectionName])
            {
                EditorGUI.indentLevel++;
                foreach (var fieldName in fields)
                {
                    var prop = serializedObject.FindProperty(fieldName);
                    if (prop != null)
                        EditorGUILayout.PropertyField(prop, true);
                }
                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
