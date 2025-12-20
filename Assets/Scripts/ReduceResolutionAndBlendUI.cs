using System;
using UnityEngine;

public class ReduceResolutionAndBlendUI : MonoBehaviour
{
	public static void Init()
	{
		ReduceResolutionAndBlendUI.Instance = R.Ui.CameraGO.AddComponent<ReduceResolutionAndBlendUI>();
	}

	private void OnPreRender()
	{
		RenderTexture gamePlayRenderTexture = ReduceResolutionAndBlend.Instance.GamePlayRenderTexture;
		if (gamePlayRenderTexture)
		{
			Graphics.Blit(gamePlayRenderTexture, null, this._material);
		}
	}

	private void Awake()
	{
		if (!this.BlendShader)
		{
			this.BlendShader = Shader.Find("Hidden/BlendTwoRenderTexture");
		}
		if (!this._material)
		{
			this._material = new Material(this.BlendShader);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public Shader BlendShader;

	private Material _material;

	public static ReduceResolutionAndBlendUI Instance;
}
