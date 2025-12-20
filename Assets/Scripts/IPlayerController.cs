using System;
using UnityEngine;

public interface IPlayerController
{
	Vector2 Velocity { get; }

	void SetRespawnPoint(Vector2 pos);

	void PlayAnimation(string name);

	GameObject GameObject { get; }

	Transform Transform { get; }
}
