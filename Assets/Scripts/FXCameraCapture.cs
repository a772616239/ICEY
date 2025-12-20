using System;
using UnityEngine;

public class FXCameraCapture : BaseBehaviour
{
	private void Update()
	{
	}

	public void OnEnable()
	{
		if (this.texture == null)
		{
			Log.Error("纹理为空!");
			return;
		}
		if (!Application.isEditor && this.texture != null)
		{
			this.texture.Release();
			this.texture.width = Screen.width;
			this.texture.height = Screen.height;
			if (!this.texture.IsCreated())
			{
				this.texture.Create();
			}
		}
		this.origCamera = base.GetComponent<Camera>();
		this.capCamera = new GameObject("CaptureTexture", new Type[]
		{
			typeof(Camera)
		}).GetComponent<Camera>();
		UnityEngine.Object.DontDestroyOnLoad(this.capCamera);
	}

	public void OnDisable()
	{
		this.origCamera.targetTexture = null;
		UnityEngine.Object.DestroyObject(this.capCamera);
	}

	public void OnPreRender()
	{
		this.capCamera.CopyFrom(this.origCamera);
		this.capCamera.cullingMask = this.cullMaskLayerMask;
		this.capCamera.targetTexture = this.texture;
		this.capCamera.gameObject.SetActive(false);
		this.capCamera.Render();
	}

	private Camera origCamera;

	private Camera capCamera;

	[HideInInspector]
	public RenderTexture texture;

	public LayerMask cullMaskLayerMask;
}
