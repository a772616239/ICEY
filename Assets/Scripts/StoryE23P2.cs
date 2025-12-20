using System;
using System.Collections;
using UnityEngine;

public class StoryE23P2 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(new int?(1));
			R.Ui.Tutorial.Hide(new int?(2));
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Ui.Tutorial.Show(1);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Tutorial.Hide(new int?(1));
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Tutorial.Show(2);
		yield return new WaitForSeconds(10f);
		yield return R.Ui.Tutorial.Hide(new int?(2));
		yield break;
	}
}
