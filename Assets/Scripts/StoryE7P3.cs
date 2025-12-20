using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE7P3 : BaseBehaviour
{
	private bool _e7p6
	{
		get
		{
			return RoundStorage.Get("E7_P6", false);
		}
		set
		{
			RoundStorage.Set("E7_P6", value);
		}
	}

	private bool _e7p7
	{
		get
		{
			return RoundStorage.Get("E7_P7", false);
		}
		set
		{
			RoundStorage.Set("E7_P7", value);
		}
	}

	private bool _e7p8
	{
		get
		{
			return RoundStorage.Get("E7_P8", false);
		}
		set
		{
			RoundStorage.Set("E7_P8", value);
		}
	}

	private bool e7Finish
	{
		set
		{
			SaveStorage.Set("E7_Finish", value);
		}
	}

	private void Start()
	{
		if (!this._e7p6)
		{
			this._e7p6 = true;
			R.Audio.PlayVoiceOver("e7t7", delegate
			{
				this._e7p6Finished = true;
			}, false);
		}
		if (this._e7p7)
		{
			this._e7p8 = true;
			R.Audio.StopBGM(true);
			R.Audio.PlayVoiceOver("e7t9", delegate
			{
				this._e7p8Finished = true;
			}, false);
		}
	}

	private void Update()
	{
		if (!this._e7p10 && this._e7p6Finished && !this._e7p7)
		{
			this._e7p10 = true;
			base.StartCoroutine(this.P3Coroutine());
		}
		if (!this._e7p11 && this._e7p7 && this._e7p8Finished)
		{
			this._e7p11 = true;
			base.StartCoroutine(this.P4Coroutine());
		}
	}

	private void OnDestroy()
	{
		if (this._audioSource != null)
		{
			this._audioSource.Stop();
			this._audioSource.clip = null;
		}
		R.Ui.Volume.Hide();
	}

	private IEnumerator P3Coroutine()
	{
		yield return new WaitForSeconds(3f);
		R.Audio.StopBGM(true);
		yield return R.Audio.PlayVoiceOver("e7t11", null, false);
		yield return R.Ui.Volume.Show(0.5f);
		yield return R.Ui.Volume.BarAnim(1f, 0.5f);
		yield return R.Ui.Volume.Hide();
		yield return R.Audio.PlayVoiceOver("e7t12", null, false);
		yield return R.Audio.PlayVoiceOver("e7t13", null, false);
		YieldInstruction coroutine = R.Audio.PlayVoiceOver("e7t14", null, false);
		yield return R.Ui.Volume.Show(1f);
		yield return R.Ui.Volume.BarAnim(0.5f, 0.5f);
		yield return R.Ui.Volume.Hide();
		yield return coroutine;
		this.guide.SetActive(true);
		yield return new WaitForSeconds(1f);
		yield return R.Audio.PlayVoiceOver("e7t15", null, false);
		this.guide.SetActive(false);
		yield return new WaitForSeconds(1f);
		yield return base.StartCoroutine(this.P4Coroutine());
		yield break;
	}

	private IEnumerator P4Coroutine()
	{
		yield return R.Audio.PlayVoiceOver("e7t16", null, false);
		yield return new WaitForSeconds(5f);
		if (UIAudioLanguage.IsChinese)
		{
			this._audioSource = R.Audio.PlayEffect(473, null);
			yield return new WaitForSeconds(this._audioSource.clip.length);
			yield return new WaitForSeconds(5f);
			this._audioSource = R.Audio.PlayEffect(475, null);
			yield return new WaitForSeconds(this._audioSource.clip.length);
		}
		else
		{
			this._audioSource = R.Audio.PlayEffect(481, null);
			yield return new WaitForSeconds(this._audioSource.clip.length);
			yield return new WaitForSeconds(5f);
			this._audioSource = R.Audio.PlayEffect(483, null);
			yield return new WaitForSeconds(this._audioSource.clip.length);
		}
		yield return new WaitForSeconds(30f);
		R.Trophy.AwardTrophy(7);
		this.gate.openType = SceneGate.OpenType.None;
		yield return R.Audio.PlayVoiceOver("e7t19", null, false);
		yield return R.Audio.PlayVoiceOver("e7t20", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s1, 0.1f);
		yield return R.Ui.Terminal.CloseWithAnim();
		this.copperCup.SetActive(true);
		while (this.copperCup != null)
		{
			yield return null;
		}
		R.Trophy.AwardTrophy(8);
		yield return R.Audio.PlayVoiceOver("e7t21", null, false);
		this.silverCup.SetActive(true);
		while (this.silverCup != null)
		{
			yield return null;
		}
		R.Trophy.AwardTrophy(9);
		yield return R.Audio.PlayVoiceOver("e7t22", null, false);
		CameraFilterUtils.Create<CameraFilterPack_VHS_Tracking>(null);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s2, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		YieldInstruction voiceOver = R.Audio.PlayVoiceOver("e7t23", null, false);
		R.Ui.Terminal.SetProgressBarValueWithAnim(0.99f, 40f);
		yield return voiceOver;
		R.Ui.Terminal.HideProgressBar();
		R.Ui.Terminal.CloseWithoutAnim();
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(10, WorldTime.FrozenArgs.FrozenType.Player, false);
		R.Audio.StopBGM(true);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s3.Split(new char[]
		{
			'\n'
		})[0], 0.1f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s3.Split(new char[]
		{
			'\n'
		})[1], 0.1f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s4, 0.1f);
		this.e7Finish = true;
		R.Trophy.AwardTrophy(6);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E7_Finish"))
			{
				gameData.ThisSaveValidStorage["E7_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E7_Finish", 1);
			}
		});
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.Clear();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s5, 0.1f);
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.Alpha = 1f;
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.FadeTransparent(0.3f, false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e7s6, 0.1f);
		yield return new WaitForSeconds(1f);
		R.Ui.BlackScene.FadeBlack(0.3f, false);
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return new WaitForSeconds(5f);
		CameraFilterUtils.Remove<CameraFilterPack_VHS_Tracking>(null);
		SingletonMono<WorldTime>.Instance.FrozenResume();
		R.Ui.BlackScene.FadeTransparent(0.3f, false);
		R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	[SerializeField]
	private GameObject copperCup;

	[SerializeField]
	private GameObject silverCup;

	private bool _e7p6Finished;

	private bool _e7p8Finished;

	private bool _e7p10;

	private bool _e7p11;

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private GameObject guide;

	private AudioSource _audioSource;
}
