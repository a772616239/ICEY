using System;
using System.Collections;
using DG.Tweening;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class StoryE16P6 : BaseBehaviour
{
	private bool e16Finish
	{
		set
		{
			SaveStorage.Set("E16_Finish", value);
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.MainCoroutine());
	}

	private void OnDestroy()
	{
		CameraFilterUtils.Remove<CameraFilterPack_TV_Old_Movie_2>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_BrokenGlass2>(null);
		CameraFilterUtils.Remove<CameraFilterPack_FB_AAA_Blood>(null);
	}

	private IEnumerator MainCoroutine()
	{
		this.gate.openType = SceneGate.OpenType.None;
		yield return new WaitForSeconds(3f);
		CameraFilterUtils.Create<CameraFilterPack_TV_Old_Movie_2>(null);
		this.text.text = ScriptLocalization.Story.e16s11;
		yield return DOTween.To(delegate(float a)
		{
			this.text.color = this.text.color.SetAlpha(a);
		}, 0f, 1f, 1.5f).WaitForCompletion();
		yield return new WaitForSeconds(1f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Old_Movie_2>(null);
		yield return R.Audio.PlayVoiceOver("e16t21", null, false);
		R.Audio.PlayEffect(466, null);
		YieldInstruction t22 = R.Audio.PlayVoiceOver("e16t22", null, false);
		if (UIAudioLanguage.IsChinese)
		{
			yield return new WaitForSeconds(12.23f);
		}
		else
		{
			yield return new WaitForSeconds(18.55f);
		}
		CameraFilterUtils.Create<CameraFilterPack_TV_BrokenGlass2>(null);
		CameraFilterUtils.Create<CameraFilterPack_FB_AAA_Blood>(null);
		yield return t22;
		R.Audio.PlayEffect(467, null);
		InputSetting.Stop(false);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16s12.Split(new char[]
		{
			'\n'
		})[0], 0.1f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16s12.Split(new char[]
		{
			'\n'
		})[1], 0.1f);
		yield return new WaitForSeconds(3f);
		R.Ui.Terminal.Clear();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16s13, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return new WaitForSeconds(1f);
		R.Ui.Terminal.HideProgressBar();
		R.Ui.Terminal.CloseWithoutAnim();
		R.Ui.BlackScene.Alpha = 1f;
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.green));
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16s14, 0.1f);
		yield return new WaitForSeconds(1f);
		R.Ui.Terminal.CloseWithoutAnim();
		R.Ui.BlackScene.Alpha = 1f;
		this.e16Finish = true;
		R.Trophy.AwardTrophy(19);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E16_Finish"))
			{
				gameData.ThisSaveValidStorage["E16_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E16_Finish", 1);
			}
		});
		yield return new WaitForSeconds(3f);
		yield return R.Audio.PlayVoiceOver("e16t23", null, false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_BrokenGlass2>(null);
		CameraFilterUtils.Remove<CameraFilterPack_FB_AAA_Blood>(null);
		yield return new WaitForSeconds(1f);
		InputSetting.Resume(false);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private TextMesh text;
}
