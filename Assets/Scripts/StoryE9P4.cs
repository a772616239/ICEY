using System;
using UnityEngine;

public class StoryE9P4 : BaseBehaviour
{
	private bool IsE9P4FirstBossNotDie
	{
		get
		{
			return RoundStorage.Get("E9_p4FirstBossNotDie", true);
		}
		set
		{
			RoundStorage.Set("E9_p4FirstBossNotDie", value);
		}
	}

	private bool IsE9BossDie
	{
		get
		{
			return RoundStorage.Get("E9_BossDie", false);
		}
	}

	private void Start()
	{
		if (!this.IsE9P4FirstBossNotDie || this.IsE9BossDie)
		{
			this._e9Anim.Play("StoryE9Close");
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (this._e9Anim == null)
		{
			Log.Error("E9Anim为空");
			return;
		}
		if (this.IsE9P4FirstBossNotDie && !this.IsE9BossDie)
		{
			this.IsE9P4FirstBossNotDie = false;
			this.DoorClose();
		}
		if (this._isE9P4FirstBossDie && this.IsE9BossDie)
		{
			this._isE9P4FirstBossDie = false;
			this.DoorOpen();
		}
	}

	private void DoorClose()
	{
		this._e9Anim.Play("StoryE9Close");
		R.Audio.PlayVoiceOver("e9t6", null, false);
	}

	private void DoorOpen()
	{
		this._e9Anim.Play("StoryE9Open");
		R.Audio.PlayVoiceOver("e9t7", null, false);
	}

	private bool _isE9P4FirstBossDie = true;

	[SerializeField]
	private Animator _e9Anim;

	[SerializeField]
	private SceneGate _gate;
}
