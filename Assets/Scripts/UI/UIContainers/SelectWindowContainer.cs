﻿using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Контейнер для окна выбора
/// </summary>
public class SelectWindowContainer : SEUIContainerItem, IContainerUI<BaseDataForSelectWindow>
{
    /// <summary>
    /// Аватарка
    /// </summary>
    [SerializeField]
    private Image _AvatarImage;

    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Text _priceText;

    public override void RefreshView()
    {
        _AvatarImage.sprite = DiplomCore.Instance.DefaultSprite;
        DiplomCore.Instance.LoadSprite(CurrentData.AvatarSprite, OnLoadSprite);
        _nameText.text = CurrentData.Name;
        _priceText.text = CurrentData.Price;
    }

    public override void ClearView()
    {
        _AvatarImage.sprite = DiplomCore.Instance.DefaultSprite;
        _nameText.text = "";
        _priceText.text = "";
    }

    public void OnLoadSprite(Sprite sprite)
    {
        _AvatarImage.sprite = sprite;
    }

    public void UpdateDataView(BaseDataForSelectWindow newdata)
    {
        if (newdata != null)
        {
            CurrentData = newdata;
            RefreshView();
        }
    }

    public BaseDataForSelectWindow CurrentData { get; private set; }
}