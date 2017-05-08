using System;
using UnityEngine.EventSystems;

namespace ShutEye.UI.Core
{
    public class SEBoolStateButton : SEStateButton<bool>, IPointerClickHandler
    {
        protected override void PrepareUI(Action _onComplete)
        {
            this.ClickOnViewEvent += (arg1, arg2) => UpdateDataView(!CurrentData);
            base.PrepareUI(_onComplete);
        }
    }
}