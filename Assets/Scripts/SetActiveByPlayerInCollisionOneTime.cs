using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SetActiveByPlayerInCollisionOneTime : BaseBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && R.Enemy.Count == 0 && this.firstTime)
		{
			this.firstTime = false;
			((this.chapt == null) ? base.gameObject : this.chapt).SetActive(true);
		}
	}

	[Header("需要设置的 GameObject (默认为自身)")]
	[SerializeField]
	private GameObject chapt;

	private bool firstTime = true;
}
