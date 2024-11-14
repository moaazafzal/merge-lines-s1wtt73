using System;
using System.Collections;
using UnityEngine;

public class _Ads : MonoBehaviour
{
    public static _Ads instance = null;

    public BannerPosition bannerPosition;

    #region singalton
    void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public void OnEnable()
    {
        Advertisements.Instance.Initialize();
        //StartCoroutine(BannerLoop());
    }

    private void Start()
    {
        hasBanner = Advertisements.Instance.IsBannerAvailable();
        ShowBanner(bannerPosition);
           
    }

    #region Banner
    public bool hasBanner;

    private IEnumerator BannerLoop()
    {

        yield return new WaitForSeconds(2f);
        hasBanner = Advertisements.Instance.IsBannerAvailable();

        while (hasBanner)
        {
            yield return new WaitForSeconds(5f);
            ShowBanner(bannerPosition);
            hasBanner = Advertisements.Instance.IsBannerAvailable();
        }

        yield return new WaitForSeconds(15f);
        StartCoroutine(BannerLoop());
    }

    public void ShowBanner(BannerPosition _bannerPosition)
    {
        bannerPosition = _bannerPosition;
        Advertisements.Instance.ShowBanner(bannerPosition);

    }

    public void HideBanner()
    {

        Advertisements.Instance.HideBanner();


    }
    #endregion

    #region Interstitial

    private static System.Action _onIntestitialAction;

    public void ShowInterstitialAd(string location, Action onContinueAction = null)
    {
        bool isLoaded = IsInterstitialAdLoaded();
        _onIntestitialAction = onContinueAction;
        if (onContinueAction != null)
        {
            if (isLoaded)
            {
                Advertisements.Instance.ShowInterstitial(OnInterstitialAdClosedEvent);
            }
            else
            {
                onContinueAction();
            }
        }

    }

    private void OnInterstitialAdClosedEvent()
    {
        if (_onIntestitialAction != null)
        {
            _onIntestitialAction?.Invoke();
            _onIntestitialAction = null;
           // ByteBrewSDK. ByteBrew.TrackAdEvent("Interstitial", "timer", "3253k3302-3r3j4i343-3nij343-405403", "unity ad");

        }

    }

    bool IsInterstitialAdLoaded()
    {
        return Advertisements.Instance.IsInterstitialAvailable();
    }
    #endregion

    #region RewardedVideo
    private static System.Action<bool> _onResultAction;
    public void ShowRewardedAd(string location, System.Action<bool> onComplete)
    {

        _onResultAction = onComplete;
        bool isLoaded = Is_RewardedVideoAvalaible();
        if (onComplete != null)
        {
            if (isLoaded)
            {
                Advertisements.Instance.ShowRewardedVideo(OnRewardedAdClosedEvent);
            }
            else
            {
                onComplete.Invoke(false);
            }
        }
    }

    private void OnRewardedAdClosedEvent(bool rewardGiven)
    {
        if (_onResultAction != null)
        {
            if (rewardGiven)
            {
                _onResultAction.Invoke(true);
                //TimerAd.instance.ResetTimer();
            }
            else
            {
                _onResultAction.Invoke(false);
            }
        }
        _onResultAction = null;

    }

    public bool Is_RewardedVideoAvalaible()
    {
        return Advertisements.Instance.IsRewardVideoAvailable();
    }
    #endregion

}