using System;
using UnityEngine;

public class BlackHole : BaseBehaviour
{
	public void StartDistort(float time)
	{
		this._distortStep = 0.5f / time;
		this._distorting = true;
		this._distortTime = time;
	}

	public void StartAbsorb(float time)
	{
		this._absorbStep = 0.5f / time;
		this._absorbing = true;
		this._absorbtTime = time;
	}

	private void Start()
	{
		this._material = base.GetComponent<Renderer>().material;
		this._material.SetFloat("_Radius", this._radius);
		this._material.SetFloat("_Absorb", this._absorb);
	}

	private void Update()
	{
		if (this._distorting)
		{
			this._radius += this._distortStep * Time.deltaTime * this._distortFactor;
			this._material.SetFloat("_Radius", this._radius);
			this._distortFactor += Time.deltaTime / this._distortTime;
		}
		if (this._absorbing)
		{
			this._absorb += this._absorbStep * Time.deltaTime * this._absorbtFactor;
			this._material.SetFloat("_Absorb", this._absorb);
			this._absorbtFactor += Time.deltaTime / this._absorbtTime;
		}
	}

	private Material _material;

	private float _radius;

	private float _distortStep;

	private bool _distorting;

	private float _distortFactor;

	private float _distortTime;

	private float _absorb;

	private float _absorbStep;

	private bool _absorbing;

	private float _absorbtFactor;

	private float _absorbtTime;
}
