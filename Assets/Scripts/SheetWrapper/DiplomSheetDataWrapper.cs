using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GDataDB;
using GDataDB.Impl;
using Google.GData.Spreadsheets;
using ShutEye.Core;

public class DiplomSheetDataWrapper : MonoBehaviour
{
    public string TableName = "GoogleData";

    
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

        bool ready = false;
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
                    case 0: _runtime_DB[sheet].Add(new BaseDataForSelectWindow()
                        {
                            Id = int.Parse(cell.Value)
                        }); break;
                    case 1: _runtime_DB[sheet].Last().Name = cell.Value; break;
                    case 2: _runtime_DB[sheet].Last().Price = cell.Value; break;
                    case 3: _runtime_DB[sheet].Last().AvatarSprite = cell.Value;
                        counter = -1; break;
                }
                counter++;
            }
           // ready = false;
           //var hread = new Thread(() =>
           // {
         
           //     ready = true;
           // });

           // hread.Start();
           // while (ready == false)
           // {
           //     yield return null;
           // }
        }

        foreach (var VARIABLE in _runtime_DB)
        {
            Debug.Log(VARIABLE.Key + " " + VARIABLE.Value.Count);
        }
        
        // Fetch the cell feed of the worksheet.
        
    }

    public IList<BaseDataForSelectWindow> GetAllInfoAbout(MenuItemType type)
    {
        return _runtime_DB[type];
    }
}
