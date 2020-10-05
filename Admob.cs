using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour
{

    public static Admob inst = null;   
    InterstitialAd interstitial;

    // Use this for initialization
    void Awake()
    {
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }
    }

    public void loadBannerAd()
    {
        print("LOAD ADV");
        string adID = "ca-app-pub-9910378041227673/2262095207";
        //string deviceId = "B21E137E9B0F4CA8";

        interstitial = new InterstitialAd(adID);

        AdRequest request = new AdRequest.Builder().Build();

        /*
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
            .AddTestDevice(deviceId)  // My test device.
            .Build();
         */

        interstitial.LoadAd(request);

    }

    public void showInterstitialAd()
    {
        if (interstitial.IsLoaded())
        {
            //Условия для показа!!!
            if (Saves.inst.ADS_COUNTER % 1 == 0)
            {
                print("SHOW ADV");
                interstitial.Show();
            }
        }
    }

}
