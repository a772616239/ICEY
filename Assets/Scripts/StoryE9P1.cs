using System;
using System.Collections;
using UnityEngine;

public class StoryE9P1 : BaseBehaviour
{
	private bool IsFirst
	{
		get
		{
			return RoundStorage.Get("E9_p1", true);
		}
		set
		{
			RoundStorage.Set("E9_p1", value);
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
		if (this.IsFirst)
		{
			this.IsFirst = false;
			if (!this.IsE9BossDie)
			{
				base.StartCoroutine(this.Sequence0());
			}
			else
			{
				base.StartCoroutine(this.Sequence1());
			}
		}
	}

	private IEnumerator Sequence0()
	{
		yield return new WaitForSeconds(0.5f);
		R.Trophy.AwardTrophy(11);
		yield return R.Audio.PlayVoiceOver("e9t1", null, false);
		yield break;
	}

	private IEnumerator Sequence1()
	{
		yield return new WaitForSeconds(0.5f);
		yield return R.Audio.PlayVoiceOver("e9t9", null, false);
		yield break;
	}
}
