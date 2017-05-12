using UnityEngine;
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

    [SerializeField]
    private Text _priceText;

    public override void RefreshView()
    {
        _AvatarImage.sprite = CurrentData.AvatarSprite;
        _nameText.text = CurrentData.Name;
        _priceText.text = CurrentData.Price;
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
