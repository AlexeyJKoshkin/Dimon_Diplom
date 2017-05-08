using GameKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static partial class AssemblyReflectionHelper
{
    public struct CallBackWrapper
    {
        public Delegate Delegate;
        public string Key;
    }

    public struct GenericWrapperCallbac<T> where T : class
    {
        public T Delegate;
        public string Key;
    }

    private static readonly Dictionary<Type, List<CallBackWrapper>> _temp = new Dictionary<Type, List<CallBackWrapper>>();

    public static T FindAction<T>(Type script, string key) where T : class
    {
        // ListCallBackList(script, type).ForEach(e => Debug.Log(e.Info.Key));
        return ListCallBackList(script, typeof(T)).First(o => o.Key == key).Delegate as T;
    }

    public static Delegate FindDelegate<T>(Type script, string key) where T : class
    {
        // ListCallBackList(script, type).ForEach(e => Debug.Log(e.Info.Key));
        return ListCallBackList(script, typeof(T)).First(o => o.Key == key).Delegate;
    }

    private static IEnumerable<CallBackWrapper> ListCallBackList(Type script, Type type)
    {
        if (!_temp.ContainsKey(script))
        {
            var list = (from m in script.GetMethods(BindingFlags.Static | BindingFlags.Public)
                        let action = Delegate.CreateDelegate(type, m, false)
                        where action != null
                        let attObjects = m.GetCustomAttributes(typeof(HandlerMetodAttribute), true)
                        let attr = (attObjects.Length > 0) ? attObjects[0] as HandlerMetodAttribute : null
                        let keyName = attr == null ? m.Name : string.IsNullOrEmpty(attr.Key) ? m.Name : attr.Key
                        select new CallBackWrapper()
                        {
                            Delegate = action,
                            Key = keyName
                        }).ToList();
            _temp.Add(script, list);
        }
        return _temp[script];
    }

    public static string[] GetNamesMethodsPack<T>(Type script) where T : class
    {
        return ListCallBackList(script, typeof(T)).Select(o => o.Key).ToArray();
    }

    public static IDictionary<string, string> GetInfoMethodsPack<T>(Type script) where T : class
    {
        return ListCallBackList(script, typeof(T)).ToDictionary(o => o.Key, o => o.Key);
    }
}