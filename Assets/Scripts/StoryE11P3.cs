using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE11P3 : BaseBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (this._shiTrigger)
		{
			return;
		}
		this._shiTrigger = true;
		base.StartCoroutine(this.Sequence0());
	}

	private IEnumerator Sequence0()
	{
		InputSetting.Stop(false);
		CameraFilterUtils.Create<CameraFilterPack_TV_PlanetMars>(null);
		yield return R.Audio.PlayVoiceOver("e11t6", null, false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.red));
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e11s1, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e11s2, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Player.ActionController.TurnRound(1);
		this._gateToShi.Enter(false);
		InputSetting.Resume(false);
		yield break;
	}

	[SerializeField]
	private SceneGate _gateToShi;

	private bool _shiTrigger;
}
