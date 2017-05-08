using System;
using UnityEngine.EventSystems;

namespace ShutEye.UI.Core
{
    /// <summary>
    /// Для UI элементов интерактивных
    /// </summary>
    public interface IShutEyeSSelectable : IComponentUI
    {
        void SetSelect(bool isSecelected);

        event Action<SEUIContainerItem, PointerEventData.InputButton> ClickOnViewEvent;
    }
}