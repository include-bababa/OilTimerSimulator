using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogViewer : MonoBehaviour
{
    private const int Capacity = 10;

    private List<string> logs;

    public static LogViewer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        this.logs = new List<string>();
    }

    private void OnEnable()
    {
        Application.logMessageReceived += this.OnLogMessageReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= this.OnLogMessageReceived;
    }

    private void OnGUI()
    {
        GUILayout.Label("Logs:");
        foreach (var log in this.logs)
        {
            GUILayout.Label(log);
        }
    }

    public void AddLog(string log)
    {
        this.logs.Add(log);
        if (this.logs.Count > Capacity)
        {
            this.logs.RemoveAt(0);
        }
    }

    private void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            this.AddLog(logString + "\n" + stackTrace);
        }
        else
        {
            this.AddLog(logString);
        }
    }
}
