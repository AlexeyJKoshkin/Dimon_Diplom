using ShutEye.UI.Core;
using UnityEngine.UI;

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