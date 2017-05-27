using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameKit.UI;
using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectWindiow : BaseWindow, IEnhancedScrollerDelegate
{
    public override Enum TypeWindow
    {
        get { return  WindowType.SelectWindiow; }
    }
    public override WindowState State { get; set; }

    //[SerializeField]
    //private SelectItemsContloller _itemsContloller;

    [SerializeField]
    private UnityEngine.UI.Button _mainMenuBtn;

    private MenuItemType _currentViewType;

    /// <summary>
    /// The data for the scroller
    /// </summary>
    private IList<BaseDataForSelectWindow> _data;

    /// <summary>
    /// The scroller to control
    /// </summary>
    public EnhancedScroller scroller;

    /// <summary>
    /// The prefab of the cell view
    /// </summary>
    public SelectWindowContainer cellViewPrefab;

    public override void RefreshView()
    {
    }

    protected override void PrepareUI(Action _onComplete)
    {
        // set the scroller's delegate to this controller
        scroller.Delegate = this;

        // set the scroller's cell view visbility changed delegate to a method in this controller
        scroller.cellViewVisibilityChanged = CellViewVisibilityChanged;

        // tell the scroller to reload now that we have the data
       
      //  _itemsContloller.OnChange += ItemsContlollerOnOnChange;
        _mainMenuBtn = _mainMenuBtn ?? GetComponentInChildren<UnityEngine.UI.Button>();
        _mainMenuBtn.onClick.AddListener(this.BackMainMenu);
        base.PrepareUI(_onComplete);
    }

    private void CellViewVisibilityChanged(SEUIContainerItem cellview)
    {
        // cast the cell view to our custom view
        SelectWindowContainer view = cellview as SelectWindowContainer;

        // if the cell is active, we set its data, 
        // otherwise we will clear the image back to 
        // its default state

        if (cellview.active)
            view.UpdateDataView(_data[cellview.dataIndex]);
        else
            view.ClearView();
    }

    public void ShowType(MenuItemType type)
    {
        _data = GameCore.Instance.MainBD.GetAllInfoAbout(type);
        _currentViewType = type;
        //_itemsContloller.InitDataToList<BaseDataForSelectWindow>(_data);
        if (_data.Count == 0)
        {
            Debug.LogError("Никого нет");
        }
        scroller.ReloadData();
        this.ShowWindow(null);
    }


    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data == null ? 0 : _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // return a fixed cell size of 200 pixels
        return cellViewPrefab.GetComponent<RectTransform>().rect.height;
    }

    public SEUIContainerItem GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        var cellView = scroller.GetCellView(cellViewPrefab);

        cellView.ClickOnViewEvent += CellViewOnClickOnViewEvent;
        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.NameCell= "Cell " + dataIndex.ToString();

        // In this example, we do not set the data here since the cell is not visibile yet. Use a coroutine
        // before the cell is visibile will result in errors, so we defer loading until the cell has
        // become visible. We can trap this in the cellViewVisibilityChanged delegate handled below

        // return the cell to the scroller
        return cellView;
    }

    private void CellViewOnClickOnViewEvent(IContainerUI containerUi, PointerEventData.InputButton inputButton)
    {
        var data = ((IContainerUI<BaseDataForSelectWindow>) containerUi).CurrentData;
        BaseDataForProfileWindow fullInfo = GameCore.Instance.MainBD.GetFullInfo(_currentViewType, data.Id);
        UIInstance.Instance.GetWindow<ProfileWindow>().UpdateDataView(fullInfo);
        this.HideWindow(null);
    }
}
