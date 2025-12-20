using System;
using System.Collections;
using UnityEngine;

internal class UVTextureAnimator : MonoBehaviour
{
	private void Start()
	{
		this.InitMaterial();
		this.InitDefaultVariables();
		this.isInizialised = true;
		this.isVisible = true;
		base.StartCoroutine(this.UpdateCorutine());
	}

	public void SetInstanceMaterial(Material mat, Vector2 offsetMat)
	{
		this.instanceMaterial = mat;
		this.InitDefaultVariables();
	}

	private void InitDefaultVariables()
	{
		this.allCount = 0;
		this.deltaFps = 1f / this.Fps;
		this.count = this.Rows * this.Columns;
		this.index = this.Columns - 1;
		Vector2 value = new Vector2((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
		this.OffsetMat = (this.IsRandomOffsetForInctance ? UnityEngine.Random.Range(0, this.count) : (this.OffsetMat - this.OffsetMat / this.count * this.count));
		Vector2 value2 = (!(this.SelfTiling == Vector2.zero)) ? this.SelfTiling : new Vector2(1f / (float)this.Columns, 1f / (float)this.Rows);
		if (this.AnimatedMaterialsNotInstance.Length > 0)
		{
			foreach (Material material in this.AnimatedMaterialsNotInstance)
			{
				material.SetTextureScale("_MainTex", value2);
				material.SetTextureOffset("_MainTex", Vector2.zero);
				if (this.IsBump)
				{
					material.SetTextureScale("_BumpMap", value2);
					material.SetTextureOffset("_BumpMap", Vector2.zero);
				}
				if (this.IsHeight)
				{
					material.SetTextureScale("_HeightMap", value2);
					material.SetTextureOffset("_HeightMap", Vector2.zero);
				}
				if (this.IsCutOut)
				{
					material.SetTextureScale("_CutOut", value2);
					material.SetTextureOffset("_CutOut", Vector2.zero);
				}
			}
		}
		else if (this.instanceMaterial != null)
		{
			this.instanceMaterial.SetTextureScale("_MainTex", value2);
			this.instanceMaterial.SetTextureOffset("_MainTex", value);
			if (this.IsBump)
			{
				this.instanceMaterial.SetTextureScale("_BumpMap", value2);
				this.instanceMaterial.SetTextureOffset("_BumpMap", value);
			}
			if (this.IsBump)
			{
				this.instanceMaterial.SetTextureScale("_HeightMap", value2);
				this.instanceMaterial.SetTextureOffset("_HeightMap", value);
			}
			if (this.IsCutOut)
			{
				this.instanceMaterial.SetTextureScale("_CutOut", value2);
				this.instanceMaterial.SetTextureOffset("_CutOut", value);
			}
		}
		else if (this.currentRenderer != null)
		{
			this.currentRenderer.material.SetTextureScale("_MainTex", value2);
			this.currentRenderer.material.SetTextureOffset("_MainTex", value);
			if (this.IsBump)
			{
				this.currentRenderer.material.SetTextureScale("_BumpMap", value2);
				this.currentRenderer.material.SetTextureOffset("_BumpMap", value);
			}
			if (this.IsHeight)
			{
				this.currentRenderer.material.SetTextureScale("_HeightMap", value2);
				this.currentRenderer.material.SetTextureOffset("_HeightMap", value);
			}
			if (this.IsCutOut)
			{
				this.currentRenderer.material.SetTextureScale("_CutOut", value2);
				this.currentRenderer.material.SetTextureOffset("_CutOut", value);
			}
		}
	}

	private void InitMaterial()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this.currentRenderer = base.GetComponent<Renderer>();
		}
		else
		{
			Projector component = base.GetComponent<Projector>();
			if (component != null)
			{
				if (!component.material.name.EndsWith("(Instance)"))
				{
					component.material = new Material(component.material)
					{
						name = component.material.name + " (Instance)"
					};
				}
				this.instanceMaterial = component.material;
			}
		}
	}

	private void OnEnable()
	{
		if (!this.isInizialised)
		{
			return;
		}
		this.InitDefaultVariables();
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	private void OnDisable()
	{
		this.isCorutineStarted = false;
		this.isVisible = false;
		base.StopAllCoroutines();
	}

	private void OnBecameVisible()
	{
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	private IEnumerator UpdateCorutine()
	{
		this.isCorutineStarted = true;
		while (this.isVisible && (this.IsLoop || this.allCount != this.count))
		{
			this.UpdateCorutineFrame();
			if (!this.IsLoop && this.allCount == this.count)
			{
				break;
			}
			yield return new WaitForSeconds(this.deltaFps);
		}
		this.isCorutineStarted = false;
		yield break;
	}

	private void UpdateCorutineFrame()
	{
		if (this.currentRenderer == null && this.instanceMaterial == null && this.AnimatedMaterialsNotInstance.Length == 0)
		{
			return;
		}
		this.allCount++;
		if (this.IsReverse)
		{
			this.index--;
		}
		else
		{
			this.index++;
		}
		if (this.index >= this.count)
		{
			this.index = 0;
		}
		if (this.AnimatedMaterialsNotInstance.Length > 0)
		{
			for (int i = 0; i < this.AnimatedMaterialsNotInstance.Length; i++)
			{
				int num = i * this.OffsetMat + this.index + this.OffsetMat;
				num -= num / this.count * this.count;
				Vector2 value = new Vector2((float)num / (float)this.Columns - (float)(num / this.Columns), 1f - (float)(num / this.Columns) / (float)this.Rows);
				this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_MainTex", value);
				if (this.IsBump)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_BumpMap", value);
				}
				if (this.IsHeight)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_HeightMap", value);
				}
				if (this.IsCutOut)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_CutOut", value);
				}
			}
		}
		else
		{
			Vector2 value2;
			if (this.IsRandomOffsetForInctance)
			{
				int num2 = this.index + this.OffsetMat;
				value2 = new Vector2((float)num2 / (float)this.Columns - (float)(num2 / this.Columns), 1f - (float)(num2 / this.Columns) / (float)this.Rows);
			}
			else
			{
				value2 = new Vector2((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
			}
			if (this.instanceMaterial != null)
			{
				this.instanceMaterial.SetTextureOffset("_MainTex", value2);
				if (this.IsBump)
				{
					this.instanceMaterial.SetTextureOffset("_BumpMap", value2);
				}
				if (this.IsHeight)
				{
					this.instanceMaterial.SetTextureOffset("_HeightMap", value2);
				}
				if (this.IsCutOut)
				{
					this.instanceMaterial.SetTextureOffset("_CutOut", value2);
				}
			}
			else if (this.currentRenderer != null)
			{
				this.currentRenderer.material.SetTextureOffset("_MainTex", value2);
				if (this.IsBump)
				{
					this.currentRenderer.material.SetTextureOffset("_BumpMap", value2);
				}
				if (this.IsHeight)
				{
					this.currentRenderer.material.SetTextureOffset("_HeightMap", value2);
				}
				if (this.IsCutOut)
				{
					this.currentRenderer.material.SetTextureOffset("_CutOut", value2);
				}
			}
		}
	}

	public Material[] AnimatedMaterialsNotInstance;

	public int Rows = 4;

	public int Columns = 4;

	public float Fps = 20f;

	public int OffsetMat;

	public Vector2 SelfTiling = default(Vector2);

	public bool IsLoop = true;

	public bool IsReverse;

	public bool IsRandomOffsetForInctance;

	public bool IsBump;

	public bool IsHeight;

	public bool IsCutOut;

	private bool isInizialised;

	private int index;

	private int count;

	private int allCount;

	private float deltaFps;

	private bool isVisible;

	private bool isCorutineStarted;

	private Renderer currentRenderer;

	private Material instanceMaterial;
}
