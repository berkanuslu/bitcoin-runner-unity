using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectHelper : MonoBehaviour
{
	public void EnableDisable(bool activate, float time = 5.0f)
	{
		gameObject.SetActive(activate);
		if (gameObject.activeInHierarchy)
		{
			StartCoroutine(VisibilityTimer(!activate, time));
		}
	}

	public IEnumerator VisibilityTimer(bool visible, float time)
	{
		yield return new WaitForSeconds(time);

		gameObject.SetActive(visible);
	}

	public IEnumerator Move(float endPosX, float endPosY, float time, bool hide)
	{
		float i = 0.0f;
		float rate = 1.0f / time;

		Vector3 startPos = transform.localPosition;
		Vector3 endPos = new Vector3(endPosX, endPosY, transform.localPosition.z);

		while (i < 1.0)
		{
			i += Time.deltaTime * rate;
			transform.localPosition = Vector3.Lerp(startPos, endPos, i);
			yield return new WaitForEndOfFrame();
		}

		if (hide)
		{
			gameObject.SetActive(false);
		}
	}
}
