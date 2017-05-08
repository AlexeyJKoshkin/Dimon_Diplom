using ShutEye.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LoggerUI : MonoBehaviour, ILogHandler
{
    private Queue<string> _messages;

    public Text TextLog;

    public void InitLogger()
    {
        _messages = new Queue<string>();
        Debug.logger.logHandler = this;
    }

    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
        string log = "";
        args.ForEach(s => log += s);

        if (_messages.Count > 10)
        {
            _messages.Dequeue();
        }
        _messages.Enqueue(log);

        TextLog.text = "";

        _messages.ForEach(s => TextLog.text += s + "\n");
        Thread.Sleep(100);
    }

    public void LogException(Exception exception, Object context)
    {
    }
}