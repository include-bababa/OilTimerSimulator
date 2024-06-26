using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    private Camera particleCamera;

    [SerializeField]
    private ParticleSystem tapParticle;

    [SerializeField]
    private ParticleSystem createEmberParticle;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = this.particleCamera.ScreenToWorldPoint(Input.mousePosition);
            this.tapParticle.transform.position = new Vector3(pos.x, pos.y, 0.0f);
            this.tapParticle.Emit(1);
        }
    }

    private void LateUpdate()
    {
        this.particleCamera.rect = Camera.main.rect;
        this.particleCamera.orthographicSize = Camera.main.orthographicSize;
    }

    public void EmitCreateParticle(Vector3 pos)
    {
        this.createEmberParticle.transform.position = pos;
        this.createEmberParticle.Emit(10);
    }
}