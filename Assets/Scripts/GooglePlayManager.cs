using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
    bool _isLogined = false;
    AdMob _adMob;
    void Awake()
    {
        DontDestroyOnLoad(this);

        //GPGS Setting
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        //AdMob
        if(_adMob == null)
        {
            gameObject.AddComponent<AdMob>();
            _adMob = GetComponent<AdMob>();
        }
    }

    void Start()
    {
        Login();
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

}