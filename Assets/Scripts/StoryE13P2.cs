using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE13P2 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.P2Coroutine());
	}

	private void Update()
	{
	}

	private IEnumerator P2Coroutine()
	{
		InputSetting.Stop(false);
		yield return new WaitForSeconds(2f);
		base.StartCoroutine(this.RunCotoutine());
		yield return R.Audio.PlayVoiceOver("e13t9", null, false);
		yield return R.Ui.HideUI(false);
		this.white.SetActive(true);
		R.Audio.PlayEffect(361, new Vector3?(R.Player.Transform.position));
		yield return R.Audio.PlayVoiceOver("e13t10", null, false);
		R.Player.RoleName = "UCEY";
		yield return new WaitForSeconds(5f);
		yield return R.Audio.PlayVoiceOver("e13t11", null, false);
		R.Ui.BlackScene.Alpha = 1f;
		yield return R.Audio.PlayVoiceOver("e13t12", null, false);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e13s1, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BlackScene.Alpha = 1f;
		this.gate.Enter(false);
		yield break;
	}

	private IEnumerator RunCotoutine()
	{
		R.Player.ActionController.StartMove();
		while (R.Player.Transform.position.x - this.stopPoint.position.x < 0f)
		{
			yield return null;
		}
		R.Player.ActionController.StopMove();
		yield break;
	}

	[SerializeField]
	private Transform stopPoint;

	[SerializeField]
	private GameObject white;

	[SerializeField]
	private SceneGate gate;
}
