using UnityEditor;
using UnityEngine;

namespace GameKit.Data.Editor
{
    public class SetPathesWindow : EditorWindow
    {
        public static void ShowSetPathWindow()
        {
            var w = GetWindow<SetPathesWindow>();
            w.Init();
            w.ShowPopup();
        }

        private string Path_Scripts_Generate;
        private string Path_Scripts_Viewers;

        private void Init()
        {
            Path_Scripts_Generate = EditorPrefs.GetString("Path_Scripts_Generate", Application.dataPath + "/Scripts/Data");
            Path_Scripts_Viewers = EditorPrefs.GetString("Path_Scripts_Viewers", Application.dataPath + "/Scripts/Data/Editor");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Path_Scripts_Generate", Path_Scripts_Generate);
            EditorGUILayout.LabelField("Path_Scripts_Viewers", Path_Scripts_Viewers);
        }
    }
}