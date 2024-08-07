using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.SceneManagement;
using Google.Android.PerformanceTuner;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
    bool _isLogined = false;
    AdMob _adMob;

    AndroidPerformanceTuner<FidelityParams, Annotation> tuner =
    new AndroidPerformanceTuner<FidelityParams, Annotation>();
    void Awake()
    {
        DontDestroyOnLoad(this);

        //GPGS Setting
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Login();

        tuner.EnableLocalEndpoint();
        ErrorCode startErrorCode = tuner.Start();
        Debug.Log("Android Performance Tuner started with code: " + startErrorCode);

        tuner.onReceiveUploadLog += request =>
        {
            Debug.Log("Telemetry uploaded with request name: " + request.name);
        };
    }

    public void Login()
    {
        if (_isLogined) {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool isSuccess, string error) =>
            {
                if (!isSuccess)
                {
                    Debug.Log($"Fail: {error}");
                }
                else
                {
                    _isLogined = true;
                }
                return;
            });
         }
    }
    public void Login(Action OnLogin)
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool isSuccess, string error) =>
            {
                if (!isSuccess)
                    Debug.Log($"Fail: {error}");

                else
                    OnLogin.Invoke();

                return;
            });
        }
        OnLogin.Invoke();
    }


    public void OnShowLeaderBoard()
    {
        Login(Social.ShowLeaderboardUI);
    }


    public void OnShowAchievement()
    {
        Login(Social.ShowAchievementsUI);
    }

    public void ShowAds(AdType adType)
    {
        _adMob.ShowAds(adType);
    }

    public void LoadAds()
    {
        _adMob.LoadAds();
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby" || scene.name == "LevelMenu")
            return;

        //AdMob
        if (_adMob == null)
        {
            gameObject.AddComponent<AdMob>();
            _adMob = GetComponent<AdMob>();
        }
        LoadAds();
    }

}