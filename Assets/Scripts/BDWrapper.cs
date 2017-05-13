using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GDataDB;
using GDataDB.Impl;
using Google.GData.Spreadsheets;
using ShutEye.Core;
using UnityEngine;
using UnityQuickSheet;

public static class BDWrapper
{
    static IList<BaseDataForSelectWindow> GetAllInfoAbout(MenuItemType type)
    {
        List<BaseDataForSelectWindow> result = new List<BaseDataForSelectWindow>();

        //switch (type)
        //{
        //    case MenuItemType.Leading:
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //           Id= 1,
        //            Name = "Дмитрий Кошкин",
        //            AvatarSprite = @"Iconc\avatar_leading\Koshkin",
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 2,
        //            Name = "Владимир Пахомов",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_leading\Pakhomov"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 3,
        //            Name = "Максим Лобов",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_leading\Lobov"),
        //            Price = "Blabala"
        //        });
        //        break;
        //    case MenuItemType.Music:
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 1,
        //            Name = "French",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_music\French"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 2,
        //            Name = "Dj_Lime",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_music\Dj_Lime"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 3,
        //            Name = "Hobots",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_music\Hobots"),
        //            Price = "Blabala"
        //        });
        //        break;
        //    case MenuItemType.Fotographer:
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 1,
        //            Name = "Вероника Чернявская",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Fotographer\Chernyavskaya"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 2,
        //            Name = "Христя Мармонов",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Fotographer\Marmarov"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 3,
        //            Name = "Александр Сеоев",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Fotographer\Seoev"),
        //            Price = "Blabala"
        //        });
        //        break;
        //    case MenuItemType.Decorator:
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 1,
        //            Name = "Сад",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Decorator\Sad"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 2,
        //            Name = "Пронина Анастасия ",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Decorator\Pronina"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id= 3,
        //            Name = "Чудное Мгновение",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Decorator\Mgnovenie"),
        //            Price = "Blabala"
        //        });
        //        break;
        //    case MenuItemType.Videograph:
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id = 1,
        //            Name = "Алексей Иванов",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Videograph\Ivanov"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id = 2,
        //            Name = "Артем Дрягин",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Videograph\Draygin"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id = 3,
        //            Name = "Сергей Студеникин",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_Videograph\Studenikin"),
        //            Price = "Blabala"
        //        });
        //        break;
        //    case MenuItemType.ShowProgramm:
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id = 1,
        //            Name = "Старт Наука",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_ShowProgramm\Start"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id = 2,
        //            Name = "Арт-Бар",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_ShowProgramm\ArtBar"),
        //            Price = "Blabala"
        //        });
        //        result.Add(new BaseDataForSelectWindow()
        //        {
        //            Id = 3,
        //            Name = "Бар-Молекула",
        //            AvatarSprite = GameCore.LoadSprite(@"Iconc\avatar_ShowProgramm\Molecula"),
        //            Price = "Blabala"
        //        });
        //        break;
        //    default:
        //        throw new ArgumentOutOfRangeException("type", type, null);
        //}
        return result;
    }

    /// <summary>
    /// A delegate called on each of a cell query.
    /// </summary>
    delegate void OnEachCell(CellEntry cell);

    /// <summary>
    /// Connect to google-spreadsheet with the specified account and password 
    /// then query cells and call the given callback.
    /// </summary>
    private static void DoCellQuery(BaseRuntimeMachine machine, OnEachCell onCell)
    {
        // first we need to connect to the google-spreadsheet to get all the first row of the cells
        // which are used for the properties of data class.
        var client = new DatabaseClient(GameCore.GoogleSettings);

        if (string.IsNullOrEmpty(machine.SpreadSheetName))
            return;
        if (string.IsNullOrEmpty(machine.WorkSheetName))
            return;

        string error = string.Empty;
        var db = client.GetDatabase(machine.SpreadSheetName, ref error);
        if (db == null)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(error))
                message = @"Unknown error.";
            else
                message = string.Format(@"{0}", error);

            message += "\n\nOn the other hand, see 'GoogleDataSettings.asset' file and check the oAuth2 setting is correctly done.";
            Debug.LogError(message);
          //  EditorUtility.DisplayDialog("Error", message, "OK");
            return;
        }

        // retrieves all cells
        var worksheet = ((Database)db).GetWorksheetEntry(machine.WorkSheetName);

        // Fetch the cell feed of the worksheet.
        CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
        var cellFeed = client.SpreadsheetService.Query(cellQuery);

        // Iterate through each cell, printing its value.
        foreach (CellEntry cell in cellFeed.Entries)
        {
            if (onCell != null)
                onCell(cell);
        }
    }

    /// <summary>
    /// Connect to the google spreadsheet and retrieves its header columns.
    /// </summary>
    public static List<ColumnHeader> Import(BaseRuntimeMachine machine, bool reimport = false)
    {
        Regex re = new Regex(@"\d+");

        Dictionary<string, ColumnHeader> headerDic = null;
        if (reimport)
            machine.ColumnHeaderList.Clear();
        else
            headerDic = machine.ColumnHeaderList.ToDictionary(k => k.name);

        List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

        int order = 0;
        // query the first columns only.
        DoCellQuery(machine,(cell) =>
        {
            Debug.Log(cell.Title.Text);
            Debug.Log(cell.Title.Type);
            Debug.Log(cell.Value);
            Debug.Log("_______________");
            // get numerical value from a cell's address in A1 notation
            // only retrieves first column of the worksheet 
            // which is used for member fields of the created data class.
            Match m = re.Match(cell.Title.Text);
            if (int.Parse(m.Value) > 1)
                return;

            // check the column header is valid
            if (!IsValidHeader(cell.Value))
            {
                string error = string.Format(@"Invalid column header name {0}. Any c# keyword should not be used for column header. Note it is not case sensitive.", cell.Value);
                Debug.LogError(error);
//                EditorUtility.DisplayDialog("Error", error, "OK");
                return;
            }

            ColumnHeader column = ParseColumnHeader(cell.Value, order++);
            if (headerDic != null && headerDic.ContainsKey(cell.Value))
            {
                // if the column is already exist, copy its name and type from the exist one.
                ColumnHeader h = machine.ColumnHeaderList.Find(x => x.name == column.name);
                if (h != null)
                {
                    column.type = h.type;
                    column.isArray = h.isArray;
                }
            }

            tmpColumnList.Add(column);
        });

        // update (all of settings are reset when it reimports)
        machine.ColumnHeaderList = tmpColumnList;
        return tmpColumnList;

        //EditorUtility.SetDirty(machine);
        //AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Check the given header column has valid name which should not be any c# keywords.
    /// </summary>
    private static bool IsValidHeader(string s)
    {
        // no case sensitive!
        string comp = s.ToLower();

        string found = Array.Find(Util.Keywords, x => x == comp);
        if (string.IsNullOrEmpty(found))
            return true;

        return false;
    }

    /// <summary>
    /// Try to parse column-header if it contains '|'. Note postfix '!' means it has array type.
    /// e.g) 'Skill|string': Skill is string type.
    ///      'MyArray | int!' : MyArray is int array type. 
    /// </summary>
    /// <param name="s">A column header string in the spreadsheet.</param>
    /// <param name="order">A order number to sort column header.</param>
    /// <returns>A newly created ColumnHeader class instance.</returns>
    private static ColumnHeader ParseColumnHeader(string columnheader, int order)
    {
        // remove all white space. e.g.) "SkillLevel | uint"
        string cHeader = new string(columnheader.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());

        CellType ctype = CellType.Undefined;
        bool bArray = false;
        if (cHeader.Contains('|'))
        {
            // retrive columnheader name.
            string substr = cHeader;
            bArray = cHeader.Contains("!");
            substr = cHeader.Substring(0, cHeader.IndexOf('|'));

            // retrieve CellType from the columnheader.
            int startIndex = cHeader.IndexOf('|') + 1;
            int length = cHeader.Length - cHeader.IndexOf('|') - (bArray ? 2 : 1);
            string strType = cHeader.Substring(startIndex, length).ToLower();
            ctype = (CellType)Enum.Parse(typeof(CellType), strType, true);

            return new ColumnHeader { name = substr, type = ctype, isArray = bArray, OrderNO = order };
        }

        return new ColumnHeader { name = cHeader, type = CellType.Undefined, OrderNO = order };
    }

}
