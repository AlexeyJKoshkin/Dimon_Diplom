using UnityEngine;
using System.Collections;
using ShutEye.UI.Core;
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

    public override void RefreshView()
    {
        _AvatarImage.sprite = CurrentData.AvatarSprite;
        _nameText.text = CurrentData.Name;

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
