using ShutEye.Core;
using ShutEye.Data;
using ShutEye.Data.Provider;
using ShutEye.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShutEye.EditorsScripts
{
    public abstract partial class BaseEditorStatic
    {
        public const int WIDTH_SLIDER = 70;

        protected static string DefaultPath
        {
            get { return Application.dataPath + "/Resources/"; }
        }

        public const string PREFIX_PATH = "Assets/Resources/";
        public const string PREFAB_EXTENSION = ".prefab";

        public const string SE_NAME_SKIN = "EntityEditorSkin";

        public static IDataEditorStorage DataStorager
        {
            get
            {
                if (_data == null || _data.CountProviders == 0)
                {
                    _data = new LocalDataBoxStorage();
                    AssemblyReflectionHelper.AllDataBox().ForEach(b =>
                      {
                          _data.RegisterProvider(b);
                      });
                }
                return _data;
            }
        }

        private static IDataEditorStorage _data;

        private static IDataEditorStorage _storage;

        protected static readonly GUIStyle ReachTextStyle = new GUIStyle() { richText = true };

        public static GUISkin GetSkinByName(string nameskin = SE_NAME_SKIN)
        {
            var guids = AssetDatabase.FindAssets(String.Format("{0} t:GUISkin", nameskin));
            if (guids.Length > 1)
            {
                Debug.LogWarning("More than one guiskin with name " + nameskin);
            }
            return guids.Length == 1 ? AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(GUISkin)) as GUISkin : null;
        }

        #region Static

        protected static string CheckPathName(string path)
        {
            return String.IsNullOrEmpty(path) ? "Emty" : path;
        }

        protected static int SortColectionIComparer<T>(T o1, T o2) where T : DataObject
        {
            return (o1.Id < o2.Id) ? -1 : 1;
        }

        public static string[] GetArray<T>(params string[] ignore) where T : struct
        {
            if (!typeof(T).IsEnum)
                return new string[] { "NONE" };

            List<string> temp = new List<string>(Enum.GetNames(typeof(T)));

            temp.Insert(0, "All");

            foreach (var name in ignore)
            {
                temp.Remove(name);
            }
            return temp.ToArray();
        }

        public static string CutPath(string path)
        {
            return path.Replace(PREFAB_EXTENSION, String.Empty).
                Replace(PREFIX_PATH, String.Empty);
        }

        public static string DropAreaObject<T2>(Rect drop_area) where T2 : Object
        {
            Event evt = Event.current;
            var result = String.Empty;
            // GUI.Box(drop_area, "");
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return result;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        if (DragAndDrop.objectReferences != null)
                        {
                            var dddd = DragAndDrop.objectReferences.OfType<T2>();
                            foreach (T2 draggedObject in dddd)
                            {
                                result = AssetDatabase.GetAssetPath(draggedObject);
                            }
                        }
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static Transform DropAreaMapObject(Rect drop_area)
        {
            Event evt = Event.current;
            // GUI.Box(drop_area, "");
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return null;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        if (DragAndDrop.objectReferences != null)
                        {
                            var ddd = DragAndDrop.objectReferences.OfType<GameObject>();
                            {
                                var res = ddd.FirstOrDefault();
                                return res == null
                                    ? null : res.transform;
                            }
                        }
                    }
                    return null;

                default:
                    return null;
            }
        }

        public static string DropAreaSprite(Rect drop_area)
        {
            Event evt = Event.current;
            var result = String.Empty;
            // GUI.Box(drop_area, "");
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return result;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        if (DragAndDrop.objectReferences != null)
                        {
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                string fullpath = AssetDatabase.GetAssetPath(draggedObject);
                                if (draggedObject is Sprite)
                                {
                                    result = fullpath.Replace(PREFIX_PATH, String.Empty)
                                        .Replace(Path.GetExtension(fullpath), String.Empty) + ":" +
                                        draggedObject.name;
                                }
                                if (draggedObject is Texture)
                                {
                                    result = fullpath.Replace(PREFIX_PATH, String.Empty)
                                        .Replace(Path.GetExtension(fullpath), String.Empty);
                                }
                            }
                        }
                    }
                    return result;

                default:
                    return result;
            }
        }

        public static Texture2D GetTextureForPreview(string path)
        {
            Sprite sprite = GameCore.LoadSprite(path);
            if (sprite == null)
            {
                return
                    AssetDatabase.LoadAssetAtPath<Texture2D>(STRH.Editorstr.PathToNoPicture);
            }
            if (!path.Contains(":"))
                return sprite.texture;
            Color[] pixels = sprite.texture.GetPixels(
                (int)sprite.rect.x,
                (int)sprite.rect.y,
                (int)sprite.rect.width,
                (int)sprite.rect.height);
            var tex2d_image = new Texture2D(
                (int)sprite.rect.width,
                (int)sprite.rect.height);
            tex2d_image.SetPixels(pixels);
            tex2d_image.Apply();
            return tex2d_image;
        }

        public static Texture2D GetTextureForPrefab(string path)
        {
            var res = AssetPreview.GetAssetPreview(AssetDatabase.LoadAssetAtPath(PREFIX_PATH + path + PREFAB_EXTENSION,
                typeof(GameObject)));

            Thread.Sleep(100);
            return res ?? AssetDatabase.LoadAssetAtPath<Texture2D>(STRH.Editorstr.PathToNoPicture);
        }

        #endregion Static

        public static void DrawBtn(Action OnClickAction, string text, params GUILayoutOption[] args)
        {
            if (GUILayout.Button(text, args) && OnClickAction != null)
            {
                OnClickAction.Invoke();
            }
        }
    }
}