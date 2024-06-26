using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    private const int DropCountMax = 120;

    private EditManager editManager;

    private bool isLightingEnabled = false;
    private float lastDropTime;
    private List<DropController> drops;

    private Vector3 spawnPosition;

    [SerializeField]
    private GameObject dropPrefab;

    [SerializeField]
    private SpriteRenderer topPoolSprite;

    [SerializeField]
    private SpriteRenderer bottomPoolSprite;

    [SerializeField]
    private Renderer dropRenderer;

    [SerializeField]
    private GameObject spawnRoot;

    [SerializeField]
    private float dropInterval = 5.0f;

    [SerializeField]
    private bool isSubDrop;

    public EditManager EditManager => this.editManager;

    public SpriteRenderer TopPoolSprite => this.topPoolSprite;

    public SpriteRenderer BottomPoolSprite => this.bottomPoolSprite;

    public Renderer Renderer => this.dropRenderer;

    public Vector3 SpawnPosition
    {
        get => this.spawnPosition;
        set => this.spawnPosition = value;
    }

    public float DropInterval
    {
        get => this.dropInterval;
        set => this.SetInterval(value);
    }

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);
        this.drops = new List<DropController>();
    }

    private void Start()
    {
        this.lastDropTime = 0.01f;
    }

    private void Update()
    {
        if (this.editManager.SetTime <= 0 || this.editManager.ElapsedTime < this.editManager.SetTime)
        {
            if (this.dropInterval > 0)
            {
                var time = this.editManager.ElapsedTime + (this.isSubDrop ? this.dropInterval * 0.5f : 0);
                var nextDropTime = ((int)(this.lastDropTime / this.dropInterval) + 1) * this.dropInterval;
                if (time >= nextDropTime)
                {
                    this.Spawn();
                    this.lastDropTime = time;
                }
            }
        }

        if (this.editManager.SetTime > 0)
        {
            var ratio = this.editManager.ElapsedTime / this.editManager.SetTime;
            this.SetTime(Mathf.Clamp01(ratio));
        }
        else
        {
            this.SetTime(0.0f);
        }
    }

    public void SetColor(Color fill, Color stroke)
    {
        this.dropRenderer.material.SetColor("_Color", fill);
        this.dropRenderer.material.SetColor("_StrokeColor", stroke);
    }

    public void SetLightingEnable(bool enable)
    {
        this.isLightingEnabled = enable;
        if (this.drops != null)
        {
            foreach (var drop in this.drops)
            {
                drop.IsLightingEnabled = enable;
            }
        }
    }

    public void ClearDrops()
    {
        var drops = new List<DropController>(this.drops);
        foreach (var drop in drops)
        {
            Destroy(drop.gameObject);
        }

        this.lastDropTime = 0.01f;
    }

    public void RemoveDrop(DropController drop)
    {
        this.drops.Remove(drop);
    }

    private void SetTime(float ratio)
    {
        if (ratio < 1.0f)
        {
            ratio = ratio * 0.9f;
        }

        var center = this.topPoolSprite.transform.position.y;
        var height = this.topPoolSprite.transform.lossyScale.y;
        var top = center + height * 0.5f;
        var line = top - height * ratio;

        var renderHeight = this.dropRenderer.transform.lossyScale.y;
        var renderBottom = this.dropRenderer.transform.position.y - renderHeight * 0.5f;
        var val = Mathf.Clamp01((line - renderBottom) / renderHeight);
        this.dropRenderer.material.SetFloat("_OilHeight", val);
    }

    private void Spawn()
    {
        if (this.drops.Count >= DropCountMax)
        {
            // このあたりからコリジョン計算が重くなるのでスポーン停止
            return;
        }

        var gravityY = Physics2D.gravity.y;
        if (Mathf.Abs(gravityY) < 1.0f)
        {
            return;
        }

        var pos = this.spawnPosition;
        if (gravityY > 0.0f)
        {
            pos.y *= -1;
        }

        // add randomness
        pos.x += Random.Range(-1.0f, 1.0f) * 0.005f;

        var quat = Quaternion.identity;
        /*
        var instance = Instantiate(
            this.dropPrefab, pos, quat, this.spawnRoot.transform);
        var controller = instance.GetComponent<DropController>();
        controller.Spawner = this;
        controller.IsLightingEnabled = this.isLightingEnabled;

        this.drops.Add(controller);
        */

        {
            var instance = Instantiate(
                this.dropPrefab, pos + new Vector3(0.0f, 0.035f, 0.0f), quat, this.spawnRoot.transform);
            var controller = instance.GetComponent<DropController>();
            controller.Spawner = this;
            controller.IsLightingEnabled = this.isLightingEnabled;

            this.drops.Add(controller);
        }
        {
            var instance = Instantiate(
                this.dropPrefab, pos + new Vector3(-0.02f, 0.0f, 0.0f), quat, this.spawnRoot.transform);
            var controller = instance.GetComponent<DropController>();
            controller.Spawner = this;
            controller.IsLightingEnabled = this.isLightingEnabled;

            this.drops.Add(controller);
        }
        {
            var instance = Instantiate(
                this.dropPrefab, pos + new Vector3(0.02f, 0.0f, 0.0f), quat, this.spawnRoot.transform);
            var controller = instance.GetComponent<DropController>();
            controller.Spawner = this;
            controller.IsLightingEnabled = this.isLightingEnabled;

            this.drops.Add(controller);
        }
    }

    private void SetInterval(float interval)
    {
        this.dropInterval = interval;
    }
}
