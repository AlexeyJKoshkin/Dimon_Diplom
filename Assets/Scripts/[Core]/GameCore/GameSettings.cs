using UnityEngine;

namespace ShutEye.Core
{
    public class GameSettings : Singleton<GameSettings>
    {
        public const int SORTING_LAYER_DELTA = -100;

        public const string SCENE_PRELOADER = "Preloader";
        public const string SCENE_PRELOADER_SCENE = "PreloaderLvl";

        public const string SCENE_MAIN_SCREEN = "MainScene";

        public static Sprite DefaultIcon
        {
            get
            {
                return Instance._defauiltIcon;
            }
        }

        [SerializeField] private Sprite _defauiltIcon;
    }
}