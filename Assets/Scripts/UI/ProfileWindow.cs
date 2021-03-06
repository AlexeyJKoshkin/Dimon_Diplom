﻿using EnhancedUI.EnhancedScroller;
using GameKit;
using GameKit.UI;
using ShutEye.Core;
using ShutEye.UI.Core;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Окно профиля приложения
/// </summary>
public class ProfileWindow : BaseWindow, IDataBinding<BaseDataForProfileWindow>
{
    #region Элементы интерфейса

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

    [SerializeField]
    private VkButton _vk;

    #endregion Элементы интерфейса

    #region Скролл с превью портфолию

    /// <summary>
    /// The scroller to control
    /// </summary>
    public EnhancedScroller scroller;

    [SerializeField]
    private scrolportfolioContrpller _controller;

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

    #endregion Скролл с превью портфолию

    /// <summary>
    /// метод инициализации вызывается из DiplomU.
    /// подписка на события, и клики пользователя
    /// </summary>
    /// <param name="_onComplete"></param>
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
            UIInstance.Instance.GetWindow<SelectWindiow>().ShowType(CurrentData.Type);
            HideWindow(null);
        });
        base.PrepareUI(_onComplete);
    }

    /// <summary>
    /// клик на фото из превью - отобразить фотку на месте аватарки
    /// </summary>
    /// <param name="portfolioContainerUi"></param>
    /// <param name="inputButton"></param>
    private void ControllerOnOnChange(PortfolioContainerUI portfolioContainerUi, PointerEventData.InputButton inputButton)
    {
        DiplomCore.Instance.LoadSprite(portfolioContainerUi.CurrentData, OnLoadPhoto);
    }

    /// <summary>
    /// спрятать окно, очистить всю информацию о профайле
    /// </summary>
    /// <param name="callback"></param>
    public override void HideWindow(Action callback)
    {
        base.HideWindow(callback);
        foreach (var portfolioContainerUi in _controller.GetComponentsInChildren<PortfolioContainerUI>())
        {
            portfolioContainerUi.ClearView();
        }
        _fullSprite.sprite = null;
    }

    /// <summary>
    /// обновить информацию в UI
    ///
    /// выставляем фотки, контакты и тому подобное
    /// </summary>
    public override void RefreshView()
    {
        DiplomCore.Instance.LoadSprite(CurrentData.Foto, OnLoadPhoto);

        _fio.text = CurrentData.Name;
        _fullInfo.text = CurrentData.FullInfo;
        _contacts.text = CurrentData.Contatcts;
        _vk.UpdateDataView(CurrentData.Vk);
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