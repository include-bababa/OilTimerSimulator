using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum RotationDirection
{
    ClockwiseSlow,
    ClockwiseFast,
    CounterClockwiseSlow,
    CounterClockwiseFast,
}

[CreateAssetMenu(menuName = "ScriptableObjects/CreateRotationSetInfo")]
public class RotationSetInfo : ScriptableObject
{
    [SerializeField]
    private RotationData[] rotationData;

    public int NumRotationData => this.rotationData.Length;

    public RotationData GetRotationData(RotationDirection rot)
    {
        return this.rotationData[(int)rot];
    }

    [Serializable]
    public class RotationData
    {
        [SerializeField]
        private LocalizedString displayName;

        [SerializeField]
        private LocalizedString shortName;

        public LocalizedString DisplayName => this.displayName;

        public LocalizedString ShortName => this.shortName;
    }
}
