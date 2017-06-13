using System;
using EnhancedUI.EnhancedScroller;
using GameKit;
using GameKit.UI;
using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileWindow : BaseWindow, IDataBinding<BaseDataForProfileWindow>
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

    [SerializeField]
    private scrolportfolioContrpller _controller;
    

    //[SerializeField]
    //private PortfolioContainersController _allPhotos;

    protected override void PrepareUI(Action _onComplete)
    {
        // set the scroller's delegate to this controller
        scroller.Delegate = _controller;

        // set the scroller's cell view visbility changed delegate to a method in this controller
        scroller.cellViewVisibilityChanged = CellViewVisibilityChanged;
        _controller.OnChange += ControllerOnOnChange;
        _mainMenuBtn = _mainMenuBtn ?? GetComponentInChildren<UnityEngine.UI.Button>();
        _mainMenuBtn.onClick.AddListener(() =>
        {
            HideWindow(null);
            UIInstance.Instance.GetWindow<SelectWindiow>().ShowType(CurrentData.Type);
        });
        base.PrepareUI(_onComplete);
    }

    private void ControllerOnOnChange(PortfolioContainerUI portfolioContainerUi, PointerEventData.InputButton inputButton)
    {
        GameCore.Instance.LoadSprite(portfolioContainerUi.CurrentData, OnLoadPhoto);
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

    public override void HideWindow(Action callback)
    {
        base.HideWindow(callback);
        if (CurrentData != null)
        {
            _fullSprite.sprite = null;
            _controller.UpdateDataView(new[] {"", "", ""});
            scroller.ReloadData();
            CurrentData = null;
        }
    }


    public override void RefreshView()
    {
        GameCore.Instance.LoadSprite(CurrentData.Foto, OnLoadPhoto);

        _fio.text = CurrentData.Name;
        _fullInfo.text = CurrentData.FullInfo;
        _contacts.text = CurrentData.Contatcts;
        _controller.UpdateDataView(CurrentData.Porfolio);
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

}
