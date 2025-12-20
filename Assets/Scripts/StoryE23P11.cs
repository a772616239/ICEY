using System;
using System.Collections;
using Core;
using I2.Loc;
using UnityEngine;

public class StoryE23P11 : BaseBehaviour
{
	private void Awake()
	{
		this._blackMask.color = Color.black;
	}

	private void Start()
	{
		R.Ui.LevelSelect.StartButtonClickEvent += this.StartButtonClickEvent;
		R.Ui.HideUI(true);
		R.Mode.EnterMode(Mode.AllMode.Story);
		R.Ui.Pause.Enabled = false;
		base.StartCoroutine(this.Sequence0());
	}

	private IEnumerator Sequence0()
	{
		R.Ui.Pause.Enabled = false;
		CameraFilterUtils.Create<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		this._errorBoom.SetActive(true);
		AudioSource audiosource = R.Audio.PlayEffect(362, null);
		audiosource.loop = true;
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		audiosource.loop = false;
		audiosource.Stop();
		this._errorBoom.SetActive(false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		R.Ui.Pause.Enabled = true;
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e23s1, 0.1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e23s1, 0.1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e23s1, 0.1f);
		yield return R.Ui.Terminal.PrintlnInstantly(string.Empty);
		yield return new WaitForSeconds(0.5f);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e23s2, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(0.99f, 3f);
		if (UILanguage.IsSimplifiedChinese)
		{
			yield return R.Ui.Terminal.ShowTexture(this._repairError_cn, 1f, 1f);
		}
		else if (UILanguage.IsTraditionalChinese)
		{
			yield return R.Ui.Terminal.ShowTexture(this._repairError_cn_tw, 1f, 1f);
		}
		else
		{
			yield return R.Ui.Terminal.ShowTexture(this._repairError, 1f, 1f);
		}
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		yield return R.Ui.Terminal.HideTexture();
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.Println(string.Empty, 0.1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e23s3, 0.1f);
		yield return R.Ui.Terminal.Clear();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e23s4, 0.5f);
		yield return R.Ui.Terminal.CloseWithoutAnim();
		yield return R.Ui.LevelSelect.OpenWithAnim(false, false);
		yield break;
	}

	private IEnumerator Sequence1()
	{
		yield return R.Ui.LevelSelect.CloseWithAnim();
		yield return R.Ui.Terminal.OpenWithoutAnim(null);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BlackScene.Alpha = 1f;
		this._blackMask.gameObject.SetActive(false);
		R.Audio.PlayBGM(277, true);
		yield return R.Ui.BlackScene.FadeTransparent(3f, false);
		R.Mode.ExitMode(Mode.AllMode.Story);
		yield break;
	}

	private void StartButtonClickEvent(object sender, EventArgs args)
	{
		base.StartCoroutine(this.Sequence1());
		R.Ui.LevelSelect.StartButtonClickEvent -= this.StartButtonClickEvent;
	}

	[SerializeField]
	private SpriteRenderer _blackMask;

	[SerializeField]
	private GameObject _errorBoom;

	[SerializeField]
	private Texture _repairError;

	[SerializeField]
	private Texture _repairError_cn;

	[SerializeField]
	private Texture _repairError_cn_tw;
}
