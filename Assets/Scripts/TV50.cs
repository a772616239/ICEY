using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class TV50 : BaseBehaviour
{
	private Material material
	{
		get
		{
			if (this.SCMaterial == null)
			{
				this.SCMaterial = new Material(this.SCShader);
				this.SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.SCMaterial;
		}
	}

	private void Start()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.size = this.meshRenderer.bounds.size;
		this.SCShader = Shader.Find("CameraFilterPack/FB_TV_50");
		if (this.meshRenderer.sharedMaterial == null)
		{
			this.meshRenderer.sharedMaterial = this.material;
		}
	}

	public void LateUpdate()
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetVector("_ScreenResolution", new Vector4(this.size.x, this.size.y, 0f, 0f));
		}
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	public Shader SCShader;

	private MeshRenderer meshRenderer;

	private Vector3 size;

	private float TimeX = 1f;

	private Material SCMaterial;
}
