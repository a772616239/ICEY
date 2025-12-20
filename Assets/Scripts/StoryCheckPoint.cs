using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[ExecuteInEditMode]
public class StoryCheckPoint : BaseBehaviour
{
	private bool IsFirstTime
	{
		get
		{
			return RoundStorage.Get(LevelManager.SceneName + base.name, true);
		}
		set
		{
			RoundStorage.Set(LevelManager.SceneName + base.name, this._repeatable || value);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (this._isPlaying || !collision.CompareTag("Player") || !this.IsFirstTime)
		{
			return;
		}
		R.Audio.PlayVoiceOverArray(this._voiceOverList.ToArray(), this._eachTime, delegate
		{
			this._isPlaying = false;
		}, this._delay);
		this._isPlaying = true;
		this.IsFirstTime = false;
	}

	[SerializeField]
	[Header("可重复触发")]
	private bool _repeatable;

	[SerializeField]
	[Header("播放台词延迟时间")]
	private float _delay;

	private bool _isPlaying;

	[SerializeField]
	[Header("旁白列表")]
	private List<string> _voiceOverList = new List<string>();

	[Header("每两句之间间隔时间")]
	[SerializeField]
	private float _eachTime = 0.5f;
}
