using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using ShutEye.UI.Core;

namespace EnhancedScrollerDemos.RefreshDemo
{
    /// <summary>
    /// This is the view of our cell which handles how the cell looks.
    /// </summary>
    public class CellView : SEButtonUI<Data>
    {

        /// <summary>
        /// A reference to the UI Text element to display the cell data
        /// </summary>
        public Text someTextText;

        public RectTransform RectTransform
        {
            get
            {
                var rt = gameObject.GetComponent<RectTransform>();
                return rt;
            }
        }

        public override void RefreshView()
        {
            base.RefreshView();
            someTextText.text = CurrentData.someText;
        }
    }
}