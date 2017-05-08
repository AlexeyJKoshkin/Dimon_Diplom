using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using UnityEngine;

namespace ShutEye.Data
{
    public static class SEJsonConverter
    {
        public static string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new IsoDateTimeConverter());
            //settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        public static T Deserialize<T>(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogError(String.Format("deserialize {0}:{1}", typeof(T).Name, e.Message));
                Debug.LogError(String.Format(e.StackTrace));
                return default(T);
            }

            //return default(T);
        }

        public static object Deserialize(string jsonString, Type toType)
        {
            try
            {
                return JsonConvert.DeserializeObject(jsonString, toType);
            }
            catch (Exception e)
            {
                Debug.LogError(String.Format("deserialize {0}:{1}", toType.FullName, e.Message));
            }

            return null;
        }

        public static object Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }
    }
}