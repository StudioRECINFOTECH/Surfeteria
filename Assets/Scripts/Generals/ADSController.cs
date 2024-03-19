using System;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS || !EDITOR
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif

public class ADSController : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardBasedVideo;

    public static ADSController Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadInterstitialAd();
            LoadRewardedAd();
        });

        CreateBannerView();
#endif
    }

#if UNITY_ANDROID || UNITY_IOS || !EDITOR
    private AdRequest adRequest = new AdRequest.Builder().Build();
#endif

    #region Banner
    [SerializeField]
    string bannerAndroid, bannerIOS;

    public void CreateBannerView()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = bannerAndroid;
#elif UNITY_IPHONE
        string adUnitId = bannerIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        Debug.Log("Creating banner view");

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        LoadAd();
#endif
    }

    public void LoadAd()
    {
        if (bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();

        Debug.Log("Loading banner ad.");
        // bannerView.LoadAd(adRequest);
    }

    public void DestroyBanner()
    {
#if UNITY_ANDROID || UNITY_IOS || !EDITOR
        this.bannerView.Destroy();
#endif
    }

    public void HideBanner()
    {
#if UNITY_ANDROID || UNITY_IOS || !EDITOR
        this.bannerView.Hide();
#endif
    }

    public void ShowBanner()
    {
#if UNITY_ANDROID || UNITY_IOS || !EDITOR
        this.bannerView.Show();
#endif
    }
    #endregion

    #region Interstitial
    [SerializeField]
    string interstitialAndroid, interstitialIOS;
    AdsState interstitialState = AdsState.None;

    public void LoadInterstitialAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = interstitialAndroid;
#elif UNITY_IPHONE
        string adUnitId = interstitialIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        Debug.Log("Loading the interstitial ad.");

        var adRequest = new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();

        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());

                interstitial = ad;
                RegisterEventHandlers(interstitial);
            });
#endif
    }

    public void ShowInterstitial()
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (interstitial != null && interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitial.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
#endif
    }

    public void DestroyInterstitial()
    {
#if UNITY_ANDROID || UNITY_IOS || !EDITOR
        this.interstitial.Destroy();
#endif
    }
    #endregion

    #region Reward Based Video
    [SerializeField]
    string rewardAndroid, rewardIOS;
    bool rewardVideoAutoShow = false;
    AdsState rewardState = AdsState.None;
    Action<bool> callBack;

    public void RequestRewardBasedVideo(bool autoShow, Action<bool> callBack = null)
    {
        Debug.Log("RewardAD Requested");
        this.callBack = callBack;
        if (rewardState == AdsState.Loaded && autoShow)
        {
            Debug.Log("Called Show REWARDED AD");
            ShowRewardBasedVideo();
            return;
        }
        else if (rewardState == AdsState.Loading)
        {
            rewardVideoAutoShow = autoShow;
            return;
        }

        rewardVideoAutoShow = autoShow;
        LoadRewardedAd();  // Ensure to load the rewarded ad when requested.
    }

    public void LoadRewardedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = rewardAndroid;
#elif UNITY_IPHONE
        string adUnitId = rewardIOS;
#else
        string adUnitId = "unexpected_platform";
#endif
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (rewardBasedVideo != null)
        {
            rewardBasedVideo.Destroy();
            rewardBasedVideo = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " +
                          ad.GetResponseInfo());

                rewardBasedVideo = ad;
                RegisterEventHandlers(rewardBasedVideo);

                // Check if autoShow is true after loading the ad.
                if (rewardVideoAutoShow)
                {
                    ShowRewardBasedVideo();
                    rewardVideoAutoShow = false;
                }
            });
#endif
    }

    public void ShowRewardBasedVideo()
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";
        Debug.Log("Showing rewarded ad is running");

        if (rewardBasedVideo != null)
        {
            Debug.Log("Showing rewarded ad.");

            rewardBasedVideo.Show((Reward reward) =>
            {
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));

                // Handle reward and inform UI or perform any other actions.

                rewardState = AdsState.None;
                if (callBack != null)
                {
                    callBack(true);
                }
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
            if (callBack != null)
            {
                callBack(false);
            }
        }
#endif
    }
    #endregion

#if UNITY_ANDROID || UNITY_IOS || !EDITOR
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            rewardState = AdsState.Loaded;
            this.callBack(false);
            if (rewardVideoAutoShow) ShowRewardBasedVideo();
        };

        ad.OnAdImpressionRecorded += () =>
        {
            this.callBack(true);
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            LoadRewardedAd();
        };
    }
#endif
}

enum AdsState
{
    None,
    Created,
    Loading,
    Loaded,
    Error
}
