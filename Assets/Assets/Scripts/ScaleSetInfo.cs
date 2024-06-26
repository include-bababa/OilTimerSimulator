using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "ScriptableObjects/CreateScaleSetInfo")]
public class ScaleSetInfo : ScriptableObject
{
    [SerializeField]
    private ScaleData[] scaleData;

    public int NumScaleData => this.scaleData.Length;

    public ScaleData GetScaleData(ScaleGrade scaleGrade)
    {
        return this.scaleData[(int)scaleGrade];
    }

    [Serializable]
    public class ScaleData
    {
        [SerializeField]
        private LocalizedString displayName;

        [SerializeField]
        private LocalizedString shortName;

        public LocalizedString DisplayName => this.displayName;

        public LocalizedString ShortName => this.shortName;
    }
}
