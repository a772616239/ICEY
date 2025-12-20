using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE9P3 : BaseBehaviour
{
	private bool IsE9P3FirstBossNotDie
	{
		get
		{
			return RoundStorage.Get("E9_P3BossNotDie", true);
		}
		set
		{
			RoundStorage.Set("E9_P3BossNotDie", value);
		}
	}

	private bool IsE9P3FirstBossDie
	{
		get
		{
			return RoundStorage.Get("E9_P3BossDie", true);
		}
		set
		{
			RoundStorage.Set("E9_P3BossDie", value);
		}
	}

	private bool IsE9P4FirstBossNotDie
	{
		get
		{
			return RoundStorage.Get("E9_p4FirstBossNotDie", true);
		}
	}

	private bool IsE9BossDie
	{
		get
		{
			return RoundStorage.Get("E9_BossDie", false);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (this.IsE9P3FirstBossNotDie && !this.IsE9BossDie)
		{
			this.IsE9P3FirstBossNotDie = false;
			R.Audio.PlayVoiceOver((!this.IsE9P4FirstBossNotDie) ? "e9t10" : "e9t4", null, false);
		}
		if (this.IsE9P3FirstBossDie && this.IsE9BossDie)
		{
			this.IsE9P3FirstBossDie = false;
			R.Audio.PlayVoiceOver("e9t5", null, false);
		}
	}
}
