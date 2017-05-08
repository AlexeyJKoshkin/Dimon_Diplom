using System;
using UnityEngine;
using ShutEye.UI.Core;

//контейнер для главного меню
public class MainMenuItemContainer : SEUIContainerItem, IContainerUI<MenuItemType>
{
    [SerializeField]
    private MenuItemType _item;

    protected override void PrepareUI(Action _onComplete)
    {
        UpdateDataView(_item);
        base.PrepareUI(_onComplete);
    }

    public override void RefreshView()
    {
    }

    public void UpdateDataView(MenuItemType newdata)
    {
        CurrentData = newdata;
        RefreshView();
    }

    public MenuItemType CurrentData { get; private set; }
}
