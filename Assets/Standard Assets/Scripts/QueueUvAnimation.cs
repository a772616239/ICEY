using System;
using System.Collections;
using UnityEngine;

public class QueueUvAnimation : MonoBehaviour
{
	private void Start()
	{
		this.deltaTime = 1f / this.Fps;
		this.InitDefaultTex(this.RowsFadeIn, this.ColumnsFadeIn);
	}

	private void InitDefaultTex(int rows, int colums)
	{
		this.count = rows * colums;
		this.index += colums - 1;
		Vector2 value = new Vector2(1f / (float)colums, 1f / (float)rows);
		base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", value);
		if (this.IsBump)
		{
			base.GetComponent<Renderer>().material.SetTextureScale("_BumpMap", value);
		}
	}

	private void OnBecameVisible()
	{
		this.isVisible = true;
		base.StartCoroutine(this.UpdateTiling());
	}

	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	private IEnumerator UpdateTiling()
	{
		while (this.isVisible && this.allCount != this.count)
		{
			this.allCount++;
			this.index++;
			if (this.index >= this.count)
			{
				this.index = 0;
			}
			Vector2 offset = this.isFadeHandle ? new Vector2((float)this.index / (float)this.ColumnsLoop - (float)(this.index / this.ColumnsLoop), 1f - (float)(this.index / this.ColumnsLoop) / (float)this.RowsLoop) : new Vector2((float)this.index / (float)this.ColumnsFadeIn - (float)(this.index / this.ColumnsFadeIn), 1f - (float)(this.index / this.ColumnsFadeIn) / (float)this.RowsFadeIn);
			if (!this.isFadeHandle)
			{
				base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
				if (this.IsBump)
				{
					base.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
				}
			}
			else
			{
				base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", offset);
				if (this.IsBump)
				{
					base.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
				}
			}
			if (this.allCount == this.count)
			{
				this.isFadeHandle = true;
				base.GetComponent<Renderer>().material = this.NextMaterial;
				this.InitDefaultTex(this.RowsLoop, this.ColumnsLoop);
			}
			yield return new WaitForSeconds(this.deltaTime);
		}
		yield break;
	}

	public int RowsFadeIn = 4;

	public int ColumnsFadeIn = 4;

	public int RowsLoop = 4;

	public int ColumnsLoop = 4;

	public float Fps = 20f;

	public bool IsBump;

	public Material NextMaterial;

	private int index;

	private int count;

	private int allCount;

	private float deltaTime;

	private bool isVisible;

	private bool isFadeHandle;
}
