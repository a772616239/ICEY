using System;
using System.Collections;
using UnityEngine;

public class StoryE7P1 : BaseBehaviour
{
	private bool _e7p2
	{
		get
		{
			return RoundStorage.Get("E7_P2", false);
		}
		set
		{
			RoundStorage.Set("E7_P2", value);
		}
	}

	private bool _e7p4
	{
		get
		{
			return RoundStorage.Get("E7_P4", false);
		}
		set
		{
			RoundStorage.Set("E7_P4", value);
		}
	}

	private bool _e7p5
	{
		get
		{
			return RoundStorage.Get("E7_P5", false);
		}
	}

	private bool _e7guide
	{
		get
		{
			return RoundStorage.Get("E7_guide", false);
		}
		set
		{
			RoundStorage.Set("E7_guide", value);
		}
	}

	private void Start()
	{
		this.guide.SetActive(this._e7guide);
		if (!this._e7p2)
		{
			base.StartCoroutine(this.P1Coroutine());
		}
		else
		{
			this._supplyBoxAction.gameObject.SetActive(false);
		}
		if (this._e7p5 && !this._e7p4)
		{
			this._e7p4 = true;
			R.Audio.PlayVoiceOver("e7t5", null, false);
		}
	}

	private IEnumerator P1Coroutine()
	{
		yield return this._supplyBoxAction.WaitForBreak();
		this._e7guide = true;
		this.guide.SetActive(true);
		this._e7p2 = true;
		yield return R.Audio.PlayVoiceOver("e7t2", null, false);
		yield break;
	}

	[SerializeField]
	private SupplyBoxAction _supplyBoxAction;

	[SerializeField]
	private GameObject guide;
}
