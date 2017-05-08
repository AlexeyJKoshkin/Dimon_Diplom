using System;
using UnityEngine;

namespace GameKit
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }

    public class ViewTypeAttribute : PropertyAttribute
    {
        public Type ScripType { get; private set; }

        public ViewTypeAttribute(Type t)
        {
            ScripType = t;
        }
    }

    /// <summary>
    /// Атрибут для физических детектов
    /// </summary>
    public class HandlerMetodAttribute : Attribute
    {
        public string LayemName;

        public string Key
        {
            get { return _key; }
        }

        private string _key;

        public HandlerMetodAttribute(string layer = "Default", string key = "")
        {
            _key = key;
            LayemName = layer;
        }
    }

    public class EnumFlagsAttribute : PropertyAttribute
    {
    }
}