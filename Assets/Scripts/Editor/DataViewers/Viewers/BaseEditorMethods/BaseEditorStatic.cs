using ShutEye.EditorsScripts.ViewerData;
using ShutEye.Extensions;
using ShutEye.Extensions.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShutEye.EditorsScripts
{
    public abstract partial class BaseEditorStatic
    {
        #region DataViewers

        private static readonly Dictionary<Type, Type> _allViewer = GetAll<BaseViewer>();
        private static readonly Dictionary<Type, Type> _allFilter = GetAll<BaseFilterViewer>();

        private static Dictionary<Type, Type> GetAll<T>() where T : BaseEditorStatic
        {
            var res = new Dictionary<Type, Type>();
            AssemblyReflectionHelper.GetAllTypesInSolution<T>().ForEach(t =>
            {
                var attr = t.GetCustomAttributes(typeof(ViewWrapperAttribute), false);
                if (attr.Length <= 0) return;
                var atr = attr[0] as ViewWrapperAttribute;
                res.Add(atr.WrapperType, t);
            });
            return res;
        }

        public static BaseViewer GetDataViewer<T>() where T : DataObject
        {
            return _getDataViewer(typeof(T));
        }

        public static BaseFilterViewer GetDataFilter<T>() where T : DataObject
        {
            return _getDataFilterer(typeof(T));
        }

        internal static BaseFilterViewer GetDataFilter(Type objectType, ICollection data = null)
        {
            if (!objectType.IsSubclassOf(typeof(DataObject)))
            {
                Debug.LogErrorFormat("{0} no DataObject");
                return null;
            }
            if (data == null)
            {
                return _getDataFilterer(objectType);
            }
            return _getDataFilterer(objectType).InitData(new ArrayList(data));
        }

        internal static BaseViewer GetDataViewer(Type objectType)
        {
            if (objectType.IsSubclassOf(typeof(DataObject))) return _getDataViewer(objectType);
            Debug.LogErrorFormat("{0} no DataObject");
            return null;
        }

        private static BaseViewer _getDataViewer(Type objectType)
        {
            Type viewerType = null;
            if (_allViewer.ContainsKey(objectType))
            {
                viewerType = _allViewer[objectType];
            }
            else
            {
                viewerType = typeof(BaseViewer<>).MakeGenericType(objectType);
            }
            return Activator.CreateInstance(viewerType) as BaseViewer;
        }

        private static BaseFilterViewer _getDataFilterer(Type objectType)
        {
            Type viewerType = null;

            viewerType = _allFilter.ContainsKey(objectType) ? _allFilter[objectType] : typeof(BaseFilterViewer<>).MakeGenericType(objectType);
            return Activator.CreateInstance(viewerType) as BaseFilterViewer;
        }

        #endregion DataViewers


        public event Action RefreshWindow;

        protected void RefreshData()
        {
            if (RefreshWindow != null)
                RefreshWindow.Invoke();
        }
    }
}