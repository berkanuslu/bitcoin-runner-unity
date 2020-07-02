using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

	static UIManager _instance;
	static int instances = 0;

	public bool isPausedOrFinished = false;
	public float cameraHorizontalExtent = 0f;
	public float cameraVerticalExtent = 0f;

	[Header("Game UI Elements")]
	public Text gameCollectedCoinCount;
	public Text gameDistanceFromBeginning;

	[Header("Pause/Finish Menu Elements")]
	public Text pauseHeader;
	public Text pauseDailyWord;
	public Text pauseDistance;
	public Text pauseTotal;
	public Text pauseCoin1Count;
	public Text pauseCoin5Count;
	public Text pauseCoin10Count;
	public Text pauseCoin25Count;
	public Text pauseCoin50Count;
	public Text pauseCoin100Count;
	public Button pauseResumeButton;
	public Text pauseBestDistance;

	[Header("Powerup Activate Buttons")]
	public Button activateSecondChance;
	public Button activateSpeed;
	public Button activateShield;
	public Button activateObstacleBlaster;
	public Button activateCoinMagnet;
	public Button activateLongJump;
	public Button activateDoubleBitcoin;
	public Button activateFlinger;

	[Header("Current Powerup Images")]
	public Image collectedSecondChance;
	public Image boughtSecondChance;
	public Image collectedSpeed;
	public Image collectedShield;
	public Image collectedObstacleBlaster;
	public Image collectedCoinMagnet;
	public Image collectedLongJump;
	public Image collectedDoubleBitcoin;
	public Image collectedFlinger;

	[Header("Bitcoin Count Texts")]
	public Text shopMenuBitcoinCount;
	public Text extrasMenuBitcoinCount;

	[Header("Panel Animators")]
	public Animator inGameMenu;
	public Animator pauseFinishMenu;
	private Coroutine EndCoroutine = null;

	public static UIManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(UIManager)) as UIManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one UIManager");
		}
		else
		{
			_instance = this;
		}

		cameraHorizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
		cameraVerticalExtent = Camera.main.orthographicSize;
	}

	void Update()
	{
		if (!isPausedOrFinished)
		{
			float coinCount = ((float)LevelManager.Instance.Coins()) / 100.0f;
			gameCollectedCoinCount.text = coinCount.ToString("0.00");

			int distance = (int)LevelSpawnManager.Instance.distance;
			gameDistanceFromBeginning.text = distance.ToString();
		}
	}

	public void RestartGame()
	{
		SoundManager.Instance.StopGameMusic();
		DisableAllStartPowerups();
		this.isPausedOrFinished = false;
		CheckSecondChance();
		PlayerManager.Instance.characterAnimator.GetComponent<SpriteRenderer>().enabled = true;

		LevelManager.Instance.Restart();
		SoundManager.Instance.PlayGameMusic();
	}

	public IEnumerator RestartGameCoroutine()
	{
		yield return new WaitForSeconds(1);
		RestartGame();
	}

	public void PauseGame()
	{
		this.isPausedOrFinished = true;

		if (PlayerManager.Instance.Crashed())
			return;

		PlayerManager.Instance.Pause();
		LevelManager.Instance.PauseGame();

		pauseHeader.text = "Game Paused";
		pauseResumeButton.gameObject.SetActive(true);

		UpdatePauseFinishMenu();
	}

	public void ResumeGame()
	{
		PlayerManager.Instance.Resume();
		LevelManager.Instance.ResumeGame();
		StartCoroutine(DisablePauseGame());
	}

	private IEnumerator DisablePauseGame()
	{
		yield return new WaitForSeconds(0.1f);
		this.isPausedOrFinished = false;
	}

	public void StartGame()
	{
		DisableAllStartPowerups();
		this.isPausedOrFinished = false;
		CheckSecondChance();
		PlayerManager.Instance.characterAnimator.GetComponent<SpriteRenderer>().enabled = true;

		LevelManager.Instance.StartLevel();
	}

	private void DisableAllStartPowerups()
	{
		collectedCoinMagnet.gameObject.SetActive(false);
		collectedDoubleBitcoin.gameObject.SetActive(false);
		collectedFlinger.gameObject.SetActive(false);
		collectedLongJump.gameObject.SetActive(false);
		collectedObstacleBlaster.gameObject.SetActive(false);
		collectedSecondChance.gameObject.SetActive(false);
		collectedShield.gameObject.SetActive(false);
		collectedSpeed.gameObject.SetActive(false);
		boughtSecondChance.gameObject.SetActive(false);
		activateCoinMagnet.gameObject.SetActive(false);
		activateDoubleBitcoin.gameObject.SetActive(false);
		activateFlinger.gameObject.SetActive(false);
		activateLongJump.gameObject.SetActive(false);
		activateObstacleBlaster.gameObject.SetActive(false);
		activateSecondChance.gameObject.SetActive(false);
		activateShield.gameObject.SetActive(false);
		activateSpeed.gameObject.SetActive(false);
	}

	public void ReturnToMainMenu()
	{
		LevelManager.Instance.QuitGame();

		PlayerManager.Instance.characterAnimator.enabled = false;
		PlayerManager.Instance.characterAnimator.GetComponent<SpriteRenderer>().enabled = false;
	}

	public void ShowEnd()
	{
		inGameMenu.SetTrigger("HidePanel");
		pauseFinishMenu.gameObject.SetActive(true);
		pauseFinishMenu.SetTrigger("ShowPanel");

		SoundManager.Instance.StopGameMusic();

		pauseHeader.text = "Game Over";
		pauseResumeButton.gameObject.SetActive(false);

#if (!UNITY_EDITOR)
		//report score to the leader board
		long reportedScore = LevelManager.Instance.Coins();
		SocialLeaderboardManager.Instance.ReportScore(reportedScore, SocialLeaderboardManager.LeaderboardIDs.TopBitcoiner);

		int reportedDistance = (int)LevelSpawnManager.Instance.distance;
		SocialLeaderboardManager.Instance.ReportScore(reportedDistance, SocialLeaderboardManager.LeaderboardIDs.TopRunner);
#endif
		//Add the collected coins to the account
		PreferencesManager.Instance.SetCoins(PreferencesManager.Instance.GetCoins() + LevelManager.Instance.Coins());

		FirebaseEventManager.Instance.SendEarnVirtualCurrency();

		UpdatePauseFinishMenu();
	}

	private void UpdatePauseFinishMenu()
	{
		int currentDistance = (int)LevelSpawnManager.Instance.distance;
		float currentCoinCount = ((float)LevelManager.Instance.Coins()) / 100.0f;

		//Apply the data to the pause/finish menu
		pauseDistance.text = currentDistance + "M";
		pauseTotal.text = currentCoinCount.ToString("0.00");

		//If the current distance is greater than the best distance
		if (currentDistance > PreferencesManager.Instance.GetBestDistance())
		{
			pauseBestDistance.gameObject.SetActive(true);
			PreferencesManager.Instance.SetBestDistance(currentDistance);
		}
		else
		{
			pauseBestDistance.gameObject.SetActive(false);
		}

		string completedText = "";
		if (PreferencesManager.Instance.HasCompletedWord())
		{
			completedText = PreferencesManager.Instance.GetCompletedWord();
		}
		pauseDailyWord.text = completedText;
		pauseCoin1Count.text = LevelManager.Instance.Coins1().ToString();
		pauseCoin5Count.text = LevelManager.Instance.Coins5().ToString();
		pauseCoin10Count.text = LevelManager.Instance.Coins10().ToString();
		pauseCoin25Count.text = LevelManager.Instance.Coins25().ToString();
		pauseCoin50Count.text = LevelManager.Instance.Coins50().ToString();
		pauseCoin100Count.text = LevelManager.Instance.Coins100().ToString();

		SoundManager.Instance.UpdateAudioButton();

		CheckDailyWordCompleted();
	}

	public void CheckDailyWordCompleted()
	{
		string dailyWord = PreferencesManager.Instance.GetDailyWord();
		string completedWord = PreferencesManager.Instance.GetCompletedWord();
		if (dailyWord.Equals(completedWord))
		{

			PreferencesManager.Instance.SetDailyWordCompleted();
		}
	}

	public void ShowSecondChance()
	{
		activateSecondChance.gameObject.GetComponent<GameObjectHelper>().EnableDisable(true, 4.0f);
		EndCoroutine = StartCoroutine(ShowEndTimeout(4.0f));
	}

	IEnumerator ShowEndTimeout(float time)
	{
		yield return new WaitForSeconds(time);

		ShowEnd();
	}

	public void DisableShowEndTimeout()
	{
		if (EndCoroutine != null)
		{
			StopCoroutine(EndCoroutine);
		}
	}

	public void UpdateMenuCoinCounts()
	{
		float coins = ((float)PreferencesManager.Instance.GetCoins()) / 100.0f;
		extrasMenuBitcoinCount.text = coins.ToString("0.00");
		shopMenuBitcoinCount.text = coins.ToString("0.00");
	}

	public IEnumerator ShowStartPowerups()
	{

		yield return new WaitForSeconds(0.5f);
		activateSpeed.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("Speed"));
		activateSecondChance.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("SecondChance"));
		activateShield.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("Shield"));
		activateObstacleBlaster.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("ObstacleBlaster"));
		activateCoinMagnet.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("CoinMagnet"));
		activateLongJump.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("LongJump"));
		activateDoubleBitcoin.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("DoubleBitcoin"));
		activateFlinger.gameObject.GetComponent<GameObjectHelper>().EnableDisable(PreferencesManager.Instance.HasPowerup("Flinger"));
	}

	public void CheckSecondChance()
	{
		//If the player owns a second chance power up, display it's heart icon
		boughtSecondChance.gameObject.SetActive(PreferencesManager.Instance.HasPowerup("SecondChance"));
	}

	public void ActivateShieldPowerup()
	{
		PlayerManager.Instance.ActivateShield();
		PreferencesManager.Instance.ModifyPowerup("Shield", -1);
		activateShield.gameObject.SetActive(false);
	}
	public void ActivateSpeedPowerup()
	{
		PlayerManager.Instance.ActivateSpeed();
		PreferencesManager.Instance.ModifyPowerup("Speed", -1);
		activateSpeed.gameObject.SetActive(false);
	}
	public void ActivateObstacleBlasterPowerup()
	{
		PlayerManager.Instance.ActivateObstacleBlaster();
		PreferencesManager.Instance.ModifyPowerup("ObstacleBlaster", -1);
		activateObstacleBlaster.gameObject.SetActive(false);
	}

	public void ActivateCoinMagnetPowerup()
	{
		PlayerManager.Instance.ActivateCoinMagnet();
		PreferencesManager.Instance.ModifyPowerup("CoinMagnet", -1);
		activateCoinMagnet.gameObject.SetActive(false);
	}
	public void ActivateLongJumpPowerup()
	{
		PlayerManager.Instance.ActivateLongJump();
		PreferencesManager.Instance.ModifyPowerup("LongJump", -1);
		activateLongJump.gameObject.SetActive(false);
	}
	public void ActivateFlingerPowerup()
	{
		PlayerManager.Instance.ActivateFlinger();
		PreferencesManager.Instance.ModifyPowerup("Flinger", -1);
		activateFlinger.gameObject.SetActive(false);
	}
	public void ActivateDoubleBitcoinPowerup()
	{
		PlayerManager.Instance.ActivateDoubleBitcoin();
		PreferencesManager.Instance.ModifyPowerup("DoubleBitcoin", -1);
		activateDoubleBitcoin.gameObject.SetActive(false);
	}
}
