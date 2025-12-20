using System;
using UnityEngine;

public class ReduceResolutionAndBlend : MonoBehaviour
{
	public Camera Camera
	{
		get
		{
			return R.Camera.Camera;
		}
	}

	public static void Init()
	{
		ReduceResolutionAndBlend.Instance = R.Camera.GameObject.AddComponent<ReduceResolutionAndBlend>();
		ReduceResolutionAndBlend.Instance.SetResolution(960, 540);
	}

	public void SetResolution(Resolution resolution)
	{
		if (this.GamePlayRenderTexture != null)
		{
			this.GamePlayRenderTexture.Release();
		}
		RenderTexture renderTexture = new RenderTexture(resolution.width, resolution.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Linear)
		{
			filterMode = FilterMode.Bilinear
		};
		this.Camera.targetTexture = renderTexture;
		this.GamePlayRenderTexture = renderTexture;
	}

	public void SetResolution(int width, int height)
	{
		this.SetResolution(new Resolution
		{
			height = height,
			width = width
		});
	}

	private void OnEnable()
	{
		if (this.GamePlayRenderTexture != null)
		{
			this.Camera.targetTexture = this.GamePlayRenderTexture;
		}
	}

	private void OnDisable()
	{
		this.Camera.targetTexture = null;
	}

	public RenderTexture GamePlayRenderTexture;

	public static ReduceResolutionAndBlend Instance;
}
