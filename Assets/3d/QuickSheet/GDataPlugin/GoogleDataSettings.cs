///////////////////////////////////////////////////////////////////////////////
///
/// GoogleDataSettings.cs
/// 
/// (c)2013 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;

using System.Collections.Generic;
using ShutEye.Core;

namespace UnityQuickSheet
{
    /// <summary>
    /// A class manages google account setting.
    /// </summary>
    public class GoogleDataSettings : ScriptableObject
    {
        public static string AssetPath = "Assets/3d/QuickSheet/GDataPlugin/";

        public static readonly string AssetFileName = "GoogleDataSettings.asset";

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

        /// <summary>
        /// A path where generated ScriptableObject derived class and its data class script files are to be put.
        /// </summary>
        public string RuntimePath = string.Empty;

        /// <summary>
        /// A path where generated editor script files are to be put.
        /// </summary>
        public string EditorPath = string.Empty;

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