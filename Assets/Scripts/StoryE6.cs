using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE6 : BaseBehaviour
{
	private static bool IsFirstE6
	{
		get
		{
			return RoundStorage.Get("E6_p10", true);
		}
		set
		{
			RoundStorage.Set("E6_p10", value);
		}
	}

	private void Start()
	{
		if (StoryE6.IsFirstE6)
		{
			base.StartCoroutine(this.Sequcence0());
		}
	}

	private IEnumerator Sequcence0()
	{
		R.Mode.EnterMode(Mode.AllMode.Story);
		R.Ui.HideUI(false);
		R.Player.SetPosition(base.transform.position);
		R.Camera.Controller.CameraResetPostionAfterSwitchScene();
		yield return new WaitForSeconds(2f);
		R.Camera.Controller.CameraZoom(R.Camera.Transform.position, 13f, -10f);
		yield return R.Ui.Toast.Show(ScriptLocalization.Story.e6s10, 40, 3f, false);
		yield return R.Ui.Toast.Show(ScriptLocalization.Story.e6s11, 40, 3f, false);
		yield return R.Ui.Toast.Show(ScriptLocalization.Story.e6s12, 40, 3f, false);
		yield return R.Ui.Toast.Show(ScriptLocalization.Story.e6s13, 40, 4f, false);
		SingletonMono<FullScreenColorMaskController>.Instance.color = Color.black;
		yield return new WaitForSeconds(2f);
		R.Audio.PlayEffect(458, null);
		R.Ui.Toast.Appear(ScriptLocalization.Story.e6s14, 80, false);
		R.Trophy.AwardTrophy(5);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		SingletonMono<FullScreenColorMaskController>.Instance.color = Color.clear;
		R.Ui.Toast.Disappear();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e6s15, 0.1f);
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.FadeBlack(0.3f, false);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Mode.ExitMode(Mode.AllMode.Story);
		yield return R.Ui.ShowUI(false);
		R.Ui.BlackScene.FadeTransparent(0.3f, false);
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}
}
