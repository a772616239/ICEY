using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE3P1 : BaseBehaviour
{
	private static bool IsFirstE3P1
	{
		get
		{
			return RoundStorage.Get("E3_p1", true);
		}
		set
		{
			RoundStorage.Set("E3_p1", value);
		}
	}

	private static bool GuideBoardActive
	{
		get
		{
			return RoundStorage.Get("E3_guideBoardActive", false);
		}
		set
		{
			RoundStorage.Set("E3_guideBoardActive", value);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !this._supplyBoxAction.Opened && StoryE3P1.IsFirstE3P1)
		{
			StoryE3P1.IsFirstE3P1 = false;
			base.StartCoroutine(this.Sequcence0());
			base.StartCoroutine(this.Sequcence1());
		}
	}

	private void Start()
	{
		this._guideArrow.SetActive(StoryE3P1.GuideBoardActive);
	}

	private IEnumerator Sequcence0()
	{
		this._battleZoneGate.Appear();
		yield return R.Audio.PlayVoiceOver("e3t1", null, false);
		yield break;
	}

	private IEnumerator Sequcence1()
	{
		yield return this._supplyBoxAction.WaitForBreak();
		yield return R.Audio.PlayVoiceOver("e3t2", null, false);
		this._battleZoneGate.DisAppear();
		StoryE3P1.GuideBoardActive = true;
		this._guideArrow.SetActive(true);
		yield break;
	}

	[SerializeField]
	private SupplyBoxAction _supplyBoxAction;

	[SerializeField]
	private BattleZoneGate _battleZoneGate;

	[SerializeField]
	private GameObject _guideArrow;
}
