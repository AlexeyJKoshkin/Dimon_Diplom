using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using ShutEye.UI.Core;

namespace EnhancedScrollerDemos.JumpToDemo
{
    public class CellView : SEUIContainerItem
    {
        public Text cellText;

        public void SetData(Data data)
        {
            cellText.text = data.cellText;
        }
    }
}