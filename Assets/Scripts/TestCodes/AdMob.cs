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
        //adUnitId ����
#if UNITY_ANDROID
        _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif

        // ����� ���� SDK�� �ʱ�ȭ��.
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
        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����

        _rewardedAd = new RewardedAd(_adUnitId);

        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
        _rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
        _rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��

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
        Debug.Log("���� �ε� �Ϸ�~!!!");
    }

    void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("���� �ε� ����..");
    }

    void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("���� ����~~!");
    }

    void HandleRewardedAdFailedToShow(object sender, EventArgs args)
    {
        Debug.Log("���� ǥ�� ����");
    }

    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("���� ����~");
        LoadAds();
    }

    void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("��Ȱ!");
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
