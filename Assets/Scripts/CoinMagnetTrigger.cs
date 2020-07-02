using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinMagnetTrigger : MonoBehaviour
{
	public GameObject target;
	public float coinAnimationSpeed;
	private List<GameObject> tempObjectList = new List<GameObject>();

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Coin")
		{
			GameObject tempOther = (GameObject)Instantiate(other.transform.gameObject, other.transform.position, other.transform.rotation);
			tempObjectList.Add(tempOther);

			other.GetComponent<Renderer>().enabled = false;
			other.GetComponent<Collider>().enabled = false;

			try
			{
				int coin_value = int.Parse(other.transform.name.Replace("Coin", ""));
				LevelManager.Instance.CoinGathered(coin_value);
			}
			catch (System.Exception ex)
			{
				Debug.LogError(ex.Message);
			}

			StartCoroutine(CoinMagnetAnimation(tempOther, other, coinAnimationSpeed));
		}
	}

	IEnumerator CoinMagnetAnimation(GameObject tempOther, Collider other, float time)
	{
		other.transform.Find("CoinParticle").gameObject.GetComponent<ParticleSystem>().Play();

		var rate = 1.0f / time;
		var t = 0.0f;

		Vector3 startPosition = tempOther.transform.position;
		Vector3 move = target.transform.position;

		while (t < 1.0f)
		{
			t += Time.deltaTime * rate;
			tempOther.transform.position = Vector3.Lerp(startPosition, move, t);

			yield return new WaitForEndOfFrame();
		}

		DestroyImmediate(tempOther);
	}

	public void ResetCoinMagnetTrigger()
	{
		foreach (GameObject item in tempObjectList)
		{
			DestroyImmediate(item);
		}
	}

	private void OnDisable()
	{
		ResetCoinMagnetTrigger();
	}
}
