using System;
using UnityEngine;

public class StoryE2P9 : BaseBehaviour
{
	private static bool IsFirstE2P9
	{
		get
		{
			return RoundStorage.Get("E2_p9", true);
		}
		set
		{
			RoundStorage.Set("E2_p9", value);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player") || !StoryE2P9.IsFirstE2P9)
		{
			return;
		}
		StoryE2P9.IsFirstE2P9 = false;
		R.Audio.PlayVoiceOver("e2t12", null, false);
		R.Audio.PlayEffect(375, null);
	}
}
