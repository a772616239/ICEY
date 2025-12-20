using System;
using System.Collections;
using Core;
using DG.Tweening;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class StoryE16P4 : BaseBehaviour
{
	private bool p11
	{
		get
		{
			return RoundStorage.Get("E16_p11", false);
		}
		set
		{
			RoundStorage.Set("E16_p11", value);
		}
	}

	private bool p1112
	{
		get
		{
			return RoundStorage.Get("E16_p1112", false);
		}
		set
		{
			RoundStorage.Set("E16_p1112", value);
		}
	}

	private bool e16log4
	{
		get
		{
			return RoundStorage.Get("E16_log4", false);
		}
		set
		{
			RoundStorage.Set("E16_log4", value);
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.StartShineCoroutine());
		this.gate.openType = SceneGate.OpenType.None;
		if (this.e16log4)
		{
			UnityEngine.Object.Destroy(this.detailUI);
		}
	}

	private IEnumerator StartShineCoroutine()
	{
		this.text.text = ScriptLocalization.Story.e16s1;
		for (;;)
		{
			yield return DOTween.To(delegate(float a)
			{
				this.text.color = this.text.color.SetAlpha(a);
			}, 1f, 0f, 1f).WaitForCompletion();
		}
		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p11)
		{
			this.p11 = true;
			base.StartCoroutine(this.LogEvent());
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p1112 && Core.Input.Game.Search.OnClick && !this.showImg)
		{
			base.StartCoroutine(this.ShowImg());
		}
	}

	private IEnumerator LogEvent()
	{
		yield return R.Audio.PlayVoiceOver("e16t10", null, false);
		yield break;
	}

	private IEnumerator ShowImg()
	{
		this.showImg = true;
		this.e16log4 = true;
		R.Audio.PlayEffect(364, new Vector3?(R.Player.Transform.position));
		yield return R.Ui.Terminal.OpenWithAnim(null);
		UnityEngine.Object.Destroy(this.detailUI);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16log4, 0.1f);
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		base.StartCoroutine(this.HideImg());
		yield break;
	}

	private IEnumerator HideImg()
	{
		this.p1112 = true;
		this.showImg = false;
		R.Audio.PlayEffect(372, new Vector3?(R.Player.Transform.position));
		yield return R.Audio.PlayVoiceOver("e16t11", null, false);
		YieldInstruction t12 = R.Audio.PlayVoiceOver("e16t12", null, false);
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return t12;
		yield break;
	}

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private TextMesh text;

	[SerializeField]
	private GameObject detailUI;

	private bool showImg;
}
