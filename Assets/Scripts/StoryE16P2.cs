using System;
using System.Collections;
using Core;
using I2.Loc;
using UnityEngine;

public class StoryE16P2 : BaseBehaviour
{
	private bool p5
	{
		get
		{
			return RoundStorage.Get("E16_p5", false);
		}
		set
		{
			RoundStorage.Set("E16_p5", value);
		}
	}

	private bool p67
	{
		get
		{
			return RoundStorage.Get("E16_p56", false);
		}
		set
		{
			RoundStorage.Set("E16_p56", value);
		}
	}

	private bool e16log2
	{
		get
		{
			return RoundStorage.Get("E16_log2", false);
		}
		set
		{
			RoundStorage.Set("E16_log2", value);
		}
	}

	private void Start()
	{
		if (this.e16log2)
		{
			UnityEngine.Object.Destroy(this.detailUI);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p5)
		{
			this.p5 = true;
			base.StartCoroutine(this.LogEvent());
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p67 && Core.Input.Game.Search.OnClick && !this.showImg)
		{
			base.StartCoroutine(this.ShowImg());
		}
	}

	private IEnumerator LogEvent()
	{
		yield return R.Audio.PlayVoiceOver("e16t5", null, false);
		yield break;
	}

	private IEnumerator ShowImg()
	{
		this.showImg = true;
		this.e16log2 = true;
		R.Audio.PlayEffect(364, new Vector3?(R.Player.Transform.position));
		R.Audio.PlayVoiceOver("e16t6", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		UnityEngine.Object.Destroy(this.detailUI);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16log2, 0.1f);
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		base.StartCoroutine(this.HideImg());
		yield break;
	}

	private IEnumerator HideImg()
	{
		this.p67 = true;
		this.showImg = false;
		R.Audio.PlayEffect(372, new Vector3?(R.Player.Transform.position));
		YieldInstruction t7 = R.Audio.PlayVoiceOver("e16t7", null, false);
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return t7;
		yield break;
	}

	private bool showImg;

	[SerializeField]
	private GameObject detailUI;
}
