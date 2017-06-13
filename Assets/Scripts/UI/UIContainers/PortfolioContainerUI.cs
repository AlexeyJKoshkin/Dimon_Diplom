using System;
using UnityEngine;
using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine.UI;

public class PortfolioContainerUI : SEButtonUI<string>
{
    public Sprite LoadPhoto {
        get { return _foto.sprite; }
    }

    [SerializeField]
    private Image _foto;


    protected override void PrepareUI(Action _onComplete)
    {
        var la = GetComponent<LayoutElement>();
        if (la != null)
        {
            var delta = _foto.GetComponent<RectTransform>().sizeDelta;
            _foto.GetComponent<RectTransform>().sizeDelta = new Vector2(la.minWidth, delta.y);
        }
         
        base.PrepareUI(_onComplete);
    }

    public override void ClearView()
    {
        _foto.sprite = null;
        base.ClearView();
    }

    public override void RefreshView()
    {
        GameCore.Instance.LoadSprite(CurrentData, OnLoadPhoto);
    }

    private void OnLoadPhoto(Sprite obj)
    {
        _foto.sprite = obj;
    }
}
