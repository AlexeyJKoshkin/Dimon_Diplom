using ShutEye.EditorsScripts.ViewerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts
{
    public static class EditorExtensionsShutEye
    {
        public static T Deserilase<T>(this SerializedProperty properties, string key) where T : class
        {
            // Profiler.BeginSample("Desserialize");
            var data = properties.FindPropertyRelative(key);

            if (data == null || !data.isArray || data.arraySize == 0) return default(T);
            var bytes = new byte[data.arraySize];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)data.GetArrayElementAtIndex(i).intValue;
            }
            //    Profiler.EndSample();
            return AssemblyReflectionHelper.Deserialse<T>(bytes);
        }

        public static BaseFilterViewer InitData<T>(this BaseFilterViewer filter, IEnumerable<T> enumerable) where T : DataObject
        {
            var _filter = filter as BaseFilterViewer<T>;
            if (enumerable == null || _filter == null)
            {
                Debug.LogWarningFormat("enumerable {0} Collection {1} {2}", enumerable, _filter, typeof(T).Name);
                return _filter;
            }
            return _filter.InitData(enumerable.ToList());
        }

        private static List<string> layers;
        private static string[] layerNames;

        public static LayerMask LayerMaskField(string label, LayerMask selected)
        {
            if (layers == null)
            {
                layers = new List<string>();
                layerNames = new string[4];
            }
            else
            {
                layers.Clear();
            }

            int emptyLayers = 0;
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);

                if (string.IsNullOrEmpty(layerName))
                {
                    emptyLayers++;
                }
                else
                {
                    //                    for (; emptyLayers > 0; emptyLayers--) layers.Add("Layer " + (i - emptyLayers));
                    layers.Add(layerName);
                }
            }

            if (layerNames.Length != layers.Count)
            {
                layerNames = new string[layers.Count];
            }
            for (int i = 0; i < layerNames.Length; i++) layerNames[i] = layers[i];

            selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);

            return selected;
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <author>
//   HiddenMonk
//   http://answers.unity3d.com/users/496850/hiddenmonk.html
//
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------
public static class SerializedPropertyExtensions
{
    /// <summary>
    /// Get the object the serialized property holds by using reflection
    /// </summary>
    /// <typeparam name="T">The object type that the property contains</typeparam>
    /// <param name="property"></param>
    /// <returns>Returns the object type T if it is the type the property actually contains</returns>
    public static T GetValue<T>(this SerializedProperty property)
    {
        return GetNestedObject<T>(property.propertyPath, GetSerializedPropertyRootComponent(property));
    }

    /// <summary>
    /// Set the value of a field of the property with the type T
    /// </summary>
    /// <typeparam name="T">The type of the field that is set</typeparam>
    /// <param name="property">The serialized property that should be set</param>
    /// <param name="value">The new value for the specified property</param>
    /// <returns>Returns if the operation was successful or failed</returns>
    public static bool SetValue<T>(this SerializedProperty property, T value)
    {
        object obj = GetSerializedPropertyRootComponent(property);
        //Iterate to parent object of the value, necessary if it is a nested object
        string[] fieldStructure = property.propertyPath.Split('.');
        for (int i = 0; i < fieldStructure.Length - 1; i++)
        {
            obj = GetFieldOrPropertyValue<object>(fieldStructure[i], obj);
        }
        string fieldName = fieldStructure.Last();

        return SetFieldOrPropertyValue(fieldName, obj, value);
    }

    /// <summary>
    /// Get the component of a serialized property
    /// </summary>
    /// <param name="property">The property that is part of the component</param>
    /// <returns>The root component of the property</returns>
    public static UnityEngine.Object GetSerializedPropertyRootComponent(SerializedProperty property)
    {
        return (UnityEngine.Object)property.serializedObject.targetObject;
    }

    /// <summary>
    /// Iterates through objects to handle objects that are nested in the root object
    /// </summary>
    /// <typeparam name="T">The type of the nested object</typeparam>
    /// <param name="path">Path to the object through other properties e.g. PlayerInformation.Health</param>
    /// <param name="obj">The root object from which this path leads to the property</param>
    /// <param name="includeAllBases">Include base classes and interfaces as well</param>
    /// <returns>Returns the nested object casted to the type T</returns>
    public static T GetNestedObject<T>(string path, object obj, bool includeAllBases = false)
    {
        foreach (string part in path.Split('.'))
        {
            obj = GetFieldOrPropertyValue<object>(part, obj, includeAllBases);
        }
        return (T)obj;
    }

    public static T GetFieldOrPropertyValue<T>(string fieldName, object obj, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null) return (T)field.GetValue(obj);

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null) return (T)property.GetValue(obj, null);

        if (includeAllBases)
        {
            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null) return (T)field.GetValue(obj);

                property = type.GetProperty(fieldName, bindings);
                if (property != null) return (T)property.GetValue(obj, null);
            }
        }

        return default(T);
    }

    public static bool SetFieldOrPropertyValue(string fieldName, object obj, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null)
        {
            field.SetValue(obj, value);
            return true;
        }

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);

        if (property != null)
        {
            property.SetValue(obj, value, null);
            return true;
        }

        if (includeAllBases)
        {
            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null)
                {
                    field.SetValue(obj, value);
                    return true;
                }

                property = type.GetProperty(fieldName, bindings);

                if (property != null)
                {
                    property.SetValue(obj, value, null);
                    return true;
                }
            }
        }
        return false;
    }

    public static IEnumerable<Type> GetBaseClassesAndInterfaces(this Type type, bool includeSelf = false)
    {
        List<Type> allTypes = new List<Type>();

        if (includeSelf) allTypes.Add(type);

        if (type.BaseType == typeof(object))
        {
            allTypes.AddRange(type.GetInterfaces());
        }
        else
        {
            allTypes.AddRange(
                    Enumerable
                    .Repeat(type.BaseType, 1)
                    .Concat(type.GetInterfaces())
                    .Concat(type.BaseType.GetBaseClassesAndInterfaces())
                    .Distinct());
        }

        return allTypes;
    }
}