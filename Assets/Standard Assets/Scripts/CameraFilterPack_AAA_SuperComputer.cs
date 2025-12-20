using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/AAA/Super Computer")]
public class CameraFilterPack_AAA_SuperComputer : MonoBehaviour
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
		CameraFilterPack_AAA_SuperComputer.Changecenter = this.center;
		CameraFilterPack_AAA_SuperComputer.ChangeRadius = this.Radius;
		CameraFilterPack_AAA_SuperComputer.ChangeValue = this.ShapeFormula;
		CameraFilterPack_AAA_SuperComputer.ChangeAlphaHexa = this._AlphaHexa;
		CameraFilterPack_AAA_SuperComputer.ChangeBorderSize = this._BorderSize;
		CameraFilterPack_AAA_SuperComputer.ChangeBorderColor = this._BorderColor;
		CameraFilterPack_AAA_SuperComputer.ChangeSpotSize = this._SpotSize;
		this.SCShader = Shader.Find("CameraFilterPack/AAA_Super_Computer");
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
			this.TimeX += Time.deltaTime / 4f;
			if (this.TimeX > 100f)
			{
				this.TimeX = 0f;
			}
			this.material.SetFloat("_TimeX", this.TimeX);
			this.material.SetFloat("_Value", this.ShapeFormula);
			this.material.SetFloat("_Value2", this.Shape);
			this.material.SetFloat("_PositionX", this.center.x);
			this.material.SetFloat("_PositionY", this.center.y);
			this.material.SetFloat("_Radius", this.Radius);
			this.material.SetFloat("_BorderSize", this._BorderSize);
			this.material.SetColor("_BorderColor", this._BorderColor);
			this.material.SetFloat("_AlphaHexa", this._AlphaHexa);
			this.material.SetFloat("_SpotSize", this._SpotSize);
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
		CameraFilterPack_AAA_SuperComputer.ChangeValue = this.ShapeFormula;
		CameraFilterPack_AAA_SuperComputer.Changecenter = this.center;
		CameraFilterPack_AAA_SuperComputer.ChangeRadius = this.Radius;
		CameraFilterPack_AAA_SuperComputer.ChangeBorderSize = this._BorderSize;
		CameraFilterPack_AAA_SuperComputer.ChangeBorderColor = this._BorderColor;
		CameraFilterPack_AAA_SuperComputer.ChangeSpotSize = this._SpotSize;
		CameraFilterPack_AAA_SuperComputer.ChangeAlphaHexa = this._AlphaHexa;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			this.ShapeFormula = CameraFilterPack_AAA_SuperComputer.ChangeValue;
			this.center = CameraFilterPack_AAA_SuperComputer.Changecenter;
			this.Radius = CameraFilterPack_AAA_SuperComputer.ChangeRadius;
			this._BorderSize = CameraFilterPack_AAA_SuperComputer.ChangeBorderSize;
			this._BorderColor = CameraFilterPack_AAA_SuperComputer.ChangeBorderColor;
			this._SpotSize = CameraFilterPack_AAA_SuperComputer.ChangeSpotSize;
			this._AlphaHexa = CameraFilterPack_AAA_SuperComputer.ChangeAlphaHexa;
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

	[Range(0f, 1f)]
	public float _AlphaHexa = 1f;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(-20f, 20f)]
	public float ShapeFormula = 10f;

	[Range(0f, 6f)]
	public float Shape = 1f;

	[Range(-4f, 4f)]
	public float _BorderSize = 1f;

	public Color _BorderColor = new Color(0f, 0.2f, 1f, 1f);

	public float _SpotSize = 2.5f;

	public static float ChangeBorderSize = 1f;

	public static Color ChangeBorderColor;

	public static Color ChangeHexaColor;

	public static float ChangeSpotSize = 2.8f;

	public static float ChangeAlphaHexa = 1f;

	public static float ChangeValue;

	public Vector2 center = new Vector2(0f, 0f);

	public float Radius = 0.77f;

	public static Vector2 Changecenter;

	public static float ChangeRadius;
}
