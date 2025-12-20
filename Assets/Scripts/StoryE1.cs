using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class StoryE1 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Sequence0());
	}

	private void Update()
	{
		if (this._skippable && this._useHint && Core.Input.AnyKey.OnClick && R.Mode.IsInStoryMode() && (this._pressTrangleToPassTweener == null || !this._pressTrangleToPassTweener.IsPlaying()))
		{
			this._pressTrangleToPass.gameObject.SetActive(true);
			this._pressTrangleToPassTweener = DOTween.Sequence().Append(this._pressTrangleToPass.DOFade(1f, 0.5f)).Append(this._pressTrangleToPass.DOFade(0f, 0.5f).SetDelay(1.5f));
		}
		if (this._skippable && Core.Input.AnyKey.LongPressed && R.Mode.IsInStoryMode())
		{
			base.StopCoroutine(this.Sequence0());
			base.StartCoroutine(this.Sequence1());
		}
	}

	private void OnDestroy()
	{
		if (!SingletonMono<AudioManager>.ApplicationIsQuitting)
		{
			R.Audio.StopVoiceOver();
		}
	}

	private IEnumerator Sequence0()
	{
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.MovableCamera.position = Vector3.zero.SetZ(-10f);
		R.Ui.HideUI(true);
		R.Mode.EnterMode(Mode.AllMode.Story);
		R.Player.Action.TurnRound(1);
		this._useHint = true;
		yield return new WaitForSeconds(0.5f);
		this.BlackMaskFade(0f, 0f);
		R.Ui.BlackScene.Alpha = 1f;
		YieldInstruction voiceOver = R.Audio.PlayVoiceOver("e1t1", null, false);
		yield return R.Ui.BlackScene.FadeTransparent(2f, false);
		yield return voiceOver;
		voiceOver = R.Audio.PlayVoiceOver("e1t2", null, false);
		this.audioSources.Add(R.Audio.PlayEffect(205, null));
		yield return this._sceneSprites.DOScale(new Vector3(2.5f, 2.5f, 1f), 10f).SetEase(Ease.InSine).WaitForCompletion();
		AudioSource audioEffect206 = R.Audio.PlayEffect(206, null);
		audioEffect206.loop = true;
		this.audioSources.Add(audioEffect206);
		yield return voiceOver;
		this._skippable = true;
		yield return R.Audio.PlayVoiceOver("e1t3", null, false);
		SunShafts sun = Camera.main.gameObject.AddComponent<SunShafts>();
		sun.sunThreshold = Color.black;
		sun.sunShaftBlurRadius = 10f;
		sun.sunShaftsShader = Shader.Find("Hidden/SunShaftsComposite");
		sun.simpleClearShader = Shader.Find("Hidden/SimpleClear");
		DOTween.To(() => sun.sunShaftIntensity, delegate(float intensity)
		{
			sun.sunShaftIntensity = intensity;
		}, 1.5f, 0.1f).OnComplete(delegate
		{
			UnityEngine.Object.Destroy(sun);
		}).WaitForCompletion();
		this._judasShadow.parent.parent.gameObject.SetActive(true);
		SpriteRenderer judas = this._judasShadow.parent.GetComponent<SpriteRenderer>();
		this._judasShadow.DOShakePosition(1.5f, new Vector3(0.4f, 0.4f, 0f), 30, 90f, false, true).WaitForCompletion();
		this._judasShadow.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f).SetDelay(1f);
		yield return judas.DOFade(0f, 0.5f).SetDelay(1f).WaitForCompletion();
		this._judasShadow.parent.parent.gameObject.SetActive(false);
		yield return R.Audio.PlayVoiceOver("e1t4", null, false);
		yield return R.Audio.PlayVoiceOver("e1t5", null, false);
		yield return this.BlackMaskFade(1f, 5f);
		yield return R.Audio.PlayVoiceOver("e1t6", null, false);
		yield return R.Audio.PlayVoiceOver("e1t7", null, false);
		this._useHint = false;
		this._pressTrangleToPass.DOFade(0f, 0.5f);
		this._pressTrangleToStart.gameObject.SetActive(true);
		this._pressTrangleToStart.DOFade(1f, 1f);
		yield return new WaitForSeconds(10f);
		this._pressTrangleToStart.DOFade(0f, 1f);
		this._skippable = false;
		this.BlackMaskFade(0f, 5f);
		yield return R.Audio.PlayVoiceOver("e1t8", null, false);
		voiceOver = R.Audio.PlayVoiceOver("e1t9", null, false);
		audioEffect206.DOFade(0f, 1f);
		this.audioSources.Add(R.Audio.PlayEffect(207, null));
		yield return this._sceneSprites.DOScale(new Vector3(1f, 1f, 1f), 10f).SetEase(Ease.InSine).WaitForCompletion();
		yield return voiceOver;
		R.Camera.Controller.CameraShake(1f, 0.1f, CameraController.ShakeTypeEnum.Rect, false);
		yield return R.Audio.PlayVoiceOver("e1t10", null, false);
		yield return R.Audio.PlayVoiceOver("e1t11", null, false);
		voiceOver = R.Audio.PlayVoiceOver("e1t12", null, false);
		CameraFilterUtils.Create<CameraFilterPack_Blur_Movie>(null);
		yield return DOTween.To(delegate(float r)
		{
			CameraFilterPack_Blur_Movie.ChangeRadius = r;
		}, 0f, 500f, 5f).WaitForCompletion();
		yield return DOTween.To(delegate(float r)
		{
			CameraFilterPack_Blur_Movie.ChangeRadius = r;
		}, 500f, 0f, 5f).WaitForCompletion();
		CameraFilterUtils.Remove<CameraFilterPack_Blur_Movie>(null);
		yield return voiceOver;
		yield return R.Audio.PlayVoiceOver("e1t13", null, false);
		this.BlackMaskFade(1f, 2f);
		yield return R.Audio.PlayVoiceOver("e1t14", null, false);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e1s1, 0.1f);
		R.Trophy.AwardTrophy(1);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return R.Ui.BlackScene.FadeBlack(0f, false);
		R.Mode.ExitMode(Mode.AllMode.Story);
		InputSetting.Stop(true);
		R.Camera.Controller.IsFollowPivot = true;
		this._gate1.Enter(false);
		yield break;
	}

	private IEnumerator Sequence1()
	{
		R.Mode.ExitMode(Mode.AllMode.Story);
		InputSetting.Stop(true);
		yield return R.Ui.BlackScene.FadeBlack(2f, false);
		this.BlackMaskFade(1f, 0f);
		R.Camera.Controller.IsFollowPivot = true;
		for (int i = 0; i < this.audioSources.Count; i++)
		{
			if (this.audioSources[i] != null)
			{
				this.audioSources[i].Stop();
			}
		}
		R.Ui.UISubtitle.FadeOut();
		CameraFilterUtils.Remove<CameraFilterPack_Blur_Movie>(null);
		this._gate2.Enter(false);
		yield break;
	}

	private YieldInstruction BlackMaskFade(float endValue, float duration)
	{
		return this._blackMask.DOFade(endValue, duration).WaitForCompletion();
	}

	[SerializeField]
	private SceneGate _gate1;

	[SerializeField]
	private SceneGate _gate2;

	[SerializeField]
	private UIWidget _pressTrangleToPass;

	[SerializeField]
	private UIWidget _pressTrangleToStart;

	[SerializeField]
	private Transform _sceneSprites;

	[SerializeField]
	private SpriteRenderer _blackMask;

	[SerializeField]
	private Transform _judasShadow;

	private bool _skippable;

	private bool _useHint;

	private Sequence _pressTrangleToPassTweener;

	private List<AudioSource> audioSources = new List<AudioSource>();
}
