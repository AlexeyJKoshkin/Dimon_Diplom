using ShutEye.EditorsScripts;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SAction), true)]
public class SActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty properties, GUIContent label)
    {
        if (properties.FindPropertyRelative("_returnTypeData").arraySize == 0 ||
            properties.FindPropertyRelative("_paramsTypeData").arraySize == 0)
            return;
        EditorGUI.IndentedRect(pos);
        SerializedProperty showProperty = properties.FindPropertyRelative("Show");
        var height = showProperty.boolValue ? pos.height * 0.33f : pos.height;
        showProperty.boolValue = EditorGUI.Foldout(new Rect(pos.x, pos.y, pos.width, height), showProperty.boolValue, new GUIContent() { text = "SAction Show" });
        if (showProperty.boolValue)
        {
            SerializedProperty targetProperty = properties.FindPropertyRelative("_target");

            SerializedProperty methodNameProperty = properties.FindPropertyRelative("MethodName");
            SerializedProperty candidateNamesProperty = properties.FindPropertyRelative("candidatesMethods");
            SerializedProperty NameProperty = properties.FindPropertyRelative("InspectorName");
            SerializedProperty indexProperty = properties.FindPropertyRelative("index");

            // polulate method candidate names
            string[] methodCandidateNames = new string[0];

            pos.y += pos.height * 0.33f;
            EditorGUI.BeginChangeCheck(); // if target changes we need to repopulate the candidate method lists
            {
                GUIContent content = new GUIContent(NameProperty.stringValue);
                // select target
                EditorGUI.LabelField(new Rect(pos.x, pos.y, 70, pos.height * 0.33f), content);
                targetProperty.objectReferenceValue =
                    EditorGUI.ObjectField(new Rect(pos.x + 70, pos.y, 150, pos.height * 0.33f),
                        targetProperty.objectReferenceValue, typeof(UnityEngine.Object), true);

                if (targetProperty.objectReferenceValue == null)
                {
                    return; // null objects have no methods - don't continue
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                // lets do some reflection work -> search, filter, collect candidate methods..
                methodCandidateNames = RepopulateCandidateList(targetProperty, candidateNamesProperty, indexProperty,
                    properties);
            }
            else
            {
                methodCandidateNames = new string[candidateNamesProperty.arraySize];

                int i = 0;
                foreach (SerializedProperty element in candidateNamesProperty)
                {
                    methodCandidateNames[i++] = element.stringValue;
                }
            }
            //pos.y += pos.height * 0.33f;

            EditorGUI.LabelField(new Rect(pos.x, pos.y + pos.height * 0.33f, 70, pos.height * 0.33f), "Method :");
            // place holder when no candidates are available
            if (methodCandidateNames.Length == 0)
            {
                return; // no names no game
            }

            //  EditorGUI.LabelField(new Rect(pos.x + 140, pos.y, 70, pos.height), "Method :");
            // select method from candidates
            indexProperty.intValue = EditorGUI.Popup(
                new Rect(pos.x + 70, pos.y + pos.height * 0.33f, 150, pos.height * 0.33f),
                indexProperty.intValue,
                methodCandidateNames
                );

            methodNameProperty.stringValue = methodCandidateNames[indexProperty.intValue];
            // EditorGUI.indentLevel--;
            // EditorGUILayout.EndVertical();
        }
    }

    public string[] RepopulateCandidateList(SerializedProperty targetProperty, SerializedProperty candidateNamesProperty,
                                            SerializedProperty indexProperty, SerializedProperty propepty)
    {
        System.Type type = targetProperty.objectReferenceValue.GetType();
        if (targetProperty.objectReferenceValue is GameObject)
        {
            var obj = targetProperty.objectReferenceValue as GameObject;
            var script = obj.GetComponent<MonoBehaviour>();
            targetProperty.objectReferenceValue = script;
            type = script.GetType();
        }
        System.Type[] paramTypes = propepty.Deserilase<Type[]>("_paramsTypeData");
        System.Type returnType = propepty.Deserilase<Type>("_returnTypeData");
        IList<MemberInfo> candidateList = new List<MemberInfo>();
        string[] candidateNames;
        int i = 0;
        Debug.Log("Candidate Criteria:");
        Debug.Log("\treturn type:" + returnType.ToString());
        Debug.Log("\tparam count:" + paramTypes.Length);
        foreach (System.Type paramType in paramTypes)
            Debug.Log("\t\t" + paramType.ToString());

        type.FindMembers(
            MemberTypes.Method,
            BindingFlags.Instance | BindingFlags.Public,
            (member, criteria) =>
            {
                //  Debug.Log("matching " + member.Name);
                MethodInfo method;
                if ((method = type.GetMethod(member.Name, paramTypes)) != null && method.ReturnType == returnType)
                {
                    candidateList.Add(method);
                    return true;
                }
                return false;
            },
            null
        );

        // clear/resize/initialize storage containers
        candidateNamesProperty.ClearArray();
        candidateNamesProperty.arraySize = candidateList.Count;
        candidateNames = new string[candidateList.Count];

        // assign storage containers
        i = 0;
        foreach (SerializedProperty element in candidateNamesProperty)
        {
            element.stringValue = candidateNames[i] = candidateList[i++].Name;
        }

        // reset popup index
        indexProperty.intValue = 0;

        return candidateNames;
    }

    //public System.Type returnType
    //{
    //    get { return attribute != null ? (attribute as SActionHelperAttribute).returnType : typeof(void); }
    //}

    //public System.Type[] paramTypes
    //{
    //    get
    //    {
    //        return (attribute != null && (attribute as SActionHelperAttribute).paramTypes != null) ? (attribute as SActionHelperAttribute).paramTypes : new System.Type[0];
    //    }
    //}

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty showProperty = property.FindPropertyRelative("Show");
        float n = showProperty.boolValue ? 3 : 1;
        return base.GetPropertyHeight(property, label) * n;
    }
}