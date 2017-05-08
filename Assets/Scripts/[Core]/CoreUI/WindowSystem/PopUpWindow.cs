using Entitas;
using ShutEye.Core;
using ShutEye.UI.Core;
using System;
using UnityEngine;

namespace GameKit.UI.Core
{
    public interface IPopUpWindow
    {
        int Depth { get; set; }

        event Action<IPopUpWindow> OnHideEvent;

        void ShowPopUp();

        void Hide();
    }

    public interface IPopUpResult<T> : IPopUpWindow
    {
        event Action<T> ResultEvent;

        void CloseWith(T result);
    }

    public abstract class PopUpWindows : SEUIContainerItem, IPopUpWindow
    {
        public bool IsDestroy { get { return _destroyOnHide; } }

        [SerializeField]
        private bool _destroyOnHide = false;

        public event Action<IPopUpWindow> OnHideEvent
        {
            add
            {
                _onHide -= value;
                _onHide += value;
            }
            remove { _onHide -= value; }
        }

        private Action<IPopUpWindow> _onHide;

        public abstract void ShowPopUp();


        public virtual void Hide()
        {
            var temp = _onHide;
            _onHide = null;
            if (_destroyOnHide)
            {
                Destroy(gameObject);
                Destroy(this);
            }
            if (temp != null)
                temp.Invoke(this);
        }

        int IPopUpWindow.Depth { get; set; }
    }

    public abstract class PopUpWindows<T> : PopUpWindows, IDataBinding<T>
    {
        public void UpdateDataView(T newdata)
        {
            CurrentData = newdata;
            if (CurrentData.Equals(default(T)))
                ClearView();
            else
            {
                RefreshView();
                ShowPopUp();
            }
        }

        public T CurrentData { get; private set; }
    }
}