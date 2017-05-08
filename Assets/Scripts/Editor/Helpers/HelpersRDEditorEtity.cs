using UnityEditor;
using UnityEngine;

namespace ShutEye.EditorsScripts
{
    //public struct AreaSizesMainWindow
    //{
    //    public const int TOP = 40;

    //    public Rect EntityRect;
    //    public Rect EditorChooseRect;
    //    public Rect SearchFilterRect;
    //    public Rect AllElemntsRect;
    //    public Rect BtnsRect;

    //    public AreaSizesMainWindow(EditorWindow window)
    //    {
    //        EditorChooseRect = new Rect(0, TOP, 120, window.position.height);
    //        SearchFilterRect = new Rect(EditorChooseRect.xMax, TOP, 200, 200);
    //        AllElemntsRect = new Rect(EditorChooseRect.xMax, SearchFilterRect.yMax, SearchFilterRect.width, window.position.height - SearchFilterRect.height);
    //        BtnsRect = new Rect(AllElemntsRect.xMax, 0, window.position.width - SearchFilterRect.width - EditorChooseRect.width, 70);
    //        EntityRect = new Rect(AllElemntsRect.xMax, BtnsRect.yMax, window.position.width - AllElemntsRect.width - EditorChooseRect.width, window.position.height - BtnsRect.height - TOP);
    //    }
    //}

    public struct AreaSizesChooseWindow
    {
        public const int TOP = 10;
        public const int BORDER = 10;

        public Rect BtnsArea;
        public Rect MainRect;
        public Rect FilterRect;
        public Rect AllElemntsRect;

        public AreaSizesChooseWindow(EditorWindow window)
        {
            AllElemntsRect = new Rect(BORDER, TOP, 250, window.position.height);
            FilterRect = new Rect(AllElemntsRect.xMax + BORDER, TOP, window.position.width - AllElemntsRect.width - 300, 150);
            BtnsArea = new Rect(FilterRect.xMax + BORDER, TOP, 300, 150);
            MainRect = new Rect(AllElemntsRect.xMax + BORDER, BtnsArea.yMax, window.position.width - AllElemntsRect.width - 50, AllElemntsRect.height - 150);
        }
    }

    public struct AreaSizesHelperSettingsWindow
    {
        public const int TOP = 10;
        public const int BORDER = 10;

        public Rect HeaderArea;
        public Rect SettingsArea;

        public AreaSizesHelperSettingsWindow(EditorWindow window)
        {
            HeaderArea = new Rect(BORDER, TOP, window.position.width, 80);
            SettingsArea = new Rect(BORDER, HeaderArea.yMax + TOP, window.position.width, window.position.height);
        }
    }

    public struct GameDesinerWindowRect
    {
        public const int TOP = 10;
        public const int BORDER = 2;

        public Rect MobRect;
        public Rect ItemsRect;
        public Rect SpawnPointsRect;

        public GameDesinerWindowRect(EditorWindow window)
        {
            MobRect = new Rect(BORDER, TOP, window.position.width - BORDER, window.position.height / 3f);
            ItemsRect = new Rect(BORDER, TOP + MobRect.height, window.position.width - BORDER, window.position.height / 3f);
            SpawnPointsRect = new Rect(BORDER, TOP + MobRect.height + ItemsRect.height, window.position.width - BORDER, window.position.height / 3f);
        }
    }
}