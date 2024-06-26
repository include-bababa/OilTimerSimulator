using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedActivation : MonoBehaviour
{
    [SerializeField]
    private GameObject[] linkedObjects;

    private void OnEnable()
    {
        if (this.linkedObjects != null)
        {
            foreach (var obj in this.linkedObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (this.linkedObjects != null)
        {
            foreach (var obj in this.linkedObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}
