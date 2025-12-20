using System;
using System.Collections;
using Core;
using DG.Tweening;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class StoryE10P3 : BaseBehaviour
{
	private void Update()
	{
		if (!this._trigger && Mathf.Abs(R.Player.Transform.position.x - base.transform.position.x) < 1f)
		{
			this._trigger = true;
			base.StartCoroutine(this.P3Coroutine());
		}
	}

	private void OnDestroy()
	{
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
	}

	private IEnumerator ShowTextWithContent(TextMesh text, string content, float time)
	{
		if (text.text != content)
		{
			text.text = content;
		}
		yield return DOTween.To(delegate(float a)
		{
			text.color = text.color.SetAlpha(a);
		}, 0f, 1f, time).WaitForCompletion();
		yield break;
	}

	private IEnumerator HideText(TextMesh text)
	{
		yield return DOTween.To(delegate(float a)
		{
			text.color = text.color.SetAlpha(a);
		}, 1f, 0f, 1f).WaitForCompletion();
		yield break;
	}

	private IEnumerator P3Coroutine()
	{
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		this._gateL.Appear();
		this._gateR.Appear();
		this._airWall1.SetActive(true);
		this._airWall2.SetActive(true);
		this.gateCollider.enabled = false;
		YieldInstruction t3 = R.Audio.PlayVoiceOver("e10t3", null, false);
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s2, 2f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_Yes, ScriptLocalization.Story.e10s3, 1f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_No, ScriptLocalization.Story.e10s4, 1f));
		yield return new WaitForSeconds(1.2f);
		yield return t3;
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(ScriptLocalization.Story.e10s3, ScriptLocalization.Story.e10s4);
		while (!Core.Input.Game.R2.OnPressed)
		{
			if (Core.Input.Game.L2.OnPressed)
			{
				base.StartCoroutine(this.P7Coroutine());
				
				yield break;
			}
			yield return null;
		}
		base.StartCoroutine(this.P4Coroutine());
        yield break;
    }

	private IEnumerator P4Coroutine()
	{
		base.StartCoroutine(this.HideText(this.text_Main));
		base.StartCoroutine(this.HideText(this.text_Yes));
		base.StartCoroutine(this.HideText(this.text_No));
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		yield return R.Audio.PlayVoiceOver("e10t4", null, false);
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s2, 2f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_Yes, ScriptLocalization.Story.e10s3, 1f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_No, ScriptLocalization.Story.e10s4, 1f));
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(ScriptLocalization.Story.e10s3, ScriptLocalization.Story.e10s4);
		yield return new WaitForSeconds(1.2f);
		while (!Core.Input.Game.R2.OnPressed)
		{
			if (Core.Input.Game.L2.OnPressed)
			{
				base.StartCoroutine(this.P7Coroutine());
				
				yield break;
			}
			yield return null;
		}
		base.StartCoroutine(this.P5Coroutine());
        yield break;
    }

	private IEnumerator P5Coroutine()
	{
		YieldInstruction t5 = R.Audio.PlayVoiceOver("e10t5", null, false);
		CameraFilterUtils.Create<CameraFilterPack_TV_VHS_Rewind>(null);
		R.Camera.Controller.CameraShake(1f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		base.StartCoroutine(this.HideText(this.text_Main));
		base.StartCoroutine(this.HideText(this.text_Yes));
		base.StartCoroutine(this.HideText(this.text_No));
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		yield return new WaitForSeconds(1f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_VHS_Rewind>(null);
		yield return t5;
		YieldInstruction t6 = R.Audio.PlayVoiceOver("e10t6", null, false);
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s5, 1f));
		yield return new WaitForSeconds(1f);
		yield return base.StartCoroutine(this.HideText(this.text_Main));
		yield return t6;
		yield return R.Audio.PlayVoiceOver("e10t7", null, false);
		base.StopCoroutine("P4L2Coroutine");
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s2, 2f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_Yes, ScriptLocalization.Story.e10s3, 1f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_No, ScriptLocalization.Story.e10s4, 1f));
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(ScriptLocalization.Story.e10s3, ScriptLocalization.Story.e10s4);
		yield return new WaitForSeconds(1.2f);
		while (!Core.Input.Game.R2.OnPressed)
		{
			if (Core.Input.Game.L2.OnPressed)
			{
				base.StartCoroutine(this.P8Coroutine());
			
				yield break;
			}
			yield return null;
		}
		base.StartCoroutine("P6_1Coroutine");
        yield break;
    }

	private IEnumerator P6_1Coroutine()
	{
		this.P6Break = true;
		base.StartCoroutine(this.P6BreakCoroutine());
		YieldInstruction t8 = R.Audio.PlayVoiceOver("e10t8", null, false);
		CameraFilterUtils.Create<CameraFilterPack_TV_VHS_Rewind>(null);
		R.Camera.Controller.CameraShake(1f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Audio.PlayEffect(382, new Vector3?(this.text_Main.transform.position));
		base.StartCoroutine(this.HideText(this.text_Main));
		base.StartCoroutine(this.HideText(this.text_Yes));
		base.StartCoroutine(this.HideText(this.text_No));
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		CameraFilterUtils.Remove<CameraFilterPack_TV_VHS_Rewind>(null);
		yield return t8;
		YieldInstruction t9 = R.Audio.PlayVoiceOver("e10t23", null, false);
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s6, 1f));
		yield return new WaitForSeconds(1f);
		yield return base.StartCoroutine(this.HideText(this.text_Main));
		yield return t9;
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e10s7, 0.1f);
		YieldInstruction t10 = R.Audio.PlayVoiceOver("e10t9", null, false);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 2f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s10, 2f));
		this.text_Yes.characterSize = 0.07f;
		this.text_No.characterSize = 0.07f;
		base.StartCoroutine(this.ShowTextWithContent(this.text_Yes, ScriptLocalization.Story.e10s9, 1f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_No, ScriptLocalization.Story.e10s9, 1f));
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(ScriptLocalization.Story.e10s9, ScriptLocalization.Story.e10s9);
		yield return new WaitForSeconds(1.2f);
		yield return t10;
		yield return R.Audio.PlayVoiceOver("e10t10", null, false);
		yield return new WaitForSeconds(10f);
		yield return R.Audio.PlayVoiceOver("e10t11", null, false);
		yield return new WaitForSeconds(15f);
		yield return R.Audio.PlayVoiceOver("e10t12", null, false);
		yield return new WaitForSeconds(30f);
		yield return R.Audio.PlayVoiceOver("e10t13", null, false);
		yield return new WaitForSeconds(20f);
		yield return R.Audio.PlayVoiceOver("e10t14", null, false);
		this.P6Break = false;
		base.StartCoroutine(this.P6_2Coroutine());
		yield break;
	}

	private IEnumerator P6_2Coroutine()
	{
		yield return new WaitForSeconds(20f);
		yield return R.Audio.PlayVoiceOver("e10t15", null, false);
		yield return new WaitForSeconds(10f);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Audio.PlayVoiceOver("e10t16", null, false);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e10s11, 0.1f);
		yield return R.Audio.PlayVoiceOver("e10t17", null, false);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e10s12, 0.1f);
		yield return R.Audio.PlayVoiceOver("e10t18", null, false);
		YieldInstruction t19 = R.Audio.PlayVoiceOver("e10t19", null, false);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 2f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return t19;
		CameraFilterUtils.Create<CameraFilterPack_TV_CompressionFX>(null);
		yield return base.StartCoroutine(this.ShowTextWithContent(this.text_Main, ScriptLocalization.Story.e10s13, 1f));
		yield return R.Audio.PlayVoiceOver("e10t20", null, false);
		base.StartCoroutine(this.ShowTextWithContent(this.text_Yes, ScriptLocalization.Story.e10s13, 1f));
		base.StartCoroutine(this.ShowTextWithContent(this.text_No, ScriptLocalization.Story.e10s13, 1f));
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(ScriptLocalization.Story.e10s13, ScriptLocalization.Story.e10s13);
		yield return R.Audio.PlayVoiceOver("e10t21", null, false);
		yield return R.Audio.PlayVoiceOver("e10t22", null, false);
		this.pig.SetActive(true);
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.Alpha = 1f;
		CameraFilterUtils.Remove<CameraFilterPack_TV_CompressionFX>(null);
		UnityEngine.Object.Destroy(this.pig);
		R.Audio.StopBGM(true);
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(1, WorldTime.FrozenArgs.FrozenType.Player, false);
		yield return new WaitForSeconds(5f);
		R.Trophy.AwardTrophy(13);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Print(ScriptLocalization.Story.e10s15, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		SingletonMono<WorldTime>.Instance.FrozenResume();
		R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private IEnumerator P6BreakCoroutine()
	{
		while (this.P6Break)
		{
			yield return null;
			if (Core.Input.Game.L2.OnPressed || Core.Input.Game.R2.OnPressed)
			{
				base.StopCoroutine("P6_1Coroutine");
				base.StartCoroutine(this.P8Coroutine());
				break;
			}
		}
		yield break;
	}

	private IEnumerator P7Coroutine()
	{
		R.Audio.PlayEffect(382, new Vector3?(this.text_Main.transform.position));
		base.StartCoroutine(this.HideText(this.text_Main));
		base.StartCoroutine(this.HideText(this.text_Yes));
		base.StartCoroutine(this.HideText(this.text_No));
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		yield return R.Audio.PlayVoiceOver("e10t24", null, false);
		this.gateCollider.enabled = true;
		this._gateL.DisAppear();
		this._gateR.DisAppear();
		this._airWall1.SetActive(false);
		this._airWall2.SetActive(false);
		this.gate2.openType = SceneGate.OpenType.Left;
		yield break;
	}

	private IEnumerator P8Coroutine()
	{
		R.Audio.PlayEffect(382, new Vector3?(this.text_Main.transform.position));
		base.StartCoroutine(this.HideText(this.text_Main));
		base.StartCoroutine(this.HideText(this.text_Yes));
		base.StartCoroutine(this.HideText(this.text_No));
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		yield return R.Audio.PlayVoiceOver("e10t25", null, false);
		this.gateCollider.enabled = true;
		this._gateL.DisAppear();
		this._gateR.DisAppear();
		this._airWall1.SetActive(false);
		this._airWall2.SetActive(false);
		this.gate2.openType = SceneGate.OpenType.Left;
		yield break;
	}

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	[SerializeField]
	private BoxCollider2D gateCollider;

	[SerializeField]
	private TextMesh text_Main;

	[SerializeField]
	private TextMesh text_Yes;

	[SerializeField]
	private TextMesh text_No;

	[SerializeField]
	private GameObject pig;

	[SerializeField]
	private BattleZoneGate _gateL;

	[SerializeField]
	private BattleZoneGate _gateR;

	[SerializeField]
	private GameObject _airWall1;

	[SerializeField]
	private GameObject _airWall2;

	private bool P6Break;

	private bool _trigger;
}
