using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class PreferencesManager : MonoBehaviour
{
	static PreferencesManager _instance;
	static int instances = 0;

	public static PreferencesManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(PreferencesManager)) as PreferencesManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one PreferencesManager");
		}
		else
		{
			_instance = this;
		}

		if (!PlayerPrefs.HasKey("CoinCount"))
		{
			CreateBlankData();
		}
	}

	public void CreateBlankData()
	{
		PlayerPrefs.SetInt("CoinCount", 0);
		PlayerPrefs.SetInt("BestDistance", 0);
		PlayerPrefs.SetInt("PlayCount", 0);
		PlayerPrefs.SetInt("NoAdMode", 0);
		PlayerPrefs.SetInt("Speed", 0);
		PlayerPrefs.SetInt("Shield", 0);
		PlayerPrefs.SetInt("ObstacleBlaster", 0);
		PlayerPrefs.SetInt("SecondChance", 0);
		PlayerPrefs.SetInt("CoinMagnet", 0);
		PlayerPrefs.SetInt("LongJump", 0);
		PlayerPrefs.SetInt("DoubleBitcoin", 0);
		PlayerPrefs.SetInt("Flinger", 0);

		PlayerPrefs.Save();
	}

	public int GetCoins()
	{
		return PlayerPrefs.GetInt("CoinCount", 0);
	}

	public int GetBestDistance()
	{
		return PlayerPrefs.GetInt("BestDistance", 0);
	}

	public int GetPlayCount()
	{
		return PlayerPrefs.GetInt("PlayCount", 0);
	}

	public int GetNoAdMode()
	{
		return PlayerPrefs.GetInt("NoAdMode", 0);
	}

	public int GetPowerup(string key)
	{
		return PlayerPrefs.GetInt(key, 0);
	}

	public bool HasPowerup(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetInt(key, 0) > 0;
		}
		return false;
	}

	public void SetFirstOpen()
	{
		PlayerPrefs.SetInt("first_open", 1);
		PlayerPrefs.Save();
	}

	public bool HasFirstOpen()
	{
		return PlayerPrefs.HasKey("first_open");
	}

	public void SetCoins(int count)
	{
		PlayerPrefs.SetInt("CoinCount", count);
		PlayerPrefs.Save();
	}

	public void IncreaseCoinCount()
	{
		int coinCount = this.GetCoins();
		coinCount++;
		PlayerPrefs.SetInt("CoinCount", coinCount);
		PlayerPrefs.Save();
	}

	public void SetBestDistance(int distance)
	{
		PlayerPrefs.SetInt("BestDistance", distance);
		PlayerPrefs.Save();
	}

	public void SetNoAdMode(int mode)
	{
		PlayerPrefs.SetInt("NoAdMode", mode);
		PlayerPrefs.Save();
	}

	public void SetPlayCount(int count)
	{
		PlayerPrefs.SetInt("PlayCount", count);
		PlayerPrefs.Save();
	}

	public void IncreasePlayCount()
	{
		int playCount = this.GetPlayCount();
		playCount++;
		PlayerPrefs.SetInt("PlayCount", playCount);
		PlayerPrefs.Save();
	}

	public void ModifyPowerup(string key, int value)
	{
		int loadedValue = PlayerPrefs.GetInt(key, 0);
		loadedValue += value;
		PlayerPrefs.SetInt(key, loadedValue);
		PlayerPrefs.Save();
	}

	public bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public List<DailyWordRow> GetDailyWordRows()
	{
		List<DailyWordRow> wordRows = new List<DailyWordRow>();
		for (int i = 0; i < 365; i++)
		{
			string dailyWordKey = "DailyWord" + i.ToString();
			if (PlayerPrefs.HasKey(dailyWordKey))
			{
				DateTime dt = new DateTime(DateTime.Now.Year, 1, 1).AddDays(i - 1);
				wordRows.Add(new DailyWordRow(PlayerPrefs.GetString(dailyWordKey), dt.ToShortDateString()));
			}
		}
		return wordRows;
	}

	public string GetCompletedWord()
	{
		string completedWordKey = "DailyWord" + (DateTime.Now.DayOfYear - 1).ToString();
		return PlayerPrefs.GetString(completedWordKey);
	}

	public void SetCompletedWord(string completedWord)
	{
		string completedWordKey = "DailyWord" + (DateTime.Now.DayOfYear - 1).ToString();
		PlayerPrefs.SetString(completedWordKey, completedWord);
		PlayerPrefs.Save();
	}

	public bool HasCompletedWord()
	{
		string dailyWordKey = "DailyWord" + (DateTime.Now.DayOfYear - 1).ToString();
		return PlayerPrefs.HasKey(dailyWordKey);
	}

	public bool HasAudioVolume()
	{
		return PlayerPrefs.HasKey("AudioVolume");
	}

	public int GetAudioVolume()
	{
		return PlayerPrefs.GetInt("AudioVolume");
	}

	public void SetAudioVolume(int volume)
	{
		PlayerPrefs.SetInt("AudioVolume", volume);
		PlayerPrefs.Save();
	}

	public void ClearAllDailyWordRecords()
	{
		PlayerPrefs.DeleteKey("Words");

		for (int i = 0; i < 365; i++)
		{
			string dailyWordKey = "DailyWord" + i.ToString();
			if (PlayerPrefs.HasKey(dailyWordKey))
			{
				PlayerPrefs.DeleteKey(dailyWordKey);
			}
		}

		Debug.Log("All words deleted.");
	}

	public string GetDailyWord()
	{
		string xmlText = PlayerPrefs.GetString("Words");

		ArrayList wordsList = new ArrayList();

		StringReader rd = new StringReader(xmlText);

		if (rd == null)
		{
			return "";
		}
		else
		{
			int i = -1;
			do
			{
				i++;
				wordsList.Add(rd.ReadLine());
			}
			while (wordsList[i] != null);

			return (wordsList[DateTime.Now.DayOfYear - 1].ToString());
		}
	}

	public void LoadDailyWords()
	{
		if (PlayerPrefs.GetString("Words") == "")
		{
			TextAsset puzdata = (TextAsset)Resources.Load("words", typeof(TextAsset));
			StringReader reader = new StringReader(puzdata.text);

			if (reader != null)
			{
				PlayerPrefs.SetString("Words", reader.ReadToEnd());
				PlayerPrefs.Save();
				Debug.Log("Words successfully loaded.");
			}
		}
	}

	public void InitializeDailyWord(string word)
	{
		string completedWordKey = "DailyWord" + (DateTime.Now.DayOfYear - 1).ToString();
		if (!PlayerPrefs.HasKey(completedWordKey))
		{
			string uncompletedWord = string.Empty;
			for (int i = 0; i < word.Length; i++)
			{
				if (word[i] == ' ')
				{
					uncompletedWord += " ";
				}
				else if (word[i] == '\'')
				{
					uncompletedWord += "'";
				}
				else
				{
					uncompletedWord += "-";
				}
			}
			Debug.Log("uncompletedWord : " + uncompletedWord);
			PlayerPrefs.SetString(completedWordKey, uncompletedWord);
			PlayerPrefs.Save();
		}
	}

	public void SetDailyWordCompleted()
	{
		string completedWordKey = "CompletedDailyWord" + (DateTime.Now.DayOfYear - 1).ToString();
		if (!PlayerPrefs.HasKey(completedWordKey))
		{
			PlayerPrefs.SetInt(completedWordKey, 1);
			PlayerPrefs.Save();
			FirebaseEventManager.Instance.SendLevelUp();
			Debug.Log("Congrats! Daily words completed.");
		}
	}
}