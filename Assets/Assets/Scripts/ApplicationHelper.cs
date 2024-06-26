using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApplicationHelper
{
    public static void MoveTaskToBack()
    {
        using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            activity.Call<bool>("moveTaskToBack", true);
        }
    }
}
