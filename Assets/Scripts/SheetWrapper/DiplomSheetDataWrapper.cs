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
using ShutEye.Core;
using ShutEye.Data;

public class DiplomSheetDataWrapper : MonoBehaviour
{
    private const string SelectTableName = "GoogleData";
    private const string ProfileTableName = "GoogleData";

    public const string LastVersionSelected = "LAST_VERSION_SELECTED";
    public const string LastVersionProfile = "LAST_VERSION_PROFILE";

    public static string PathSelectData
    {
        get { return Path.Combine(Application.persistentDataPath, "db_GoogleData.txt"); }
    }

    public static string PathProfileData
    {
        get { return Path.Combine(Application.persistentDataPath, "db_pfofileData.txt"); }
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

    private Dictionary<MenuItemType, List<BaseDataForSelectWindow>> _runtimeDbSelect = new Dictionary<MenuItemType, List<BaseDataForSelectWindow>>();

    private Dictionary<MenuItemType, List<BaseDataForProfileWindow>> _runTimeDB_Profile = new Dictionary<MenuItemType, List<BaseDataForProfileWindow>>();

    public IEnumerator GetSelectDb()
    {
        _runtimeDbSelect.Clear();
        foreach (MenuItemType t in Enum.GetValues(typeof(MenuItemType)))
        {
            _runtimeDbSelect.Add(t, new List<BaseDataForSelectWindow>());
        }

        if (string.IsNullOrEmpty(SelectTableName))
            yield break;
        var client = new DatabaseClient(GameCore.GoogleSettings);
        _getDbThread = new Thread(() =>
        {
            string error = string.Empty;
            _bd = client.GetDatabase(SelectTableName, ref error);
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
            lastVesr = PlayerPrefs.GetInt(LastVersionSelected, 0);
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
            PlayerPrefs.SetInt(LastVersionSelected, currentVersion);
            PlayerPrefs.Save();
            lastVesr = currentVersion;
            yield return UpdateSelectDB(client);

            var list = _runtimeDbSelect.Select(o => new SaveSelectData()
            {
                Type = o.Key,
                Data = o.Value.ToArray()
            }).ToList();
            SaveToSelectFile(PathSelectData, SEJsonConverter.Serialize(list));

        }
        else
        {
            var res = SEJsonConverter.Deserialize<List<SaveSelectData>>(LoadSelectTableFromFile(PathSelectData));
            _runtimeDbSelect = res.ToDictionary(o => o.Type, o => o.Data.ToList());
            yield return null;
        }

        // Fetch the cell feed of the worksheet.
    }

    public IEnumerator GetPfofileDb()
    {
        _runTimeDB_Profile.Clear();
        foreach (MenuItemType t in Enum.GetValues(typeof(MenuItemType)))
        {
            _runTimeDB_Profile.Add(t, new List<BaseDataForProfileWindow>());
        }

        if (string.IsNullOrEmpty(ProfileTableName))
            yield break;
        var client = new DatabaseClient(GameCore.GoogleSettings);
        _getDbThread = new Thread(() =>
        {
            string error = string.Empty;
            _bd = client.GetDatabase(ProfileTableName, ref error);
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
            lastVesr = PlayerPrefs.GetInt(LastVersionProfile, 0);
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
            PlayerPrefs.SetInt(LastVersionProfile, currentVersion);
            PlayerPrefs.Save();
            lastVesr = currentVersion;
            yield return UpdateProfileDB(client);
            {
                var list = _runTimeDB_Profile.Select(o => new SaveProfileData()
                {
                    Type = o.Key,
                    Data = o.Value.ToArray()
                }).ToList();
                SaveToSelectFile(PathProfileData, SEJsonConverter.Serialize(list));
            }
        }
        else
        {
            var res = SEJsonConverter.Deserialize<List<SaveProfileData>>(LoadSelectTableFromFile(PathProfileData));
            _runTimeDB_Profile = res.ToDictionary(o => o.Type, o => o.Data.ToList());
            yield return null;
        }

        // Fetch the cell feed of the worksheet.
    }

    private string LoadSelectTableFromFile(string path)
    {
        if (!File.Exists(path))
        {
            File.CreateText(path).Close();
        }
        return File.ReadAllText(path);
    }

    private void SaveToSelectFile(string path, string saveData)
    {
        if (!File.Exists(path))
        {
            File.CreateText(path).Close();
        }
        File.WriteAllText(path, saveData);
        GameCore.LogSys("Succesfull Update");
    }

    IEnumerator UpdateSelectDB(DatabaseClient client)
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
                        _runtimeDbSelect[sheet].Add(new BaseDataForSelectWindow()
                        {
                            Id = int.Parse(cell.Value)
                        }); break;
                    case 1: _runtimeDbSelect[sheet].Last().Name = cell.Value; break;
                    case 2: _runtimeDbSelect[sheet].Last().Price = cell.Value; break;
                    case 3:
                        _runtimeDbSelect[sheet].Last().AvatarSprite = cell.Value;
                        counter = -1; break;
                }
                counter++;
            }
            yield return null;
        }

        foreach (var VARIABLE in _runtimeDbSelect)
        {
            Debug.Log(VARIABLE.Key + " " + VARIABLE.Value.Count);
        }
    }


    IEnumerator UpdateProfileDB(DatabaseClient client)
    {
        foreach (MenuItemType sheet in Enum.GetValues(typeof(MenuItemType)))
        {
            var worksheet = ((Database)_bd).GetWorksheetEntry(sheet.ToString());
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            var cellFeed = client.SpreadsheetService.Query(cellQuery);
            int counter = 0;
            for (int i = 4; i < cellFeed.Entries.Count; i++)
            {
                //CellEntry cell = cellFeed.Entries[i] as CellEntry;
                //switch (counter)
                //{
                //    case 0:
                //        _runTimeDB_Profile[sheet].Add(new BaseDataForSelectWindow()
                //        {
                //            Id = int.Parse(cell.Value)
                //        }); break;
                //    case 1: _runTimeDB_Profile[sheet].Last().Name = cell.Value; break;
                //    case 2: _runTimeDB_Profile[sheet].Last().Price = cell.Value; break;
                //    case 3:
                //        _runTimeDB_Profile[sheet].Last().AvatarSprite = cell.Value;
                //        counter = -1; break;
                //}
                //counter++;
            }
            yield return null;
        }

        foreach (var VARIABLE in _runtimeDbSelect)
        {
            Debug.Log(VARIABLE.Key + " " + VARIABLE.Value.Count);
        }
    }

    public IList<BaseDataForSelectWindow> GetAllInfoAbout(MenuItemType type)
    {
        return _runtimeDbSelect[type];
    }
}