using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AutoDestroyAfterAudioPlay : MonoBehaviour
{
	private void Awake()
	{
		this._source = base.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!this._source.isPlaying)
		{
			this.Disable();
		}
	}

	private void OnDisable()
	{
		this.Disable();
	}

	private void Disable()
	{
		base.transform.localPosition = Vector3.zero;
		this._source.Stop();
		this._source.clip = null;
	}

	[SerializeField]
	private float delayTime = 0.5f;

	private AudioSource _source;
}
