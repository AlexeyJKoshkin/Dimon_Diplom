using GameKit;
using System;
using UnityEngine;

namespace ShutEye.UI.Core
{
    public abstract class SEButtonUI<TData> : SEUIContainerItem, IContainerUI<TData>
    {
        public TData CurrentData { get; protected set; }
        //[SerializeField]
        //private SAction SHandler = new SAction<Action<TData, PointerEventData.InputButton>>();

        public virtual void UpdateDataView(TData data)
        {
            CurrentData = data;
            if (typeof(TData).IsClass)
            {
                if (CurrentData == null)
                    ClearView();
                else
                {
                    RefreshView();
                }
            }
            else
            {
                if (CurrentData.Equals(default(TData)))
                    ClearView();
                else
                    RefreshView();
            }
        }

        protected override void PrepareUI(Action _onComplete)
        {
            base.PrepareUI(null);
            //   SHandler.CreateDelegate();
            if (_onComplete != null)
                _onComplete.Invoke();
        }
    }

    public abstract class SEStateButton<T> : SEButtonUI<T> where T : struct
    {
#pragma warning disable

        [SerializeField]
        [ReadOnly]
        protected T _state;

#pragma warning enable

        public override void RefreshView()
        {
            _state = CurrentData;
        }
    }
}