using System;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts.ViewerData
{
    public abstract class BaseViewer : BaseEditorStatic
    {
        public abstract void SetCurrent(DataObject item);

        public bool CanShow { get { return GetCurrent() != null; } }

        public abstract void ShowCurrent();

        public abstract void PrepareForSave();

        public abstract string CurrentName { get; }

        public abstract DataObject GetCurrent();

        public abstract T GetCurrent<T>() where T : DataObject;
    }

    public class BaseViewer<T> : BaseViewer where T : DataObject
    {
        public bool IsNew { get; protected set; }

        public override string CurrentName
        {
            get { return CurrentItem != null ? CurrentItem.Name : String.Empty; }
        }

        /// <summary>
        /// Текущая редактируемая сущность
        /// </summary>
        public T CurrentItem { get; protected set; }

        public BaseViewer()
        {
            IsNew = false;
        }

        public BaseViewer(T t) : this()
        {
            SetCurrent(t);
        }

        public void SetCurrent(T t)
        {
            CurrentItem = t;
            PrepareCurrentEntity();
        }

        protected virtual void PrepareCurrentEntity()
        {
            if (CurrentItem == null)
            {
                Debug.LogWarningFormat("Null Object {0}", typeof(T).Name);
            }
        }

        public override void PrepareForSave()
        {
            //Debug.Log ("Saving prepare");
        }

        public override void SetCurrent(DataObject item)
        {
            CurrentItem = item as T;
            PrepareCurrentEntity();
        }

        public override DataObject GetCurrent()
        {
            return CurrentItem;
        }

        public override void ShowCurrent()
        {
            DrawIdName(CurrentItem);
        }

        public virtual void DrawIdName(DataObject obj)
        {
            EditorGUILayout.BeginHorizontal("Box");
            {
                GUILayout.Label("ID", GUILayout.Width(30));
                GUILayout.Label(obj.Id.ToString(), GUILayout.Width(30));

                GUILayout.Label("Name", GUILayout.Width(40));
                obj.Name = EditorGUILayout.TextField(obj.Name, GUILayout.MinWidth(60), GUILayout.MaxWidth(300));
                GUILayout.Space(2);
            }
            EditorGUILayout.EndHorizontal();
        }

        public override T1 GetCurrent<T1>()
        {
            if (typeof(T1) == typeof(T))
                return CurrentItem as T1;
            return null;
        }
    }
}