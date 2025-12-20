using System;
using UnityEngine;

public class BattleStroy : BaseBehaviour
{
	private bool show
	{
		get
		{
			return RoundStorage.Get("BattleStory" + LevelManager.SceneName + base.name, false);
		}
		set
		{
			RoundStorage.Set("BattleStory" + LevelManager.SceneName + base.name, value);
		}
	}

	private void Update()
	{
		if (this.battle.isBattleOver && !this.show)
		{
			this.show = true;
			R.Audio.PlayVoiceOver(this.text, null, false);
		}
	}

	[SerializeField]
	private BattleCheckPoint battle;

	[SerializeField]
	private string text;
}
