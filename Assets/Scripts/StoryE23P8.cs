using System;
using System.Collections;
using UnityEngine;

public class StoryE23P8 : BaseBehaviour
{
	private static bool IsFirstE23P8
	{
		get
		{
			return SaveStorage.Get("E23_p8", true);
		}
		set
		{
			SaveStorage.Set("E23_p8", value);
		}
	}

	private bool HasEnteredLevelSelectSystem
	{
		get
		{
			return SaveStorage.Get("HasEnteredLevelSelectSystem", false);
		}
		set
		{
			SaveStorage.Set("HasEnteredLevelSelectSystem", value);
		}
	}

	private void Update()
	{
		if (StoryE23P8.IsFirstE23P8 && Mathf.Abs(R.Player.Transform.position.x - base.transform.position.x) < 1f)
		{
			StoryE23P8.IsFirstE23P8 = false;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.StartCoroutine(this.Sequence0());
		}
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(new int?(7));
		}
	}

	private IEnumerator Sequence0()
	{
		this.HasEnteredLevelSelectSystem = true;
		yield return R.Ui.Tutorial.Show(7);
		yield return new WaitForSeconds(5f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}
}
