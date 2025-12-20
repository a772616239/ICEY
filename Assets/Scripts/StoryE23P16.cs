using System;
using System.Collections;
using System.Text;
using Core;
using DG.Tweening;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class StoryE23P16 : BaseBehaviour
{
	private void OnDestroy()
	{
		this._gateL.DisAppear();
		this._gateR.DisAppear();
		this._airWall1.SetActive(false);
		this._airWall2.SetActive(false);
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
	}

	private void Update()
	{
		if (Core.Input.Game.L2.OnPressed)
		{
			StoryE23P16.choice = 2;
		}
		if (Core.Input.Game.R2.OnPressed)
		{
			StoryE23P16.choice = 3;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			base.StartCoroutine(this.Sequence0());
			UnityEngine.Object.Destroy(base.GetComponent<Collider2D>());
		}
	}

	private IEnumerator Sequence0()
	{
		this._gateL.Appear();
		this._gateR.Appear();
		this._airWall1.SetActive(true);
		this._airWall2.SetActive(true);
		string userName = R.Player.UserName;
		if (string.IsNullOrEmpty(userName))
		{
			userName = ScriptLocalization.ui.anonymous;
		}
		this._question.text = ScriptLocalization.Story.e23s8 + "\n" + userName;
		this._yes.text = ScriptLocalization.Story.e23s14;
		this._no.text = ScriptLocalization.Story.e23s15;
		this._question.fontSize = Mathf.Clamp(-4 * Encoding.Default.GetBytes(userName).Length / 2 + 100, 16, 40);
		yield return this.Fade(0f, 1f, true);
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(this._yes.text, this._no.text);
		StoryE23P16.choice = 1;
		while (StoryE23P16.choice < 2)
		{
			yield return new WaitForFixedUpdate();
		}
		yield return this.Fade(1f, 0f, true);
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		int num = StoryE23P16.choice;
		if (num != 2)
		{
			if (num == 3)
			{
				R.Audio.PlayEffect(380, null);
				this._question.text = ScriptLocalization.Story.e23s9;
				this._yes.text = string.Empty;
				this._no.text = string.Empty;
				this._question.fontSize = 40;
				yield return this.Fade(0f, 1f, false);
				yield return this._question.transform.DOShakePosition(2f, 0.1f, 10, 90f, false, true).WaitForCompletion();
				yield return this.Fade(1f, 0f, false);
			}
		}
		else
		{
			R.Audio.PlayEffect(381, null);
		}
		StoryE23P16.choice = 0;
		this._question.text = ScriptLocalization.Story.e23s10;
		this._yes.text = ScriptLocalization.Story.e23s14;
		this._no.text = ScriptLocalization.Story.e23s15;
		this._question.fontSize = 40;
		yield return this.Fade(0f, 1f, true);
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(this._yes.text, this._no.text);
		StoryE23P16.choice = 1;
		while (StoryE23P16.choice < 2)
		{
			yield return new WaitForFixedUpdate();
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		yield return this.Fade(1f, 0f, true);
		int num2 = StoryE23P16.choice;
		if (num2 != 2)
		{
			if (num2 == 3)
			{
				R.Audio.PlayEffect(380, null);
			}
		}
		else
		{
			R.Audio.PlayEffect(381, null);
		}
		StoryE23P16.choice = 0;
		this._question.text = ScriptLocalization.Story.e23s11;
		this._yes.text = ScriptLocalization.Story.e23s14;
		this._no.text = ScriptLocalization.Story.e23s15;
		yield return this.Fade(0f, 1f, true);
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(this._yes.text, this._no.text);
		StoryE23P16.choice = 1;
		while (StoryE23P16.choice < 2)
		{
			yield return new WaitForFixedUpdate();
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		yield return this.Fade(1f, 0f, true);
		this._yes.text = string.Empty;
		this._no.text = string.Empty;
		if (StoryE23P16.choice == 2)
		{
			R.GameData.Difficulty = 1;
			R.GameData.Save(true);
			this._question.text = ScriptLocalization.Story.e23s12;
			yield return this.Fade(0f, 1f, true);
			StoryE23P16.choice = 0;
		}
		else if (StoryE23P16.choice == 3)
		{
			this._question.text = ScriptLocalization.Story.e23s16;
			this._yes.text = ScriptLocalization.Story.e23s14;
			this._no.text = ScriptLocalization.Story.e23s15;
			yield return this.Fade(0f, 1f, true);
			SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(this._yes.text, this._no.text);
			StoryE23P16.choice = 1;
			while (StoryE23P16.choice < 2)
			{
				yield return new WaitForFixedUpdate();
			}
			SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
			yield return this.Fade(1f, 0f, true);
			this._yes.text = string.Empty;
			this._no.text = string.Empty;
			int num3 = StoryE23P16.choice;
			if (num3 != 2)
			{
				if (num3 == 3)
				{
					R.GameData.Difficulty = 0;
					R.GameData.Save(true);
					this._question.text = this.DifficultyStr();
					yield return this.Fade(0f, 1f, true);
				}
			}
			else
			{
				R.GameData.Difficulty = 1;
				R.GameData.Save(true);
				this._question.text = this.DifficultyStr();
				yield return this.Fade(0f, 1f, true);
			}
			StoryE23P16.choice = 0;
		}
		this._gateL.DisAppear();
		this._gateR.DisAppear();
		this._airWall1.SetActive(false);
		this._airWall2.SetActive(false);
		R.Ui.Pause.Enabled = false;
		yield return R.Ui.BlackScene.FadeBlack(3f, false);
		R.Audio.PlayEffect(347, null);
		UIStartController.IsEnterWithVoice = true;
		yield return R.Ui.ShowUI(false);
		this._gate.Enter(false);
		R.Ui.Pause.Enabled = true;
		yield break;
	}

	private string DifficultyStr()
	{
		switch (R.GameData.Difficulty)
		{
		case 0:
			return ScriptLocalization.Story.e23s13;
		case 1:
			return ScriptLocalization.Story.e23s17;
		case 2:
			return ScriptLocalization.Story.e23s12;
		case 3:
			return ScriptLocalization.Story.e23s18;
		default:
			return string.Empty;
		}
	}

	private YieldInstruction Fade(float startValue, float endValue, bool withAudio = true)
	{
		if (withAudio)
		{
			R.Audio.PlayEffect((startValue >= endValue) ? 139 : 2, new Vector3?(base.transform.position));
		}
		int num = 1;
		DOTween.To(delegate(float a)
		{
			this._question.color = this._question.color.SetAlpha(a);
		}, startValue, endValue, (float)num);
		DOTween.To(delegate(float a)
		{
			this._yes.color = this._yes.color.SetAlpha(a);
		}, startValue, endValue, (float)num);
		return DOTween.To(delegate(float a)
		{
			this._no.color = this._no.color.SetAlpha(a);
		}, startValue, endValue, (float)num).WaitForCompletion();
	}

	public static int choice;

	[SerializeField]
	private TextMesh _question;

	[SerializeField]
	private TextMesh _yes;

	[SerializeField]
	private TextMesh _no;

	[SerializeField]
	private SceneGate _gate;

	[SerializeField]
	private BattleZoneGate _gateL;

	[SerializeField]
	private BattleZoneGate _gateR;

	[SerializeField]
	private GameObject _airWall1;

	[SerializeField]
	private GameObject _airWall2;
}
