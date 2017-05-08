using ShutEye.EditorsScripts.ViewerData;
using ShutEye.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts
{
    public class ChooseEntetyFromListHelper : EditorWindow
    {
        public event Action<DataObject> ChooseDataObjectEvent;

        private AreaSizesChooseWindow _area;

        private Rect _oldSize;

        protected GUISkin _currentSkin;

        /// <summary>
        /// Вьювер сущности
        /// </summary>
        protected BaseViewer ItemEditorViewer;

        protected BaseFilterViewer FilterViewer;

        private Vector2 _scrollPosAllElemntsWindow;
        private Vector2 _scrollPosMainWindow;

        private Texture2D _iconSelect;
        // private Texture2D _iconEdit;

        private Dictionary<string, DataObject> _dicItems;

        private string[] _btnNames;
        private int _currentIndex;

        protected virtual void OnEnable()
        {
            this.minSize = new Vector2(600, 600);
            this.titleContent = new GUIContent() { text = "Chose Entity" };
            _iconSelect = AssetDatabase.LoadAssetAtPath<Texture2D>(STRH.Editorstr.PathToSelectBtn);
            _currentSkin = BaseEditorStatic.GetSkinByName();
        }

        public static void ShowChoose<T>(Action<T> onChoose) where T : DataObject
        {
            var window = GetWindow<ChooseEntetyFromListHelper>("Choose Data", true).InitWindow<T>();
            window.ChooseDataObjectEvent += res => onChoose.Invoke(res as T);
            window.ShowPopup();
        }

        public static void ShowChoose<T>(Action<DataObject> onChoose) where T : DataObject
        {
            var window = GetWindow<ChooseEntetyFromListHelper>("Choose Data", true).InitWindow<T>();
            window.ChooseDataObjectEvent += onChoose;
            window.ShowPopup();
        }

        public ChooseEntetyFromListHelper InitWindow<T>() where T : DataObject
        {
            ItemEditorViewer = BaseEditorStatic.GetDataViewer<T>();
            FilterViewer = BaseEditorStatic.GetDataFilter<T>();
            var listItems = BaseEditorStatic.DataStorager.All<T>().ToList();

            FilterViewer.InitData(new ArrayList(listItems));
            _dicItems = new Dictionary<string, DataObject>();
            listItems.ForEach(i => _dicItems.Add(string.Format("{0}-{1}", i.Id, i.Name), i));
            _btnNames = _dicItems.Keys.ToArray();

            if (ItemEditorViewer.CanShow)
            {
                for (var i = 0; i < _btnNames.Length; i++)
                {
                    if (_btnNames[i] == string.Format("{0}-{1}", ItemEditorViewer.GetCurrent().Id, ItemEditorViewer.CurrentName))
                    {
                        _currentIndex = i;
                        break;
                    }
                }
            }
            else
            {
                _currentIndex = 0;
                ItemEditorViewer.SetCurrent(_dicItems[_btnNames[_currentIndex]]);
            }
            return this;
        }

        protected void InitBtnsName<T>(IEnumerable<T> items) where T : DataObject
        {
            if (items == null) return;
            _dicItems = new Dictionary<string, DataObject>();
            items.ForEach(i => _dicItems.Add(string.Format("{0}-{1}", i.Id, i.Name), i));
            _btnNames = _dicItems.Keys.ToArray();
        }

        protected virtual void OnGUI()
        {
            if (_oldSize != position)
            {
                _oldSize = position;
                _area = new AreaSizesChooseWindow(this);
                Repaint();
            }

            DrawBtnsElements(_area.BtnsArea);

            DrawAllElements(_area.AllElemntsRect);

            GUILayout.BeginArea(_area.FilterRect);
            {
                FilterViewer.DrawSearch();
                FilterViewer.DrawFilter();
            }

            GUILayout.EndArea();
            DrawCurrentElement(_area.MainRect);
        }

        protected void DrawAllElements(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                _scrollPosAllElemntsWindow = GUILayout.BeginScrollView(_scrollPosAllElemntsWindow, false, false, GUILayout.ExpandHeight(true), GUILayout.Height(rect.height - 2));
                EditorGUI.BeginChangeCheck();
                _currentIndex = GUILayout.SelectionGrid(_currentIndex, _btnNames, 1);
                if (EditorGUI.EndChangeCheck())
                {
                    ItemEditorViewer.SetCurrent(_dicItems[_btnNames[_currentIndex]]);
                    //  FilterViewer.SetCurrent(_dicItems[_btnNames[_currentIndex]]);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        protected void DrawBtnsElements(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginHorizontal();
                if (ItemEditorViewer.CanShow)
                {
                    GUIContent content = new GUIContent()
                    {
                        image = _iconSelect,
                        text = string.Format("Choose Current id"),
                        tooltip = string.Format("Choose Current")
                    };

                    if (GUILayout.Button(content, GUILayout.Width(170), GUILayout.Height(30)))
                    {
                        if (ChooseDataObjectEvent != null)
                            ChooseDataObjectEvent.Invoke(ItemEditorViewer.GetCurrent());
                        Close();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        protected void DrawCurrentElement(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                _scrollPosMainWindow = GUILayout.BeginScrollView(_scrollPosMainWindow, false, false, GUILayout.ExpandHeight(true), GUILayout.Height(_area.MainRect.height - 2));
                if (ItemEditorViewer.CanShow)
                    ItemEditorViewer.ShowCurrent();
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
    }
}