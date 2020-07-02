using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "SpawnTriggerer")
		{
			switch (other.tag)
			{
				case "CloudLayer":
					LevelSpawnManager.Instance.SpawnCloudLayer(LevelSpawnManager.PlaceLocation.Normal);
					break;

				case "CityBackgroundLayer":
					LevelSpawnManager.Instance.SpawnCityBackgroundLayer(LevelSpawnManager.PlaceLocation.Normal);
					break;

				case "CityLayer":
					LevelSpawnManager.Instance.SpawnCityLayer(LevelSpawnManager.PlaceLocation.Normal);
					break;

				case "ForegroundLayer":
					LevelSpawnManager.Instance.SpawnForegroundLayer(LevelSpawnManager.PlaceLocation.Normal);
					break;

				case "Obstacles":
					LevelSpawnManager.Instance.SpawnObstacles();
					break;
			}
		}
		else if (other.name == "ResetTriggerer")
		{
			switch (other.tag)
			{
				case "CloudLayer":
				case "CityBackgroundLayer":
				case "CityLayer":
				case "ForegroundLayer":
					LevelSpawnManager.Instance.ResetObject(other.transform.parent.gameObject);
					break;

				case "Obstacles":
					other.transform.parent.GetComponent<ObstacleManager>().DeactivateChild();
					LevelSpawnManager.Instance.ResetObject(other.transform.parent.gameObject);
					break;
			}
		}
		else if (other.tag == "PowerUps")
		{
			other.GetComponent<Powerup>().ResetObject();
		}
		else if (other.name == "Enemy")
		{
			other.transform.parent.gameObject.GetComponent<Enemy>().ResetObject();
		}
	}
}
