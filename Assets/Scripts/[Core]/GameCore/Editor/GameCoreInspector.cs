using ShutEye.Core;
using ShutEye.Data;
using ShutEye.Data.Provider;
using ShutEye.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameKit.CustomEditors
{
    [CustomEditor(typeof(DiplomCore))]
    public class GameCoreInspector : Editor
    {
        private enum StateGenerate
        {
            None,
            ChooseType,
            Generating,
        }

        public static IDataEditorStorage LoadAllPrividers(IDataEditorStorage storage = null)
        {
            storage = storage ?? new LocalDataBoxStorage();
            AssemblyReflectionHelper.AllDataBox().ForEach(provider =>
               {
                   storage.RegisterProvider(provider);
               });
            return storage;
        }

        private List<DataBox> _allProviders;

        private bool _IsInited = false;

        private SerializedProperty _allProvidersproperty
        {
            get { return serializedObject.FindProperty("_allProviders"); }
        }

        private StateGenerate _state;

        private string NameNewType = string.Empty;
        private List<CodeGenerators> _allGenerators = new List<CodeGenerators>();

        private Type _parentType;

        private Dictionary<int, Type> _dicTypes;

        //List<Type> _allTypes;
        private string[] _namesType;

        private int ChooseIndex = 0;

        private void Init()
        {
            var allTypes = AssemblyReflectionHelper.GetAllTypesInSolution<DataObject>(true).ToList();
            _state = StateGenerate.None;
            _namesType = new string[allTypes.Count];
            _dicTypes = new Dictionary<int, Type>();
            for (var i = 0; i < allTypes.Count; i++)
            {
                _namesType[i] = allTypes[i].Name;
                _dicTypes.Add(i, allTypes[i]);
            }

            _parentType = typeof(DataObject);
            ChooseIndex = _dicTypes.FirstOrDefault(o => o.Value == _parentType).Key;

            _allGenerators = new List<CodeGenerators> { new DataGenerator(), new ProviderGenerator() };
            _IsInited = true;
        }

        private void OnEnable()
        {
            if (!_IsInited || _dicTypes == null)
                Init();
        }

        //private void RefreshProviders()
        //{
        //    _allProviders = new List<DataBox>();
        //    for (var i = 0; i < _allProvidersproperty.arraySize; i++)
        //    {
        //        var provider = _allProvidersproperty.GetArrayElementAtIndex(i).objectReferenceValue as DataBox;
        //        _allProviders.Add(provider);
        //    }
        //}

        private void RefreshView()
        {
            _allProvidersproperty.ClearArray();
            for (var i = 0; i < _allProviders.Count; i++)
            {
                _allProvidersproperty.InsertArrayElementAtIndex(i);
                _allProvidersproperty.GetArrayElementAtIndex(i).objectReferenceValue = _allProviders[i];
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
        }

        public override void OnInspectorGUI()
        {
            switch (_state)
            {
                case StateGenerate.None:
                    {
                        if (GUILayout.Button("Create"))
                        {
                            _state = StateGenerate.ChooseType;
                        }
                        if (GUILayout.Button("Check"))
                        {
                            AssemblyReflectionHelper.GetAllTypesInSolution<DataBox>().Where(t => !t.IsAbstract && !t.IsGenericType).ForEach(t =>
                            {
                                CreateAsset(t);
                            });
                            _allProviders = AssemblyReflectionHelper.AllDataBox().ToList();
                            RefreshView();
                        }
                        break;
                    }
                case StateGenerate.ChooseType:
                    EditorGUILayout.BeginVertical();
                    DrawChooseType();
                    EditorGUILayout.EndVertical();
                    break;

                case StateGenerate.Generating:
                    {
                        _allGenerators.ForEach(g => g.Generate());
                        AssetDatabase.Refresh();
                        _state = StateGenerate.None;
                        break;
                    }
            }

            base.OnInspectorGUI();

            if (GUILayout.Button("Set generate settings"))
            {
                foreach (var dataBox in AssemblyReflectionHelper.AllDataBox())
                {
                    Debug.Log(dataBox.ObjectType.Name);
                }
                //   SetPathesWindow.ShowSetPathWindow();
            }
        }

        private void DrawChooseType()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Add ", GUILayout.Width(30));
                NameNewType = EditorGUILayout.TextField(NameNewType);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Parent Type ", GUILayout.Width(80));
                EditorGUI.BeginChangeCheck();
                ChooseIndex = EditorGUILayout.Popup(ChooseIndex, _namesType, GUILayout.Width(80));
                if (EditorGUI.EndChangeCheck())
                {
                    _parentType = _dicTypes[ChooseIndex];
                }
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate"))
            {
                Debug.Log("Check Enter");
                if (NameNewType.Length < 3)
                {
                    Debug.LogWarning("Less than 3 ");
                    return;
                }

                if (_namesType.Contains(NameNewType))
                {
                    Debug.LogWarning("Same Name ");
                    return;
                }

                _allGenerators.ForEach(g => g.SetType(NameNewType, _parentType));
                _state = StateGenerate.Generating;
            }
        }

        public const string PATH_DATABOX = "Assets/DATA/";
        public const string EXTENSION = ".asset";

        public static void CreateAsset(string type)
        {
            var asset = ScriptableObject.CreateInstance(type);
            string assetPathAndName = string.Format("{0}{1}{2}", PATH_DATABOX, type, EXTENSION);
            if (File.Exists(assetPathAndName)) return;
            Debug.Log(assetPathAndName);
            //assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (assetPathAndName);
            //Debug.Log (assetPathAndName);
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateAsset<T>() where T : ScriptableObject
        {
            string assetPathAndName = string.Format("{0}{1}{2}", PATH_DATABOX, typeof(T).Name, EXTENSION);
            if (File.Exists(assetPathAndName)) return;
            T asset = ScriptableObject.CreateInstance<T>();
            SaveAsset(asset, assetPathAndName);
        }

        private static void CreateAsset(Type type)
        {
            string assetPathAndName = string.Format("{0}{1}{2}", PATH_DATABOX, type.Name, EXTENSION);
            if (File.Exists(assetPathAndName)) return;
            var asset = ScriptableObject.CreateInstance(type);
            SaveAsset(asset, assetPathAndName);
        }

        private static void SaveAsset(ScriptableObject dataBox, string assetPathAndName)
        {
            Debug.Log(assetPathAndName);
            //assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (assetPathAndName);
            //Debug.Log (assetPathAndName);
            AssetDatabase.CreateAsset(dataBox, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}