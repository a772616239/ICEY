using System;
using System.Collections;
using UnityEngine;

public class StoryE23P3 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(new int?(3));
			R.Ui.Tutorial.DisposeVideo();
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Ui.Tutorial.Show(3);
		yield return R.Ui.Tutorial.PlayVideo("Movies/ps4/tutorial", "1.mp4");
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Tutorial.Hide(new int?(3));
		yield break;
	}
}
