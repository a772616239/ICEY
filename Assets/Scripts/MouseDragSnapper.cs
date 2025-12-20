using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class MouseDragSnapper : MonoBehaviour
{
	private void Awake()
	{
		if (Application.isPlaying)
		{
			if (this.removeRigidbody)
			{
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody2D>());
			}
			if (this.removeCollider)
			{
				UnityEngine.Object.Destroy(base.GetComponent<Collider2D>());
			}
			UnityEngine.Object.Destroy(this);
			return;
		}
		Collider2D component = base.gameObject.GetComponent<Collider2D>();
		if (component == null)
		{
			this.removeCollider = true;
			base.gameObject.AddComponent<BoxCollider2D>();
		}
		Rigidbody2D component2 = base.gameObject.GetComponent<Rigidbody2D>();
		if (component2 == null)
		{
			this.removeRigidbody = true;
			base.gameObject.AddComponent<Rigidbody2D>();
		}
	}

	private void RePosition(Vector2 dir, SpriteRenderer sprite)
	{
		float x = sprite.bounds.size.x;
		float y = sprite.bounds.size.y;
		float x2 = this.mySprite.bounds.size.x;
		float y2 = this.mySprite.bounds.size.y;
		Vector2 vector = sprite.transform.position;
		Vector2 vector2 = new Vector2(x / 2f, y / 2f);
		Vector2 normalized = vector2.normalized;
		Vector2 vector3 = new Vector2(x / 2f, -y / 2f);
		Vector2 normalized2 = vector3.normalized;
		Vector2 vector4 = new Vector2(-x / 2f, y / 2f);
		Vector2 normalized3 = vector4.normalized;
		Vector2 vector5 = new Vector2(-x / 2f, -y / 2f);
		Vector2 normalized4 = vector5.normalized;
		Vector2 right = Vector2.right;
		float num = Vector2.Angle(right, dir);
		float num2 = Vector2.Angle(right, normalized);
		float num3 = Vector2.Angle(right, normalized2);
		float num4 = Vector2.Angle(right, normalized3);
		float num5 = Vector2.Angle(right, normalized4);
		if (dir.y > 0f)
		{
			if (num < num2)
			{
				base.transform.position = new Vector2(vector.x + x / 2f + x2 / 2f, vector.y);
			}
			else if (num < num4)
			{
				base.transform.position = new Vector2(vector.x, vector.y + y / 2f + y2 / 2f);
			}
			else
			{
				base.transform.position = new Vector2(vector.x - x / 2f - x2 / 2f, vector.y);
			}
		}
		else if (num < num3)
		{
			base.transform.position = new Vector2(vector.x + x / 2f + x2 / 2f, vector.y);
		}
		else if (num < num5)
		{
			base.transform.position = new Vector2(vector.x, vector.y - y / 2f - y2 / 2f);
		}
		else
		{
			base.transform.position = new Vector2(vector.x - x / 2f - x2 / 2f, vector.y);
		}
	}

	private bool Collide(Vector2 origin, Vector2 dir, float dis)
	{
		RaycastHit2D[] array = Physics2D.RaycastAll(origin, dir, dis);
		RaycastHit2D[] array2 = array;
		int i = 0;
		while (i < array2.Length)
		{
			RaycastHit2D raycastHit2D = array2[i];
			if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject != base.gameObject)
			{
				Vector2 b = raycastHit2D.collider.transform.position;
				Vector2 a = base.transform.position;
				SpriteRenderer component = raycastHit2D.collider.gameObject.GetComponent<SpriteRenderer>();
				if (component == null)
				{
					return false;
				}
				this.RePosition((a - b).normalized, component);
				return true;
			}
			else
			{
				i++;
			}
		}
		return false;
	}

	private void Update()
	{
		this.mySprite = base.gameObject.GetComponent<SpriteRenderer>();
		Vector2 vector = base.transform.position;
		float x = this.mySprite.bounds.size.x;
		float y = this.mySprite.bounds.size.y;
		bool flag = this.Collide(new Vector2(vector.x + x / 2f, vector.y + y / 2f), Vector2.up, 5f);
		if (flag)
		{
			return;
		}
		flag = this.Collide(new Vector2(vector.x + x / 2f, vector.y + y / 2f), Vector2.right, 5f);
		if (flag)
		{
			return;
		}
		flag = this.Collide(new Vector2(vector.x + x / 2f, vector.y - y / 2f), -Vector2.up, 5f);
		if (flag)
		{
			return;
		}
		flag = this.Collide(new Vector2(vector.x + x / 2f, vector.y - y / 2f), Vector2.right, 5f);
		if (flag)
		{
			return;
		}
		flag = this.Collide(new Vector2(vector.x - x / 2f, vector.y - y / 2f), -Vector2.up, 5f);
		if (flag)
		{
			return;
		}
		flag = this.Collide(new Vector2(vector.x - x / 2f, vector.y - y / 2f), -Vector2.right, 5f);
		if (flag)
		{
			return;
		}
		flag = this.Collide(new Vector2(vector.x - x / 2f, vector.y + y / 2f), Vector2.up, 5f);
		if (flag)
		{
			return;
		}
		this.Collide(new Vector2(vector.x - x / 2f, vector.y + y / 2f), -Vector2.right, 5f);
	}

	public bool removeRigidbody;

	public bool removeCollider;

	private SpriteRenderer mySprite;
}
