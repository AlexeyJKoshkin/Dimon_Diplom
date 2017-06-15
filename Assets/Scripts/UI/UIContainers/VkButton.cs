using ShutEye.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

public class VkButton : SEButtonUI<string>
{
    [SerializeField]
    private Text _text;

    protected override void PrepareUI(Action _onComplete)
    {
        this.ClickOnViewEvent += (o, btn) => Application.OpenURL(CurrentData);
        _text.color = Color.blue;
        base.PrepareUI(_onComplete);
    }

    public override void ClearView()
    {
        _text.text = "";
        base.ClearView();
    }

    public override void RefreshView()
    {
        _text.text = CurrentData;
    }
}