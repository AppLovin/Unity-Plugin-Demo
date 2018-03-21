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
	// Put AppLovin SDK Key here or in your AndroidManifest.xml / Info.plist
	private const string SDK_KEY = "YOUR_SDK_KEY_HERE";

	// Rewarded Video Button Texts
	private const string REWARDED_VIDEO_BUTTON_TITLE_PRELOAD = "Preload Rewarded Video";
	private const string REWARDED_VIDEO_BUTTON_TITLE_LOADING = "Show Rewarded Video";
	private const string REWARDED_VIDEO_BUTTON_TITLE_SHOW = "Show Rewarded Video";

	// UI Components
	private Text RewardedVideoButtonTitle;
	private Text StatusText;

	// Controlled State
	private bool IsPreloadingRewardedVideo = false;

	void Start ()
	{
		// Setup UI
		RewardedVideoButtonTitle = GameObject.Find ("RewardedVideoButtonTitle").GetComponent<Text> ();
		RewardedVideoButtonTitle.text = REWARDED_VIDEO_BUTTON_TITLE_PRELOAD;
		StatusText = GameObject.Find ("StatusText").GetComponent<Text> ();
		StatusText.text = "";

		// Check if user replaced the SDK key
		if ("YOUR_SDK_KEY_HERE".Equals (SDK_KEY)) {
			StatusText.text = "ERROR: PLEASE UPDATE YOUR SDK KEY IN Assets/MainMenu.cs";
		} else {
			// Set SDK key and initialize SDK
			AppLovin.SetSdkKey (SDK_KEY);
			AppLovin.InitializeSdk ();
			AppLovin.SetTestAdsEnabled ("true");
			AppLovin.SetUnityAdListener ("MainMenu");
			AppLovin.SetRewardedVideoUsername ("demo_user");
		}
	}

	public void ShowInterstitial ()
	{
		Log ("Showing interstitial ad");

		// Optional: You can call `AppLovin.PreloadInterstitial()` and listen to the "LOADED" event to preload the ad from the network before showing it
		AppLovin.ShowInterstitial ();
	}

	public void PreloadOrShowRewardedInterstitial ()
	{
		if (AppLovin.IsIncentInterstitialReady ()) {

			Log ("Showing rewarded ad...");

			IsPreloadingRewardedVideo = false;
			RewardedVideoButtonTitle.text = REWARDED_VIDEO_BUTTON_TITLE_PRELOAD;

			AppLovin.ShowRewardedInterstitial ();
		} else {

			Log ("Preloading rewarded ad...");

			IsPreloadingRewardedVideo = true;
			RewardedVideoButtonTitle.text = REWARDED_VIDEO_BUTTON_TITLE_LOADING;

			AppLovin.LoadRewardedInterstitial ();
		}
	}

	public void ShowBanner ()
	{
		Log ("Showing banner ad");
		AppLovin.ShowAd (AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);
	}

	private void onAppLovinEventReceived (string ev)
	{
		// Log AppLovin event
		Log (ev);

		//
		// Special Handling for Rewarded events
		//

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
		// Check if this is a Rewarded Video preloading event
		else if (IsPreloadingRewardedVideo && (ev.Equals ("LOADED") || ev.Equals ("LOADFAILED"))) {

			IsPreloadingRewardedVideo = false;

			if (ev.Equals ("LOADED")) {
				RewardedVideoButtonTitle.text = REWARDED_VIDEO_BUTTON_TITLE_SHOW;
			} else {
				RewardedVideoButtonTitle.text = REWARDED_VIDEO_BUTTON_TITLE_PRELOAD;
			}
		}
	}

	private void Log (string message)
	{
		StatusText.text = message;
		Debug.Log (message);
	}
}
