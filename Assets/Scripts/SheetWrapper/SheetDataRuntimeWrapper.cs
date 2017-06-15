using GDataDB;
using GDataDB.Impl;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using ShutEye.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityQuickSheet;

public class SheetDataRuntimeWrapper : MonoBehaviour
{
    /// <summary>
    /// имена таблиц на гугл драйве
    /// </summary>
    private const string SelectTableName = "GoogleData";

    private const string ProfileTableName = "ProfileData";

    /// <summary>
    /// Ключи в которых сохраненны данные по локальной БД
    /// </summary>
    public const string LastVersionSelected = "LAST_VERSION_SELECTED";

    public const string LastVersionProfile = "LAST_VERSION_PROFILE";

    /// <summary>
    /// вспомогательный класс для работы с гугл таблицами
    /// </summary>
    private DatabaseClient client;

    /// <summary>
    /// Путь по которому храниться локальная БД
    /// </summary>
    public static string PathSelectData
    {
        get { return Path.Combine(Application.persistentDataPath, "db_GoogleData.txt"); }
    }

    /// <summary>
    /// Путь по которому храниться локальная БД профилей
    /// </summary>
    public static string PathProfileData
    {
        get { return Path.Combine(Application.persistentDataPath, "db_pfofileData.txt"); }
    }

    /// <summary>
    /// Очистисть локальную БД
    /// </summary>
    [ContextMenu(("Clean local Cash"))]
    public void ClearLocalCashData()
    {
        PlayerPrefs.SetInt(LastVersionProfile, 0);
        PlayerPrefs.SetInt(LastVersionSelected, 0);
        PlayerPrefs.Save();
        if (File.Exists(PathProfileData))
        {
            File.Delete(PathProfileData);
        }
        if (File.Exists(PathSelectData))
        {
            File.Delete(PathSelectData);
        }
    }

    /// <summary>
    /// Таблица
    /// </summary>
    private IDatabase _bdSelect;

    /// <summary>
    /// Таблица
    /// </summary>
    private IDatabase _bdProfile;

    /// <summary>
    /// локальная Модель данных приложения
    /// </summary>
    private Dictionary<MenuItemType, List<BaseDataForSelectWindow>> _runtimeDbSelect = new Dictionary<MenuItemType, List<BaseDataForSelectWindow>>();

    /// <summary>
    /// локальная Модель данных приложения профайлов
    /// </summary>
    private Dictionary<MenuItemType, List<BaseDataForProfileWindow>> _runTimeDB_Profile = new Dictionary<MenuItemType, List<BaseDataForProfileWindow>>();

    /// <summary>
    /// инициализация модели данных
    /// </summary>
    /// <param name="settings"></param>
    public void Init(GoogleDataSettings settings)
    {
        client = new DatabaseClient(settings); //создаем клиента для доступа к гугл диску
    }

    /// <summary>
    /// Обновить локальную БД со списком услуг
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetSelectDb()
    {
        _runtimeDbSelect.Clear(); //очистка

        //формирование списка услуг потипу
        foreach (MenuItemType t in Enum.GetValues(typeof(MenuItemType)))
        {
            _runtimeDbSelect.Add(t, new List<BaseDataForSelectWindow>());
        }

        //Загрузка таблицы с гугл диска
        yield return LoadBD(client, (bd) => _bdSelect = bd, SelectTableName);

        #region Получение версии таблицы

        int lastVesr = PlayerPrefs.GetInt(LastVersionSelected, 0);
        int currentVersion = 0;
        try
        {
            Debug.LogFormat("Last Version {0}", lastVesr);
            currentVersion = GetVersion(_bdSelect as Database, client);
            Debug.LogFormat("Data Version {0}", currentVersion);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            yield break;
        }

        #endregion Получение версии таблицы

        //если версия интернета свежеее
        if (currentVersion > lastVesr)
        {
            Debug.LogFormat("Preprare Uptdate To {0}", currentVersion);
            lastVesr = currentVersion;
            //обновляем локальную БД
            yield return UpdateDB(client, _bdSelect as Database, AddSelectTableData);

            var list = _runtimeDbSelect.Select(o => new SaveSelectData()
            {
                Type = o.Key,
                Data = o.Value.ToArray()
            }).ToList();
            //сохранить изменения на устройстве
            SaveToSelectFile(PathSelectData, SEJsonConverter.Serialize(list));
            PlayerPrefs.SetInt(LastVersionSelected, currentVersion);
            PlayerPrefs.Save();
        }
        else
        {
            //подготовка данных для работы приложения
            var res = SEJsonConverter.Deserialize<List<SaveSelectData>>(LoadSelectTableFromFile(PathSelectData));
            _runtimeDbSelect = res.ToDictionary(o => o.Type, o => o.Data.ToList());
            yield return null;
        }
    }

    public IEnumerator GetPfofileDb()
    {
        _runTimeDB_Profile.Clear();
        //очистка

        //формирование списка людей потипу
        foreach (MenuItemType t in Enum.GetValues(typeof(MenuItemType)))
        {
            _runTimeDB_Profile.Add(t, new List<BaseDataForProfileWindow>());
        }
        //Загрузка таблицы с гугл диска
        yield return LoadBD(client, bd => _bdProfile = bd, ProfileTableName);

        #region Получение версии таблицы

        int lastVesr = PlayerPrefs.GetInt(LastVersionProfile, 0);
        int currentVersion = 0;
        try
        {
            Debug.LogFormat("Last Prfile Version {0}", lastVesr);
            currentVersion = GetVersion(_bdProfile as Database, client);
            Debug.LogFormat("Prfile Version {0}", currentVersion);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            yield break;
        }

        #endregion Получение версии таблицы

        if (currentVersion > lastVesr)
        {
            Debug.LogFormat("Preprare Update Prfile To {0}", currentVersion);
            //обновляем локальную БД
            yield return UpdateDB(client, _bdProfile as Database, AddPfofileTableData);
            {
                var list = _runTimeDB_Profile.Select(o => new SaveProfileData()
                {
                    Type = o.Key,
                    Data = o.Value.ToArray()
                }).ToList();
                //сохранить изменения на устройстве
                SaveToSelectFile(PathProfileData, SEJsonConverter.Serialize(list));
                PlayerPrefs.SetInt(LastVersionProfile, currentVersion);
                PlayerPrefs.Save();
            }
        }
        else
        {
            //подготовка данных для работы приложения
            var res = SEJsonConverter.Deserialize<List<SaveProfileData>>(LoadSelectTableFromFile(PathProfileData));
            _runTimeDB_Profile = res.ToDictionary(o => o.Type, o => o.Data.ToList());
            yield return null;
        }

        // Fetch the cell feed of the worksheet.
    }

    /// <summary>
    /// Загрузка таблица, по ее имени
    /// </summary>
    /// <param name="client"></param>
    /// <param name="dbSetter"></param>
    /// <param name="TableName"></param>
    /// <returns></returns>
    private IEnumerator LoadBD(DatabaseClient client, Action<IDatabase> dbSetter, string TableName)
    {
        IDatabase bd = null;
        var _getDbThread = new Thread(() =>
        {
            string error = string.Empty;
            bd = client.GetDatabase(TableName, ref error);
        });
        _getDbThread.Start();
        while (bd == null)
        {
            yield return null;
        }
        if (dbSetter != null)
            dbSetter.Invoke(bd);
    }

    /// <summary>
    /// Обновление локальной БД
    /// </summary>
    /// <param name="client"></param>
    /// <param name="bd"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator UpdateDB(DatabaseClient client, Database bd, Action<AtomEntryCollection, MenuItemType> action)
    {
        foreach (MenuItemType sheet in Enum.GetValues(typeof(MenuItemType)))
        {
            var worksheet = bd.GetWorksheetEntry(sheet.ToString());
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            var cellFeed = client.SpreadsheetService.Query(cellQuery);
            action(cellFeed.Entries, sheet);
            yield return null;
        }
    }

    private void AddSelectTableData(AtomEntryCollection colectCollection, MenuItemType sheet)
    {
        int counter = 0;
        for (int i = 4; i < colectCollection.Count; i++)
        {
            CellEntry cell = colectCollection[i] as CellEntry;
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
    }

    private void AddPfofileTableData(AtomEntryCollection colectCollection, MenuItemType sheet)
    {
        int counter = 0;
        for (int i = 6; i < colectCollection.Count; i++)
        {
            CellEntry cell = colectCollection[i] as CellEntry;
            switch (counter)
            {
                case 0:
                    _runTimeDB_Profile[sheet].Add(new BaseDataForProfileWindow()
                    {
                        Id = int.Parse(cell.Value),
                        Type = sheet
                    }); break;
                case 1: _runTimeDB_Profile[sheet].Last().Name = cell.Value; break;
                case 2: _runTimeDB_Profile[sheet].Last().FullInfo = cell.Value; break;
                case 3: _runTimeDB_Profile[sheet].Last().Foto = cell.Value; break;
                case 4:
                    {
                        var list = cell.Value.Split('\n').ToList();
                        list.RemoveAll(string.IsNullOrEmpty);
                        _runTimeDB_Profile[sheet].Last().Contatcts = list[0];
                        _runTimeDB_Profile[sheet].Last().Vk = "https://" + list[1];
                        break;
                    }
                case 5:
                    _runTimeDB_Profile[sheet].Last().Porfolio = cell.Value.Split('\n');
                    counter = -1; break;
            }
            counter++;
        }
    }

    /// <summary>
    /// Загрузить данные из файла на устройстве
    /// </summary>
    /// <param name="path">путь</param>
    /// <returns></returns>
    private string LoadSelectTableFromFile(string path)
    {
        if (!File.Exists(path))
        {
            File.CreateText(path).Close();
        }
        return File.ReadAllText(path);
    }

    /// <summary>
    /// сохранить данный в файл
    /// </summary>
    /// <param name="path">путь</param>
    /// <param name="saveData">данный</param>
    private void SaveToSelectFile(string path, string saveData)
    {
        if (!File.Exists(path))
        {
            File.CreateText(path).Close();
        }
        File.WriteAllText(path, saveData);
        Debug.LogFormat("Succesfull Update");
    }

    /// <summary>
    /// получить версию таблицы
    /// </summary>
    /// <param name="bd"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    private int GetVersion(Database bd, DatabaseClient client)
    {
        if (bd == null)
        {
            Debug.LogError("Db is NULL");
            return 0;
        }
        var worksheet = (bd).GetWorksheetEntry("Version");
        CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
        var cellFeed = client.SpreadsheetService.Query(cellQuery);
        Debug.Log(((CellEntry)cellFeed.Entries[0]).Value);
        return int.Parse(((CellEntry)cellFeed.Entries[1]).Value);
    }

    /// <summary>
    /// получить список всех услуг в данной категории
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public IList<BaseDataForSelectWindow> GetAllInfoAbout(MenuItemType type)
    {
        return _runtimeDbSelect[type];
    }

    /// <summary>
    /// получить информацию по личному номеру человека в конкретной категории
    /// </summary>
    /// <param name="currentViewType">категория</param>
    /// <param name="currentDataId">личный номер при регистрации</param>
    /// <returns></returns>
    public BaseDataForProfileWindow GetFullInfo(MenuItemType currentViewType, int currentDataId)
    {
        return _runTimeDB_Profile[currentViewType].FirstOrDefault(o => o.Id == currentDataId);
    }
}