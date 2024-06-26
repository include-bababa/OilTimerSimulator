using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelSettingsToggleController : MonoBehaviour
{
    [SerializeField]
    private GameObject trueObject;

    [SerializeField]
    private GameObject falseObject;

    private void Start()
    {
        this.SetIsLabelVisible(SaveManager.Instance.IsLabelVisible);
    }

    public void SetIsLabelVisible(bool flag)
    {
        this.trueObject.SetActive(flag);
        this.falseObject.SetActive(!flag);

        SaveManager.Instance.IsLabelVisible = flag;
    }

    public void ToggleIsLabelVisible()
    {
        this.SetIsLabelVisible(!SaveManager.Instance.IsLabelVisible);
    }
}
