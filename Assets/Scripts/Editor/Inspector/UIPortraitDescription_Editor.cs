using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(UIPortraitDescription))]
public class UIPortraitDescription_Editor : Editor
{
    private GUIStyle _descriptionStyle;
    private GUILayoutOption[] _descriptionLayout;

    private void OnEnable()
    {
        _descriptionLayout = new GUILayoutOption[]
        {
            GUILayout.ExpandHeight(true),
            GUILayout.MinHeight(128.0f)
        };

        _descriptionStyle = new GUIStyle(EditorStyles.textField);
        _descriptionStyle.wordWrap = true;
    }

    public override void OnInspectorGUI()
    {
        UIPortraitDescription d = (target as UIPortraitDescription);

        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.LabelField("Character Description: ");

        EditorGUI.BeginChangeCheck();
        string newDesc = EditorGUILayout.TextArea(d.description, _descriptionStyle, _descriptionLayout);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Character Portrait Description");
            d.description = newDesc; 
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);

    }
}
