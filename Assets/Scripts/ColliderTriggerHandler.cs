using System;
using UnityEngine;

public class ColliderTriggerHandler : MonoBehaviour
{
	public virtual void _onTriggerEnter2D(Collider2D coll)
	{
	}

	public virtual void _onTriggerExit2D(Collider2D coll)
	{
	}

	public bool entered;
}
