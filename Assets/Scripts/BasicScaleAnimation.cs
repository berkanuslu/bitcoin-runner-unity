using System.Collections;
using UnityEngine;

public class BasicScaleAnimation : MonoBehaviour
{
	public float time;
	public Vector3 targetScale;
	public Vector3 startScale;
	private Coroutine coroutine;

	private void OnEnable()
	{
		coroutine = StartCoroutine(ScaleObject());
	}

	private void OnDisable()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
	}

	IEnumerator ScaleObject()
	{
		var rate = 1.0f / time;
		var t = 0.0f;
		while (t < 1.0f)
		{
			t += Time.deltaTime * rate;
			this.gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

			yield return new WaitForEndOfFrame();
		}

		t = 0.0f;
		while (t < 1.0f)
		{
			t += Time.deltaTime * rate;
			this.gameObject.transform.localScale = Vector3.Lerp(targetScale, startScale, t);

			yield return new WaitForEndOfFrame();
		}

		coroutine = StartCoroutine(ScaleObject());
	}
}
