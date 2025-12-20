using System;
using System.Collections;
using Core;
using UnityEngine;

public class StoryE19P1 : BaseBehaviour
{
	private bool p1
	{
		get
		{
			return RoundStorage.Get("E19_P1", false);
		}
		set
		{
			RoundStorage.Set("E19_P1", value);
		}
	}

	private bool e19elevator
	{
		get
		{
			return RoundStorage.Get("E19_Elevator", false);
		}
		set
		{
			RoundStorage.Set("E19_Elevator", value);
		}
	}

	private void Start()
	{
		if (!this.e19elevator)
		{
			base.StartCoroutine(this.MainCoroutine());
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && this.p1 && !this.e19elevator)
		{
			this.detailUI.SetActive(true);
			this.showHint = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && this.p1 && !this.e19elevator)
		{
			this.showHint = false;
		}
	}

	private IEnumerator MainCoroutine()
	{
		while (Mathf.Abs(R.Player.Transform.position.x - this.triggerPoint.position.x) > 1f)
		{
			yield return null;
		}
		yield return R.Audio.PlayVoiceOver("e19t1", null, false);
		yield return new WaitForSeconds(10f);
		yield return R.Audio.PlayVoiceOver("e19t2", null, false);
		yield return new WaitForSeconds(10f);
		yield return R.Audio.PlayVoiceOver("e19t3", null, false);
		yield return new WaitForSeconds(10f);
		this.p1 = true;
		while (!this.showHint || !Core.Input.Game.Search.OnClick)
		{
			yield return null;
		}
		InputSetting.Stop(false);
		R.Audio.StopBGM(true);
		R.Ui.BlackScene.Alpha = 1f;
		yield return R.Audio.PlayVoiceOver("e19t4", null, false);
		R.Audio.PlayEffect(373, null);
		yield return new WaitForSeconds(5f);
		this.e19elevator = true;
		InputSetting.Resume(false);
		this.outGate.Enter(false);
		yield break;
	}

	[SerializeField]
	private GameObject detailUI;

	[SerializeField]
	private Transform triggerPoint;

	private bool showHint;

	[SerializeField]
	private SceneGate outGate;
}
