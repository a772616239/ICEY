using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformDisable : MonoBehaviour
{
	private void Start()
	{
		this.m_collider = base.GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		Vector2 size = this.m_collider.size;
		Vector3 v = base.transform.position + new Vector3(-size.x / 2f, size.y / 2f, 0f);
		Vector3 v2 = base.transform.position + new Vector3(size.x / 2f, size.y, 0f);
		Collider2D exists = Physics2D.OverlapArea(v, v2, LayerManager.PlayerMask);
		if (exists && !this.playerHit)
		{
			this.playerHit = true;
			if (!base.IsInvoking("DisableCollider"))
			{
				base.Invoke("DisableCollider", this.existTime);
			}
		}
	}

	private void DisableCollider()
	{
		base.GetComponent<SpriteRenderer>().sprite = null;
		this.m_collider.enabled = false;
	}

	private BoxCollider2D m_collider;

	public float existTime;

	private bool playerHit;
}
