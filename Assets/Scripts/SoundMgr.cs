using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class SoundMgr : MonoBehaviour
{
	public static SoundMgr Instance
	{
		get
		{
			if (SoundMgr._instance == null)
			{
				SoundMgr._instance = UnityEngine.Object.FindObjectOfType<SoundMgr>();
			}
			return SoundMgr._instance;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < this.sfxs.Length; i++)
		{
			AudioSource audioSource = new GameObject("audiosource_sfx")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<AudioSource>();
			audioSource.clip = this.sfxs[i].clip;
			this.sfxs[i].audioSource = audioSource;
			this.sfxTable.Add(this.sfxs[i].name, this.sfxs[i]);
		}
	}

	private void _playSfx(string name, float volume, bool loop, float fadeinTime = 0f)
	{
		if (!this.sfxTable.ContainsKey(name))
		{
			return;
		}
		AudioSource audioSource = this.sfxTable[name].audioSource;
		if (audioSource != null)
		{
			audioSource.loop = loop;
			if (loop)
			{
				if (fadeinTime != 0f)
				{
					audioSource.volume = 0f;
					audioSource.Play();
					audioSource.DOFade(volume, fadeinTime);
				}
				else
				{
					audioSource.volume = volume;
					audioSource.Play();
				}
			}
			else
			{
				audioSource.volume = volume;
				audioSource.PlayOneShot(this.sfxTable[name].clip, volume);
			}
		}
	}

	private IEnumerator _playSfxAfterTime(string name, float volume, bool loop, float waitTime, float fadeinTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		this._playSfx(name, volume, loop, fadeinTime);
		yield break;
	}

	public void PlaySfx(string name, float volume, bool loop, float waitTime = 0f, float fadeinTime = 0f)
	{
		volume *= R.Audio.EffectsVolume / 100f;
		if (waitTime > 0f)
		{
			base.StartCoroutine(this._playSfxAfterTime(name, volume, loop, waitTime, fadeinTime));
		}
		else
		{
			this._playSfx(name, volume, loop, fadeinTime);
		}
	}

	public void StopSfx(string name, float fadeoutTime = 0f)
	{
		if (!this.sfxTable.ContainsKey(name))
		{
			return;
		}
		AudioSource audioSource = this.sfxTable[name].audioSource;
		if (audioSource != null)
		{
			audioSource.DOFade(0f, fadeoutTime).OnComplete(delegate
			{
				audioSource.Stop();
			});
		}
	}

	public AudioSet[] sfxs;

	private Dictionary<string, AudioSet> sfxTable = new Dictionary<string, AudioSet>();

	private static SoundMgr _instance;
}
