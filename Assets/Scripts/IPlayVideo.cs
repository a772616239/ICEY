using System;
using UnityEngine;

public interface IPlayVideo
{
	Renderer VideoRenderer { get; }

	Coroutine Play(string moviePath, string moiveName, bool isLooping = false);

	void Pause();

	void Resume();

	void Stop();

	void Destroy();
}
