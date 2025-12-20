using System;
using UnityEngine;

public class PlayBGMProxy : BaseBehaviour
{
	private void OnEnable()
	{
		//R.Audio.PlayBGM(this.id, true);
	}

	[SerializeField]
	private int id;
}
