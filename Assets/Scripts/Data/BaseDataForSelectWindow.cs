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
    public string[] Porfolio;
    public string FullInfo;
    public string Foto;
    public string Contatcts;
}
