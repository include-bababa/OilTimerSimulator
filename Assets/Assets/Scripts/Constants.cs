using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static readonly Color HighlightColor = new Color(0.375f, 0.5f, 0.5f, 1.0f);
    public static readonly Color DisabledColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    public static Color GetHighlightColor(float alpha)
    {
        return new Color(HighlightColor.r, HighlightColor.g, HighlightColor.b, alpha);
    }

    public static Color GetDisabledColor(float alpha)
    {
        return new Color(DisabledColor.r, DisabledColor.g, DisabledColor.b, alpha);
    }
}
