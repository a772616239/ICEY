using System;
using UnityEngine;

namespace Distort
{
	public class FXCameraDistort : MonoBehaviour
	{
		private void Start()
		{
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
		}

		private void Update()
		{
		}

		private void OnDisable()
		{
			this.mainCamera.targetTexture = null;
			this.mainCamera = null;
			if (this.tempTexture)
			{
				RenderTexture.ReleaseTemporary(this.texture);
			}
		}

		private void OnEnable()
		{
			if (this.mainCamera == null)
			{
				this.mainCamera = Camera.main.GetComponent<Camera>();
			}
			if (this.texture == null)
			{
				this.texture = RenderTexture.GetTemporary(this.mainCamera.pixelWidth, this.mainCamera.pixelHeight);
				this.tempTexture = true;
			}
			if (this.texture != null && !this.texture.IsCreated())
			{
				this.texture.Create();
			}
			this.screenPad.localScale = new Vector3(this.mainCamera.aspect, 1f, 1f) * base.GetComponent<Camera>().orthographicSize * 2f;
			this.mainCamera.targetTexture = this.texture;
			this.screenPad.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", this.texture);
		}

		public Transform screenPad;

		private Camera mainCamera;

		public RenderTexture texture;

		private bool tempTexture;
	}
}
