using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SupplyBoxAction))]
public class SupplyBoxAnimatorControlTools : BaseBehaviour
{
	private void Start()
	{
		this._supplyBox = base.GetComponent<SupplyBoxAction>();
		base.StartCoroutine(this.WaitForBreakCoroutine());
	}

	private IEnumerator WaitForBreakCoroutine()
	{
		yield return this._supplyBox.WaitForBreak();
		this._animator.SetBool("IsSupplyBoxBreak", true);
		yield break;
	}

	[SerializeField]
	private Animator _animator;

	private SupplyBoxAction _supplyBox;
}
