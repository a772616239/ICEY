using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE8P1 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private void OnDestroy()
	{
		CameraFilterUtils.Remove<CameraFilterPack_TV_CompressionFX>(null);
	}

	private IEnumerator Sequence0()
	{
		yield return new WaitForSeconds(1f);
		R.Ui.HideUI(false);
		R.Audio.StopBGM(true);
		CameraFilterUtils.Create<CameraFilterPack_TV_CompressionFX>(null);
		yield return R.Audio.PlayVoiceOver("e8t1", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e8s1, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return new WaitForSeconds(1f);
		R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithoutAnim();
		CameraFilterUtils.Remove<CameraFilterPack_TV_CompressionFX>(null);
		this._gate2.Enter(false);
		yield break;
	}

	[SerializeField]
	private SceneGate _gate2;
}
