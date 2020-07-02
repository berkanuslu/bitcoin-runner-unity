using System.Collections;
using UnityEngine;

public class BasicMoveAnimation : MonoBehaviour
{
	public float time;
	public Vector3 move;

	private void OnEnable()
	{
		StartCoroutine(MoveObject());
	}

	IEnumerator MoveObject()
	{
		Vector3 startPosition = this.gameObject.transform.localPosition;

		var rate = 1.0f / time;
		var t = 0.0f;
		while (t < 1.0f)
		{
			t += Time.deltaTime * rate;
			this.gameObject.transform.localPosition = Vector3.Lerp(startPosition, move, t);

			yield return new WaitForEndOfFrame();
		}

		t = 0.0f;
		while (t < 1.0f)
		{
			t += Time.deltaTime * rate;
			this.gameObject.transform.localPosition = Vector3.Lerp(move, startPosition, t);

			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(MoveObject());
	}
}
