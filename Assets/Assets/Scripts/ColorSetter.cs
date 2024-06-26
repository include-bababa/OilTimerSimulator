using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private Color color = Color.white;

    [SerializeField]
    private SpriteRenderer[] targets;

    public Color Color => this.color;

    public void SetColor(Color color)
    {
        this.color = color;

        if (this.targets != null)
        {
            foreach (var target in this.targets)
            {
                target.color = new Color(color.r, color.g, color.b, target.color.a);
            }
        }
    }
}
