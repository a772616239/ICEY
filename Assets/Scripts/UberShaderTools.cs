using System;
using UnityEngine;

[ExecuteInEditMode]
public class UberShaderTools : BaseBehaviour
{
	private void Start()
	{
		this._Color1 = this._Color1Default;
		this._Color2 = this._Color2Default;
		this._Float1 = this._Float1Default;
		this._Float2 = this._Float2Default;
		this._Float3 = this._Float3Default;
	}

	private void OnEnable()
	{
		this.UberReset();
	}

	private void Update()
	{
		Material[] materials = this.GetMaterials();
		foreach (Material material in materials)
		{
			material.SetColor("_Color1", this._Color1);
			material.SetColor("_Color2", this._Color2);
			material.SetFloat("_Float1", this._Float1);
			material.SetFloat("_Float2", this._Float2);
			material.SetFloat("_Float3", this._Float3);
		}
	}

	private Material[] GetMaterials()
	{
		Material[] result;
		if (Application.isEditor || this.编辑shareMaterial)
		{
			result = base.GetComponent<MeshRenderer>().sharedMaterials;
		}
		else
		{
			result = base.GetComponent<MeshRenderer>().materials;
		}
		return result;
	}

	public void UberReset()
	{
		this._Color1 = this._Color1Default;
		this._Color2 = this._Color2Default;
		this._Float1 = this._Float1Default;
		this._Float2 = this._Float2Default;
		this._Float3 = this._Float3Default;
		Material[] materials = this.GetMaterials();
		foreach (Material material in materials)
		{
			material.SetColor("_Color1", this._Color1Default);
			material.SetColor("_Color2", this._Color2Default);
			material.SetFloat("_Float1", this._Float1Default);
			material.SetFloat("_Float2", this._Float2Default);
			material.SetFloat("_Float3", this._Float3Default);
		}
	}

	public Color _Color1;

	public Color _Color2;

	public float _Float1;

	public float _Float2;

	public float _Float3;

	public Color _Color1Default = new Color(1f, 1f, 1f, 1f);

	public Color _Color2Default = new Color(1f, 1f, 1f, 1f);

	public float _Float1Default;

	public float _Float2Default;

	public float _Float3Default;

	public bool 编辑shareMaterial = true;
}
