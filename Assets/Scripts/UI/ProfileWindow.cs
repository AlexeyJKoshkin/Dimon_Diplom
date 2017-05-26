using System;
using EnhancedUI.EnhancedScroller;
using GameKit;
using GameKit.UI;
using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileWindow : BaseWindow, IDataBinding<BaseDataForProfileWindow> , IEnhancedScrollerDelegate
{
    public override Enum TypeWindow
    {
        get { return WindowType.WindowInfo; }
    }

    public override WindowState State { get; set; }

    [SerializeField]
    private UnityEngine.UI.Button _mainMenuBtn;

    [SerializeField]
    private Image _fullSprite;

    [SerializeField]
    private Text _fio;

    [SerializeField]
    private Text _fullInfo;

    [SerializeField]
    private Text _contacts;

    /// <summary>
    /// The scroller to control
    /// </summary>
    public EnhancedScroller scroller;

    /// <summary>
    /// The prefab of the cell view
    /// </summary>
    public PortfolioContainerUI cellViewPrefab;

    //[SerializeField]
    //private PortfolioContainersController _allPhotos;

    protected override void PrepareUI(Action _onComplete)
    {
        // set the scroller's delegate to this controller
        scroller.Delegate = this;

        // set the scroller's cell view visbility changed delegate to a method in this controller
        scroller.cellViewVisibilityChanged = CellViewVisibilityChanged;
        _mainMenuBtn = _mainMenuBtn ?? GetComponentInChildren<UnityEngine.UI.Button>();
        _mainMenuBtn.onClick.AddListener(() =>
        {
            HideWindow(null);
            UIInstance.Instance.GetWindow<SelectWindiow>().ShowType(CurrentData.Type);
        });
        base.PrepareUI(_onComplete);
    }

    private void CellViewVisibilityChanged(SEUIContainerItem cellview)
    {
        // cast the cell view to our custom view
        PortfolioContainerUI view = cellview as PortfolioContainerUI;

        // if the cell is active, we set its data, 
        // otherwise we will clear the image back to 
        // its default state

        if (cellview.active)
            view.UpdateDataView(CurrentData.Porfolio[cellview.dataIndex]);
        else
            view.ClearView();
    }


    public override void RefreshView()
    {
        GameCore.Instance.LoadSprite(CurrentData.Foto, OnLoadPhoto);
        
        _fio.text = CurrentData.Name;
        _fullInfo.text = CurrentData.FullInfo;
        _contacts.text = CurrentData.Contatcts;
        scroller.ReloadData();
        ShowWindow(null);
    }

    private void OnLoadPhoto(Sprite obj)
    {
        _fullSprite.sprite = obj;
    }

    public void UpdateDataView(BaseDataForProfileWindow newdata)
    {
        if (newdata != null)
        {
            CurrentData = newdata;
        }
        RefreshView();
    }

    public BaseDataForProfileWindow CurrentData { get; private set; }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return CurrentData.Porfolio.Length;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 150;
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

    private void CellViewOnClickOnViewEvent(IContainerUI arg1, PointerEventData.InputButton arg2)
    {
        _fullSprite.sprite = ((PortfolioContainerUI)arg1).LoadPhoto;
        Debug.Log("Show Photo");
    }
}
