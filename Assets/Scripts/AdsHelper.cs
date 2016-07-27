using UnityEngine;
using System.Collections;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using System;

public class AdsHelper : MonoBehaviour
{
    public static AdsHelper Instance;

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE
		string appKey = "";
#elif UNITY_ANDROID
    string appKey = "e0a5d378573af516ba8bbe7e9ca9d59694dcff185519e8b2";
#elif UNITY_IPHONE
	string appKey = "";
#else
		string appKey = "";
#endif

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void Init()
    {
        Appodeal.confirm(Appodeal.SKIPPABLE_VIDEO);
        Appodeal.initialize(appKey, Appodeal.INTERSTITIAL | Appodeal.BANNER | Appodeal.SKIPPABLE_VIDEO | Appodeal.REWARDED_VIDEO);
    }

    public void showInterstitial()
    {
        if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
            Appodeal.show(Appodeal.INTERSTITIAL, "interstitial_button_click");
    }

    public void showSkippableVideo()
    {
        if (Appodeal.isLoaded(Appodeal.SKIPPABLE_VIDEO))
            Appodeal.show(Appodeal.SKIPPABLE_VIDEO, "skippable_video_button_click");
    }

    public void showRewardedVideo()
    {
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
            Appodeal.show(Appodeal.REWARDED_VIDEO, "rewarded_video_button_click");
    }

    public void showBanner()
    {
            Appodeal.show(Appodeal.BANNER_BOTTOM, "banner_button_click");
    }

    public void hideBanner()
    {
        Appodeal.hide(Appodeal.BANNER);
    }
}
