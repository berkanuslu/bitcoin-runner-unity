using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	static LevelManager _instance;
	static int instances = 0;

	Dictionary<string, int> coinDictionary = new Dictionary<string, int>();
	public int coinParameter = 1;
	public bool clearAllData = false;

	public static LevelManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one LevelManager");
		}
		else
		{
			_instance = this;
		}

		coinDictionary.Add("Coin1", 0);
		coinDictionary.Add("Coin5", 0);
		coinDictionary.Add("Coin10", 0);
		coinDictionary.Add("Coin25", 0);
		coinDictionary.Add("Coin50", 0);
		coinDictionary.Add("Coin100", 0);
		coinDictionary.Add("CoinTotal", 0);

		if (clearAllData)
		{
			PreferencesManager.Instance.CreateBlankData();
			PreferencesManager.Instance.ClearAllDailyWordRecords();
		}
	}

	public void StartLevel()
	{
		StartCoroutine(LevelSpawnManager.Instance.StartLevelSpawner(1.25f));

		PlayerManager.Instance.ResetStatus(true);
		StartCoroutine(UIManager.Instance.ShowStartPowerups());
		SoundManager.Instance.PlayGameMusic();

		coinDictionary["Coin1"] = 0;
		coinDictionary["Coin5"] = 0;
		coinDictionary["Coin10"] = 0;
		coinDictionary["Coin25"] = 0;
		coinDictionary["Coin50"] = 0;
		coinDictionary["Coin100"] = 0;
		coinDictionary["CoinTotal"] = 0;
	}

	public void PauseGame()
	{
		PlayerManager.Instance.DisablePlayerControls();
		LevelSpawnManager.Instance.Pause();

		SoundManager.Instance.PauseGameMusic();
	}

	public void ResumeGame()
	{
		PlayerManager.Instance.EnablePlayerControls();
		LevelSpawnManager.Instance.Resume();

		SoundManager.Instance.PlayGameMusic();
	}

	//Called when the click from Button_ActivateSecondChance
	public void SecondChance()
	{
		StartCoroutine(PlayerManager.Instance.UseSecondChance());
	}

	public void CoinGathered(int value)
	{
		if (value == 5)
		{
			coinDictionary["Coin5"] += 1 * coinParameter;
		}
		else if (value == 10)
		{
			coinDictionary["Coin10"] += 1 * coinParameter;
		}
		else if (value == 25)
		{
			coinDictionary["Coin25"] += 1 * coinParameter;
		}
		else if (value == 50)
		{
			coinDictionary["Coin50"] += 1 * coinParameter;
		}
		else if (value == 100)
		{
			coinDictionary["Coin100"] += 1 * coinParameter;
		}
		else
		{
			coinDictionary["Coin1"] += 1 * coinParameter;
		}

		coinDictionary["CoinTotal"] += value * coinParameter;
	}

	public void CoinParameterNormal()
	{
		coinParameter = 1;
	}

	public void CoinParameterDouble()
	{
		coinParameter = 2;
	}

	public int Coins()
	{
		return coinDictionary["CoinTotal"];
	}

	public int Coins1()
	{
		return coinDictionary["Coin1"];
	}

	public int Coins5()
	{
		return coinDictionary["Coin5"];
	}

	public int Coins10()
	{
		return coinDictionary["Coin10"];
	}

	public int Coins25()
	{
		return coinDictionary["Coin25"];
	}

	public int Coins50()
	{
		return coinDictionary["Coin50"];
	}

	public int Coins100()
	{
		return coinDictionary["Coin100"];
	}

	public void Restart()
	{
		coinDictionary["Coin1"] = 0;
		coinDictionary["Coin5"] = 0;
		coinDictionary["Coin10"] = 0;
		coinDictionary["Coin25"] = 0;
		coinDictionary["Coin50"] = 0;
		coinDictionary["Coin100"] = 0;
		coinDictionary["CoinTotal"] = 0;

		SoundManager.Instance.PlayGameMusic();
		LevelSpawnManager.Instance.Restart(true);
		PlayerManager.Instance.ResetStatus(true);
		UIManager.Instance.CheckSecondChance();
		StartCoroutine(UIManager.Instance.ShowStartPowerups());
	}

	public void QuitGame()
	{
		SoundManager.Instance.StopGameMusic();
		LevelSpawnManager.Instance.Restart(false);
		PlayerManager.Instance.ResetStatus(false);
	}
}
