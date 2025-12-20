using System;
using System.Collections;
using Core;
using I2.Loc;
using UnityEngine;

public class StoryE16P3 : BaseBehaviour
{
	private bool p8
	{
		get
		{
			return RoundStorage.Get("E16_p8", false);
		}
		set
		{
			RoundStorage.Set("E16_p8", value);
		}
	}

	private bool p910
	{
		get
		{
			return RoundStorage.Get("E16_p910", false);
		}
		set
		{
			RoundStorage.Set("E16_p910", value);
		}
	}

	private bool e16log3
	{
		get
		{
			return RoundStorage.Get("E16_log3", false);
		}
		set
		{
			RoundStorage.Set("E16_log3", value);
		}
	}

	private void Start()
	{
		if (this.e16log3)
		{
			UnityEngine.Object.Destroy(this.detailUI);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p8)
		{
			this.p8 = true;
			base.StartCoroutine(this.LogEvent());
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p910 && Core.Input.Game.Search.OnClick && !this.showImg)
		{
			base.StartCoroutine(this.ShowImg());
		}
	}

	private IEnumerator LogEvent()
	{
		yield break;
	}

	private IEnumerator ShowImg()
	{
		this.showImg = true;
		this.e16log3 = true;
		R.Audio.PlayEffect(364, new Vector3?(R.Player.Transform.position));
		R.Audio.PlayVoiceOver("e16t9", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		UnityEngine.Object.Destroy(this.detailUI);
		yield return R.Ui.Terminal.Print(ScriptLocalization.Story.e22s3, 0.1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e22s4, 0.5f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16log3, 0.1f);
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		base.StartCoroutine(this.HideImg());
		yield break;
	}

	private IEnumerator HideImg()
	{
		this.p910 = true;
		this.showImg = false;
		R.Audio.PlayEffect(372, new Vector3?(R.Player.Transform.position));
		yield return R.Ui.Terminal.CloseWithAnim();
		yield break;
	}

	[SerializeField]
	private GameObject detailUI;

	private bool showImg;
}
