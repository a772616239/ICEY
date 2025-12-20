using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE7 : BaseBehaviour
{
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !this._supplyBoxAction.Opened)
		{
			base.StartCoroutine(this.Sequcence0());
		}
	}

	private IEnumerator Sequcence0()
	{
		yield return R.Audio.PlayVoiceOver("e7t1", null, false);
		yield return this._supplyBoxAction.WaitForBreak();
		yield return R.Audio.PlayVoiceOver("e7t2", null, false);
		yield break;
	}

	[SerializeField]
	private SupplyBoxAction _supplyBoxAction;
}
