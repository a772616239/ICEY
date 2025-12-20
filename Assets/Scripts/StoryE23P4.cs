using System;
using System.Collections;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE23P4 : BaseBehaviour
{
	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(null);
			R.Ui.Tutorial.Hide(new int?(4));
			R.Ui.Tutorial.DisposeVideo();
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private bool OnBattle(string eventName, object sender, BattleEventArgs battleEventArgs)
	{
		BattleEventArgs.BattleStatus status = battleEventArgs.Status;
		if (status != BattleEventArgs.BattleStatus.Begin)
		{
			if (status != BattleEventArgs.BattleStatus.End)
			{
				throw new ArgumentOutOfRangeException();
			}
			R.Ui.Tutorial.Hide(null);
			R.Ui.Tutorial.Hide(new int?(4));
		}
		else
		{
			base.StartCoroutine(this.Sequence0());
		}
		return true;
	}

	private IEnumerator Sequence0()
	{
		yield return R.Ui.Tutorial.Show(ScriptLocalization.ui_tutorial.breakAmmor);
		yield return R.Ui.Tutorial.PlayVideo("Movies/ps4/tutorial", "2.mp4");
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Tutorial.Hide(null);
		yield return R.Ui.Tutorial.Show(4);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Tutorial.Hide(new int?(4));
		yield break;
	}
}
