using ShutEye.EditorsScripts.ViewerData;
using System;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts
{
    public class SettingMiniHelperWindow : EditorWindow
    {
        protected Action OnCloseEvent;

        private Rect _oldSize;

        protected GUISkin _currentSkin;

        private AreaSizesHelperSettingsWindow _area;

        /// <summary>
        /// Вьювер сущности
        /// </summary>
        protected BaseViewer _dataViewer;

        private Vector2 _scrollPosMainWindow;

        public string NameObject { get; private set; }

        protected virtual void OnEnable()
        {
            this.minSize = new Vector2(600, 600);
            this.titleContent = new GUIContent() { text = "Set Setings " + NameObject };
            _area = new AreaSizesHelperSettingsWindow(this);
            _oldSize = position;
            _currentSkin = BaseEditorStatic.GetSkinByName();
        }

        protected virtual void OnGUI()
        {
            if (_oldSize != position)
            {
                _oldSize = position;
                _area = new AreaSizesHelperSettingsWindow(this);
                Repaint();
            }

            DrawHeader(_area.HeaderArea);
            DrawCurrentElement(_area.SettingsArea);
        }

        public static void ShowSettings<T>(T obj, string title) where T : DataObject
        {
            var window = GetWindow<SettingMiniHelperWindow>("Settings " + title, true);
            window.NameObject = typeof(T).Name;
            window._dataViewer = BaseEditorStatic.GetDataViewer<T>();
            window._dataViewer.RefreshWindow += window.Repaint;
            window._dataViewer.SetCurrent(obj);
            window._area = new AreaSizesHelperSettingsWindow(window);
            window._oldSize = window.position;
            window.OnCloseEvent += () =>
            {
                window._dataViewer.PrepareForSave();
                BaseEditorStatic.DataStorager.Update(window._dataViewer.GetCurrent<T>());
            };
            window.ShowPopup();
        }

        private void DrawCurrentElement(Rect areaSettingsArea)
        {
            GUILayout.BeginArea(areaSettingsArea);
            {
                _scrollPosMainWindow = GUILayout.BeginScrollView(_scrollPosMainWindow, false, false,
                    GUILayout.ExpandHeight(true), GUILayout.Height(_area.SettingsArea.height - 2));
                if (_dataViewer.CanShow)
                    _dataViewer.ShowCurrent();
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DrawHeader(Rect areaHeaderArea)
        {
            GUILayout.BeginArea(areaHeaderArea);
            {
                if (GUILayout.Button("Save"))
                {
                    OnCloseEvent.Invoke();
                    //_dataViewer.SetCurrent(null);
                    Close();
                }
            }
            GUILayout.EndArea();
        }
    }
}