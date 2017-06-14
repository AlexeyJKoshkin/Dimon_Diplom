using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameKit.UI;
using ShutEye.Core;
using ShutEye.Extensions;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Контроллер отобраения списка услуг в выбранной категории
/// </summary>
public class SelectWindiow : BaseWindow, IEnhancedScrollerDelegate
{
    #region логические элементы интерфейса
    public override Enum TypeWindow
    {
        get { return  WindowType.SelectWindiow; }
    }
    public override WindowState State { get; set; }

    [SerializeField]
    private UnityEngine.UI.Button _mainMenuBtn;
    /// <summary>
    /// скролл 
    /// </summary>
    public EnhancedScroller scroller;

    /// <summary>
    /// префаб для отображения списка услуг
    /// </summary>
    public SelectWindowContainer cellViewPrefab;
    #endregion

    #region Методы для заполнения скрола данными
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data == null ? 0 : _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
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
        cellView.NameCell = "Cell " + dataIndex.ToString();

        // In this example, we do not set the data here since the cell is not visibile yet. Use a coroutine
        // before the cell is visibile will result in errors, so we defer loading until the cell has
        // become visible. We can trap this in the cellViewVisibilityChanged delegate handled below

        // return the cell to the scroller
        return cellView;
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
    #endregion

    /// <summary>
    /// текущая категория
    /// </summary>
    private MenuItemType _currentViewType;

    /// <summary>
    /// список услуг
    /// </summary>
    private IList<BaseDataForSelectWindow> _data = new List<BaseDataForSelectWindow>();

    
    public override void RefreshView()
    {
    }

    /// <summary>
    /// спрятать окно, очищаем все списки 
    /// </summary>
    /// <param name="callback"></param>
    public override void HideWindow(Action callback)
    {
        base.HideWindow(callback);
        _data.ForEach(e => e.Clear());
        scroller.RefreshActiveCellViews();
    }

    /// <summary>
    /// метод инициализации вызывается из DiplomU.
    /// подписка на события, и клики пользователя
    /// </summary>
    /// <param name="_onComplete"></param>
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

    

    public void ShowType(MenuItemType type)
    {
        _data = DiplomCore.Instance.MainBD.GetAllInfoAbout(type);
        _currentViewType = type;
        //_itemsContloller.InitDataToList<BaseDataForSelectWindow>(_data);
        if (_data.Count == 0)
        {
            Debug.LogError("Никого нет");
        }
        scroller.ReloadData();
        this.ShowWindow(null);
    }

    
    /// <summary>
    /// Метод по обработке клика пользователя на ячейку с услугой
    /// </summary>
    /// <param name="containerUi"></param>
    /// <param name="inputButton"></param>
    private void CellViewOnClickOnViewEvent(IContainerUI containerUi, PointerEventData.InputButton inputButton)
    {
        var data = ((IContainerUI<BaseDataForSelectWindow>) containerUi).CurrentData; 
        BaseDataForProfileWindow fullInfo = DiplomCore.Instance.MainBD.GetFullInfo(_currentViewType, data.Id); // запрос в БД
        UIInstance.Instance.GetWindow<ProfileWindow>().UpdateDataView(fullInfo); // показать окно
        this.HideWindow(null);
    }
}
