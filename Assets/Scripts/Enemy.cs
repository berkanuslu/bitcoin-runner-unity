using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject enemyIndicator;
	public GameObject enemy;
	public ParticleSystem explosion;

	bool canMove = false;
	bool paused = false;

	float originalSpeed = 0;
	float speed = 0;
	Vector3 originalPos = new Vector3();
	Vector3 originalExplosionPos = new Vector3();

	bool explosionPlaying = false;

	void Start()
	{
		originalPos = enemy.transform.position;
		originalPos.x = UIManager.Instance.cameraHorizontalExtent + 10;
		enemy.transform.position = originalPos;

		originalExplosionPos = explosion.transform.position;
		originalExplosionPos.x = UIManager.Instance.cameraHorizontalExtent + 10;
		explosion.transform.position = originalExplosionPos;
	}

	void Update()
	{
		if (canMove && !paused)
		{
			enemy.transform.position -= Vector3.right * speed * Time.deltaTime;
		}
	}

	IEnumerator PlaceEnemyIndicator(float minY, float maxY)
	{
		float randomYPos = Random.Range(minY, maxY);

		enemyIndicator.transform.position = new Vector3(UIManager.Instance.cameraHorizontalExtent - 10, randomYPos, -5f);
		enemyIndicator.SetActive(true);

		if (!paused)
		{
			yield return new WaitForSeconds(3.0f);
		}

		enemyIndicator.SetActive(false);
		enemyIndicator.transform.position = new Vector3(UIManager.Instance.cameraHorizontalExtent - 10, 0, -5f);

		Vector3 pos = enemy.transform.position;
		pos.y = randomYPos;
		enemy.transform.position = pos;

		this.speed = originalSpeed * LevelSpawnManager.Instance.SpeedMultiplier();

		enemy.SetActive(true);

		AudioSource.PlayClipAtPoint(PlayerManager.Instance.enemyAudioEffect, Vector3.up, SoundManager.Instance.audioVolume);

		canMove = true;
	}

	IEnumerator PlaceExplosion(float x, float y)
	{
		explosion.transform.position = new Vector3(x - 6, y, originalExplosionPos.z);
		explosion.Play();

		explosionPlaying = true;
		LevelSpawnManager.Instance.AddExplosion(explosion.gameObject);

		if (!paused)
		{
			yield return new WaitForSeconds(2.0f);
		}

		LevelSpawnManager.Instance.RemoveExplosion(explosion.gameObject);
		explosionPlaying = false;

		explosion.transform.position = originalExplosionPos;
	}

	public void Spawn(float s, float minY, float maxY)
	{
		originalSpeed = s;
		StartCoroutine(PlaceEnemyIndicator(minY, maxY));
	}

	public void ResetObject()
	{
		StopAllCoroutines();

		if (explosionPlaying)
		{
			LevelSpawnManager.Instance.RemoveExplosion(explosion.gameObject);
		}

		canMove = false;
		paused = false;

		enemy.transform.position = originalPos;
		enemy.SetActive(false);

		enemyIndicator.SetActive(false);
		enemyIndicator.transform.position = new Vector3(UIManager.Instance.cameraHorizontalExtent - 10, 0, -5f);

		EnemyManager.Instance.ResetEnemy(this);
	}

	public void TargetHit(bool playExplosion)
	{
		canMove = false;
		paused = false;

		if (playExplosion)
		{
			StartCoroutine(PlaceExplosion(enemy.transform.position.x, enemy.transform.position.y));
		}

		enemy.transform.position = originalPos;
		enemy.SetActive(false);

		EnemyManager.Instance.ResetEnemy(this);
	}

	// It is called from broadcast messages by EnemyManager
	public void Pause()
	{
		paused = true;
	}

	// It is called from broadcast messages by EnemyManager
	public void Resume()
	{
		paused = false;
	}
}
