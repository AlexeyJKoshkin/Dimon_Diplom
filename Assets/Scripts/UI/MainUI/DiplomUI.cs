using GameKit.UI.Core;
using ShutEye.Extensions;
using ShutEye.UI.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace GameKit.UI
{
    public class DiplomUI : SEComponentUI
    {
        public bool IsReady { get; private set; }

        /// <summary>
        /// Окна указанные в префабах
        /// </summary>
        [SerializeField]
        private BaseWindow[] _prefabsWindow;

        [SerializeField]
        private SEComponentUI[] _popUPWindow;

        public ReadOnlyCollection<BaseWindow> AllWindows { get { return _allWindow.AsReadOnly(); } }

        [SerializeField]
        [ReadOnly]
        private List<BaseWindow> _allWindow;

        private Stack<IPopUpWindow> _stackPopUps = new Stack<IPopUpWindow>();

        private List<IPopUpWindow> _AlivePopups = new List<IPopUpWindow>();
        private BaseWindow _currentWindow;

        //public Enum CurrentWindowKey {get { return _currentWindowType;}}
#pragma warning disable

        [SerializeField]
        [ReadOnlyAttribute]
        private string _currentWindowType;

#pragma warning enable

        protected override void PrepareUI(Action _onComplete)
        {
            //_currentWindowType = "Still Initing...";
            var windowOnScreen = gameObject.GetComponentsInChildren<BaseWindow>(true).ToList();

            ////TODO: переделать инициализацию попапов

            windowOnScreen.ForEach(w =>
               {
                   _allWindow.Add(w);
                   w.InitUIComponent();
               });

            _prefabsWindow.ForEach(pw =>
              {
                  if (_allWindow.All(w => !Equals(w.TypeWindow, pw.TypeWindow)))
                  {
                      _allWindow.Add(InStantiateWindow<BaseWindow>(pw));
                  }
              });

            _allWindow.ForEach(w =>
            {
                w.HideWindow(null, true);
            });
            _onComplete += () => IsReady = true;
            base.PrepareUI(_onComplete);
        }

        private T InStantiateWindow<T>(SEComponentUI pref)
        {
            var old = pref.GetComponent<RectTransform>();
            var window = Instantiate(pref);

            window.InitUIComponent();
            window.transform.SetParent(this.transform);
            window.CashedTransform.sizeDelta = old.sizeDelta;
            window.CashedTransform.anchorMin = old.anchorMin;
            window.CashedTransform.anchorMax = old.anchorMax;
            window.CashedTransform.pivot = old.pivot;
            window.CashedTransform.localPosition = Vector3.zero;
            window.CashedTransform.localScale = Vector3.one;
            //Debug.Log(window.CashedTransform.sizeDelta);
            return window.GetComponent<T>();
        }

        public void ShowWindow(Enum type, Action callback = null)
        {
            var window = _allWindow.FirstOrDefault(o => o.TypeWindow.Equals(type));
            if (window == null) //значит окно попап
            {
                Debug.LogError(string.Format("Нет окна {0}", type));
                return;
            }
            ShowWindow(window, callback);
        }

        public T GetWindow<T>() where T : BaseWindow
        {
            return _allWindow.OfType<T>().FirstOrDefault();
        }

        private void ShowWindow(BaseWindow window, Action callback = null)
        {
            if (_currentWindow != null)
            {
                if (_currentWindow.TypeWindow.Equals(window.TypeWindow)) return;

                Debug.LogWarningFormat("{0} -> {1}", _currentWindow.TypeWindow, window.TypeWindow);

                if (_currentWindow.State == WindowState.Open)
                {
                    _currentWindow.HideWindow(() => _showwindow(window, callback));
                }
                else _showwindow(window, callback);
            }
            else
            {
                _showwindow(window, callback);
            }
        }

        /// <summary>
        /// просто показать попап
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetPopUp<T>() where T : IPopUpWindow
        {
            IPopUpWindow window = _AlivePopups.FirstOrDefault(o => o is T);
            if (window != null)
            {
                _AlivePopups.Remove(window);
            }
            else
            {
                var popup = _popUPWindow.FirstOrDefault(o => o is T);
                window = InStantiateWindow<T>(popup);
                if (window == null)
                {
                    throw new NullReferenceException("No PopUp type " + typeof(T).Name);
                }
            }
            _showPopUpwindow(window);
            return (T)window;
        }

        private void _showwindow(BaseWindow window, Action callback)
        {
            window.ShowWindow(callback);
            _currentWindow = window;
            _currentWindowType = _currentWindow.TypeWindow.ToString();
        }

        private void _showPopUpwindow(IPopUpWindow window)
        {
            if (_stackPopUps.Count > 0)
            {
                window.Depth = _stackPopUps.Peek().Depth + 50;
            }
            window.OnHideEvent += _clear;
            _stackPopUps.Push(window);
        }

        private void _clear(IPopUpWindow obj)
        {
            if (obj == _stackPopUps.Peek())
                _stackPopUps.Pop();
            else
                Debug.LogErrorFormat("{0} close before {1}", obj.GetType().Name, _stackPopUps.Peek().GetType().Name);
        }
    }
}