using System;
using System.Collections;
using Core;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE22P1 : BaseBehaviour
{
	private bool E22Email
	{
		set
		{
			RoundStorage.Set("E22_Email", value);
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private void Start()
	{
		if (R.Player.Attribute.flashLevel >= 2)
		{
			UnityEngine.Object.Destroy(this.supplyBox.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private bool BattleOver(string eventName, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			base.StartCoroutine(this.P1Coroutine());
		}
		return true;
	}

	private IEnumerator P1Coroutine()
	{
		this.gate.openType = SceneGate.OpenType.None;
		this._battleZoneGateL.Appear();
		yield return R.Audio.PlayVoiceOver("e22t1", null, false);
		yield return this.supplyBox.WaitForBreak();
		yield return new WaitForSeconds(2f);
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		R.Player.Attribute.flashLevel = 2;
		R.Player.Attribute.currentFlashTimes = R.Player.Attribute.flashTimes;
		R.Player.Attribute.FlashCd = 0;
		R.Ui.Flash.RecoverAll(R.Player.Attribute.flashTimes);
		yield return R.Audio.PlayVoiceOver("e22t2", null, false);
		R.Mode.EnterMode(Mode.AllMode.Story);
		yield return R.Ui.Tutorial.PlayVideo("Movies/ps4", "flash2.mp4");
		R.Mode.ExitMode(Mode.AllMode.Story);
		yield return new WaitForSeconds(3f);
		yield return R.Audio.PlayVoiceOver("e22t3", null, false);
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(1, WorldTime.FrozenArgs.FrozenType.Player, false);
		WorldTime.Pause();
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		R.Audio.IsEffectsMute = true;
		R.Audio.StopBGM(false);
		CameraFilterUtils.Create<CameraFilterPack_TV_Artefact>(null);
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(3f);
		R.Audio.IsEffectsMute = false;
		this.E22Email = true;
		yield return R.Ui.uiNotifacation.Show(ScriptLocalization.Story.e22s1, 2f);
		R.Ui.uiNotifacation.FadeIn(ScriptLocalization.mobile.e22s2);
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		R.Audio.PlayEffect(364, null);
		yield return R.Ui.uiNotifacation.FadeOut(0f, 1f);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		WorldTime.Resume();
		yield return R.Ui.Terminal.Print(ScriptLocalization.Story.e22s3, 0.1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e22s4, 0.5f);
		yield return R.Ui.Terminal.Println(string.Format(ScriptLocalization.mobile.e22s5, DateTime.Now), 0.1f);
		yield return R.Ui.Terminal.ShowTexture((!UILanguage.IsChinese) ? this._map : this._mapCn, 0.3f, 1f);
		bool hasShowRawImage = false;
		for (;;)
		{
			if (Core.Input.AnyKey.OnClick)
			{
				if (hasShowRawImage)
				{
					break;
				}
				R.Ui.Terminal.SetTextureFullScreen();
				hasShowRawImage = true;
				yield return new WaitForSeconds(1f);
			}
			yield return null;
		}
		R.Ui.Terminal.SetTextureSize(0.3f);
		yield return new WaitForSeconds(0.3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.Terminal.HideTexture();
		SingletonMono<WorldTime>.Instance.FrozenResume();
		CameraFilterUtils.Remove<CameraFilterPack_TV_Artefact>(null);
		this.gate.openType = SceneGate.OpenType.Right;
		this._battleZoneGateL.DisAppear();
		R.Mode.ExitMode(Mode.AllMode.UI);
		yield break;
	}

	[SerializeField]
	private BattleZoneGate _battleZoneGateL;

	[SerializeField]
	private Texture _map;

	[SerializeField]
	private Texture _mapCn;

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private SupplyBoxAction supplyBox;
}
