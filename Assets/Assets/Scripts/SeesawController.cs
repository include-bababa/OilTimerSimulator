using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawController : MonoBehaviour
{
    private EditManager editManager;
    private Rigidbody2D rigidbody2d;
    private ScaleSetter scaleSetter;

    private float defaultCenterCircleScale;
    private float torqueScale;

    [SerializeField]
    private GameObject centerCircle;

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);

        this.rigidbody2d = this.GetComponent<Rigidbody2D>();
        this.defaultCenterCircleScale = this.centerCircle.transform.localScale.x;

        this.scaleSetter = this.GetComponent<ScaleSetter>();
        this.scaleSetter.ScaleChanged += this.ScaleSetter_ScaleChanged;
    }

    private void Start()
    {
        this.torqueScale = this.scaleSetter.GetMultiplierX(this.scaleSetter.Scale);
    }

    private void FixedUpdate()
    {
        if (this.editManager.IsDropPaused)
        {
            return;
        }

        this.rigidbody2d.AddTorque(this.transform.rotation.z * -6.0f * this.torqueScale);
    }

    public void Reset()
    {
        this.transform.rotation = Quaternion.identity;
        this.rigidbody2d.angularVelocity = 0.0f;
    }

    private void ScaleSetter_ScaleChanged(Vector2 multiplier)
    {
        var scale = this.centerCircle.transform.localScale;
        scale.x = this.defaultCenterCircleScale / multiplier.x;
        this.centerCircle.transform.localScale = scale;

        this.torqueScale = this.scaleSetter.GetMultiplierX(this.scaleSetter.Scale);
    }
}
