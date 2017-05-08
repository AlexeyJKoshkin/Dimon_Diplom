using System;
using UnityEngine;

namespace ShutEye.UI.Core
{
    /// <summary>
    /// базовый интерфейс
    /// </summary>
    public interface IComponentUI
    {
        RectTransform CashedTransform { get; }

        void InitUIComponent(Action onReady = null);
    }
}