using System;
using System.Collections;
using UnityEngine;

public class PS4PlayVideo : BaseBehaviour, IPlayVideo
{
	public Renderer VideoRenderer { get; private set; }

	public static PS4PlayVideo CreateVideo(Renderer videoRenderer)
	{
		PS4PlayVideo ps4PlayVideo = R.Camera.AddComponent<PS4PlayVideo>();
		ps4PlayVideo.VideoRenderer = videoRenderer;
		return ps4PlayVideo;
	}

	public static PS4PlayVideo CreateFullScreenVideo(Renderer videoRenderer = null)
	{
		PS4PlayVideo ps4PlayVideo = R.Ui.CameraGO.AddComponent<PS4PlayVideo>();
		if (videoRenderer == null)
		{
			GameObject original = Asset.LoadFromResources<GameObject>("Prefab/Scene/VideoPlane", "FullScreenVideoPlane");
			ps4PlayVideo.VideoRenderer = UnityEngine.Object.Instantiate<GameObject>(original).GetComponent<Renderer>();
		}
		else
		{
			ps4PlayVideo.VideoRenderer = videoRenderer;
		}
		return ps4PlayVideo;
	}

	public Coroutine Play(string moviePath, string moiveName, bool isLooping = false)
	{
		return base.StartCoroutine(this.PlayCoroutine(moviePath, moiveName, isLooping));
	}

	private IEnumerator PlayCoroutine(string moviePath, string moiveName, bool isLooping = false)
	{
		yield return null;
		yield break;
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
		UnityEngine.Object.Destroy(this);
	}
}
