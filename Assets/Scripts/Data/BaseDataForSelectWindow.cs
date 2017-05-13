using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct SaveDataStruct
{
    public MenuItemType Type;
    public BaseDataForSelectWindow[] Data;
}

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
