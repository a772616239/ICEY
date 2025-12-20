using System;
using System.Collections;
using UnityEngine;

public class WaterWaveUvAnimation : MonoBehaviour
{
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
		this.delta = 1f / (float)this.fps * this.speed;
		base.StartCoroutine(this.updateTiling());
	}

	private IEnumerator updateTiling()
	{
		for (;;)
		{
			this.offset += this.delta;
			this.offsetHeight += this.delta;
			if (this.offset >= 1f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			Vector2 vec = new Vector2(0f, this.offset);
			this.mat.SetTextureOffset("_BumpMap", vec);
			this.mat.SetFloat("_OffsetYHeightMap", this.offset);
			if (this.offset < 0.3f)
			{
				this.mat.SetColor("_Color", new Color(this.color.r, this.color.g, this.color.b, this.offset / 0.3f));
			}
			if (this.offset > 0.7f)
			{
				this.mat.SetColor("_Color", new Color(this.color.r, this.color.g, this.color.b, (1f - this.offset) / 0.3f));
			}
			yield return new WaitForSeconds(1f / (float)this.fps);
		}
		yield break;
	}

	public float speed = 1f;

	public int fps = 30;

	public Color color;

	private Material mat;

	private float offset;

	private float offsetHeight;

	private float delta;
}
