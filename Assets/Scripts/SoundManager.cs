using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	static SoundManager _instance;
	static int instances = 0;

	public int audioVolume = 1;
	public bool audioEnabled = true;

	public GameObject[] audioOnButtons;
	public GameObject[] audioOffButtons;

	public AudioClip buttonClickEffect;

	public AudioSource gameMusic;

	public static SoundManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
			}

			return _instance;
		}
	}

	void Start()
	{
		instances++;
		if (instances > 1)
		{
			Debug.LogWarning("There are more than one SoundManager");
		}
		else
		{
			_instance = this;
		}

		if (PreferencesManager.Instance.HasAudioVolume())
		{
			audioVolume = PreferencesManager.Instance.GetAudioVolume();
			audioEnabled = (audioVolume == 1);
		}
		else
		{
			audioVolume = 1;
			audioEnabled = true;
			PreferencesManager.Instance.SetAudioVolume(audioVolume);
		}
		UpdateAudioButton();
	}

	public void UpdateAudioButton()
	{
		foreach (var button in audioOnButtons)
		{
			button.SetActive((audioVolume == 1));
		}
		foreach (var button in audioOffButtons)
		{
			button.SetActive((audioVolume != 1));
		}
	}

	public void SetAudioVolume(bool audioOn)
	{
		if (audioOn)
			audioVolume = 1;
		else
			audioVolume = 0;

		PreferencesManager.Instance.SetAudioVolume(audioVolume);
		audioEnabled = (audioVolume == 1);
		UpdateAudioButton();
	}

	public void PlayGameMusic()
	{
		if (gameMusic != null)
		{
			gameMusic.volume = audioVolume * 0.25f;
			gameMusic.Play();
		}
	}

	public void PauseGameMusic()
	{
		if (gameMusic != null)
		{
			gameMusic.volume = audioVolume * 0.0f;
			gameMusic.Pause();
		}
	}

	public void StopGameMusic()
	{
		if (gameMusic != null)
		{
			gameMusic.Stop();
		}
	}

	public void PlayButtonClickSound()
	{
		AudioSource.PlayClipAtPoint(buttonClickEffect, Vector3.up, audioVolume);
	}
}