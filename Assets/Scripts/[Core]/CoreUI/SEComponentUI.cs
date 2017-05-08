using System;
using UnityEngine;

namespace ShutEye.UI.Core
{
    public class SEComponentUI : MonoBehaviour, IComponentUI
    {
        public RectTransform CashedTransform { get; private set; }

        protected bool? _isInited = null;

        protected virtual void Awake()
        {
            InitUIComponent();
        }

        public void InitUIComponent(Action onReady = null)
        {
            if (_isInited.HasValue)
            {
                if (onReady != null)
                {
                    onReady.Invoke();
                }
                else return;
            }
            _isInited = false;

            PrepareUI(() =>
               {
                   if (onReady != null)
                       onReady.Invoke();
                   _isInited = true;
               });
        }

        protected virtual void PrepareUI(Action _onComplete)
        {
            CashedTransform = this.GetComponent<RectTransform>();
            if (_onComplete != null)
                _onComplete.Invoke();
        }

        protected bool CheckIsInited()
        {
            return _isInited.HasValue && _isInited.Value;
        }

        protected virtual void OnDestroy()
        {
            Destroy(this);
        }
    }
}