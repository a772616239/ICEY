using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/EXTRA/SHOWFPS")]
public class CameraFilterPack_EXTRA_SHOWFPS : MonoBehaviour
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
		this.FPS = 0;
		base.StartCoroutine(this.FPSX());
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue = this.Size;
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue2 = this.FPS;
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue3 = this.Value3;
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue4 = this.Value4;
		this.SCShader = Shader.Find("CameraFilterPack/EXTRA_SHOWFPS");
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (this.SCShader != null)
		{
			this.TimeX += Time.deltaTime;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.Size);
			this.material.SetFloat("_Value2", (float)this.FPS);
			this.material.SetFloat("_Value3", this.Value3);
			this.material.SetFloat("_Value4", this.Value4);
			this.material.SetVector("_ScreenResolution", new Vector4((float)sourceTexture.width, (float)sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, this.material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue = this.Size;
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue2 = this.FPS;
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue3 = this.Value3;
		CameraFilterPack_EXTRA_SHOWFPS.ChangeValue4 = this.Value4;
	}

	private IEnumerator FPSX()
	{
		for (;;)
		{
			float fps = this.accum / (float)this.frames;
			this.FPS = (int)fps;
			CameraFilterPack_EXTRA_SHOWFPS.ChangeValue2 = (int)fps;
			this.accum = 0f;
			this.frames = 0;
			yield return new WaitForSeconds(this.frequency);
		}
		yield break;
	}

	private void Update()
	{
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_EXTRA_SHOWFPS.ChangeValue;
			this.FPS = CameraFilterPack_EXTRA_SHOWFPS.ChangeValue2;
			this.Value3 = CameraFilterPack_EXTRA_SHOWFPS.ChangeValue3;
			this.Value4 = CameraFilterPack_EXTRA_SHOWFPS.ChangeValue4;
		}
		else
		{
			this.FPS = 9999;
		}
	}

	private void OnDisable()
	{
		if (this.SCMaterial)
		{
			UnityEngine.Object.DestroyImmediate(this.SCMaterial);
		}
	}

	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(8f, 42f)]
	public float Size = 12f;

	[Range(0f, 100f)]
	private int FPS = 1;

	[Range(0f, 10f)]
	private float Value3 = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	private float deltaTime;

	public static float ChangeValue;

	public static int ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	private float accum;

	private int frames;

	public float frequency = 0.5f;
}
