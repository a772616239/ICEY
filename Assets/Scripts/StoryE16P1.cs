using System;
using System.Collections;
using Core;
using I2.Loc;
using UnityEngine;

public class StoryE16P1 : BaseBehaviour
{
	private bool p1
	{
		get
		{
			return RoundStorage.Get("E16_p1", false);
		}
		set
		{
			RoundStorage.Set("E16_p1", value);
		}
	}

	private bool p2
	{
		get
		{
			return RoundStorage.Get("E16_p2", false);
		}
		set
		{
			RoundStorage.Set("E16_p2", value);
		}
	}

	private bool p34
	{
		get
		{
			return RoundStorage.Get("E16_p34", false);
		}
		set
		{
			RoundStorage.Set("E16_p34", value);
		}
	}

	private bool e16log1
	{
		get
		{
			return RoundStorage.Get("E16_Log1", false);
		}
		set
		{
			RoundStorage.Set("E16_Log1", value);
		}
	}

	private void Start()
	{
		if (!this.p1)
		{
			base.StartCoroutine(this.P1Coroutine());
		}
		if (this.e16log1)
		{
			UnityEngine.Object.Destroy(this.detailUI);
		}
	}

	private IEnumerator P1Coroutine()
	{
		this.p1 = true;
		yield return R.Audio.PlayVoiceOver("e16t1", null, false);
		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p2)
		{
			this.p2 = true;
			base.StartCoroutine(this.LogEvent());
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !this.p34 && Core.Input.Game.Search.OnClick && !this.showImg)
		{
			base.StartCoroutine(this.ShowImg());
		}
	}

	private IEnumerator LogEvent()
	{
		yield return R.Audio.PlayVoiceOver("e16t2", null, false);
		yield break;
	}

	private IEnumerator ShowImg()
	{
		this.showImg = true;
		R.Audio.PlayEffect(364, new Vector3?(R.Player.Transform.position));
		this.e16log1 = true;
		R.Audio.PlayVoiceOver("e16t3", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		UnityEngine.Object.Destroy(this.detailUI);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e16log1, 0.1f);
		while (!Core.Input.AnyKey.OnClick)
		{
			yield return null;
		}
		base.StartCoroutine(this.HideImg());
		yield break;
	}

	private IEnumerator HideImg()
	{
		this.p34 = true;
		this.showImg = false;
		R.Audio.PlayEffect(372, new Vector3?(R.Player.Transform.position));
		YieldInstruction t4 = R.Audio.PlayVoiceOver("e16t4", null, false);
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return t4;
		yield break;
	}

	private bool showImg;

	[SerializeField]
	private GameObject detailUI;
}
