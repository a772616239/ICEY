using System;
using System.Collections;
using UnityEngine;

public class StoryE5P7 : BaseBehaviour
{
	private bool IsFirstTime
	{
		get
		{
			return RoundStorage.Get("E5_p7", true);
		}
		set
		{
			RoundStorage.Set("E5_p7", value);
		}
	}

	private void Start()
	{
		this._battleZoneGate.Appear();
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player") || !this.IsFirstTime)
		{
			return;
		}
		this.IsFirstTime = false;
		base.StartCoroutine(this.Sequence0());
	}

	private IEnumerator Sequence0()
	{
		yield return R.Audio.PlayVoiceOver("e5t7", null, false);
		R.Audio.PlayEffect(353, null);
		this._battleZoneGate.DisAppear();
		this._e5p8.SetActive(true);
		yield break;
	}

	[SerializeField]
	private BattleZoneGate _battleZoneGate;

	[SerializeField]
	private GameObject _e5p8;
}
