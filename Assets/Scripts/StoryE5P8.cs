using System;
using UnityEngine;

public class StoryE5P8 : BaseBehaviour
{
	private bool IsFirstTime
	{
		get
		{
			return RoundStorage.Get(LevelManager.SceneName + base.name, true);
		}
		set
		{
			RoundStorage.Set(LevelManager.SceneName + base.name, false || value);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (this._isPlaying || !collision.CompareTag("Player") || !this.IsFirstTime)
		{
			return;
		}
		R.Audio.PlayVoiceOver("e5t8", null, false);
		this._guideBoard.SetActive(true);
		this._isPlaying = true;
		this.IsFirstTime = false;
	}

	private bool _isPlaying;

	[SerializeField]
	private GameObject _guideBoard;
}
