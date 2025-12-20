using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/Drawing/Manga_Flash")]
public class CameraFilterPack_Drawing_Manga_Flash : MonoBehaviour
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
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue = this.Size;
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue2 = this.Speed;
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue3 = this.PosX;
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue4 = this.PosY;
		this.SCShader = Shader.Find("CameraFilterPack/Drawing_Manga_Flash");
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
			this.material.SetFloat("_Value2", (float)this.Speed);
			this.material.SetFloat("_Value3", this.PosX);
			this.material.SetFloat("_Value4", this.PosY);
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
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue = this.Size;
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue2 = this.Speed;
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue3 = this.PosX;
		CameraFilterPack_Drawing_Manga_Flash.ChangeValue4 = this.PosY;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.Size = CameraFilterPack_Drawing_Manga_Flash.ChangeValue;
			this.Speed = CameraFilterPack_Drawing_Manga_Flash.ChangeValue2;
			this.PosX = CameraFilterPack_Drawing_Manga_Flash.ChangeValue3;
			this.PosY = CameraFilterPack_Drawing_Manga_Flash.ChangeValue4;
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

	[Range(1f, 10f)]
	public float Size = 1f;

	[Range(0f, 30f)]
	public int Speed = 5;

	[Range(-1f, 1f)]
	public float PosX = 0.5f;

	[Range(-1f, 1f)]
	public float PosY = 0.5f;

	public static float ChangeValue;

	public static int ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;
}
