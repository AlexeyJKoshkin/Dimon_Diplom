using DG.Tweening;
using ShutEye.UI.Core;
using System;
using UnityEngine;

namespace GameKit.UI
{
    public abstract partial class BaseWindow : SEComponentUI
    {
        public abstract Enum TypeWindow { get; }
        public abstract WindowState State { get; set; }

        public virtual void HideWindow(Action callback)
        {
            this.transform.localPosition = new Vector3(-1000, -1000, 0);
        }

        public virtual void ShowWindow(Action callback)
        {
            this.transform.localPosition = Vector3.zero;
        }

        public abstract void RefreshView();

        [Range(0, 10)]
        public float ShowAnimation = 2;

        [Range(0, 10)]
        public float Hidenimation = 2;
    }

    /// <summary>
    /// базовый класс для панелей/окон
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public abstract class BaseWindow<T> : BaseWindow where T : BaseWindow
    {
        public override WindowState State
        {
            get { return _state; }
            set { _state = value; }
        }

        [SerializeField]
        [ReadOnly]
        private WindowState _state;

        //Если что то можно будет переделать. я пока юзаю так.

        public LAction AnimationShow = new LAction<Action<BaseWindow, bool, TweenCallback>, T>("AnimationShow");
        public LAction AnimationHide = new LAction<Action<BaseWindow, bool, TweenCallback>, T>("AnimationHide");

        protected override void PrepareUI(Action _onComplete)
        {
            AnimationShow.CreateAction(typeof(T));
            AnimationHide.CreateAction(typeof(T));
            //Debug.LogErrorFormat("Delegate {0} Window {1} Method {2}", AnimationHide.Sdelegate == null, name, AnimationHide.MethodName);
            //Debug.LogErrorFormat("Delegate {0} Window {1} Method {2}", AnimationShow.Sdelegate == null, name, AnimationShow.MethodName);
            base.PrepareUI(_onComplete);
        }

        public override void RefreshView()
        {
        }

        private Action _callback;

        private void FinishAnimation()
        {
            var temp = _callback;
            _callback = null;
            if (temp != null)
            {
                temp.Invoke();
            }
        }

        [ContextMenu("Hide")]
        public override void HideWindow(Action callback)
        {
            _callback = callback;
            //_callback += () => Debug.LogFormat("{0} {1} end {2}", this.TypeWindow, this.State, this.transform.position);
            if (AnimationHide.Sdelegate != null)
                ((Action<T, bool, TweenCallback>)AnimationHide.Sdelegate).Invoke(this as T, true, FinishAnimation);
            else
            {
                this.GetComponent<Canvas>().enabled = false;
                FinishAnimation();
                State = WindowState.Closed;
            }
        }

        [ContextMenu("Show")]
        public override void ShowWindow(Action callback)
        {
            _callback = callback;
            //_callback += () => Debug.LogFormat("{0} {1} end {2}", this.TypeWindow, this.State, this.transform.position);
            if (AnimationShow.Sdelegate != null)
                ((Action<T, bool, TweenCallback>)AnimationShow.Sdelegate).Invoke(this as T, true, FinishAnimation);
            else
            {
                this.GetComponent<Canvas>().enabled = true;
                FinishAnimation();
                State = WindowState.Open;
            }
        }
    }
}