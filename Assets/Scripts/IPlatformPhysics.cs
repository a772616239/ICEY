using System;
using UnityEngine;

public interface IPlatformPhysics
{
	Vector2 velocity { get; set; }

	Vector2 position { get; set; }

	bool isOnGround { get; }
}
