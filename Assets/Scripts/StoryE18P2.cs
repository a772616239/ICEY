using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE18P2 : BaseBehaviour
{
	private bool p7
	{
		get
		{
			return RoundStorage.Get("E18_P7", false);
		}
		set
		{
			RoundStorage.Set("E18_P7", value);
		}
	}

	private void Start()
	{
		if (this.p7)
		{
			base.StartCoroutine(this.P2Coroutine());
		}
	}

	private void Update()
	{
		if (R.Player.StateMachine.currentState == "Fall1" && !this._fall && this.p7)
		{
			this._fall = true;
			base.StartCoroutine(this.FallCoroutine());
		}
	}

	private IEnumerator P2Coroutine()
	{
		R.Player.GetComponent<PlayerInput>().battlePause = true;
		R.Audio.PlayBGM(262, true);
		CameraFilterUtils.Create<CameraFilterPack_TV_Chromatical2>(null);
		this._gate.openType = SceneGate.OpenType.None;
		yield return R.Audio.PlayVoiceOver("e18t14", null, false);
		yield break;
	}

	private IEnumerator FallCoroutine()
	{
		R.Audio.StopVoiceOver();
		yield return new WaitForSeconds(0.2f);
		R.Camera.Controller.IsFollowPivot = false;
		InputSetting.Stop(false);
		R.Player.GetComponent<PlayerInput>().battlePause = false;
		yield return new WaitForSeconds(0.5f);
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.UnderAtkHitToFly);
		R.Player.Transform.GetComponent<PlayerAnimEventListener>().getUp = false;
		while (R.Player.Transform.position.y > this._trigger.position.y)
		{
			yield return null;
		}
		R.Audio.PlayEffect(357, new Vector3?(base.transform.position));
		R.Ui.BlackScene.Alpha = 1f;
		R.Audio.StopBGM(true);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Chromatical2>(null);
		yield return new WaitForSeconds(3f);
		R.Camera.Controller.IsFollowPivot = true;
		R.Audio.PlayEffect(386, new Vector3?(base.transform.position));
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e18s5, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Player.Transform.GetComponent<PlayerAnimEventListener>().getUp = true;
		InputSetting.Resume(false);
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private void OnDestroy()
	{
		R.Camera.Controller.IsFollowPivot = true;
		InputSetting.Resume(false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Chromatical2>(null);
		R.Player.Transform.GetComponent<PlayerAnimEventListener>().getUp = true;
		R.Player.GetComponent<PlayerInput>().battlePause = false;
	}

	[SerializeField]
	private SceneGate _gate;

	[SerializeField]
	private Transform _trigger;

	private bool _fall;
}
