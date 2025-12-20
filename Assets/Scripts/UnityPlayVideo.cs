using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class UnityPlayVideo : MonoBehaviour, IPlayVideo
{
	public Renderer VideoRenderer { get; private set; }

	public static UnityPlayVideo CreateVideo(Renderer videoRenderer)
	{
		UnityPlayVideo unityPlayVideo = R.Camera.AddComponent<UnityPlayVideo>();
		unityPlayVideo._playVideo = videoRenderer.gameObject.AddComponent<VideoPlayer>();
		unityPlayVideo._playVideo.renderMode = VideoRenderMode.MaterialOverride;
		unityPlayVideo._playVideo.source = VideoSource.Url;
		unityPlayVideo._playVideo.targetMaterialRenderer = videoRenderer;
		unityPlayVideo._playVideo.targetMaterialRenderer.material = Asset.LoadFromResources<Material>(string.Empty, "videoMat");
		unityPlayVideo._playVideo.audioOutputMode = VideoAudioOutputMode.None;
		return unityPlayVideo;
	}

	public Coroutine Play(string moviePath, string moiveName, bool isLooping = false)
	{
		return base.StartCoroutine(this.PlayCoroutine(moviePath, moiveName, isLooping));
	}

	private IEnumerator PlayCoroutine(string moviePath, string moiveName, bool isLooping = false)
	{
		this._playVideo.Stop();
		this._playVideo.url = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, moviePath, moiveName);
		this._playVideo.isLooping = isLooping;
		this._playVideo.Prepare();
		while (this._playVideo.isPrepared)
		{
			yield return null;
		}
		this._playVideo.Play();
		while (this._playVideo.isPlaying)
		{
			yield return null;
		}
		yield break;
	}

	public void Pause()
	{
		this._playVideo.Pause();
	}

	public void Resume()
	{
		this._playVideo.Play();
	}

	public void Stop()
	{
		if (this._playVideo != null)
		{
			this._playVideo.Stop();
		}
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(this);
	}

	private VideoPlayer _playVideo;
}
