using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyWordRecordsManager : MonoBehaviour
{
	static DailyWordRecordsManager _instance;
	static int instances = 0;

	public DailyWordRecordRow[] dailyWordRows = new DailyWordRecordRow[7];
	public List<DailyWordRow> wordRows = new List<DailyWordRow>();
	public int currentPage = 1;
	public int maxPage = -1;
	public int pageSize = 7;
	public int totalRecords = -1;
	public Text pageInfo;

	public static DailyWordRecordsManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(DailyWordRecordsManager)) as DailyWordRecordsManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one DailyWordRecordsManager");
		}
		else
		{
			_instance = this;
		}
	}

	public void GetDailyWords()
	{
		wordRows.Clear();
		wordRows = PreferencesManager.Instance.GetDailyWordRows();

		maxPage = (wordRows.Count / pageSize);
		if (wordRows.Count % pageSize != 0)
		{
			maxPage += 1;
		}

		totalRecords = wordRows.Count;

		if (totalRecords == 0)
		{
			pageInfo.text = "1 / 1";
		}
		else
		{
			PlaceWords();
		}
	}

	private void PlaceWords()
	{
		int start = (wordRows.Count - 1) - ((pageSize * currentPage) - pageSize);
		int end = start - pageSize;
		int index = 0;
		for (int i = start; i > end; i--)
		{
			if (dailyWordRows[index] != null)
			{
				if (i >= 0)
				{
					dailyWordRows[index].gameObject.SetActive(true);
					dailyWordRows[index].InitializeRow((wordRows.Count - i), wordRows[i].name, wordRows[i].desc);
				}
				else
				{
					dailyWordRows[index].gameObject.SetActive(false);
					dailyWordRows[index].InitializeRow(0, "-", "-");
				}
			}
			index++;
		}

		pageInfo.text = currentPage.ToString() + " / " + maxPage.ToString();
	}

	public void PreviousPage()
	{
		if (currentPage > 1)
		{
			currentPage--;
		}
		else
		{
			currentPage = 1;
		}

		PlaceWords();
	}

	public void NextPage()
	{
		if (currentPage < maxPage)
		{
			currentPage++;
		}
		else
		{
			currentPage = maxPage;
		}

		PlaceWords();
	}
}
