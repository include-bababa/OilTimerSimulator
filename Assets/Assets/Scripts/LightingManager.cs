using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer platformRenderer;

    [SerializeField]
    private RenderTexture lightingRenderTexture;

    [SerializeField]
    private DropSpawner mainSpawner;

    [SerializeField]
    private DropSpawner subSpawner;

    public void SetLightingEnable(bool enable)
    {
        this.mainSpawner.SetLightingEnable(enable);
        this.subSpawner.SetLightingEnable(enable);

        this.platformRenderer.material.SetTexture(
            "_AlphaTex",
            enable ? this.lightingRenderTexture : Texture2D.whiteTexture);
    }
}
