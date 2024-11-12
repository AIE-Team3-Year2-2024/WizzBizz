using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VectorRangeAttribute))]
public class VectorRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VectorRangeAttribute range = attribute as VectorRangeAttribute;
        if (property.propertyType == SerializedPropertyType.Vector4)
        {
            Vector4 n = EditorGUI.Vector4Field(position, label, property.vector4Value);
            n.x = Mathf.Clamp(n.x, range.min4.x, range.max4.x);
            n.y = Mathf.Clamp(n.y, range.min4.y, range.max4.y);
            n.z = Mathf.Clamp(n.z, range.min4.z, range.max4.z);
            n.w = Mathf.Clamp(n.w, range.min4.w, range.max4.w);
            property.vector4Value = n;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        else if (property.propertyType == SerializedPropertyType.Vector3)
        {
            Vector3 n = EditorGUI.Vector3Field(position, label, property.vector3Value);
            n.x = Mathf.Clamp(n.x, range.min3.x, range.max3.x);
            n.y = Mathf.Clamp(n.y, range.min3.y, range.max3.y);
            n.z = Mathf.Clamp(n.z, range.min3.z, range.max3.z);
            property.vector3Value = n;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        else if (property.propertyType == SerializedPropertyType.Vector2)
        {
            Vector2 n = EditorGUI.Vector2Field(position, label, property.vector2Value);
            n.x = Mathf.Clamp(n.x, range.min2.x, range.max2.x);
            n.y = Mathf.Clamp(n.y, range.min2.y, range.max2.y);
            property.vector2Value = n;
            property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            Debug.LogError("Property isn't a vector type. Integer vector types are not yet implemented.");
        }
    }
}
