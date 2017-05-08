using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class ServerInfo
{
    public string serverIp = "";

    public int version = 1;
    public int subVersion = 1;
    public string serverVersion = "1";

    public int maxTimeToCheckConnection = 20;

    public bool connectionEnable = false;
}

public class RSDebug : MonoBehaviour
{
    public string AppBundleVErsion = "Enter Version";

    public GUISkin debugSkin;

    //стиль дебага
    public string debuger;

    //
    private bool showDebug = false;

    //отображение дебага
    public bool fullDebug = false;

    //отображение полногодебага

    private float fps = 0;

    [SerializeField]
    private string _fileName = "LOG_Snake.txt";

    [SerializeField]
    private bool _enableFileLog;

    [ContextMenu("ClearPrefs")]
    public void ClearPreffs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Caching.CleanCache();
    }

    private string fullpathLog
    { get { return Application.persistentDataPath + "/" + _fileName; } }

    private StreamWriter _textFile;

    private string TestPrefs;

    private void Awake()
    {
        DebugMSG("[Log]", "Path lOG : " + fullpathLog);
        if (!_enableFileLog) return;
        GetLogFile();

        TestPrefs = PlayerPrefs.GetString("TEST RESET PREFS", "FIRTS TIME");

        if (TestPrefs == "FIRTS TIME")
        {
            PlayerPrefs.SetString("TEST RESET PREFS", "HAHHAH");
            PlayerPrefs.Save();
        }
    }

    private void GetLogFile()
    {
        if (!File.Exists(fullpathLog))
        {
            DebugMSG("[Log]", "CREARE lOG : " + fullpathLog);
            var file = File.Create(fullpathLog);
            file.Close();
            file.Dispose();
        }
        try
        {
            _textFile = File.AppendText(fullpathLog);
            DebugMSG("[Log]", "Opened");
        }
        catch (Exception ex)
        {
            DebugMSG("[Log]", "Error while OPEN file " + ex.Message);
        }
    }

    private void OnGUI()
    {
        GUI.skin = debugSkin;

        //кнопка RESET - загружает уровень 4
        if (GUI.Button(new Rect(0, Screen.height - Screen.height / 10, 50, Screen.height / 10), "Clear Prefs"))
        {
            ClearPreffs();
            //SYS_Info.loadingStage = ServerInfo.LoadingStage.error;
            //kiiroGamecall.sceneToLoad = 4;
        }
        //FORCE EXIT
        if (GUI.Button(new Rect(0, 0, 50, 40), "ForceQuit"))
        {
            Application.Quit();
            //SYS_Info.loadingStage = ServerInfo.LoadingStage.error;
            //kiiroGamecall.sceneToLoad = 4;
        }
        //кнопка DEBUG
        if (GUI.Button(new Rect(50, Screen.height - Screen.height / 10, 50, Screen.height / 10), "debug"))
        {
            showDebug = !showDebug;
        }

        if (GUI.Button(new Rect(120, Screen.height - Screen.height / 10, 70, Screen.height / 10), "CLEAR"))
        {
            debuger = "";
        }

        if (GUI.Button(new Rect(260, Screen.height - Screen.height / 10, 70, Screen.height / 10), "Clear log"))
        {
            try
            {
                if (_textFile != null)
                {
                    _textFile.Close();
                    _textFile.Dispose();
                    File.WriteAllText(fullpathLog, "");
                    GetLogFile();
                }
            }
            catch (Exception ex)
            {
                DebugMSG("[Log]", "Error while OPEN file " + ex.Message);
            }
        }

        //if (GUI.Button (new Rect (190, Screen.height - Screen.height / 10, 70, Screen.height / 10), "Send log"))
        //{
        //	GameCore.SendMail (CopyToClipboard (Application.persistentDataPath + "/" + _fileName));
        //}

        if (showDebug)
        {
            GUI.Box(new Rect(15, 20, (Screen.width - 30), (Screen.height - 40)), new GUIContent(debuger));
            //GUI.Box(new Rect(15, 20, (Screen.width - 30), (Screen.height - 40)), debuger, "Debuger");
        }
        GUI.Box(new Rect(Screen.width - 460, Screen.height - 20, 100, 20), "Ver: " + TestPrefs);
        GUI.Box(new Rect(Screen.width - 360, Screen.height - 20, 100, 20), "FPS: " + fps);
    }

    private string CopyToClipboard(string fullpath)
    {
        try
        {
            if (File.Exists(fullpath))
            {
                _textFile.Close();
                _textFile.Dispose();
                _textFile = null;

                StreamReader inp_stm = new StreamReader(fullpath);
                var res = inp_stm.ReadToEnd();
                inp_stm.Close();
                return res;
            }
            _textFile = File.AppendText(fullpath);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return "Empty";
    }

    //считаем fps. Запускаем в Start()
    private IEnumerator FPSCheck()
    {
        fps = (1 / Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FPSCheck());
    }

    private void Start()
    {
        Application.targetFrameRate = 60; //нужный fps
                                          //	Application.logMessageReceived += OnLog;
        Application.logMessageReceivedThreaded += OnLog;
        StartCoroutine(FPSCheck());
    }

    private void OnLog(string condition, string stackTrace, LogType type)
    {
        DebugMSG(type.ToString(), condition);
        if (type == LogType.Exception)
            DebugMSG("Stack ", stackTrace);
    }

    public string getHash(string input)
    {
        byte[] byteData = Encoding.Default.GetBytes(input);

        SHA1 sha1 = new SHA1CryptoServiceProvider();
        byte[] hash = sha1.ComputeHash(byteData);

        string output = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

        return output;
    }

    //вывод влог делать с использованием этого метода, а не Debug.Log().
    //Вывод будет и на устройстве и в логах
    public void DebugMSG(string plugin, object msg)
    {
        string time = System.DateTime.Now.ToString("HH:mm:ss");

        string toShow = "(" + time + ") " + plugin + ": " + msg.ToString();
        ;
        debuger = toShow + "\n\n" + debuger;

        if (debuger.Length > 5000)
        {
            debuger = "";
        }
        if (_textFile != null)
            _textFile.WriteLine(toShow);
        //  Debug.Log(toShow);
    }

    private void OnDestroy()
    {
        if (_textFile != null)
        {
            _textFile.Close();
            _textFile.Dispose();
        }
    }

    public void Log(string message, params object[] formatArgs)
    {
        string mes = "";

        foreach (var sss in formatArgs)
            mes = mes + sss.ToString() + " ";
        DebugMSG(message, mes);
    }

    public void LogWarning(string message, params object[] formatArgs)
    {
        DebugMSG("Warning", "");
        foreach (var sss in formatArgs)
            DebugMSG("", sss);
    }

    public void LogError(string message, params object[] formatArgs)
    {
        DebugMSG("Error", "");
        foreach (var sss in formatArgs)
            DebugMSG("", sss);
    }

    public string prefix { get; set; }
}