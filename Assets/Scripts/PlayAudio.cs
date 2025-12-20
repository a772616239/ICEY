using System;
using UnityEngine;

public class PlayAudio : BaseBehaviour
{
	private void Start()
	{
		if (this.playAtStart)
		{
			this.PlaySound(this.id);
		}
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?((!(this.audioPos == null)) ? this.audioPos.position : base.transform.position));
	}

	public bool playAtStart;

	public int id;

	[SerializeField]
	private Transform audioPos;
}
