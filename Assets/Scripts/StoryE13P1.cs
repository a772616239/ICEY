using System;
using System.Collections;
using UnityEngine;

public class StoryE13P1 : BaseBehaviour
{
	private void Start()
	{
		if (R.Player.Attribute.flashLevel == 3)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			UnityEngine.Object.Destroy(this.supplyBox.gameObject);
		}
	}

	private void Update()
	{
		float num = Vector2.Distance(base.transform.position, R.Player.Transform.position);
		if (num < 4f && !this.show)
		{
			base.StartCoroutine(this.P1Coroutine());
		}
	}

	private IEnumerator P1Coroutine()
	{
		if (!this.show)
		{
			this.show = true;
			yield return R.Audio.PlayVoiceOver("e13t1", null, false);
		}
		yield return this.supplyBox.WaitForBreak();
		yield return new WaitForSeconds(2f);
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		R.Player.Attribute.flashLevel = 3;
		R.Player.Attribute.currentFlashTimes = R.Player.Attribute.flashTimes;
		R.Player.Attribute.FlashCd = 0;
		R.Ui.Flash.RecoverAll(R.Player.Attribute.flashTimes);
		YieldInstruction coroutine = R.Audio.PlayVoiceOver("e13t2", null, false);
		R.Mode.EnterMode(Mode.AllMode.Story);
		yield return R.Ui.Tutorial.PlayVideo("Movies/ps4", "flash3.mp4");
		R.Mode.ExitMode(Mode.AllMode.Story);
		yield return coroutine;
		yield break;
	}

	[SerializeField]
	private SupplyBoxAction supplyBox;

	private bool show;
}
