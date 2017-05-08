using System;
using GameKit.UI;
using ShutEye.UI.Core;
using UnityEngine;

public class SelectWindiow : BaseWindow
{
    public override Enum TypeWindow
    {
        get { return  WindowType.SelectWindiow; }
    }

    public override WindowState State { get; set; }

    [SerializeField]
    private SelectItemsContloller _itemsContloller;

    public override void RefreshView()
    {
    }

    public void ShowType(MenuItemType type)
    {
        var listResults = BDWrapper.GetAllInfoAbout(type);
        _itemsContloller.InitDataToList<BaseDataForSelectWindow>(listResults);

    }
}
