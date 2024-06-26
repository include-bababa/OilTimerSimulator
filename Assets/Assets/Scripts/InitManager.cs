using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitManager : MonoBehaviour
{
    private void Awake()
    {
        Logger.Log("InitManager.Awake()");

        if (FindObjectsOfType<InitManager>(true).Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

        SaveManager.Instance.Load();
        LevelManager.Instance.LoadAllSync();
        //Shader.WarmupAllShaders();

#if ENABLE_ANDROID_REVIEW
        AndroidReviewManager.CreateInstance();
#endif

        AdManager.CreateInstance();
        AdManager.Instance.InitializeAds();
        AdManager.Instance.LoadBannerAd();
        AdManager.Instance.LoadInterstitialAd();

#if FIREBASE_USE_TEST_LOOP
        TestSequenceManager.CreateInstance();
        TestSequenceManager.Instance.StartTest();
#endif
    }

    private void Update()
    {
#if FIREBASE_USE_TEST_LOOP
        TestSequenceManager.Instance.Update(Time.unscaledDeltaTime);
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        SaveManager.Instance.Save();
    }

    private void OnApplicationQuit()
    {
        SaveManager.Instance.Save();
    }

#if FIREBASE_USE_TEST_LOOP
    private void OnGUI()
    {
        var currentTest = TestSequenceManager.Instance.CurrentTest;
        if (currentTest != null)
        {
            GUI.Label(new Rect(100, 100, 200, 40), $"CurrentTest: {currentTest.Name}");
        }
    }
#endif
}
