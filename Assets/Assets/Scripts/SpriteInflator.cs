using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteInflator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Vector3 defaultLocalScale;
    private Vector3 lastScale;

    [SerializeField]
    private Vector2 inflateSize = new Vector2(0.05f, 0.05f);

    private void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.defaultLocalScale = this.transform.localScale;
        this.UpdateInflation();
    }

    private void LateUpdate()
    {
        if (this.transform.lossyScale != lastScale)
        {
            this.UpdateInflation();
        }
    }

    private void UpdateInflation()
    {
        var pixelsPerUnit = this.spriteRenderer.sprite.pixelsPerUnit;
        var rect = this.spriteRenderer.sprite.rect;
        var size = rect.size;
        var localSize = size / pixelsPerUnit;
        Vector3 worldSize = localSize;
        worldSize.x *= this.transform.lossyScale.x;
        worldSize.y *= this.transform.lossyScale.y;

        // Debug.Log($"world_size: {worldSize}");

        var addScaleX = this.inflateSize.x / worldSize.x;
        var addScaleY = this.inflateSize.y / worldSize.y;
        var localScale = this.defaultLocalScale;
        localScale.x *= 1.0f + addScaleX;
        localScale.y *= 1.0f + addScaleY;
        this.transform.localScale = localScale;

        var shadeWidth = addScaleX / (1.0f + addScaleX);
        var shadeHeight = addScaleY / (1.0f + addScaleY);
        this.spriteRenderer.material.SetFloat("_ShadeWidth", shadeWidth * 0.5f);
        this.spriteRenderer.material.SetFloat("_ShadeHeight", shadeHeight * 0.5f);

        // Debug.Log($"shade width: {shadeWidth}, height: {shadeHeight}");

        this.lastScale = this.transform.lossyScale;
    }
}
