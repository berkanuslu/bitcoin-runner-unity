using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChildOrganizer : MonoBehaviour
{
	// Called from menu animation
	void DisableAllChilds()
	{
		this.transform.gameObject.SetActive(false);
	}

	// Called from menu animation
	void EnableAllChilds()
	{
		this.transform.gameObject.SetActive(true);
	}
}
