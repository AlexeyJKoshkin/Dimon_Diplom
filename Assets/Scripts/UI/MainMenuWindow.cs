﻿using GameKit.UI;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Контролер главного меню
/// </summary>
public class MainMenuWindow : BaseWindow
{
    public override Enum TypeWindow
    {
        get { return WindowType.Menu; }
    }

    public override WindowState State { get; set; }

    /// <summary>
    /// список категорий
    /// </summary>
    [SerializeField]
    private MenuItemController _itemsContainer;

    [SerializeField] private Button _info;

    /// <summary>
    /// метод инициализации вызывается из DiplomU.
    /// подписка на события, и клики пользователя
    /// </summary>
    /// <param name="_onComplete"></param>
    protected override void PrepareUI(Action _onComplete)
    {

        if (_itemsContainer.Containers.Length == 0) Application.Quit();

        _info.onClick.AddListener(()=> UIInstance.Instance.GetWindow<AboutCompanyWindow>().ShowWindow(null));
        _itemsContainer.OnChange += ItemsContainerOnOnChange;
        base.PrepareUI(_onComplete);
    }

    /// <summary>
    /// метод обработки клика на категории
    /// </summary>
    /// <param name="mainMenuItemContainer"></param>
    /// <param name="inputButton"></param>
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