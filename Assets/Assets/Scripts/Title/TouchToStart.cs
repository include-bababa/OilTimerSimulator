using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TouchToStart : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float elapsedTime;

    private void Start()
    {
        this.text = this.GetComponent<TextMeshProUGUI>();
        this.elapsedTime = 0.0f;
    }

    private void Update()
    {
        this.elapsedTime += Time.deltaTime;
        if (this.elapsedTime > 3.0f)
        {
            this.elapsedTime -= 3.0f;
        }

        if (this.elapsedTime < 1.5f)
        {
            var color = this.text.color;
            color.a = this.elapsedTime / 1.5f * 0.75f + 0.25f;
            this.text.color = color;
        }
        else
        {
            var color = this.text.color;
            color.a = (3.0f - this.elapsedTime) / 1.5f * 0.75f + 0.25f;
            this.text.color = color;
        }
    }
}
