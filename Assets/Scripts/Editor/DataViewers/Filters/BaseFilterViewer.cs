using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts.ViewerData
{
    public abstract class BaseFilterViewer : BaseEditorStatic
    {
        public event Action<DataObject> ChooseItemShow;

        protected void InvokeChoose(DataObject res)
        {
            if (ChooseItemShow != null)
                ChooseItemShow.Invoke(res);
        }

        public abstract void DrawFilter();

        public abstract void DrawSearch();

        public abstract BaseFilterViewer InitData(ArrayList objects);

        public abstract void SetLast();
    }

    public class BaseFilterViewer<T> : BaseFilterViewer where T : DataObject
    {
        /// <summary>
        /// все предметы данного типа
        /// </summary>
        protected IList<T> ItemsCollections;

        protected List<T> FilterCollections = new List<T>();

        /// <summary>
        /// данные для поиска
        /// </summary>
        private string[] _searchInfo;

        private int _currentIndex;

        private string[] _showNames;

        private string searchPattern;

        private bool _showFilter = false;

        public BaseFilterViewer()
        {
            _currentIndex = -1;
        }

        public BaseFilterViewer(ArrayList objects)
        {
            InitData(objects);
        }

        public BaseFilterViewer(IEnumerable<T> items)
        {
            InitData(items);
        }

        public override void SetLast()
        {
            _currentIndex = FilterCollections.Count - 1;
            InvokeChoose(FilterCollections.Find(o => o.Name == _showNames[_currentIndex]));
        }

        public override BaseFilterViewer InitData(ArrayList items)
        {
            return InitData(items.ToArray().Cast<T>().ToList());
        }

        public BaseFilterViewer InitData(IEnumerable<T> items)
        {
            if (items == null)
            {
                Debug.LogWarning("Null Collection " + typeof(T).Name);
                return this;
            }
            ItemsCollections = items.ToList();
            FilterCollections = items.ToList();
            PrepareSearchInformation();
            return this;
        }

        protected void PrepareSearchInformation()
        {
            if (FilterCollections == null)
            {
                Debug.LogWarning("Null Collection " + typeof(T).Name);
            }
            _searchInfo = new string[FilterCollections.Count];
            _showNames = FilterCollections.Select(i => i.Name).ToArray();
            for (var i = 0; i < FilterCollections.Count; i++)
            {
                _searchInfo[i] = GetInfoForSearch(FilterCollections[i]);
            }

            if (_currentIndex == -1 || _currentIndex >= _showNames.Length)
            {
                _currentIndex = FilterCollections.Count - 1;
            }
            var res = FilterCollections.Count > 0
                ? FilterCollections.Find(o => o.Name == _showNames[_currentIndex])
                : null;
            InvokeChoose(res);
        }

        protected virtual string GetInfoForSearch(T item)
        {
            return item.Name.ToLower();
        }

        public override void DrawFilter()
        {
            EditorGUILayout.BeginHorizontal();
            {
                _showFilter = EditorGUILayout.Toggle(_showFilter, GUILayout.Width(20));
                EditorGUILayout.LabelField("Filter", GUILayout.Width(40));

                EditorGUI.BeginChangeCheck();
                _currentIndex = EditorGUILayout.Popup(_currentIndex, _showNames);
                if (EditorGUI.EndChangeCheck())
                {
                    InvokeChoose(FilterCollections.Find(o => o.Name == _showNames[_currentIndex]));
                }
            }
            EditorGUILayout.EndHorizontal();

            if (_showFilter)
            {
                EditorGUILayout.BeginVertical("Box");
                ShowFilter();
                EditorGUILayout.EndVertical();
            }
        }

        protected virtual void ShowFilter()
        {
            EditorGUILayout.LabelField("Not Implemented this type");
        }

        public override void DrawSearch()
        {
            EditorGUILayout.LabelField("Search ", GUILayout.Width(45));
            EditorGUI.BeginChangeCheck();
            searchPattern = EditorGUILayout.TextField(searchPattern);
            if (EditorGUI.EndChangeCheck())
            {
                if (searchPattern.Length > 3)
                {
                    Search(searchPattern.ToLower());
                    PrepareSearchInformation();
                    return;
                }
                if (searchPattern.Length == 0)
                {
                    FilterCollections = new List<T>(ItemsCollections);
                    PrepareSearchInformation();
                }
            }
        }

        private void Search(string pattern)
        {
            var newFilterList = new List<T>();
            for (var i = 0; i < _searchInfo.Length; i++)
            {
                if (_searchInfo[i].Contains(pattern))
                {
                    newFilterList.Add(FilterCollections[i]);
                }
            }
            FilterCollections = newFilterList;
        }
    }
}