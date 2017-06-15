///////////////////////////////////////////////////////////////////////////////
using Google.GData.Client;
using System;

///
/// GDataDBRequestFactory.cs
///
/// (c)2015 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityQuickSheet;

namespace GDataDB.Impl
{
    /// <summary>
    /// Handles OAuth2 credentials to access on google spreadsheets.
    ///
    /// Note that it needs json type of private key to get access code.
    ///
    /// </summary>
    public class GDataDBRequestFactory
    {
        private const string SCOPE = "https://www.googleapis.com/auth/drive https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
        private const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        private const string TOKEN_TYPE = "refresh";

        public static GOAuth2RequestFactory RefreshAuthenticate(GoogleDataSettings settings)
        {
            if (string.IsNullOrEmpty(settings._RefreshToken) ||
                string.IsNullOrEmpty(settings._AccessToken))
                return null;

            if (string.IsNullOrEmpty(settings.OAuth2Data.client_id) ||
                string.IsNullOrEmpty(settings.OAuth2Data.client_id))
                return null;

            OAuth2Parameters parameters = new OAuth2Parameters()
            {
                RefreshToken = settings._RefreshToken,
                AccessToken = settings._AccessToken,
                ClientId = settings.OAuth2Data.client_id,
                ClientSecret = settings.OAuth2Data.client_secret,
                Scope = "https://www.googleapis.com/auth/drive https://spreadsheets.google.com/feeds",
                AccessType = "offline",
                TokenType = "refresh"
            };
            return new GOAuth2RequestFactory("spreadsheet", "MySpreadsheetIntegration-v1", parameters);
        }

        public static void InitAuthenticate(GoogleDataSettings settings)
        {
            string clientId = settings.OAuth2Data.client_id;
            string clientSecret = settings.OAuth2Data.client_secret;
            string accessCode = settings._AccessCode;

            // OAuth2Parameters holds all the parameters related to OAuth 2.0.
            OAuth2Parameters parameters = new OAuth2Parameters
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RedirectUri = REDIRECT_URI,
                Scope = SCOPE,
                AccessType = "offline",
                TokenType = TOKEN_TYPE
            };

            // Retrieves the Authorization URL
            // IMPORTANT
            // IMPORTANT

            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Debug.Log(authorizationUrl);
            //Debug.Log("Please visit the URL above to authorize your OAuth "
            //      + "request token.  Once that is complete, type in your access code to "
            //      + "continue...");

            parameters.AccessCode = accessCode;

            if (IsValidURL(authorizationUrl))
            {
                Application.OpenURL(authorizationUrl);
            }
            else
                Debug.LogError("Invalid URL: " + authorizationUrl);
        }

        /// <summary>
        ///  Check whether the given string is a valid http or https URL.
        /// </summary>
        private static bool IsValidURL(string url)
        {
            Uri uriResult;
            return (Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                                (uriResult.Scheme == Uri.UriSchemeHttp ||
                                 uriResult.Scheme == Uri.UriSchemeHttps));
        }

        public static void FinishAuthenticate(GoogleDataSettings settings)
        {
            try
            {
                OAuth2Parameters parameters = new OAuth2Parameters()
                {
                    ClientId = settings.OAuth2Data.client_id,
                    ClientSecret = settings.OAuth2Data.client_secret,
                    RedirectUri = REDIRECT_URI,

                    Scope = SCOPE,
                    AccessType = "offline", // IMPORTANT
                    TokenType = TOKEN_TYPE, // IMPORTANT

                    AccessCode = settings._AccessCode
                };
                OAuthUtil.GetAccessToken(parameters);
                string accessToken = parameters.AccessToken;
                string refreshToken = parameters.RefreshToken;
                //Debug.Log("OAuth Access Token: " + accessToken + "\n");
                //Debug.Log("OAuth Refresh Token: " + refreshToken + "\n");

                settings._RefreshToken = refreshToken;
                settings._AccessToken = accessToken;
            }
            catch (Exception e)
            {
                // To display the error message with EditorGUI.Dialogue, we throw it again.
                throw new Exception(e.Message);
            }
        }
    }
}