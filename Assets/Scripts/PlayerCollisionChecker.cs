using System;
using System.Collections;
using UnityEngine;

public class PlayerCollisionChecker : MonoBehaviour
{
	public void RegisterHandler(IPhysicsEventHandler impl)
	{
		this.handler = impl;
	}

	private void Awake()
	{
		this.myTrans = base.transform;
		this.boxPoint1 = new Vector2(this.offset.x - this.size.x / 2f, this.offset.y + this.size.y / 2f);
		this.boxPoint2 = new Vector2(this.offset.x + this.size.x / 2f, this.offset.y - this.size.y / 2f);
	}

	private bool CanReceive(Collider2D collider)
	{
		foreach (string tag in this.receiveTags)
		{
			if (collider.gameObject.CompareTag(tag))
			{
				return true;
			}
		}
		return false;
	}

	private void _FireExit(Collider2D[] colliders, int length)
	{
		DictionaryEntry[] array = new DictionaryEntry[this.currColliderTable.Count];
		this.currColliderTable.CopyTo(array, 0);
		foreach (DictionaryEntry dictionaryEntry in array)
		{
			Collider2D collider2D = (Collider2D)dictionaryEntry.Key;
			bool flag = false;
			for (int j = 0; j < length; j++)
			{
				if (colliders[j] == collider2D)
				{
					flag = true;
					if (collider2D.isTrigger)
					{
						this.handler._OnTriggerStay(collider2D);
					}
					else
					{
						this.handler._OnCollisionStay(collider2D);
					}
				}
			}
			if (!flag)
			{
				if (collider2D == null)
				{
					this.currColliderTable.Remove(collider2D);
				}
				else
				{
					if (collider2D.isTrigger)
					{
						this.handler._OnTriggerExit(collider2D);
					}
					else
					{
						this.handler._OnCollisionExit(collider2D);
					}
					this.currColliderTable.Remove(collider2D);
				}
			}
		}
	}

	private bool IsHitCollider(Vector2 dir, Collider2D collider, ref Vector2 normal)
	{
		Vector2 a = this.myTrans.position;
		if (this.shape == PlayerCollisionChecker.Shape.box)
		{
			RaycastHit2D[] array = Physics2D.BoxCastAll(a + this.offset, this.size, 0f, dir, 1f);
			foreach (RaycastHit2D raycastHit2D in array)
			{
				if (raycastHit2D.collider == collider)
				{
					normal = raycastHit2D.normal;
					return true;
				}
			}
		}
		else if (this.shape == PlayerCollisionChecker.Shape.circle)
		{
			RaycastHit2D[] array3 = Physics2D.CircleCastAll(a + this.offset, this.radius, dir, 1f);
			foreach (RaycastHit2D raycastHit2D2 in array3)
			{
				if (raycastHit2D2.collider == collider)
				{
					normal = raycastHit2D2.normal;
					return true;
				}
			}
		}
		return false;
	}

	private void _CheckCollison()
	{
		Vector2 a = this.myTrans.position;
		int num = 0;
		if (this.shape == PlayerCollisionChecker.Shape.circle)
		{
			num = Physics2D.OverlapCircleNonAlloc(a + this.offset, this.radius, this.colliders);
		}
		else if (this.shape == PlayerCollisionChecker.Shape.box)
		{
			num = Physics2D.OverlapAreaNonAlloc(a + this.boxPoint1, a + this.boxPoint2, this.colliders);
		}
		this._FireExit(this.colliders, num);
		for (int i = 0; i < num; i++)
		{
			Collider2D collider2D = this.colliders[i];
			if (this.currColliderTable[collider2D] == null && this.CanReceive(collider2D))
			{
				this.currColliderTable.Add(collider2D, collider2D);
				if (!collider2D.isTrigger)
				{
					Vector2 zero = Vector2.zero;
					if (!this.IsHitCollider(this.right, collider2D, ref zero))
					{
						if (!this.IsHitCollider(this.left, collider2D, ref zero))
						{
							if (!this.IsHitCollider(this.up, collider2D, ref zero))
							{
								if (this.IsHitCollider(this.down, collider2D, ref zero))
								{
								}
							}
						}
					}
					if (!this.handler._OnCollisionEnter(collider2D, zero))
					{
						this.currColliderTable.Remove(collider2D);
					}
				}
				else
				{
					this.handler._OnTriggerEnter(collider2D);
				}
			}
		}
	}

	private void Update()
	{
		if (this.handler == null)
		{
			return;
		}
		this._CheckCollison();
	}

	private void OnGUI()
	{
		if (this.debug)
		{
			Vector2 a = this.myTrans.position;
			a = Camera.main.WorldToViewportPoint(a + this.offset);
			a.y = 1f - a.y;
			float num = (float)Screen.width / 1920f;
			Vector2 center = new Vector2((float)Screen.width * a.x, (float)Screen.height * a.y);
			if (this.shape == PlayerCollisionChecker.Shape.box)
			{
				Vector2 vector = new Vector2(center.x - this.size.x * num / 2f, center.y + this.size.y * num / 2f);
				Vector2 vector2 = new Vector2(center.x + this.size.x * num / 2f, center.y + this.size.y * num / 2f);
				Vector2 vector3 = new Vector2(center.x + this.size.x * num / 2f, center.y - this.size.y * num / 2f);
				Vector2 vector4 = new Vector2(center.x - this.size.x * num / 2f, center.y - this.size.y * num / 2f);
				Drawing.DrawLine(vector, vector2, this.debugColor, 2f, false);
				Drawing.DrawLine(vector2, vector3, this.debugColor, 2f, false);
				Drawing.DrawLine(vector3, vector4, this.debugColor, 2f, false);
				Drawing.DrawLine(vector4, vector, this.debugColor, 2f, false);
			}
			else if (this.shape == PlayerCollisionChecker.Shape.circle)
			{
				Drawing.DrawCircle(center, (int)(this.radius * num), this.debugColor, 2f, 10);
			}
		}
	}

	public float radius;

	public Vector2 size;

	public Vector2 offset;

	public PlayerCollisionChecker.Shape shape;

	public string[] receiveTags;

	public bool debug;

	public Color debugColor = Color.blue;

	private Transform myTrans;

	private Vector2 boxPoint1;

	private Vector2 boxPoint2;

	private Collider2D[] colliders = new Collider2D[10];

	private Hashtable currColliderTable = new Hashtable();

	private IPhysicsEventHandler handler;

	private Vector2 right = Vector2.right;

	private Vector2 left = -Vector2.right;

	private Vector2 up = Vector2.up;

	private Vector2 down = -Vector2.up;

	public enum Shape
	{
		circle,
		box
	}
}
