using UnityEngine;

public class TrailEffect : MonoBehaviour
{
	void Update()
	{
		Vector3 targetPos = this.transform.position;
		targetPos.y = PlayerManager.Instance.transform.position.y;
		targetPos.x = PlayerManager.Instance.transform.position.x - 12;

		this.transform.position = targetPos;
	}
}
