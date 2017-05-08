using GameKit;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ViewTypeAttribute))]
public class ViewScriptHelperDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        _property.objectReferenceValue = EditorGUI.ObjectField(_position,
                    new GUIContent() { text = this.fieldInfo.Name },
                    _property.objectReferenceValue,
                    ((ViewTypeAttribute)this.attribute).ScripType,
                    true);
    }
}