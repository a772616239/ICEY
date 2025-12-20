using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE23P5 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(new int?(5));
			R.Ui.Tutorial.Hide(null);
			R.Ui.Tutorial.DisposeVideo();
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Ui.Tutorial.Show(5);
		yield return R.Ui.Tutorial.PlayVideo("Movies/ps4/tutorial", "3.mp4");
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Tutorial.Hide(new int?(5));
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Tutorial.Show(ScriptLocalization.ui_tutorial.moreCrytalMoreAttack);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Tutorial.Hide(null);
		yield break;
	}
}
