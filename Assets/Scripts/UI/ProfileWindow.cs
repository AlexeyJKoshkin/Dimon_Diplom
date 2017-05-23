using System;
using GameKit;
using GameKit.UI;
using ShutEye.Core;
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

    [SerializeField]
    private PortfolioContainersController _allPhotos;

    protected override void PrepareUI(Action _onComplete)
    {
        _mainMenuBtn = _mainMenuBtn ?? GetComponentInChildren<UnityEngine.UI.Button>();
        _mainMenuBtn.onClick.AddListener(this.BackMainMenu);
        base.PrepareUI(_onComplete);
    }

    public override void RefreshView()
    {
        GameCore.Instance.LoadSprite(CurrentData.Foto, OnLoadPhoto);
        
        _fio.text = CurrentData.Name;
        _fullInfo.text = CurrentData.FullInfo;
        _contacts.text = CurrentData.Contatcts;
        for (int i = 0; i < CurrentData.Porfolio.Length; i++)
        {
            _allPhotos.GetSlotByN(i).UpdateDataView(CurrentData.Porfolio[i]);
        }
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
