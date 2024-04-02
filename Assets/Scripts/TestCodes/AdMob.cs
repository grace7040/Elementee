using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdType { Revival, ReDraw }
public class AdMob : MonoBehaviour
{
    public AdType adType;
    string adUnitId;
    RewardedAd rewardedAd;

    public void Start()
    {
        // 모바일 광고 SDK를 초기화함.
        MobileAds.Initialize(initStatus => { });

        //광고 로드 : RewardedAd 객체의 loadAd메서드에 AdRequest 인스턴스를 넣음
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);

        //adUnitId 설정
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3115045377477281/4539879882";
#endif

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) {
        Debug.Log("광고 로드 완료~!!!");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Quiz_Manager.StopADs();
        Debug.Log("광고 로딩 실패..");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        Debug.Log("광고 시작~~!");
    }

    public void HandleRewardedAdFailedToShow(object sender, EventArgs args) {
        Debug.Log("광고 표시 실패");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        Debug.Log("광고 종료~");
        switch (adType)
        {
            case AdType.Revival:
                GameManager.Instance.Revival();
                break;
            case AdType.ReDraw:
                ColorManager.Instance.StartDrawing(GameManager.Instance.playerColor);
                break;
        }
        
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //if (Quiz_Manager == null) Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
        //Quiz_Manager.PostADs();
        Debug.Log("부활!");
    }

    public void ShowAds()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void InitAds()
    {
        MobileAds.Initialize(initStatus => { });

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);

        Debug.Log("광고 초기화~!!");

    }
}
