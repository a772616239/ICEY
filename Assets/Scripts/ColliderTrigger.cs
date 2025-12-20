using System;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (this.handler != null)
		{
			this.handler._onTriggerEnter2D(coll);
		}
	}

	private void OnTriggerExit2D(Collider2D coll)
	{
		if (this.handler != null)
		{
			this.handler._onTriggerExit2D(coll);
		}
	}

	public ColliderTriggerHandler handler;
}
