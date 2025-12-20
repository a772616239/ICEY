using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE9P2 : BaseBehaviour
{
	private bool IsE9P2FirstBossNotDie
	{
		get
		{
			return RoundStorage.Get("E9_P2BossNotDie", true);
		}
		set
		{
			RoundStorage.Set("E9_P2BossNotDie", value);
		}
	}

	private bool IsE9P2FirstBossDie
	{
		get
		{
			return RoundStorage.Get("E9_P2BossDie", true);
		}
		set
		{
			RoundStorage.Set("E9_P2BossDie", value);
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
		this.guideLeft.SetActive(!this.IsE9BossDie);
		this.guideRight.SetActive(this.IsE9BossDie);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (this.IsE9P2FirstBossNotDie && !this.IsE9BossDie)
		{
			this.IsE9P2FirstBossNotDie = false;
			R.Audio.PlayVoiceOver("e9t2", null, false);
		}
		if (this.IsE9P2FirstBossDie && this.IsE9BossDie)
		{
			this.IsE9P2FirstBossDie = false;
			R.Audio.PlayVoiceOver("e9t3", null, false);
		}
	}

	[SerializeField]
	private GameObject guideLeft;

	[SerializeField]
	private GameObject guideRight;
}
