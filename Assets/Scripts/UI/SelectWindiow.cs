using System;
using GameKit.UI;
using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectWindiow : BaseWindow
{
    public override Enum TypeWindow
    {
        get { return  WindowType.SelectWindiow; }
    }

    public override WindowState State { get; set; }

    [SerializeField]
    private SelectItemsContloller _itemsContloller;

    [SerializeField]
    private UnityEngine.UI.Button _mainMenuBtn;

    private MenuItemType _currentViewType;

    protected override void PrepareUI(Action _onComplete)
    {
        _itemsContloller.OnChange += ItemsContlollerOnOnChange;
        _mainMenuBtn = _mainMenuBtn ?? GetComponentInChildren<UnityEngine.UI.Button>();
        _mainMenuBtn.onClick.AddListener(this.BackMainMenu);
        base.PrepareUI(_onComplete);
    }
    public override void RefreshView()
    {
    }

    public void ShowType(MenuItemType type)
    {
        var listResults = GameCore.Instance.MainBD.GetAllInfoAbout(type);
        _currentViewType = type;
        _itemsContloller.InitDataToList<BaseDataForSelectWindow>(listResults);
        if (listResults.Count == 0)
        {
            Debug.LogError("Никого нет");
        }
        this.ShowWindow(null);
    }

    private void ItemsContlollerOnOnChange(IContainerUI<BaseDataForSelectWindow> containerUi, PointerEventData.InputButton inputButton)
    {

        BaseDataForProfileWindow fullInfo = GameCore.Instance.MainBD.GetFullInfo(_currentViewType, containerUi.CurrentData.Id);
        UIInstance.Instance.GetWindow<ProfileWindow>().UpdateDataView(fullInfo);
        this.HideWindow(null);
    }
}
