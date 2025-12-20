using System;
using System.Collections;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class StoryE2 : BaseBehaviour
{
	private static int JumpTimes
	{
		get
		{
			return RoundStorage.Get("E2_jumpTimes", 1);
		}
		set
		{
			RoundStorage.Set("E2_jumpTimes", value);
		}
	}

	private static bool HasJumped
	{
		get
		{
			return RoundStorage.Get("E2_hasJumped", true);
		}
		set
		{
			RoundStorage.Set("E2_hasJumped", value);
		}
	}

	private void Update()
	{
		if (StoryE2.HasJumped && Math.Abs(R.Player.Transform.position.x - this._point1.position.x) < 1f)
		{
			if (StoryE2.JumpTimes <= 3)
			{
				R.Audio.PlayVoiceOver("e2t" + (StoryE2.JumpTimes * 2 - 1), null, false);
			}
			StoryE2.HasJumped = false;
		}
		if (!StoryE2.HasJumped && Math.Abs(R.Player.Transform.position.y - this._cliffDead.position.y) < 1f)
		{
			StoryE2.HasJumped = true;
			InputSetting.Stop(false);
			SingletonMono<CameraController>.Instance.IsFollowPivot = false;
			switch (StoryE2.JumpTimes)
			{
			case 1:
				R.Audio.PlayVoiceOver("e2t2", delegate
				{
					R.Player.Transform.position = this._point1.position.SetZ(R.Player.Transform.position.z);
					SingletonMono<CameraController>.Instance.IsFollowPivot = true;
					InputSetting.Resume(false);
				}, false);
				break;
			case 2:
				R.Audio.PlayVoiceOver("e2t4", delegate
				{
					R.Player.Transform.position = this._point1.position.SetZ(R.Player.Transform.position.z);
					SingletonMono<CameraController>.Instance.IsFollowPivot = true;
					InputSetting.Resume(false);
				}, false);
				break;
			case 3:
				R.Audio.PlayVoiceOver("e2t6", delegate
				{
					R.Trophy.AwardTrophy(2);
					R.Player.Transform.position = this._point1.position.SetZ(R.Player.Transform.position.z);
					SingletonMono<CameraController>.Instance.IsFollowPivot = true;
					base.StartCoroutine(this.Sequence0());
				}, false);
				break;
			case 4:
				R.Audio.PlayVoiceOver("e2t8", delegate
				{
					base.StartCoroutine(this.Sequence1());
				}, false);
				break;
			default:
				R.Player.Transform.position = this._point1.position.SetZ(R.Player.Transform.position.z);
				break;
			}
			StoryE2.JumpTimes++;
		}
	}

	private IEnumerator Sequence0()
	{
		this._battleZoneGateL.Appear();
		this._battleZoneGateR.Appear();
		YieldInstruction voiceOver = R.Audio.PlayVoiceOver("e2t7", null, false);
		yield return new WaitForSeconds(2f);
		InputSetting.Resume(false);
		yield return voiceOver;
		this._machine.localPosition = this._machine.localPosition.SetY(1.62f);
		this._machine.localScale *= 1.5f;
		this._battleZoneGateL.DisAppear();
		this._battleZoneGateR.DisAppear();
		yield break;
	}

	private IEnumerator Sequence1()
	{
		Color color;
		ColorUtility.TryParseHtmlString("00c0ff", out color);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(color));
		YieldInstruction voiceOver = R.Audio.PlayVoiceOver("e2t9", null, false);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e2s1, 0.1f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e2s2, 0.1f);
		yield return voiceOver;
		yield return R.Audio.PlayVoiceOver("e2t10", null, false);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e2s3, 0.1f);
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return R.Audio.PlayVoiceOver("e2t11", null, false);
		R.Player.Rigidbody2D.gravityScale = 1f;
		this._gate.Enter(false);
		InputSetting.Resume(false);
		yield break;
	}

	[SerializeField]
	private Transform _point1;

	[SerializeField]
	private Transform _cliffDead;

	[SerializeField]
	private Transform _machine;

	[SerializeField]
	private SceneGate _gate;

	[SerializeField]
	private BattleZoneGate _battleZoneGateL;

	[SerializeField]
	private BattleZoneGate _battleZoneGateR;
}
