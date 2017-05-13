using System.Collections;
using ShutEye.Core;
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
        if (CurrentData.AvatarSprite == "No Photo")
        {
            _AvatarImage.sprite = GameCore.LoadSprite(CurrentData.AvatarSprite);
        }
        else
        {
            StartCoroutine(LoadPhoto(CurrentData.AvatarSprite));
        }
        _nameText.text = CurrentData.Name;
        _priceText.text = CurrentData.Price;
    }

    private IEnumerator LoadPhoto(string currentDataAvatarSprite)
    {
        var www = new WWW(currentDataAvatarSprite);
        yield return www;
        _AvatarImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        www.Dispose();
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
