///////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

/// GoogleDataSettings.cs
/// (c)2013 Kim, Hyoun Woo
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace UnityQuickSheet
{
    /// <summary>
    /// A class manages google account setting.
    /// </summary>
    public class GoogleDataSettings : ScriptableObject
    {
        public static string AssetPath = "Assets/3d/QuickSheet/GDataPlugin/";

        public static readonly string AssetFileName = "GoogleDataSettings.asset";

        private static GoogleDataSettings s_Instance;
        ///// <summary>
        ///// A property for a singleton instance.
        ///// </summary>
        //public static GoogleDataSettings Instance
        //{
        //    get
        //    {
        //        if (s_Instance == null)
        //        {
        //            s_Instance = Create();
        //        }
        //        return s_Instance;
        //    }
        //}

        //static GoogleDataSettings Create()
        //{
        //    string filePath = GoogleDataSettings.AssetPath + GoogleDataSettings.AssetFileName;
        //    var res = UnityEditor.AssetDatabase.LoadAssetAtPath<GoogleDataSettings>(filePath);

        //    if (res == null)
        //    {
        //        res = ScriptableObject.CreateInstance<GoogleDataSettings>();

        //        string path = AssemblyReflectionHelper.GetUniqueAssetPathNameOrFallback(GoogleDataSettings.AssetFileName);
        //        UnityEditor.AssetDatabase.CreateAsset(res, path);

        //        GoogleDataSettings.AssetPath = Path.GetDirectoryName(path);
        //        GoogleDataSettings.AssetPath += "/";

        //        // saves file path of the created asset.
        //        UnityEditor.EditorUtility.SetDirty(res);
        //        UnityEditor.AssetDatabase.SaveAssets();
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Already exist at " + filePath);
        //    }

        //    UnityEditor.Selection.activeObject = res;

        //    return res;
        //}

        // A flag which indicates use local installed oauth2 json file for authentication or not.
        static public bool useOAuth2JsonFile = false;

        public string JsonFilePath
        {
            get { return jsonFilePath; }
            set
            {
                if (string.IsNullOrEmpty(value) == false)
                    jsonFilePath = value;
            }
        }

        private string jsonFilePath = string.Empty;

        [System.Serializable]
        public struct OAuth2JsonData
        {
            public string client_id;
            public string auth_uri;
            public string token_uri;
            public string auth_provider_x509_cert_url;
            public string client_secret;
            public List<string> redirect_uris;
        };

        public OAuth2JsonData OAuth2Data;

        // enter Access Code after getting it from auth url
        public string _AccessCode = "Paste AcecessCode here!";

        // enter Auth 2.0 Refresh Token and AccessToken after succesfully authorizing with Access Code
        public string _RefreshToken = "";

        public string _AccessToken = "";
    }
}