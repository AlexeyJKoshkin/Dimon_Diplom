using UnityEngine;
using ShutEye.Core;
using ShutEye.UI.Core;
using UnityEngine.UI;

public class PortfolioContainerUI : SEButtonUI<string>
{
    [SerializeField]
    private Image _foto;

    public override void RefreshView()
    {
        GameCore.Instance.LoadSprite(CurrentData, OnLoadPhoto);
    }

    private void OnLoadPhoto(Sprite obj)
    {
        _foto.sprite = obj;
    }
}
