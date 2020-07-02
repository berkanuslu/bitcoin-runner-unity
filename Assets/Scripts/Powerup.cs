using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
	public GameObject trail;

	float verticalSpeed = 5.0f;
	float verticalDistance = 1.0f;

	float horizontalSpeed = 0;

	float verticalOffset = 0.0f;
	float originalYPos = 0;

	Vector3 nextPos = new Vector3();
	Vector3 startingPos;

	bool paused = false;
	bool canMove = false;

	void Start()
	{
		startingPos = this.transform.position;
	}

	void Update()
	{
		if (!paused && canMove)
		{
			nextPos = this.transform.position;

			verticalOffset = (1 + Mathf.Sin(Time.time * verticalSpeed)) * verticalDistance / 2.0f;
			nextPos.y = originalYPos + verticalOffset;

			nextPos.x -= horizontalSpeed * Time.deltaTime;

			this.transform.position = nextPos;
		}
	}

	public void Spawn(float vSpeed, float vDist, float hSpeed)
	{
		this.verticalSpeed = vSpeed;
		this.verticalDistance = vDist;
		this.horizontalSpeed = hSpeed;

		originalYPos = this.transform.position.y;

		trail.SetActive(true);

		canMove = true;
		paused = false;
	}

	public void DisableTrail()
	{
		trail.SetActive(false);
	}

	public void Pause()
	{
		paused = true;
	}

	public void Resume()
	{
		paused = false;
	}

	public void ResetObject()
	{
		canMove = false;
		trail.SetActive(false);

		this.transform.position = startingPos;
		PowerupManager.Instance.ResetPowerup(this);
	}
}
