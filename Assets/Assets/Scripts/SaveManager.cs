using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    private static Lazy<SaveManager> instance
        = new Lazy<SaveManager>(() => new SaveManager());

    public static SaveManager Instance => instance.Value;

    public bool IsLabelVisible { get; set; }

    public bool IsSleepEnabled { get; set; }

    public bool IsRotationEnabled { get; set; }

    public bool IsReviewRequested { get; set; }

    public void Load()
    {
        this.IsLabelVisible = PlayerPrefs.GetInt("LabelVisible", 1) != 0;
        this.IsSleepEnabled = PlayerPrefs.GetInt("Sleep", 0) != 0;
        this.IsRotationEnabled = PlayerPrefs.GetInt("Rotation", 0) != 0;
        this.IsReviewRequested = PlayerPrefs.GetInt("Review", 0) != 0;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("LabelVisible", this.IsLabelVisible ? 1 : 0);
        PlayerPrefs.SetInt("Sleep", this.IsSleepEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Rotation", this.IsRotationEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Review", this.IsReviewRequested ? 1 : 0);

        PlayerPrefs.Save();
    }
}
