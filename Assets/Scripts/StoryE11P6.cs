using System;
using System.Collections;
using UnityEngine;

public class StoryE11P6 : BaseBehaviour
{
	public static void OnShiPlayerDie()
	{
		StoryE11P6._deathTimes++;
		StoryE11P6.Trigger();
	}

	private static void Trigger()
	{
		switch (StoryE11P6._deathTimes)
		{
		case 1:
			R.Audio.PlayVoiceOver("e11t16", null, false);
			break;
		case 2:
			R.Audio.PlayVoiceOver("e11t17", null, false);
			break;
		case 3:
			R.Audio.PlayVoiceOver("e11t18", null, false);
			break;
		case 5:
			R.Coroutine.Start(StoryE11P6.Sequence0());
			break;
		case 10:
			R.Coroutine.Start(StoryE11P6.Sequence1());
			break;
		}
	}

	private static IEnumerator Sequence0()
	{
		R.Audio.PlayVoiceOver("e11t19", null, false);
		AudioSource audio = R.Audio.PlayEffect(353, null);
		while (audio.isPlaying)
		{
			yield return null;
		}
		R.Audio.PlayEffect(385, null);
		yield break;
	}

	private static IEnumerator Sequence1()
	{
		InputSetting.Stop(false);
		yield return R.Audio.PlayVoiceOver("e11t20", null, false);
		R.Ui.BlackScene.Alpha = 0f;
		R.Audio.StopBGM(false);
		yield return R.Audio.PlayVoiceOver("e11t21", null, false);
		R.Equipment.CoinNum += 100;
		InputSetting.Resume(false);
		R.Mode.ExitMode(Mode.AllMode.Shi);
		R.Ui.ShowUI(false);
		R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private static int _deathTimes;
}
