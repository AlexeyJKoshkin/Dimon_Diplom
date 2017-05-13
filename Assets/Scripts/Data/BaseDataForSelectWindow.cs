using UnityEngine;
using System.Collections;

/// <summary>
/// базовые данные под отображение
/// </summary>
public class BaseDataForSelectWindow : DataObject
{
    public string AvatarSprite;
    public string Price;
}

public class BaseDataForProfileWindow : DataObject
{
    public Sprite[] Porfolio;
    public string FullInfo;
    public Sprite Foto;
    public string Contatcts;
}
