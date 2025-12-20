using System;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

public class AVPlayVideo : MonoBehaviour, IPlayVideo
{
	public Renderer VideoRenderer { get; private set; }

	public static AVPlayVideo CreateVideo(Renderer videoRenderer)
	{
		videoRenderer.sharedMaterial = new Material(Shader.Find("AVProVideo/Unlit/Opaque (texture+color+fog+stereo support)"));
		GameObject gameObject = videoRenderer.gameObject;
		AVPlayVideo avplayVideo = gameObject.AddComponent<AVPlayVideo>();
		avplayVideo.VideoRenderer = videoRenderer;
		avplayVideo._mediaPlayer = gameObject.AddComponent<MediaPlayer>();
		avplayVideo._mediaPlayer.m_AutoOpen = false;
		avplayVideo._applyToMesh = gameObject.AddComponent<ApplyToMesh>();
		avplayVideo._applyToMesh._media = avplayVideo._mediaPlayer;
		avplayVideo._applyToMesh._mesh = videoRenderer;
		avplayVideo._applyToMesh._defaultTexture = Texture2D.blackTexture;
		return avplayVideo;
	}

	public Coroutine Play(string moviePath, string moiveName, bool isLooping = false)
	{
		return base.StartCoroutine(this.PlayCoroutine(moviePath, moiveName, isLooping));
	}

	private IEnumerator PlayCoroutine(string moviePath, string moiveName, bool isLooping = false)
	{
		string videoPath = string.Format("{0}/{1}", moviePath, moiveName);
		if (this._mediaPlayer.Control != null && this._mediaPlayer.Control.IsPlaying())
		{
			if (videoPath == this._mediaPlayer.m_VideoPath)
			{
				yield break;
			}
			this._mediaPlayer.Stop();
		}
		this._mediaPlayer.m_VideoPath = videoPath;
		this._mediaPlayer.m_Loop = isLooping;
		this._mediaPlayer.m_Volume = R.Audio.EffectsVolume / 200f;
		this._mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, videoPath, true);
		if (!isLooping)
		{
			while (!this._mediaPlayer.Control.IsFinished())
			{
				yield return null;
			}
		}
		yield break;
	}

	public void Pause()
	{
		this._mediaPlayer.Pause();
	}

	public void Resume()
	{
		this._mediaPlayer.Play();
	}

	public void Stop()
	{
		if (this._mediaPlayer != null)
		{
			this._mediaPlayer.CloseVideo();
		}
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(this._mediaPlayer);
		UnityEngine.Object.Destroy(this._applyToMesh);
		UnityEngine.Object.Destroy(this);
	}

	private void OnEnable()
	{
	}

	private MediaPlayer _mediaPlayer;

    public ApplyToMesh _applyToMesh;
}
