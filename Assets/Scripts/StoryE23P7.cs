using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE23P7 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(null);
			R.Ui.Tutorial.Hide(new int?(6));
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Ui.Tutorial.Show(ScriptLocalization.ui_tutorial.moreComboMoreAttack);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Tutorial.Hide(null);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Tutorial.Show(6);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Tutorial.Hide(new int?(6));
		yield break;
	}
}
