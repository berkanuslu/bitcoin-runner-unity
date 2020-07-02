using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlaster : MonoBehaviour
{
	bool canDisable = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.name == "Enemy")
		{
			if (!canDisable)
			{
				other.transform.parent.GetComponent<Enemy>().TargetHit(true);
			}
		}
		else if (other.name == "ResetTriggerer" && other.tag == "Obstacles" && canDisable)
		{
			ResetObject();
		}
	}

	void PlayExplosion(Transform expParent)
	{
		expParent.GetComponent<Renderer>().enabled = false;
		expParent.GetComponent<Collider>().enabled = false;

		if (canDisable)
		{
			return;
		}

		ParticleSystem explosion = expParent.Find("ExplosionParticle").gameObject.GetComponent("ParticleSystem") as ParticleSystem;
		explosion.Play();
	}

	void ResetObject()
	{
		canDisable = false;
		transform.localPosition = new Vector3((-1 * UIManager.Instance.cameraHorizontalExtent - 10), 0, -5);
		gameObject.SetActive(false);
	}

	public void Disable()
	{
		canDisable = true;
	}
}
