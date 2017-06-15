using GameKit.UI;
using System;
using UnityEngine;

/// <summary>
/// Про команию
/// </summary>
public class AboutCompanyWindow : BaseWindow
{
    #region логические элементы интерфейса

    public override Enum TypeWindow
    {
        get { return WindowType.AboutCompany; }
    }

    public override WindowState State { get; set; }

    [SerializeField]
    private UnityEngine.UI.Button _closeButton;

    [SerializeField] private VkButton _vkButton;

    [SerializeField] private VkButton _siteBtn;

    #endregion логические элементы интерфейса

    protected override void PrepareUI(Action _onComplete)
    {
        _closeButton.onClick.AddListener(() => HideWindow(null));
        base.PrepareUI(_onComplete);
    }

    public override void RefreshView()
    {
    }

    public override void ShowWindow(Action callback)
    {
        _vkButton.UpdateDataView("https://vk.com/gc_may");
        _siteBtn.UpdateDataView("http://may2010.ru/");
        base.ShowWindow(callback);
    }
}