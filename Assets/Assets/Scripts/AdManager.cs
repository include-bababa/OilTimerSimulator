using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private const string AndroidGameId = "4834269";
    private const string IOSGameId = "4834268";
    private const bool TestMode = false;

    private const string AndroidInterstitialAdUnitId = "Interstitial_Android";
    private const string AndroidBannerAdUnitId = "Banner_Android";
    private const string IOSInterstitialAdUnitId = "Interstitial_iOS";
    private const string IOSBannerAdUnitId = "Banner_iOS";

    public static AdManager Instance { get; private set; }

    private string gameId;
    private string interstitialAdUnitId;
    private string bannerAdUnitId;

    private int interstitialAdCounter;
    private DateTime interstitialLastShown;

    public bool IsInterstitialShowing { get; private set; }

    public bool IsBannerShowing { get; private set; }

    public static void CreateInstance()
    {
        Instance = new AdManager();
    }

    public void InitializeAds()
    {
        this.gameId = Application.platform == RuntimePlatform.IPhonePlayer ? IOSGameId : AndroidGameId;
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(this.gameId, TestMode, this);
        }

        this.interstitialAdUnitId = Application.platform == RuntimePlatform.IPhonePlayer ?
            IOSInterstitialAdUnitId : AndroidInterstitialAdUnitId;
        this.bannerAdUnitId = Application.platform == RuntimePlatform.IPhonePlayer ?
            IOSBannerAdUnitId : AndroidBannerAdUnitId;
    }

    public void OnInitializationComplete()
    {
        Logger.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Logger.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    public void LoadInterstitialAd()
    {
        // Note to load content AFTER initialization.
        Logger.Log("Loading Ad: " + this.interstitialAdUnitId);
        Advertisement.Load(this.interstitialAdUnitId, this);

        this.interstitialAdCounter = 4;
        this.interstitialLastShown = DateTime.MinValue;
    }

    public void ShowInterstitialAd(bool useCounter)
    {
#if !UNITY_EDITOR
        if (useCounter)
        {
            this.interstitialAdCounter--;
            if (this.interstitialAdCounter > 0)
            {
                return;
            }

            if ((DateTime.Now - this.interstitialLastShown).TotalSeconds < 240)
            {
                return;
            }

            this.interstitialAdCounter = 2;
        }

        // Note that if the ad content wasn't previously loaded, this method will fail
        Logger.Log("Showing Ad: " + this.interstitialAdUnitId);
        Advertisement.Show(this.interstitialAdUnitId, this);
        this.interstitialLastShown = DateTime.Now;
#endif
    }

    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

        var options = new BannerLoadOptions
        {
            loadCallback = this.OnBannerLoaded,
            errorCallback = this.OnBannerError,
        };
        Advertisement.Banner.Load(this.bannerAdUnitId, options);
    }

    public void ShowBannerAd()
    {
#if !FIREBASE_USE_TEST_LOOP
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);

        var options = new BannerOptions
        {
            clickCallback = this.OnBannerClicked,
            hideCallback = this.OnBannerHidden,
            showCallback = this.OnBannerShown,
        };
        Advertisement.Banner.Show(this.bannerAdUnitId, options);
#endif
    }

    public void HideBannerAd()
    {
#if !FIREBASE_USE_TEST_LOOP
        Advertisement.Banner.Hide();
#endif
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Logger.LogError($"Error loading Ad Unit: {adUnitId} - {error} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Logger.LogError($"Error showing Ad Unit {adUnitId}: {error} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        if (adUnitId == this.interstitialAdUnitId)
        {
            this.IsInterstitialShowing = true;
        }
    }

    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId == this.interstitialAdUnitId)
        {
            this.IsInterstitialShowing = false;
        }
    }

    // Implement code to execute when the loadCallback event triggers:
    private void OnBannerLoaded()
    {
        Logger.Log("Banner loaded");
    }

    // Implement code to execute when the load errorCallback event triggers:
    private void OnBannerError(string message)
    {
        Logger.LogError($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    private void OnBannerClicked() { }

    private void OnBannerShown()
    {
        this.IsBannerShowing = true;
    }

    private void OnBannerHidden()
    {
        this.IsBannerShowing = false;
    }
}
