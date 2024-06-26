using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSettingsToggleController : MonoBehaviour
{
    [SerializeField]
    private GameObject trueObject;

    [SerializeField]
    private GameObject falseObject;

    private void Start()
    {
        this.SetCanRotate(SaveManager.Instance.IsRotationEnabled);
    }

    public void SetCanRotate(bool flag)
    {
        this.trueObject.SetActive(flag);
        this.falseObject.SetActive(!flag);

        SaveManager.Instance.IsRotationEnabled = flag;
    }

    public void ToggleCanRotate()
    {
        this.SetCanRotate(!SaveManager.Instance.IsRotationEnabled);
    }
}
