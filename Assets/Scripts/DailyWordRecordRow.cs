using UnityEngine;
using UnityEngine.UI;

public class DailyWordRecordRow : MonoBehaviour
{
	public Text rowName;
	public Text rowDescription;
	public Text rowOrder;

	public void InitializeRow(int order, string recordName, string recordDesc)
	{
		rowName.text = recordName;
		rowDescription.text = recordDesc;
		rowOrder.text = order.ToString();
	}
}
