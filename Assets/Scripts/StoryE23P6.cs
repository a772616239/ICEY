using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE23P6 : BaseBehaviour
{
	private void Update()
	{
		if (this.isFirst && Mathf.Abs(R.Player.Transform.position.x - base.transform.position.x) < 1f)
		{
			this.isFirst = false;
			base.StartCoroutine(this.Sequence0());
		}
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(null);
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Ui.Tutorial.Show(ScriptLocalization.mobile_tutorial.jumpAcross);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Tutorial.Hide(null);
		yield break;
	}

	private bool isFirst = true;
}
