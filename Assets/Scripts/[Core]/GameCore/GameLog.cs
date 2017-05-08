using UnityEngine;

namespace ShutEye.Core
{
    public partial class GameCore
    {
        public static void LogGame(string builder, params object[] args)
        {
            Debug.Log(string.Format("[Game] " + builder, args));
        }

        public static void LogWarGame(string builder, params object[] args)
        {
            Debug.LogWarning(string.Format("[Game] " + builder, args));
        }

        public static void LogErrorGame(string builder, params object[] args)
        {
            Debug.LogError(string.Format("[Game] " + builder, args));
        }

        public static void LogSys(string builder, params object[] args)
        {
            Debug.Log(string.Format("[Sys] " + builder, args));
        }

        public static void LogWarSys(string builder, params object[] args)
        {
            Debug.LogWarning(string.Format("[Sys] " + builder, args));
        }

        public static void LogErrorSys(string builder, params object[] args)
        {
            Debug.LogError(string.Format("[Sys] " + builder, args));
        }
    }
}