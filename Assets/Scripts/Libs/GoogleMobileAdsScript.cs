using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleMobileAdsScript : MonoBehaviour
{

    public static GoogleMobileAdsScript advertise;      // instance of  GoogleMobileAdsScript

    private BannerView bannerView;                      // banner ads
    private InterstitialAd interstitial;                // interstitial ads

    public string androidBanner;                        // unit id banner for android
    public string androidInterstitial;                  // unit id instersitial for android

    public string iosBanner;                            // unit id banner for IOS
    public string iosInterstitial;                      // unit id instersitial for IOS

    public AdSize adSize = AdSize.Banner;               // size and type of banner ads
    public AdPosition adPosition = AdPosition.Bottom;   // position of banner ads

    void Awake()
    {
        if (advertise == null)
        {
            // Makes the object target not be destroyed automatically when loading a new scene
            DontDestroyOnLoad(gameObject);
            advertise = this;
        }
        else if (advertise != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Create a banner 
    /// </summary>
    public void RequestBanner()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = androidBanner;
#elif UNITY_IPHONE
		string adUnitId = iosBanner;
#else
		string adUnitId = "unexpected_platform";
#endif
        try
        {
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView(adUnitId, adSize, adPosition);
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            bannerView.LoadAd(request);
            // Register for ad events.
            //bannerView.AdLoaded += HandleAdLoaded;
            // bannerView.AdFailedToLoad += HandleAdFailedToLoad;
            //  bannerView.AdOpened += HandleAdOpened;
            // bannerView.AdClosing += HandleAdClosing;
            // bannerView.AdClosed += HandleAdClosed;
            //    bannerView.AdLeftApplication += HandleAdLeftApplication;
            // Load a banner ad.
            //bannerView.LoadAd(createAdRequest());
        }
        catch { }
    }
    /// <summary>
    /// create an interstitial.
    /// </summary>
    public void RequestInterstitial()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = androidInterstitial;
#elif UNITY_IPHONE
		string adUnitId = iosInterstitial;
#else
		string adUnitId = "unexpected_platform";
#endif
        try
        {
            // Create an interstitial.
            interstitial = new InterstitialAd(adUnitId);
            // Register for ad events.
            interstitial.AdLoaded += HandleInterstitialLoaded;
            interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
            interstitial.AdOpened += HandleInterstitialOpened;
            interstitial.AdClosing += HandleInterstitialClosing;
            interstitial.AdClosed += HandleInterstitialClosed;
            interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
            // Load an interstitial ad.
            interstitial.LoadAd(createAdRequest());
        }
        catch { }
    }

    /// <summary>
    /// Create a request ads
    /// </summary>
    /// <returns>Returns an ad request with custom ad targeting.</returns>
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
            //	.AddTestDevice(AdRequest.TestDeviceSimulator)
            //	.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                .AddKeyword("game")
                .SetGender(Gender.Male)
                .SetBirthday(new DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();
    }

    /// <summary>
    /// Show interstitial if loaded
    /// </summary>
    public void ShowInterstitial()
    {
        try
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }
        }
        catch { }
    }

    /// <summary>
    /// Show banner
    /// </summary>
    public void ShowBanner()
    {
        try
        {
            bannerView.Show();
        }
        catch { }
    }

    /// <summary>
    /// hidden banner on scene
    /// </summary>
    public void HideBanner()
    {

        if (bannerView != null)
            bannerView.Hide();
    }

    /// <summary>
    /// destroy banner method
    /// </summary>
    public void DestroyBanner()
    {
        bannerView.Destroy();
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}
