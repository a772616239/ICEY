using System;
using UnityEngine;

public interface IPhysicsEventHandler
{
	bool _OnCollisionEnter(Collider2D collider, Vector2 normal);

	void _OnCollisionExit(Collider2D collider);

	void _OnCollisionStay(Collider2D collider);

	void _OnTriggerEnter(Collider2D collider);

	void _OnTriggerExit(Collider2D collider);

	void _OnTriggerStay(Collider2D collider);
}
