using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public enum AdType { Revival, ReDraw }
public class AdMob : MonoBehaviour
{
    AdType _adType;
    string _adUnitId;
    RewardedAd _rewardedAd;

    public void Start()
    {
        //adUnitId 설정
#if UNITY_ANDROID
        _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif

        // 모바일 광고 SDK를 초기화함.
        MobileAds.Initialize(initStatus => { });

        LoadAds();
    }

    void LoadAds()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        //광고 로드 : RewardedAd 객체의 loadAd메서드에 AdRequest 인스턴스를 넣음

        _rewardedAd = new RewardedAd(_adUnitId);

        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출

        var adRequest = new AdRequest.Builder().Build();

        _rewardedAd.LoadAd(adRequest);
    }

    public void ShowAds(AdType adType)
    {
        if (this._rewardedAd.IsLoaded())
        {
            this._rewardedAd.Show();
            _adType = adType;
        }
        else
        {
            Debug.Log("Ad Load ERROR : Not Loaded");
        }
    }

    void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("광고 로드 완료~!!!");
    }

    void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("광고 로딩 실패..");
    }

    void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("광고 시작~~!");
    }

    void HandleRewardedAdFailedToShow(object sender, EventArgs args)
    {
        Debug.Log("광고 표시 실패");
    }

    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("광고 종료~");
        LoadAds();
    }

    void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("부활!");
        switch (_adType)
        {
            case AdType.Revival:
                GameManager.Instance.Revival();
                break;
            case AdType.ReDraw:
                ColorManager.Instance.StartDrawing(GameManager.Instance.PlayerColor);
                break;
        }
    }
}
