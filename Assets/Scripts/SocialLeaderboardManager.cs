using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class SocialLeaderboardManager : MonoBehaviour
{
	static SocialLeaderboardManager _instance;
	static int instances = 0;

	//public string leaderboardID;
	private bool isAuthenticated;

	public string topRunnersIOSId = "";
	public string topBitcoinersIOSId = "";
	public string topRunnersAndroidId = "Cgk";
	public string topBitcoinersAndroidId = "Cgk";

	public enum LeaderboardIDs
	{
		TopRunner,
		TopBitcoiner
	}

	public static SocialLeaderboardManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType(typeof(SocialLeaderboardManager)) as SocialLeaderboardManager;

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
			Debug.LogWarning(": There are more than one SocialLeaderboardManager");
		else
			_instance = this;

		// #if UNITY_ANDROID
		// 			PlayGamesPlatform.Activate();
		// #endif
		Social.localUser.Authenticate(ProcessAuthentication);

	}

	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server.
	private void ProcessAuthentication(bool success)
	{
		if (success)
		{
			Debug.Log("Authenticated, checking achievements");

			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements(ProcessLoadedAchievements);
		}
		else
			Debug.Log("Failed to authenticate");
	}

	private void ProcessLoadedAchievements(IAchievement[] achievements)
	{
		if (achievements.Length == 0)
			Debug.Log("Error: no achievements found");
		else
			Debug.Log("Got " + achievements.Length + " achievements");
	}

	public void ReportProgress(string achievementId, double progress)
	{
		if (Social.localUser.authenticated)
		{
			Social.ReportProgress(achievementId, progress, success =>
			{
				if (success)
					Debug.Log("Successfully reported achievement progress");
				else
					Debug.Log("Failed to report achievement");
			});
		}
		else
		{
			Debug.Log("Not authenticated");
		}
	}

	public void ReportScore(long score, LeaderboardIDs leaderboardID)
	{
		string leaderboard = "";
		switch (leaderboardID)
		{
			case LeaderboardIDs.TopRunner:
#if UNITY_IOS
				leaderboard = topRunnersIOSId;
#elif UNITY_ANDROID
				leaderboard = topRunnersAndroidId;
#endif
				break;
			case LeaderboardIDs.TopBitcoiner:
#if UNITY_IOS
				leaderboard = topBitcoinersIOSId;
#elif UNITY_ANDROID
				leaderboard = topBitcoinersAndroidId;
#endif
				break;
		}

		if (Social.localUser.authenticated)
		{
			Debug.Log("Reporting score " + score + " on leader board " + leaderboard);
			Social.ReportScore(score, leaderboard, success =>
			{
				if (success)
				{
					Debug.Log("Reported score successfully");
				}
				else
				{
					Debug.Log("Failed to report score");
				}
			});
		}
		else
		{
			Debug.Log("Not authenticated");
		}
	}

	public void ShowLeaderboardUI()
	{
#if UNITY_IOS
			Social.ShowLeaderboardUI();
// #elif UNITY_ANDROID
// 			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
#endif
	}
}