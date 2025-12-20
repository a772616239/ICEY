using System;
using UnityEngine;

public class StoryE9P5 : BaseBehaviour
{
	private bool IsE9P5FirstBossNotDie
	{
		get
		{
			return RoundStorage.Get("E9_P5FirstBossNotDie", true);
		}
		set
		{
			RoundStorage.Set("E9_P5FirstBossNotDie", value);
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
		if (this.IsE9P5FirstBossNotDie && !this.IsE9BossDie && !this.IsE9P4FirstBossNotDie)
		{
			this.IsE9P5FirstBossNotDie = false;
			R.Audio.PlayVoiceOver("e9t8", null, false);
		}
	}
}
