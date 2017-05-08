using System;
using GameKit.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuWindow : BaseWindow
{
    public override Enum TypeWindow
    {
        get { return WindowType.Menu; }
    }

    public override WindowState State { get; set; }

    [SerializeField]
    private MenuItemController _itemsContainer;


    protected override void PrepareUI(Action _onComplete)
    {
        _itemsContainer.OnChange  += ItemsContainerOnOnChange;
        base.PrepareUI(_onComplete);
    }

    private void ItemsContainerOnOnChange(MainMenuItemContainer mainMenuItemContainer, PointerEventData.InputButton inputButton)
    {
        Debug.LogFormat("{0} Show", mainMenuItemContainer.CurrentData);
        switch (mainMenuItemContainer.CurrentData)
        {
            case MenuItemType.Leading:
            case MenuItemType.Music:
            case MenuItemType.Fotographer:
            case MenuItemType.Decorator:
            case MenuItemType.Videograph:
            case MenuItemType.ShowProgramm:
                break;
        }
    }

   

    public override void RefreshView()
    {

    }
}
