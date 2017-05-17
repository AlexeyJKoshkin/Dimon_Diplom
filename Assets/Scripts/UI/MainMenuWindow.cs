using System;
using GameKit.UI;
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
        UIInstance.Instance.GetWindow<SelectWindiow>().ShowType(mainMenuItemContainer.CurrentData);
        this.HideWindow(null);
    }
  

    public override void RefreshView()
    {

    }
}
