#if UNITY_EDITOR
// https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
using Assets.Scripts.Controller;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeedController))]
public class SeedControllerDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Drawing the initial label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float spacing = 5, length = 150;

        var labelSeed = new Rect(position.x, position.y - 20, length, position.height);
        var seed = new Rect(position.x, position.y, length, position.height);
        var labelRandom = new Rect(position.x + (length + spacing), position.y - 20, length, position.height);
        var random = new Rect(position.x + (length + spacing), position.y, length, position.height);

        EditorGUI.LabelField(labelSeed, "Seed");
        EditorGUI.LabelField(labelRandom, "Random Seed");

        EditorGUI.PropertyField(seed, property.FindPropertyRelative("seed"), GUIContent.none);
        EditorGUI.PropertyField(random, property.FindPropertyRelative("useRandom"), GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}

// https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
[CustomPropertyDrawer(typeof(LevelControl))]
public class LevelControllerDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        // Drawing the initial label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float spacing = 5, length = 100;

        var prev = new Rect(position.x, position.y, length, position.height);
        var reg = new Rect(position.x + (length + spacing), position.y, length, position.height);
        var next = new Rect(position.x + ((length + spacing) * 2), position.y, length, position.height);

        EditorGUI.PropertyField(prev, property.FindPropertyRelative("prevLevel"), new GUIContent("Prev Level"));
        EditorGUI.PropertyField(reg, property.FindPropertyRelative("regenLevel"), new GUIContent("Regen"));
        EditorGUI.PropertyField(next, property.FindPropertyRelative("nextLevel"), new GUIContent("Next Level"));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
#endif