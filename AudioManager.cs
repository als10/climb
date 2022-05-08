using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public Sound[] sounds;
	public int themeMusic;
	string musicKey = "Music";
	public int soundMain;
	string soundKey = "Sound"; 

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}

	void Start()
	{
		themeMusic = PlayerPrefs.GetInt(musicKey, 1);
		soundMain = PlayerPrefs.GetInt(soundKey, 1);
		Play("theme");
	}

	public void setMusic(bool music)
	{
		if (music) { themeMusic = 1; }
		else { themeMusic = 0; }
		PlayerPrefs.SetInt(musicKey, themeMusic);
		PlayerPrefs.Save();
	}

	public void setSound(bool sound)
	{
		if (sound) { soundMain = 1; }
		else { soundMain = 0; }
		PlayerPrefs.SetInt(soundKey, soundMain);
		PlayerPrefs.Save();
	}

	public void Play(string sound)
	{
		if (sound == "theme") { if (themeMusic == 0) { return; } }
		else { if (soundMain == 0) { return; } }

		Sound s = findAudio(sound);

		if (s != null)
		{
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));

			s.source.Play();
		}
	}

	public void Stop(string sound)
	{
		Sound s = findAudio(sound);
		if (s != null)
		{
			s.source.Stop();
		}
	}

	public void Pause(string sound)
	{
		Sound s = findAudio(sound);
		if (s != null)
		{
			s.source.Pause();
		}
	}

	private Sound findAudio(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		return s;
	}

}
