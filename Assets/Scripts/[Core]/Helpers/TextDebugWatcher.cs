using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextDebugWatcher : MonoBehaviour
{
    [SerializeField]
    private int _logSize = 10;

    private Text _text;

    private List<string> _msgs = new List<string>();

    private void Awake()
    {
        _msgs.Clear();
        _text = GetComponent<Text>();
        _text.text = String.Empty;
        Application.logMessageReceivedThreaded += OnLog;
    }

    private void OnLog(string condition, string stackTrace, LogType type)
    {
        _msgs.Add(String.Format("{0}{1}", condition, Environment.NewLine));
        if (_msgs.Count > _logSize)
        {
            _msgs = Enumerable.Reverse(_msgs).Take(_logSize).Reverse().ToList();
        }

        _text.text = _msgs.Aggregate((text, nextText) => String.Format("{0}{1}{2}", text, Environment.NewLine, nextText));
    }

    private void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= OnLog;
    }
}