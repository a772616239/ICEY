using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerInCollisionAnimatorControlTools : BaseBehaviour
{
	private void Awake()
	{
		this._animator = base.GetComponent<Animator>();
	}

	private void Update()
	{
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		this._animator.SetBool(this._boolName, true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		this._animator.SetBool(this._boolName, false);
	}

	public void OnTriggerStay2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
	}

	[SerializeField]
	private string _boolName = "PlayerInCollision";

	private Animator _animator;
}
