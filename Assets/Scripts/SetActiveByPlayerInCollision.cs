using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SetActiveByPlayerInCollision : BaseBehaviour
{
	private GameObject GameObject
	{
		get
		{
			return this._gameObject ?? base.gameObject;
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		this.GameObject.SetActive(!this._out);
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		this.GameObject.SetActive(this._out);
	}

	[Header("需要设置的 GameObject (默认为自身)")]
	[SerializeField]
	private GameObject _gameObject;

	[Header("离开碰撞区域激活")]
	[SerializeField]
	private bool _out;
}
