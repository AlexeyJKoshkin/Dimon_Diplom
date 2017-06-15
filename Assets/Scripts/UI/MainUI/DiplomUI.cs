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
        #region параметры и переменные

        /// <summary>
        /// Указатель на готовность UI к запуску
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Окна указанные приложения
        /// </summary>
        [SerializeField]
        private BaseWindow[] _prefabsWindow;

        /// <summary>
        /// Публичный список только для чтения со списком всех активных окон приложения
        /// </summary>
        public ReadOnlyCollection<BaseWindow> AllWindows { get { return _allWindow.AsReadOnly(); } }

        [SerializeField]
        [ReadOnly]
        private List<BaseWindow> _allWindow;

        private BaseWindow _currentWindow;

        #endregion параметры и переменные

        #region Инициализация UI приложения

        /// <summary>
        /// Метод вызывается один раз для инициализации всего интерфейса
        /// </summary>
        /// <param name="_onComplete"></param>
        protected override void PrepareUI(Action _onComplete)
        {
            //_currentWindowType = "Still Initing...";
            var windowOnScreen = gameObject.GetComponentsInChildren<BaseWindow>(true).ToList();

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
                w.HideWindow(null);
            });
            _onComplete += () => IsReady = true;
            base.PrepareUI(_onComplete);
        }

        /// <summary>
        /// инициализация окон, вызывается каждый раз для каждого типа окна
        /// </summary>
        /// <typeparam name="T">тип окна для инициализации</typeparam>
        /// <param name="pref">префаб окна </param>
        /// <returns></returns>
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

        #endregion Инициализация UI приложения

        #region Вывод окна на экран

        /// <summary>
        /// Вывести окно на экран
        /// </summary>
        /// <param name="type"></param>
        /// <param name="callback"></param>
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

        /// <summary>
        /// Получить окно по его типу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetWindow<T>() where T : BaseWindow
        {
            return _allWindow.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// внутренний метод для вывода окна на экран
        /// </summary>
        /// <param name="window"></param>
        /// <param name="callback"></param>
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

        private void _showwindow(BaseWindow window, Action callback)
        {
            window.ShowWindow(callback);
            _currentWindow = window;
        }

        #endregion Вывод окна на экран
    }
}