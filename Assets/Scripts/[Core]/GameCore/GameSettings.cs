using UnityEngine;

namespace ShutEye.Core
{
    public class GameSettings : Singleton<GameSettings>
    {
        public const int SORTING_LAYER_DELTA = -100;

        public const string SCENE_PRELOADER = "Preloader";
        public const string SCENE_PRELOADER_SCENE = "PreloaderLvl";

        public const string SCENE_MAIN_SCREEN = "MainScene";

        public static int CountEquipSlots
        {
            get { return Instance._countEquipSlots; }
        }

        public static float SpeedModificator
        {
            get
            {
                return Instance._speedModeficator;
            }
        }

        [SerializeField]
        [Range(1f, 50f)]
        private float _speedModeficator = 10;

        [SerializeField]
        [Range(1, 5)]
        private int _countEquipSlots = 3;
    }
}