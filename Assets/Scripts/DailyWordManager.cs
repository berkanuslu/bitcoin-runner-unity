using UnityEngine;

public class DailyWordManager : MonoBehaviour
{
	public string word;
	public char[] estimatedWord;
	static DailyWordManager _instance;
	static int instances = 0;
	public bool resetDictionary = false;

	public static DailyWordManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(DailyWordManager)) as DailyWordManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one DailyWordManager");
		}
		else
		{
			_instance = this;
		}

		LoadWord();
	}

	public string GetDailyWord()
	{
		return PreferencesManager.Instance.GetDailyWord();
	}

	void LoadWord()
	{
		if (resetDictionary)
		{
			PreferencesManager.Instance.ClearAllDailyWordRecords();
		}

		PreferencesManager.Instance.LoadDailyWords();

		word = GetDailyWord();

		if (word != "")
		{
			PreferencesManager.Instance.InitializeDailyWord(word);
			CalculateDailyWords();
		}
	}

	//Calculate daily words on letter objects in the scene
	public void CalculateDailyWords()
	{
		string dailyWord = DailyWordManager.Instance.GetDailyWord();
		dailyWord = dailyWord.Replace(" ", string.Empty).Replace("'", string.Empty);
#if UNITY_EDITOR
		Debug.Log("Daily Word: " + dailyWord);
#endif

		//shuffle daily word string
		for (int i = 0; i < dailyWord.Length; i++)
		{
			string tmp = dailyWord[i].ToString();
			int r = Random.Range(0, dailyWord.Length);
			dailyWord = dailyWord.Insert(i, dailyWord[r].ToString()).Remove(i + 1, 1);
			dailyWord = dailyWord.Insert(r, tmp).Remove(r + 1, 1);
		}
#if UNITY_EDITOR
		Debug.Log("Shuffled Daily Word: " + dailyWord);
#endif

		string tempDailyWord = dailyWord;
		string selectedRandomLetter = string.Empty;

		for (int i = 0; i < LevelSpawnManager.Instance.letterObjects.Count; i++)
		{
			if (tempDailyWord.Length > 0)
			{
				int r = Random.Range(0, tempDailyWord.Length);
				selectedRandomLetter = tempDailyWord[r].ToString();
				tempDailyWord = tempDailyWord.Remove(r, 1);
			}

			string assetPath = GetAssetPath(selectedRandomLetter);
			LevelSpawnManager.Instance.letterObjects[i].GetComponent<Renderer>().material.mainTexture = (Texture)Resources.Load(assetPath, typeof(Texture));
			LevelSpawnManager.Instance.letterObjects[i].GetComponent<LetterInfo>().letter = selectedRandomLetter[0];
		}
	}

	public string GetAssetPath(string letter)
	{
		// check for letter = "ÜÖÇŞĞIİ"
		switch ((int)letter[0])
		{
			case 220:
				letter = "u_";
				break;
			case 214:
				letter = "o_";
				break;
			case 199:
				letter = "c_";
				break;
			case 350:
				letter = "s_";
				break;
			case 286:
				letter = "g_";
				break;
			case 304:
				letter = "i";
				break;
			case 73:
				letter = "i_";
				break;
			default:
				letter = letter.ToLower();
				break;
		}
		letter = letter.ToLower();
		string url = (letter);
		return url;
	}
}
