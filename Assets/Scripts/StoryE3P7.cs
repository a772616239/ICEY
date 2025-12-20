using System;
using System.Collections;
using DG.Tweening;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE3P7 : BaseBehaviour
{
	private bool _hasEntered
	{
		get
		{
			return RoundStorage.Get("E3P7_hasEntered", false);
		}
		set
		{
			RoundStorage.Set("E3P7_hasEntered", value);
		}
	}

	private bool e3Finish
	{
		set
		{
			SaveStorage.Set("E3_Finish", value);
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.Sequcence0());
	}

	private void OnDestroy()
	{
		CameraFilterUtils.Remove<CameraFilterPack_Retro_FBLoading>(null);
		CameraFilterUtils.Remove<CameraFilterPack_VHS_Tracking>(null);
	}

	private IEnumerator Sequcence0()
	{
		this._gate1.openType = SceneGate.OpenType.None;
		this._battleZoneGate.Appear();
		if (this._hasEntered)
		{
			yield return null;
			yield return new WaitForSeconds(20f);
		}
		else
		{
			this._hasEntered = true;
			R.Audio.PlayBGM(349, true);
			R.Ui.HideUI(false);
			yield return R.Audio.PlayVoiceOver("e3t7", null, false);
			CameraFilterPack_VHS_Tracking vhsTracking = CameraFilterUtils.Create<CameraFilterPack_VHS_Tracking>(null);
			yield return R.Audio.PlayVoiceOver("e3t8", null, false);
			this._text.text = ScriptLocalization.Story.e3s1;
			this._text.color = Color.white;
			this.TextShine(true);
			yield return R.Audio.PlayVoiceOver("e3t9", null, false);
		}
		yield return R.Audio.PlayVoiceOver("e3t10", null, false);
		this.TextShine(false);
		this._text.text = ScriptLocalization.Story.e3s2;
		this._text.color = new Color32(126, 16, 34, 1);
		yield return this.FadeIn();
		yield return R.Audio.PlayVoiceOver("e3t11", null, false);
		CameraFilterPack_Retro_FBLoading fbLoading = CameraFilterUtils.Create<CameraFilterPack_Retro_FBLoading>(null);
		yield return R.Audio.PlayVoiceOver("e3t12", null, false);
		WorldTime.Pause();
		InputSetting.Stop(false);
		R.Audio.StopBGM(false);
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(5f);
		WorldTime.Resume();
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e3s3.Split(new char[]
		{
			'\n'
		})[0], 0.1f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e3s3.Split(new char[]
		{
			'\n'
		})[1], 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Clear();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e3s4, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 5f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e3s5, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BlackScene.Alpha = 1f;
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(3f);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e3s6, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BlackScene.Alpha = 1f;
		yield return new WaitForSeconds(3f);
		yield return R.Audio.PlayVoiceOver("e3t13", null, false);
		this.e3Finish = true;
		R.Trophy.AwardTrophy(3);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E3_Finish"))
			{
				gameData.ThisSaveValidStorage["E3_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E3_Finish", 1);
			}
		});
		R.Ui.ShowUI(false);
		CameraFilterUtils.Remove<CameraFilterPack_Retro_FBLoading>(null);
		CameraFilterUtils.Remove<CameraFilterPack_VHS_Tracking>(null);
		InputSetting.Resume(false);
		SaveStorage.Set(R.Ui.LevelSelect.LevelBeginningSceneNames[0] + "HasConquer", true);
		R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private void TextShine(bool start)
	{
		this._startShine = start;
		if (start)
		{
			base.StartCoroutine(this.StartShineCoroutine());
		}
	}

	private IEnumerator StartShineCoroutine()
	{
		while (this._startShine)
		{
			yield return DOTween.To(delegate(float a)
			{
				this._text.color = this._text.color.SetAlpha(a);
			}, 1f, 0f, 1f).WaitForCompletion();
			R.Audio.PlayEffect(383, new Vector3?(this._text.transform.position));
		}
		yield break;
	}

	private YieldInstruction FadeIn()
	{
		return DOTween.To(delegate(float a)
		{
			this._text.color = this._text.color.SetAlpha(a);
		}, 0f, 1f, 5f).WaitForCompletion();
	}

	[SerializeField]
	private SceneGate _gate1;

	[SerializeField]
	private BattleZoneGate _battleZoneGate;

	[SerializeField]
	private TextMesh _text;

	private bool _startShine;
}
