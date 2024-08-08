using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AdType { Revival, ReDraw }

public class AdMob : MonoBehaviour
{
    /// <summary>
    /// UI element activated when an ad is ready to show.
    /// </summary>
    //public GameObject AdLoadedStatus;
        

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
    private const string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    private static bool? _isInitialized;


    TMP_Text text;
    /// <summary>
    /// Demonstrates how to configure Google Mobile Ads Unity plugin.
    /// </summary>
    private void Start()
    {
        //text = FindObjectOfType<UI_Game>().text;

        //text.text = "Start";
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        //text.text = "RaiseAdEventsOnUnityMainThread";

        InitializeGoogleMobileAds();
    }

    /// <summary>
    /// Initializes the Google Mobile Ads Unity plugin.
    /// </summary>
    private void InitializeGoogleMobileAds()
    {
        
        //text.text = "Start Initailize";

        // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
        if (_isInitialized.HasValue)
        {
            text.text = "Not Has Value";
            return;
        }

        _isInitialized = false;

        // Initialize the Google Mobile Ads Unity plugin.
        Debug.Log("Google Mobile Ads Initializing.");
        //text.text = "Initializing";

        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                //text.text = $"AdMob initialization failed.";
                _isInitialized = null;
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    //text.text = string.Format("Adapter {0} is {1}",
                        //item.Key,
                        //item.Value.InitializationState);
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            //text.text = $"AdMob initialized complete";
            _isInitialized = true;
        });
    }

    /// <summary>
    /// Opens the AdInspector.
    /// </summary>
    public void OpenAdInspector()
    {
        Debug.Log("Opening ad Inspector.");
        MobileAds.OpenAdInspector((AdInspectorError error) =>
        {
            // If the operation failed, an error is returned.
            if (error != null)
            {
                Debug.Log("Ad Inspector failed to open with error: " + error);
                return;
            }

            Debug.Log("Ad Inspector opened successfully.");
        });
    }

    /// <summary>
    /// Loads the ad.
    /// </summary>
    public void LoadAds()
    {
        //text = FindObjectOfType<UI_Game>().text;
        //text.text = "Loading rewarded ad.";
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading rewarded ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            //text.text = _adUnitId;
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                text.text = $"Rewarded ad failed to load an ad with error : \n{error}";
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                text.text = "Unexpected error: Rewarded load event fired with null ad and null error.";
                return;
            }

            // The operation completed successfully.
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            //text.text = $"Rewarded ad loaded with response : \n{ad.GetResponseInfo()}";
            _rewardedAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

            // Inform the UI that the ad is ready.
            //AdLoadedStatus?.SetActive(true);
        });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAds(AdType adType)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");
            //text.text = $"Showing rewarded ad.";
            _rewardedAd.Show((Reward reward) =>
            {
                Reward(adType);
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
            //text.text = $"Rewarded ad is not ready yet.";
        }
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
            //text.text = $"Destroyed";
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            //text.text = String.Format("Rewarded ad paid {0} {1}.",
                //adValue.Value,
                //adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
            //text.text = $"Rewarded ad recorded an impression. ";
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
            //text.text = "clicked";
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
            //text.text = "clicked";
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            //text.text = "clicked";
            LoadAds();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : "
                + error);
            //text.text = $"Rewarded ad failed to open full screen content with error : \n{error} ";

        };
    }
    void Reward(AdType adType)
    {
        Debug.Log("EearnedReward");
        //text.text = "EearnedReward";
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
}

