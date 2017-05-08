using System;
using UnityEngine.EventSystems;

namespace ShutEye.UI.Core
{
    public interface IContainerViews<T>
    {
        event Action<T, PointerEventData.InputButton> OnChange;

        T ChoosenCell { get; }

        T OwerMouse { get; }

        T GetSlotByN(int slotNumber);

        T GetSlotById(string id);

        T[] Containers { get; }
    }
}