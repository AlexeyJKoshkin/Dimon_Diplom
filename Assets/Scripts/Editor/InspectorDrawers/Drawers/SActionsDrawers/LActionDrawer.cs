using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LAction), true)]
public class LActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty properties, GUIContent label)
    {
        EditorGUI.IndentedRect(pos);
        SerializedProperty showProperty = properties.FindPropertyRelative("Show");
        var height = showProperty.boolValue ? pos.height * 0.5f : pos.height;

        SerializedProperty methodNameProperty = properties.FindPropertyRelative("MethodName");
        SerializedProperty candidateNamesProperty = properties.FindPropertyRelative("candidatesMethods");
        SerializedProperty indexProperty = properties.FindPropertyRelative("index");

        var isShow = showProperty.boolValue;
        if (!isShow)
            EditorGUI.BeginChangeCheck();
        showProperty.boolValue = EditorGUI.Foldout(new Rect(pos.x, pos.y, pos.width, height), showProperty.boolValue, properties.FindPropertyRelative("InspectorName").stringValue);
        if (!isShow && EditorGUI.EndChangeCheck())
        {
            var old = properties.GetValue<LAction>();
            old.CreateAction();

            candidateNamesProperty.ClearArray();
            for (int i = 0; i < old.candidatesMethods.Length; i++)
            {
                if (old.candidatesMethods[i] == old.MethodName)
                {
                    indexProperty.intValue = i;
                    methodNameProperty.stringValue = old.MethodName;
                }
                candidateNamesProperty.InsertArrayElementAtIndex(i);
                candidateNamesProperty.GetArrayElementAtIndex(i).stringValue = old.candidatesMethods[i];
            }
        }

        if (showProperty.boolValue)
        {
            // if (EditorGUI GUILayout.Button("reset"))
            {
                // ((LAction)properties.objectReferenceValue).Reset(typeof(HandlersLibrary));
            }
            // polulate method candidate names
            string[] methodCandidateNames = new string[0];

            pos.y += pos.height * 0.5f;
            //EditorGUI.BeginChangeCheck(); // if target changes we need to repopulate the candidate method lists
            methodCandidateNames = new string[candidateNamesProperty.arraySize];
            int i = 0;
            foreach (SerializedProperty element in candidateNamesProperty)
            {
                methodCandidateNames[i++] = element.stringValue;
            }
            //pos.y += pos.height * 0.33f;

            EditorGUI.LabelField(new Rect(pos.x, pos.y, 70, pos.height * 0.5f), "Method :");
            // place holder when no candidates are available
            if (methodCandidateNames.Length == 0)
            {
                return; // no names no game
            }

            //  EditorGUI.LabelField(new Rect(pos.x + 140, pos.y, 70, pos.height), "Method :");
            // select method from candidates
            indexProperty.intValue = EditorGUI.Popup(
                new Rect(pos.x + 70, pos.y, 150, pos.height * 0.5f),
                indexProperty.intValue,
                methodCandidateNames
            );
            if (indexProperty.intValue >= methodCandidateNames.Length)
                indexProperty.intValue = 0;
            methodNameProperty.stringValue = methodCandidateNames[indexProperty.intValue];
            // EditorGUI.indentLevel--;
            // EditorGUILayout.EndVertical();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty showProperty = property.FindPropertyRelative("Show");
        float n = showProperty.boolValue ? 2 : 1;
        return base.GetPropertyHeight(property, label) * n;
    }
}