using System;
using System.Collections;
using System.Collections.Generic;
using ShutEye.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityQuickSheet;

namespace ShutEye.Core
{
    /// <summary>
    /// €дро программы, точка входа. инициализаци€ модели, UI интернета
    /// </summary>
    public partial class DiplomCore : Singleton<DiplomCore>
    {
        /// <summary>
        /// кешированные фоточки
        /// </summary>
        private static readonly Dictionary<string, Sprite> _cashadSprites = new Dictionary<string, Sprite>();

        /// <summary>
        /// ƒанные дл€ гугл таблиц
        /// </summary>
        [SerializeField]
        private GoogleDataSettings _googleSettings;

        /// <summary>
        /// модель данных 
        /// </summary>
        public SheetDataRuntimeWrapper MainBD {
            get { return _wrapper; }
        }
        [SerializeField]
        private SheetDataRuntimeWrapper _wrapper;
        
        /// <summary>
        /// «агрузка €дра и инициализаци€ компонентов
        /// </summary>
        private void Awake()
        {
            UnsafeSecurityPolicy.Instate();
            OnAllReady();
        }

        /// <summary>
        /// пошагова€ проверка интернета и Ѕƒ
        /// </summary>
        /// <returns></returns>
        private IEnumerator Start()
        {
            _wrapper.Init(_googleSettings);

           // yield return CheckInternet();

            yield return _wrapper.GetSelectDb(); //проверка и обновление таблицы категорий
            yield return _wrapper.GetPfofileDb();//профайлы
            SceneManager.LoadScene(STRH.DefaultNames.MainMenuScene); // грузим главное меню
        }

        
        /// <summary>
        /// «агрузить фоточки из интернета
        /// </summary>
        /// <param name="avatarSprite"></param>
        /// <param name="onLoadSprite"></param>
        internal void LoadSprite(string avatarSprite, Action<Sprite> onLoadSprite)
        {
            avatarSprite = avatarSprite.Replace("\r", "");
            if (_cashadSprites.ContainsKey(avatarSprite))
            {
                if (onLoadSprite != null)
                    onLoadSprite.Invoke(_cashadSprites[avatarSprite]);
            }
            else
            {
                if (onLoadSprite != null)
                    StartCoroutine(LoadSpriteCoroutine(avatarSprite, onLoadSprite));
            }
        }
        /// <summary>
        /// «агрузка фотки из интернета
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Onfinish"></param>
        /// <returns></returns>
        private IEnumerator LoadSpriteCoroutine(string url, Action<Sprite> Onfinish)
        {
            if (string.IsNullOrEmpty(url)) yield break;
            var www = new WWW(url);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
                    new Vector2(0, 0));
                _cashadSprites.Add(url, sprite);
                Onfinish.Invoke(sprite);
            }
            else
            {
                Debug.LogError(url);
                Debug.LogError(www.error);
            }
            www.Dispose();
        }
        /// <summary>
        /// ѕроверка интернета
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckInternet()
        {
            throw new NotImplementedException();
        }

        #region Init как только все пройдет проверку - запускаетс€ основна€ логика приложени€

        private void OnAllReady()
        {
            IsReady = true;
            Debug.Log(">>>>> All Ready");
            if (_onReady != null)
                _onReady.Invoke();
        }

        public bool IsReady { get; private set; }

        public Sprite DefaultSprite;

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