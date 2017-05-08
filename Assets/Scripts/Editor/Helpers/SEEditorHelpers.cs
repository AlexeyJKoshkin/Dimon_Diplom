using System;
using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts.Helpers
{
    public static class SEEditorHelpers
    {
        public const string SKIN_NAME = "EntityEditorSkin";

        /// <summary>
        /// загрузить скин
        /// </summary>
        /// <param name="nameskin"></param>
        /// <returns></returns>
        public static GUISkin GetSkinByName(string nameskin)
        {
            var guids = AssetDatabase.FindAssets(String.Format("{0} t:GUISkin", nameskin));
            if (guids.Length == 0)
            {
                Debug.LogError(String.Format("No Skin {0}", nameskin));
                return null;
            }
            if (guids.Length > 1)
            {
                Debug.LogWarning(String.Format("More Than 1 skin with name {0} return first", nameskin));
                foreach (var guid in guids)
                {
                    Debug.LogWarning(AssetDatabase.GUIDToAssetPath(guid));
                }
            }
            return AssetDatabase.LoadAssetAtPath(
                  AssetDatabase.GUIDToAssetPath(guids[0]), typeof(GUISkin)) as GUISkin;
        }
    }
}