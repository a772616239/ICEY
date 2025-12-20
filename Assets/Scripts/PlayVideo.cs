using System;
using GameWorld;
using UnityEngine;

public class PlayVideo
{
	private PlayVideo()
	{
		EventManager.RegisterEvent<PauseGameArgs>("PauseGame", new EventManager.FBEventHandler<PauseGameArgs>(this.OnPauseGame), EventManager.ListenerQueue.Game);
	}

	public static PlayVideo CreateVideo(Renderer videoRenderer)
	{
		return new PlayVideo
		{
			_playVideo = AVPlayVideo.CreateVideo(videoRenderer)
		};
	}

	public Coroutine Play(string moviePath, string moiveName, bool isLooping = false)
	{
		return this._playVideo.Play(moviePath, moiveName, isLooping);
	}

	public void Pause()
	{
		this._playVideo.Pause();
	}

	public void Resume()
	{
		this._playVideo.Resume();
	}

	public void Stop()
	{
		this._playVideo.Stop();
	}

	public void Dispose()
	{
		EventManager.UnregisterEvent<PauseGameArgs>("PauseGame", new EventManager.FBEventHandler<PauseGameArgs>(this.OnPauseGame), EventManager.ListenerQueue.Game);
		this._playVideo.Destroy();
	}

	public Renderer VideoRenderer
	{
		get
		{
			return this._playVideo.VideoRenderer;
		}
	}

	private bool OnPauseGame(string eventdefine, object sender, PauseGameArgs msg)
	{
		PauseGameArgs.State status = msg.Status;
		if (status != PauseGameArgs.State.Pause)
		{
			if (status != PauseGameArgs.State.Resume)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.Resume();
		}
		else
		{
			this.Pause();
		}
		return true;
	}

	private IPlayVideo _playVideo;
}
