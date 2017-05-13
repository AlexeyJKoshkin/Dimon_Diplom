using Entitas.VisualDebugging.Unity;
using ShutEye.Data;
using ShutEye.Data.Provider;
using System;
using System.Collections;
using System.Collections.Generic;
using ShutEye.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityQuickSheet;

namespace ShutEye.Core
{
    public partial class GameCore : Singleton<GameCore>
    {
        // public static IEntityByCollider InteractiveObjects { get { return GameCore.Pools.game.ShutEyeLevel; } }
        public static IDataBoxStorage Data
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    return _data;
                }
                else
                {
                    return _data ?? (_data = AssemblyReflectionHelper.LoadAllPrividers());
                }
#else

                return _data;
#endif
            }
        }

        private static IDataBoxStorage _data;

        [SerializeField]
        private List<DataBox> _allProviders;
        
        // public static IEntityByCollider InteractiveObjects { get { return GameCore.Pools.game.ShutEyeLevel; } }
        public static GoogleDataSettings GoogleSettings
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    return googleSettings;
                }
                else
                {
                    return googleSettings ?? (googleSettings = AssemblyReflectionHelper.Create());
                }
#else

                return googleSettings;
#endif
            }
        }

        private static GoogleDataSettings googleSettings;
        /// <summary>
        /// A singleton instance.
        /// </summary>
        [SerializeField]
        private GoogleDataSettings _googleSettings;

        public DiplomSheetDataWrapper MainBD {
            get { return _wrapper; }
        }

        [SerializeField]
        private DiplomSheetDataWrapper _wrapper;

        private void Awake()
        {
            var data = new LocalDataBoxStorage();
            _allProviders.ForEach(b => data.RegisterProvider(b));
            googleSettings = _googleSettings;
            _data = data;
            var logger = FindObjectOfType<LoggerUI>();
            if (logger != null) logger.InitLogger();
            UnsafeSecurityPolicy.Instate();
            OnAllReady();
        }

        private IEnumerator Start()
        {
#if UNITY_EDITOR
            foreach (var debugSystemsBehaviour in FindObjectsOfType<DebugSystemsBehaviour>())
            {
                debugSystemsBehaviour.gameObject.AddComponent<DontDestroyOnLoad>();
            }
#endif
            yield return _wrapper.GetDb();
            SceneManager.LoadScene(STRH.DefaultNames.MainMenuScene); // грузим главное меню
        }

        #region Init

        private void OnAllReady()
        {
            IsReady = true;
            Debug.Log(">>>>> All Ready");
            if (_onReady != null)
                _onReady.Invoke();
        }

        public bool IsReady { get; private set; }

        private Action _onReady;

        public void SubscribeToReady(Action onCoreReady)
        {
            if (IsReady)
                onCoreReady.Invoke();
            else
            {
                _onReady += onCoreReady;
            }
        }

        public void UnSubscribeToReady(Action onCoreReady)
        {
            if (_onReady != null)
                _onReady -= onCoreReady;
        }

        #endregion Init
    }
}