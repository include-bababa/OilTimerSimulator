using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InitializeCaller : MonoBehaviour
{
    [SerializeField]
    private UnityEvent initializer;

    [SerializeField]
    private UnityEvent starter;

    private void Awake()
    {
        this.initializer?.Invoke();
    }

    private void Start()
    {
        this.starter?.Invoke();
    }
}
