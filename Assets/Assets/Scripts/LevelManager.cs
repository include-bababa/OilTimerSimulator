using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelManager
{
    private static Lazy<LevelManager> instance
        = new Lazy<LevelManager>(() => new LevelManager());

    public static LevelManager Instance => instance.Value;

    private ThemeInfo[] themes;
    private PropInfo[] props;
    private ScaleSetInfo scaleSet;
    private RotationSetInfo rotationSet;

    private LevelData[] builtinLevels;
    private List<LevelData> userLevels;

    public ThemeInfo[] Themes => this.themes;

    public PropInfo[] Props => this.props;

    public int NumScaleData => this.scaleSet.NumScaleData;

    public int NumRotationData => this.rotationSet.NumRotationData;

    public IEnumerable<LevelData> BuiltinLevels => this.builtinLevels;

    public IEnumerable<LevelData> UserLevels => this.userLevels;

    public void LoadAllSync()
    {
        // load built-in levels
        this.LoadBuiltinLevelsSync();

        // load user levels
        this.userLevels = LevelUtility.LoadAll();

        this.LoadThemeInfoSync();
        this.LoadPropInfoSync();
        this.LoadScaleSetInfoSync();
        this.LoadRotationSetInfoSync();

        Debug.Log("Load completed.");
    }

    public void AddUserLevel(LevelData data)
    {
        this.userLevels = this.userLevels.Prepend(data).ToList();
    }

    public void DeleteUserLevel(LevelData data)
    {
        this.userLevels.Remove(data);
    }

    public ThemeInfo GetTheme(string assetName)
    {
        foreach (var theme in this.themes)
        {
            if (theme.AssetName == assetName)
            {
                return theme;
            }
        }

        return null;
    }

    public int GetThemeIndex(ThemeInfo theme)
    {
        if (this.themes != null)
        {
            for (var index = 0; index < this.themes.Length; index++)
            {
                if (this.themes[index] == theme)
                {
                    return index;
                }
            }
        }

        return -1;
    }

    public PropInfo GetPropData(PropType propType)
    {
        return this.props[(int)propType];
    }

    public ScaleSetInfo.ScaleData GetScaleData(ScaleGrade scaleGrade)
    {
        return this.scaleSet.GetScaleData(scaleGrade);
    }

    public RotationSetInfo.RotationData GetRotationData(RotationDirection rot)
    {
        return this.rotationSet.GetRotationData(rot);
    }

    private void LoadBuiltinLevelsSync()
    {
        var handle = Addressables.LoadAssetsAsync<TextAsset>("Levels", null);
        var texts = handle.WaitForCompletion();
        this.builtinLevels = texts.Select(text => JsonUtility.FromJson<LevelData>(text.text)).OrderBy(data => data.DisplayName).ToArray();

        foreach (var level in this.builtinLevels)
        {
            level.IsBuiltIn = true;
        }
    }

    private void LoadThemeInfoSync()
    {
        var handle = Addressables.LoadAssetsAsync<ThemeInfo>("Themes", null);
        this.themes = handle.WaitForCompletion().OrderBy(x => x.AssetName).ToArray();
    }

    private void LoadPropInfoSync()
    {
        var handle = Addressables.LoadAssetsAsync<PropInfo>("Props", null);
        this.props = handle.WaitForCompletion().OrderBy(x => x.PropType).ToArray();
    }

    private void LoadScaleSetInfoSync()
    {
        var handle = Addressables.LoadAssetAsync<ScaleSetInfo>(
            "Assets/Assets/ScriptableObjects/ScaleSetInfo.asset");
        this.scaleSet = handle.WaitForCompletion();
    }

    private void LoadRotationSetInfoSync()
    {
        var handle = Addressables.LoadAssetAsync<RotationSetInfo>(
            "Assets/Assets/ScriptableObjects/RotationSetInfo.asset");
        this.rotationSet = handle.WaitForCompletion();
    }
}
