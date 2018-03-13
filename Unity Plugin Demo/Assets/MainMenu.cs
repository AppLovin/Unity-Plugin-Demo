//
//  MainMenu.cs
//  Unity Plugin Demo
//
//  Created by Thomas So on 3/12/18.
//  Copyright © 2018 AppLovin. All rights reserved.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

	private Text StatusText;

	void Start ()
	{
		StatusText = GameObject.Find ("StatusText").GetComponent<Text> (); 
		StatusText.text = "";

		// Set SDK key and initialize SDK
		AppLovin.SetSdkKey (SdkKey);
		AppLovin.InitializeSdk ();
		AppLovin.SetTestAdsEnabled ("true");
		AppLovin.SetVerboseLoggingOn ("true"); // TODO: Remove
		AppLovin.SetUnityAdListener ("MainMenu");
		AppLovin.SetRewardedVideoUsername ("demo_user");

		// Preload a Rewarded Ad
		AppLovin.LoadRewardedInterstitial ();
	}

	public void ShowInterstitial ()
	{
		Log ("Showing interstitial ad");
		AppLovin.ShowInterstitial ();
	}

	// TODO: Rename
	public void ShowRewardedInterstitial ()
	{
		if (AppLovin.IsIncentInterstitialReady ()) {
			Log ("Showing rewarded ad...");
			AppLovin.ShowRewardedInterstitial ();
		} else {
			Log ("Loading rewarded ad...");
			AppLovin.LoadRewardedInterstitial ();
		}
	}

	public void ShowBanner ()
	{
		AppLovin.SetAdPosition (AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);
		AppLovin.ShowAd ();
	}

	private void onAppLovinEventReceived (string ev)
	{
		Log ("AppLovin Event: " + ev);

		if (ev.Contains ("REWARD")) {
			if (ev.Equals ("REWARDAPPROVEDINFO")) {
				// Process an event like REWARDAPPROVEDINFO:100:Credits
				char[] delimiter = { '|' };
				string[] split = ev.Split (delimiter);

				// Pull out the amount of virtual currency.
				double amount = double.Parse (split [1]);

				// Pull out the name of the virtual currency
				string currencyName = split [2];

				// Do something with this info - for example, grant coins to the user
				// myFunctionToUpdateBalance(currencyName, amount);

				Log ("Rewarded " + amount + " " + currencyName);
			}
		}
	}

	private void Log (string message)
	{
		StatusText.text = message;  
		Debug.Log (message);
	}
}
