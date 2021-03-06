﻿using System;

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
/// Данный по услугам
/// </summary>
public class BaseDataForSelectWindow : DataObject
{
    public string AvatarSprite;
    public string Price;
}

/// <summary>
/// данные профиля
/// </summary>
public class BaseDataForProfileWindow : DataObject
{
    public MenuItemType Type;
    public string[] Porfolio;
    public string FullInfo;
    public string Foto;
    public string Contatcts;
    public string Vk;
}