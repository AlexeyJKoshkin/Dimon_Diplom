﻿using System;
using GameKit;
using GameKit.UI;
using UnityEngine;
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

    protected override void PrepareUI(Action _onComplete)
    {
        _mainMenuBtn = _mainMenuBtn ?? GetComponentInChildren<UnityEngine.UI.Button>();
        base.PrepareUI(_onComplete);
    }

    public override void RefreshView()
    {
        _fullSprite.sprite = CurrentData.Foto;
        _fio.text = CurrentData.Name;
        _fullInfo.text = CurrentData.FullInfo;
        _contacts.text = CurrentData.Contatcts;
    }

    public void UpdateDataView(BaseDataForProfileWindow newdata)
    {
        if (newdata != null)
        {
            CurrentData = newdata;
        }
    }

    public BaseDataForProfileWindow CurrentData { get; private set; }
}