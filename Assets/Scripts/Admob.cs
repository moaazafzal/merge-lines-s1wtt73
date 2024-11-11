using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class Admob : MonoBehaviour
{
	private InterstitialAd adInterstitial;


	private string idApp, idBanner, idInterstitial, idReward;

	void Start ()
	{
		idApp = "ca-app-pub-3940256099942544~3347511713";
		idBanner = "ca-app-pub-3940256099942544/6300978111";
		idInterstitial = "ca-app-pub-3940256099942544/1033173712";
		idReward = "ca-app-pub-3940256099942544/5224354917";

		MobileAds.Initialize (idApp);
		
		RequestInterstitialAd (); 
		
	}
	

	#region Interstitial methods ---------------------------------------------

	public void RequestInterstitialAd ()
	{
		adInterstitial = new InterstitialAd (idInterstitial);
		AdRequest request = AdRequestBuild ();
		adInterstitial.LoadAd (request);

		//attach events
		adInterstitial.OnAdLoaded += this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening += this.HandleOnAdOpening;
		adInterstitial.OnAdClosed += this.HandleOnAdClosed;
	}

	
	public void ShowInterstitialAd ()
	{
		if (adInterstitial.IsLoaded ())
			adInterstitial.Show ();
	}

	public void DestroyInterstitialAd ()
	{
		adInterstitial.Destroy ();
	}

	//interstitial ad events
	public void HandleOnAdLoaded (object sender, EventArgs args)
	{
	}

	public void HandleOnAdOpening (object sender, EventArgs args)
	{
	}

	public void HandleOnAdClosed (object sender, EventArgs args)
	{
		//this method executes when interstitial ad is closed
		adInterstitial.OnAdLoaded -= this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening -= this.HandleOnAdOpening;
		adInterstitial.OnAdClosed -= this.HandleOnAdClosed;

		RequestInterstitialAd (); //request new interstitial ad after close
	}

	#endregion

	
	
	//------------------------------------------------------------------------
	AdRequest AdRequestBuild ()
	{
		return new AdRequest.Builder ().Build ();
	}

	void OnDestroy ()
	{
		DestroyInterstitialAd ();

		//dettach events
		adInterstitial.OnAdLoaded -= this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening -= this.HandleOnAdOpening;
		adInterstitial.OnAdClosed -= this.HandleOnAdClosed;
	}

}

