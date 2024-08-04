using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AdType { Revival, ReDraw }
public class AdMob : MonoBehaviour
{
    public AdType adType;
    string adUnitId;
    RewardedAd rewardedAd;

    public void Start()
    {
        //adUnitId ����
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif

        // ����� ���� SDK�� �ʱ�ȭ��.
        MobileAds.Initialize(initStatus => { });

        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) {
        Debug.Log("���� �ε� �Ϸ�~!!!");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Quiz_Manager.StopADs();
        Debug.Log("���� �ε� ����..");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        Debug.Log("���� ����~~!");
    }

    public void HandleRewardedAdFailedToShow(object sender, EventArgs args) {
        Debug.Log("���� ǥ�� ����");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        Debug.Log("���� ����~");
        switch (adType)
        {
            case AdType.Revival:
                GameManager.Instance.Revival();
                break;
            case AdType.ReDraw:
                ColorManager.Instance.StartDrawing(GameManager.Instance.PlayerColor);
                break;
        }
        
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //if (Quiz_Manager == null) Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
        //Quiz_Manager.PostADs();
        Debug.Log("��Ȱ!");
    }

    public void ShowAds()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        else
        {
            Debug.Log("Not Loaded");
        }
    }

    public void InitAds()
    {
        MobileAds.Initialize(initStatus => { });

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);

        Debug.Log("���� �ʱ�ȭ~!!");

    }
}
