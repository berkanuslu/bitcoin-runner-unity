using UnityEngine;
//using GoogleMobileAds.Api;

public class AdvertisementManager : MonoBehaviour
{
	static AdvertisementManager _instance;
	static int instances = 0;

	// private InterstitialAd interstitial;

	// private RewardBasedVideoAd rewardBasedVideo;

	public bool showTestAds = true;

	public static AdvertisementManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(AdvertisementManager)) as AdvertisementManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one AdvertisementManager");
		}
		else
		{
			_instance = this;
		}

		// StartAds();
	}

	public void StartAds()
	{
#if UNITY_ANDROID
		string appId = "ca-app-pub-2";
#elif UNITY_IOS
		string appId = "ca-app-pub-0";
#else
		string appId = "unexpected_platform";
#endif
		Debug.Log("appId: " + appId);
		// MobileAds.SetiOSAppPauseOnBackground(true);
		// // Initialize the Google Mobile Ads SDK.
		// MobileAds.Initialize(appId);

		// InitRewardedVideo();
		// InitInterstitial();
	}

	private void InitInterstitial()
	{
#if UNITY_ANDROID
		string adUnitIdReal = "ca-app-pub-23";
#elif UNITY_IOS
		string adUnitIdReal = "ca-app-pub-23";
#else
		string adUnitIdReal = "unexpected_platform";
#endif
#if UNITY_ANDROID
		string adUnitIdTest = "ca-app-pub-39";
#elif UNITY_IOS
		string adUnitIdTest = "ca-app-pub-39";
#else
		string adUnitIdTest = "unexpected_platform";
#endif

		string adUnitId = adUnitIdTest;
		if (!this.showTestAds)
		{
			adUnitId = adUnitIdReal;
		}
		Debug.Log("adUnitId: " + adUnitId);

		// if (this.interstitial != null)
		// {
		// 	this.interstitial.Destroy();
		// }

		// // Initialize an InterstitialAd.
		// this.interstitial = new InterstitialAd(adUnitId);

		// // Called when the ad is closed.
		// this.interstitial.OnAdClosed += HandleOnAdClosed;

		// // Create an empty ad request.
		// AdRequest request = new AdRequest.Builder()
		// 	.AddTestDevice("67")
		// 	.AddTestDevice("D9")
		// 	.AddTestDevice("5d")
		// 	.AddTestDevice("44")
		// 	.AddTestDevice("31")
		// 	.Build();
		// // Load the interstitial with the request.
		// this.interstitial.LoadAd(request);
	}

	public void ShowAds()
	{
		// if (this.interstitial.IsLoaded())
		// {
		// 	this.interstitial.Show();
		// }
#if UNITY_EDITOR
		UIManager.Instance.RestartGame();
#endif
	}

	// public void HandleOnAdClosed(object sender, EventArgs args)
	// {
	// 	Debug.Log("HandleAdClosed event received");
	// 	this.InitInterstitial();
	// 	StartCoroutine(UIManager.Instance.RestartGameCoroutine());
	// }

	private void InitRewardedVideo()
	{
#if UNITY_ANDROID
		string adUnitIdReal = "ca-app-pub-23";
#elif UNITY_IOS
		string adUnitIdReal = "ca-app-pub-23";
#else
		string adUnitIdReal = "unexpected_platform";
#endif
#if UNITY_ANDROID
		string adUnitIdTest = "ca-app-pub-39";
#elif UNITY_IOS
		string adUnitIdTest = "ca-app-pub-39";
#else
		string adUnitIdTest = "unexpected_platform";
#endif

		string adUnitId = adUnitIdTest;
		if (!this.showTestAds)
		{
			adUnitId = adUnitIdReal;
		}
		Debug.Log("adUnitId: " + adUnitId);

		// if (this.rewardBasedVideo != null)
		// {
		// 	// Called when the user should be rewarded for watching a video.
		// 	this.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
		// 	// Called when the ad is closed.
		// 	this.rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;

		// 	this.rewardBasedVideo = null;
		// }

		// Get singleton reward based video ad reference.
		// this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// // Called when the user should be rewarded for watching a video.
		// this.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		// // Called when the ad is closed.
		// this.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;

		// // Create an empty ad request.
		// AdRequest request = new AdRequest.Builder()
		// 	.AddTestDevice("67")
		// 	.AddTestDevice("D9")
		// 	.AddTestDevice("5d")
		// 	.AddTestDevice("44")
		// 	.AddTestDevice("31")
		// 	.Build();
		// // Load the rewarded video ad with the request.
		// this.rewardBasedVideo.LoadAd(request, adUnitId);
	}

	public void WatchAds()
	{
		// if (this.rewardBasedVideo.IsLoaded())
		// {
		// 	this.rewardBasedVideo.Show();
		// }
	}

	// public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	// {
	// 	Debug.Log("HandleRewardBasedVideoClosed event received");
	// 	this.InitRewardedVideo();
	// }

	// public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	// {
	// 	string type = args.Type;
	// 	double amount = args.Amount;
	// 	Debug.Log("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);

	// 	for (int i = 0; i < amount; i++)
	// 	{
	// 		PreferencesManager.Instance.IncreaseCoinCount();
	// 	}
	// 	UIManager.Instance.UpdateMenuCoinCounts();
	// 	FirebaseEventManager.Instance.SendCustomEvent("reward_from_ad");
	// }

	// void OnDestroy()
	// {
	// 	if (this.interstitial != null)
	// 	{
	// 		this.interstitial.Destroy();
	// 	}

	// 	if (this.rewardBasedVideo != null)
	// 	{
	// 		this.rewardBasedVideo = null;
	// 	}
	// }
}