using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaleGrade
{
    Small,
    Medium,
    Large,
}

public class ScaleSetter : MonoBehaviour
{
    private ScaleGrade scale = ScaleGrade.Medium;

    private Vector3 defaultLocalScale;

    [SerializeField]
    private bool useX = true;

    [SerializeField]
    private bool useY = true;

    [SerializeField]
    private float smallMultiplier = 0.5f;

    [SerializeField]
    private float mediumMultiplier = 1.0f;

    [SerializeField]
    private float largeMultiplier = 2.0f;

    public ScaleGrade Scale => this.scale;

    public event Action<Vector2> ScaleChanged;

    public void SetScale(ScaleGrade grade)
    {
        if (IsApproximatelyZero(this.defaultLocalScale))
        {
            this.defaultLocalScale = this.transform.localScale;
        }

        var multiplier = Vector2.one;
        switch (grade)
        {
            case ScaleGrade.Small:
                multiplier.x = this.useX ? this.smallMultiplier : 1.0f;
                multiplier.y = this.useY ? this.smallMultiplier : 1.0f;
                break;
            case ScaleGrade.Medium:
                multiplier.x = this.useX ? this.mediumMultiplier : 1.0f;
                multiplier.y = this.useY ? this.mediumMultiplier : 1.0f;
                break;
            case ScaleGrade.Large:
                multiplier.x = this.useX ? this.largeMultiplier : 1.0f;
                multiplier.y = this.useY ? this.largeMultiplier : 1.0f;
                break;
        }
        var scale = this.defaultLocalScale;
        scale.x *= multiplier.x;
        scale.y *= multiplier.y;
        this.transform.localScale = scale;

        this.scale = grade;

        this.ScaleChanged?.Invoke(multiplier);
    }

    public float GetMultiplierX(ScaleGrade grade)
    {
        if (!this.useX)
        {
            return 1.0f;
        }

        switch (grade)
        {
            case ScaleGrade.Small:
                return this.smallMultiplier;
            case ScaleGrade.Medium:
                return this.mediumMultiplier;
            case ScaleGrade.Large:
                return this.largeMultiplier;
        }

        return 1.0f;
    }

    public float GetMultiplierY(ScaleGrade grade)
    {
        if (!this.useY)
        {
            return 1.0f;
        }

        switch (grade)
        {
            case ScaleGrade.Small:
                return this.smallMultiplier;
            case ScaleGrade.Medium:
                return this.mediumMultiplier;
            case ScaleGrade.Large:
                return this.largeMultiplier;
        }

        return 1.0f;
    }

    private static bool IsApproximatelyZero(Vector3 vec)
    {
        return Mathf.Approximately(vec.x, 0.0f) &&
            Mathf.Approximately(vec.y, 0.0f) &&
            Mathf.Approximately(vec.z, 0.0f);
    }
}
