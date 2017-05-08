using DG.Tweening;
using Entitas;
using ShutEye.UI.Core;
using System;
using UnityEngine;

namespace GameKit.UI
{
    public abstract partial class BaseWindow : SEComponentUI, IDataBinding<Entity>
    {
        public abstract Enum TypeWindow { get; }
        public abstract WindowState State { get; set; }

        public virtual void HideWindow(Action callback, bool immidiate = false)
        {
        }

        public virtual void ShowWindow(Action callback, bool immidiate = false)
        {
            
        }

        public abstract void RefreshView();

        [Range(0, 10)]
        public float ShowAnimation = 2;

        [Range(0, 10)]
        public float Hidenimation = 2;

        public void UpdateDataView(Entity newdata)
        {
            if (CurrentData == newdata) return;
            CurrentData = newdata;
            RefreshView();
        }

        public Entity CurrentData { get; private set; }
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
        public override void HideWindow(Action callback, bool now = false)
        {
            _callback = callback;
            //_callback += () => Debug.LogFormat("{0} {1} end {2}", this.TypeWindow, this.State, this.transform.position);
            if (AnimationHide.Sdelegate != null)
                ((Action<T, bool, TweenCallback>)AnimationHide.Sdelegate).Invoke(this as T, now, FinishAnimation);
            else
            {
                this.GetComponent<Canvas>().enabled = false;
                FinishAnimation();
                State = WindowState.Closed;
            }
        }

        [ContextMenu("Show")]
        public override void ShowWindow(Action callback, bool now = false)
        {
            _callback = callback;
            //_callback += () => Debug.LogFormat("{0} {1} end {2}", this.TypeWindow, this.State, this.transform.position);
            if (AnimationShow.Sdelegate != null)
                ((Action<T, bool, TweenCallback>)AnimationShow.Sdelegate).Invoke(this as T, now, FinishAnimation);
            else
            {
                this.GetComponent<Canvas>().enabled = true;
                FinishAnimation();
                State = WindowState.Open;
            }
        }
    }
}