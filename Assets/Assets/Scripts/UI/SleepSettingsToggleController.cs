using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepSettingsToggleController : MonoBehaviour
{
    [SerializeField]
    private GameObject trueObject;

    [SerializeField]
    private GameObject falseObject;

    private void Awake()
    {
        var flag = !SaveManager.Instance.IsSleepEnabled;
        this.SetIsSleepDisabled(flag);
    }

    private void Start()
    {
        var flag = !SaveManager.Instance.IsSleepEnabled;
        this.SetIsSleepDisabled(flag);
    }

    public void SetIsSleepDisabled(bool flag)
    {
        this.trueObject.SetActive(flag);
        this.falseObject.SetActive(!flag);

        Screen.sleepTimeout = flag ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
        SaveManager.Instance.IsSleepEnabled = !flag;
    }

    public void ToggleIsSleepDisabled()
    {
        this.SetIsSleepDisabled(!!SaveManager.Instance.IsSleepEnabled);
    }
}
