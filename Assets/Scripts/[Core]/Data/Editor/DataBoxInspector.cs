using ShutEye.Data;
using ShutEye.EditorsScripts.ViewerData;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts.CustomInspectors
{
    [CustomEditor(typeof(DataBox), true)]
    public class DataBoxInspector : UnityEditor.Editor
    {
        protected DataBox CurrentBox { get { return target as DataBox; } }

        private BaseViewer _viewerEntity;

        private BaseFilterViewer _filterViewer;

        private bool Edit = false;

        private void Awake()
        {
            _viewerEntity = BaseEditorStatic.GetDataViewer(CurrentBox.ObjectType);
            _viewerEntity.RefreshWindow += this.Repaint;
            _filterViewer = BaseEditorStatic.GetDataFilter(CurrentBox.ObjectType);
            _filterViewer.ChooseItemShow += item => _viewerEntity.SetCurrent(item);
            _filterViewer.RefreshWindow += this.Repaint;

            ReloadData();
        }

        //      protected abstract BaseEditor<T> GetViewer ();

        //protected abstract ChooseEntetyFromListHelper<T> GetChoosenWindow ();

        //protected virtual BaseFilterViewer<T> GetFilter ()
        //{
        //	return new BaseFilterViewer<T> ();
        //}

        private void ReloadData()
        {
            CurrentBox.Reload();
            _filterViewer.InitData(CurrentBox.ArrayListData);
            this.Repaint();
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();

            Edit = EditorGUILayout.Toggle(Edit);
            if (!Edit)
                EditorGUILayout.LabelField(string.Format("{0} Count {1}", CurrentBox.ObjectType.Name, CurrentBox.Count));
        }

        public override void OnInspectorGUI()
        {
            if (_viewerEntity == null)
            {
                base.OnInspectorGUI();
                return;
            }
            if (!Edit)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    AddRemoveChoose();
                }
                EditorGUILayout.EndHorizontal();

                _filterViewer.DrawFilter();

                if (_viewerEntity.CanShow)
                {
                    _viewerEntity.ShowCurrent();
                }
                else
                    EditorGUILayout.LabelField("EMTY");

                DrawSaveBtn();
            }
            else
            {
                CurrentBox.SerializedCollection = EditorGUILayout.TextArea(CurrentBox.SerializedCollection);
                DrawSaveBtn();
            }
        }

        private void DrawSaveBtn()
        {
            if (GUILayout.Button("Save"))
            {
                _viewerEntity.PrepareForSave();
                CurrentBox.OnSave();
                SaveCurrentAsset();
            }
        }

        private void AddRemoveChoose()
        {
            if (GUILayout.Button("Add"))
            {
                CurrentBox.CreateNew();

                SaveCurrentAsset();
                CurrentBox.OnBeforeSerialize();
                _filterViewer.SetLast();
            }

            if (GUILayout.Button("Del "))
            {
                CurrentBox.Remove(_viewerEntity.GetCurrent().Id);
                CurrentBox.OnBeforeSerialize();
                SaveCurrentAsset();
            }
            _filterViewer.DrawSearch();

            if (GUILayout.Button("Ser "))
            {
                CurrentBox.OnBeforeSerialize();
                CurrentBox.SaveToFile();
            }

            if (GUILayout.Button("DESer "))
            {
                CurrentBox.LoadFromFile();
                CurrentBox.Reload();
                this.Repaint();
            }
        }

        private void SaveCurrentAsset()
        {
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            ReloadData();
        }
    }
}