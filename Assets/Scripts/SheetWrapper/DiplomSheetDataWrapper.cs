using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using GDataDB;
using GDataDB.Impl;
using Google.GData.Spreadsheets;
using Newtonsoft.Json;
using ShutEye.Core;
using ShutEye.Data;

public class DiplomSheetDataWrapper : MonoBehaviour
{
    public string TableName = "GoogleData";

    public const string LastVersion = "LAST_VERSION";

    public static string PathData
    {
        get { return Path.Combine(Application.persistentDataPath, "db_GoogleData.txt"); }
    }


    /// <summary>
    /// Таблица
    /// </summary>
    private IDatabase _bd;

    /// <summary>
    /// поток для получения таблицы из гугла
    /// </summary>
    private Thread _getDbThread;

    public float Percent { get; private set; }

    private Dictionary<MenuItemType, List<BaseDataForSelectWindow>> _runtime_DB = new Dictionary<MenuItemType, List<BaseDataForSelectWindow>>();


    public IEnumerator GetDb()
    {
        _runtime_DB.Clear();
        foreach (MenuItemType t in Enum.GetValues(typeof(MenuItemType)))
        {
            _runtime_DB.Add(t, new List<BaseDataForSelectWindow>());
        }

        if (string.IsNullOrEmpty(TableName))
            yield break;
        var client = new DatabaseClient(GameCore.GoogleSettings);
        _getDbThread = new Thread(() =>
        {
            string error = string.Empty;
            _bd = client.GetDatabase(TableName, ref error);
        });
        _getDbThread.Start();
        while (_bd == null)
        {
            yield return null;
        }
        int lastVesr = 0;
        int currentVersion = 0;
        try
        {
            lastVesr = PlayerPrefs.GetInt(LastVersion, 0);
            GameCore.LogSys("Last Version {0}", lastVesr);
            var worksheet = ((Database)_bd).GetWorksheetEntry("Version");
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            var cellFeed = client.SpreadsheetService.Query(cellQuery);
            Debug.Log(((CellEntry)cellFeed.Entries[0]).Value);
            currentVersion = int.Parse(((CellEntry)cellFeed.Entries[1]).Value);
            GameCore.LogSys("Data Version {0}", currentVersion);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            yield break;
        }
        if (currentVersion > lastVesr)
        {
            GameCore.LogSys("Preprare Uptdate To {0}", currentVersion);
            PlayerPrefs.SetInt(LastVersion, currentVersion);
            PlayerPrefs.Save();
            lastVesr = currentVersion;
            yield return UpdateDB(client);
            yield return SaveToFile();
        }
        else
            yield return LoadFromFile();
        // Fetch the cell feed of the worksheet.
    }

    private IEnumerator LoadFromFile()
    {
        if (!File.Exists(PathData))
        {
            File.CreateText(PathData).Close();
        }
        string ser = File.ReadAllText(PathData);
        var res = SEJsonConverter.Deserialize<List<SaveDataStruct>>(ser);
        _runtime_DB = res.ToDictionary(o => o.Type, o => o.Data.ToList());
        yield return null;
        GameCore.LogSys("Succesfull Load");
    }

    private IEnumerator SaveToFile()
    {
        if (!File.Exists(PathData))
        {
            File.CreateText(PathData).Close();
        }
        var list = _runtime_DB.Select(o => new SaveDataStruct()
        {
            Type = o.Key,
            Data = o.Value.ToArray()
        }).ToList();
        File.WriteAllText(PathData, SEJsonConverter.Serialize(list));
        yield return null;

        GameCore.LogSys("Succesfull Update");
    }

    IEnumerator UpdateDB(DatabaseClient client)
    {
        foreach (MenuItemType sheet in Enum.GetValues(typeof(MenuItemType)))
        {
            var worksheet = ((Database)_bd).GetWorksheetEntry(sheet.ToString());
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            var cellFeed = client.SpreadsheetService.Query(cellQuery);
            int counter = 0;
            for (int i = 4; i < cellFeed.Entries.Count; i++)
            {
                CellEntry cell = cellFeed.Entries[i] as CellEntry;
                switch (counter)
                {
                    case 0:
                        _runtime_DB[sheet].Add(new BaseDataForSelectWindow()
                        {
                            Id = int.Parse(cell.Value)
                        }); break;
                    case 1: _runtime_DB[sheet].Last().Name = cell.Value; break;
                    case 2: _runtime_DB[sheet].Last().Price = cell.Value; break;
                    case 3:
                        _runtime_DB[sheet].Last().AvatarSprite = cell.Value;
                        counter = -1; break;
                }
                counter++;
            }
            yield return null;
        }

        foreach (var VARIABLE in _runtime_DB)
        {
            Debug.Log(VARIABLE.Key + " " + VARIABLE.Value.Count);
        }
    }

    public IList<BaseDataForSelectWindow> GetAllInfoAbout(MenuItemType type)
    {
        return _runtime_DB[type];
    }
}
