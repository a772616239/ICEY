using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[ExecuteInEditMode]
public class PlayAudioEffect : MonoBehaviour
{
	private bool IsFirstTime
	{
		get
		{
			return RoundStorage.Get("PlayAudioEffect" + LevelManager.SceneName + this._id, true);
		}
		set
		{
			RoundStorage.Set("PlayAudioEffect" + LevelManager.SceneName + this._id, this._repeatable || value);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player") || !this.IsFirstTime)
		{
			return;
		}
		R.Audio.PlayEffect(this._id, new Vector3?(base.transform.position));
		this.IsFirstTime = false;
	}

	[SerializeField]
	private int _id;

	[Header("可重复触发")]
	[SerializeField]
	private bool _repeatable;

	[Header("延迟时间")]
	[SerializeField]
	private float _delay;
}
