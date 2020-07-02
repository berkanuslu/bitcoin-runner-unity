using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
	static PowerupManager _instance;
	static int instances = 0;

	public float verticalSpeed = 5.0f;
	public float verticalDistance = 1.0f;

	public float horizontalSpeed = 0;
	public float maxYPosition = 10f;

	List<Powerup> deactivated = new List<Powerup>();
	List<Powerup> activated = new List<Powerup>();

	bool isObstacleBlasterPrior = false;
	bool canSpawnSecondChance = true;

	public static PowerupManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(PowerupManager)) as PowerupManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;

		if (instances > 1)
		{
			Debug.LogWarning("There are more than one PowerupManager");
		}
		else
		{
			_instance = this;
		}

		foreach (Transform child in transform)
		{
			deactivated.Add(child.GetComponent<Powerup>());
		}
	}

	Powerup FindCompatiblePowerup()
	{
		int n = 0;
		if (isObstacleBlasterPrior)
		{
			foreach (Powerup item in deactivated)
			{
				if (item.name == "ObstacleBlaster")
				{
					return item;
				}
			}

			isObstacleBlasterPrior = false;
		}
		else
		{
			if (!canSpawnSecondChance)
			{
				Powerup powerup = null;

				do
				{
					n = Random.Range(0, deactivated.Count);
					powerup = deactivated[n];
				} while (powerup.name == "SecondChance");

				return powerup;
			}
			else
			{
				n = Random.Range(0, deactivated.Count);
				return deactivated[n];
			}
		}

		return null;
	}

	public void SpawnPowerup(float multiplyValue)
	{
		Powerup powerup = FindCompatiblePowerup();

		deactivated.Remove(powerup);

		Vector3 newPos = powerup.transform.position;
		newPos.y = Random.Range(PlayerManager.Instance.minY, maxYPosition);
		powerup.transform.position = newPos;

		activated.Add(powerup);
		powerup.Spawn(verticalSpeed, verticalDistance, horizontalSpeed * multiplyValue);
	}

	public void ResetPowerup(Powerup sender)
	{
		sender.DisableTrail();
		activated.Remove(sender);
		deactivated.Add(sender);
	}

	public void SetObstacleBlasterIsPrior()
	{
		isObstacleBlasterPrior = true;
	}

	public void DisableSecondChanceSpawn()
	{
		canSpawnSecondChance = false;
	}

	public void ResetAll()
	{
		isObstacleBlasterPrior = false;
		canSpawnSecondChance = true;

		gameObject.BroadcastMessage("ResetObject");
	}

	public void PauseAll()
	{
		this.gameObject.BroadcastMessage("Pause");
	}

	public void ResumeAll()
	{
		this.gameObject.BroadcastMessage("Resume");
	}
}
