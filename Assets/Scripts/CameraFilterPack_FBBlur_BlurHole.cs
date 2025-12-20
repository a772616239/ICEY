using System;
using UnityEngine;

public class CameraFilterPack_FBBlur_BlurHole : MonoBehaviour
{
	private void Start()
	{
		this._shader = Shader.Find("Hidden/CameraFilterPack_FBBlur_BlurHole");
		this._material = new Material(this._shader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		source.filterMode = FilterMode.Bilinear;
		RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width * 0.125f), (int)((float)source.height * 0.125f), 0, source.format);
		temporary.filterMode = FilterMode.Bilinear;
		Graphics.Blit(source, temporary, this._material, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width * 0.125f), (int)((float)source.height * 0.125f), 0, source.format);
		temporary2.filterMode = FilterMode.Bilinear;
		Graphics.Blit(temporary, temporary2, this._material, 2);
		Graphics.Blit(temporary2, temporary, this._material, 3);
		this._material.SetTexture("_BlurTex", temporary);
		Graphics.Blit(source, destination, this._material, 1);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(temporary);
	}

	private Shader _shader;

	private Material _material;
}
