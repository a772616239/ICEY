using System;
using UnityEngine;

public sealed class FakePlayVideo : IPlayVideo
{
	public Renderer VideoRenderer { get; private set; }

	public Coroutine Play(string moviePath, string moiveName, bool isLooping = false)
	{
		return null;
	}

	public void Pause()
	{
	}

	public void Resume()
	{
	}

	public void Stop()
	{
	}

	public void Destroy()
	{
	}
}
