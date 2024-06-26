using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum PropType
{
    Platform,
    Pipe,
    PoweredGear,
    Pin,
    Seesaw,
    Cube,
}

[CreateAssetMenu(menuName = "ScriptableObjects/CreatePropInfo")]
public class PropInfo : ScriptableObject
{
    [SerializeField]
    private PropType propType;

    [SerializeField]
    private LocalizedString displayName;

    [SerializeField]
    private GameObject prefab;

    public PropType PropType => this.propType;

    public LocalizedString DisplayName => this.displayName;

    public GameObject Prefab => this.prefab;
}
