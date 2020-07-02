using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour
{
	public string carName;
	public string busName;

	void OnTriggerStay(Collider other)
	{
		if (other.transform.name.Contains(carName))
		{
			PlayerManager.Instance.minY = -16;
			PlayerManager.Instance.isOnTheCar = true;
			PlayerManager.Instance.isOnTheBus = false;
		}
		else if (other.transform.name.Contains(busName))
		{
			PlayerManager.Instance.minY = -9;
			PlayerManager.Instance.isOnTheCar = false;
			PlayerManager.Instance.isOnTheBus = true;
		}
	}

	void OnTriggerExit(Collider collision)
	{
		PlayerManager.Instance.minY = -23;
		PlayerManager.Instance.isOnTheCar = false;
		PlayerManager.Instance.isOnTheBus = false;
	}
}
