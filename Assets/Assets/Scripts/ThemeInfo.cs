using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "ScriptableObjects/CreateThemeData")]
public class ThemeInfo : ScriptableObject
{
    [SerializeField]
    private string assetName;

    [SerializeField]
    private LocalizedString displayName;

    [SerializeField]
    private AssetReferenceSprite backgroundTexture;

    [SerializeField]
    private AssetReferenceSprite thumbnailTexture;

    [SerializeField]
    private bool useLighting;

    public string AssetName => this.assetName;

    public LocalizedString DisplayName => this.displayName;

    public AssetReferenceSprite BackgroundTexture => this.backgroundTexture;

    public AssetReferenceSprite ThumbnailTexture => this.thumbnailTexture;

    public bool UseLighting => this.useLighting;
}
