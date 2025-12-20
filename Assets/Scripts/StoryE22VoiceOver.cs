using System;
using UnityEngine;

public class StoryE22VoiceOver : BaseBehaviour
{
	private void Start()
	{
		if (R.Player.Attribute.flashLevel >= 2)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
