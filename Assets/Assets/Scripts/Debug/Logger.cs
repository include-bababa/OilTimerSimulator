using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    public static void Log(string message)
    {
#if DEBUG
        Debug.Log(message);
#else
        LogViewer.Instance?.AddLog(message);
#endif
    }

    public static void LogError(string message)
    {
#if DEBUG
        Debug.LogError(message);
#else
        LogViewer.Instance?.AddLog(message);
#endif
    }
}
