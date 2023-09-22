using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GooglePlayManager : MonoBehaviour
{
    public TMP_Text text;
    void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void Login()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool isSuccess, string error) =>
            {
                if (!isSuccess)
                {
                    Debug.Log($"Fail: {error}");
                    text.text = "Fail";
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

}