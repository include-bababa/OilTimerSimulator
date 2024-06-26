using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPoolManager : MonoBehaviour
{
    private Color mainColor = Color.cyan;
    private Color? subColor = Color.yellow;

    [SerializeField]
    private DropSpawner mainRoot;

    [SerializeField]
    private DropSpawner subRoot;

    [SerializeField]
    private Material topLeftMaterial;

    [SerializeField]
    private Material topCenterMaterial;

    [SerializeField]
    private Material topRightMaterial;

    [SerializeField]
    private Material bottomLeftMaterial;

    [SerializeField]
    private Material bottomCenterMaterial;

    [SerializeField]
    private Material bottomRightMaterial;

    [SerializeField]
    private Vector3 sideSpawnPosition = new Vector3(-0.52f, 1.3f, 0.0f);

    [SerializeField]
    private Vector3 centerSpawnPosition = new Vector3(0.0f, 1.3f, 0.0f);

    public Color MainColor => this.mainColor;

    public Color? SubColor => this.subColor;

    public DropSpawner MainSpawner => this.mainRoot;

    public DropSpawner SubSpawner => this.subRoot;

    public void Start()
    {
        this.mainRoot.SpawnPosition = this.sideSpawnPosition;
        this.subRoot.SpawnPosition = new Vector3(
            this.sideSpawnPosition.x * -1,
            this.sideSpawnPosition.y,
            this.sideSpawnPosition.z);
    }

    public void SetMainColor(Color color)
    {
        var fill = new Color(
            color.r * 0.5f + 0.5f,
            color.g * 0.5f + 0.5f,
            color.b * 0.5f + 0.5f,
            0.625f);
        var stroke = new Color(
            color.r * 0.5f,
            color.g * 0.5f,
            color.b * 0.5f,
            0.75f);

        this.mainColor = color;
        this.mainRoot.SetColor(fill, stroke);
    }

    public void UnsetSubColor()
    {
        this.subColor = null;

        // deactivate
        this.subRoot.gameObject.SetActive(false);
        this.mainRoot.TopPoolSprite.material = this.topCenterMaterial;
        this.mainRoot.BottomPoolSprite.material = this.bottomCenterMaterial;

        // set spawn position
        this.mainRoot.SpawnPosition = this.centerSpawnPosition;
    }

    public void SetSubColor(Color color)
    {
        var fill = new Color(
            color.r * 0.5f + 0.5f,
            color.g * 0.5f + 0.5f,
            color.b * 0.5f + 0.5f,
            0.625f);
        var stroke = new Color(
            color.r * 0.5f,
            color.g * 0.5f,
            color.b * 0.5f,
            0.75f);

        this.subColor = color;

        // activate
        if (!this.subRoot.gameObject.activeSelf)
        {
            this.subRoot.gameObject.SetActive(true);
            this.mainRoot.TopPoolSprite.material = this.topLeftMaterial;
            this.mainRoot.BottomPoolSprite.material = this.bottomLeftMaterial;
            this.subRoot.TopPoolSprite.material = this.topRightMaterial;
            this.subRoot.BottomPoolSprite.material = this.bottomRightMaterial;
        }

        // set spawn position
        this.mainRoot.SpawnPosition = this.sideSpawnPosition;
        this.subRoot.SpawnPosition = new Vector3(
            this.sideSpawnPosition.x * -1,
            this.sideSpawnPosition.y,
            this.sideSpawnPosition.z);

        this.subRoot.SetColor(fill, stroke);
    }
}
