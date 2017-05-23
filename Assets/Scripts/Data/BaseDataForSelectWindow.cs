using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class SaveSelectData : BaseSaveTableData
{
    public BaseDataForSelectWindow[] Data;
}

[Serializable]
public class SaveProfileData : BaseSaveTableData
{
    public BaseDataForProfileWindow[] Data;
}



[Serializable]
public abstract class BaseSaveTableData
{
    public MenuItemType Type;
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
    public MenuItemType Type;
    public string[] Porfolio;
    public string FullInfo;
    public string Foto;
    public string Contatcts;
}
