using System;
using System.Collections;
using UnityEngine;

public class StoryE24P2 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private IEnumerator Sequence0()
	{
		yield return R.Audio.PlayVoiceOver("e24t4", null, false);
		yield return new WaitForSeconds(15f);
		yield return R.Ui.BlackScene.FadeBlack(5f, false);
		InputSetting.Stop(true);
		yield return new WaitForSeconds(3f);
		LevelManager.LoadLevelByGateId("C3L1S1", SceneGate.OpenType.Left);
		yield break;
	}
}
