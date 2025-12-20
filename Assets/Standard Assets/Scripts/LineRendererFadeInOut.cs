using System;
using UnityEngine;

public class LineRendererFadeInOut : MonoBehaviour
{
	private void Start()
	{
		this.EffectSettings.CollisionEnter += this.EffectSettings_CollisionEnter;
		this.lineRenderer = base.GetComponent<LineRenderer>();
		this.fadeInOutStatus = FadeInOutStatus.In;
		this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, 0f));
		this.lineRenderer.SetWidth(0f, 0f);
		this.lineRenderer.enabled = true;
		this.isInit = true;
	}

	private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.fadeInOutStatus = FadeInOutStatus.Out;
		this.canUpdate = true;
	}

	private void OnEnable()
	{
		if (this.isInit)
		{
			this.fadeInOutStatus = FadeInOutStatus.In;
			this.canUpdate = true;
			this.lineRenderer.enabled = true;
		}
	}

	private void Update()
	{
		FadeInOutStatus fadeInOutStatus = this.fadeInOutStatus;
		if (fadeInOutStatus != FadeInOutStatus.In)
		{
			if (fadeInOutStatus == FadeInOutStatus.Out)
			{
				if (!this.canUpdate)
				{
					return;
				}
				this.currentStartWidth -= Time.deltaTime * (this.StartWidth / this.FadeOutSpeed);
				this.currentEndWidth -= Time.deltaTime * (this.EndWidth / this.FadeOutSpeed);
				this.currentLength -= Time.deltaTime * (this.Length / this.FadeOutSpeed);
				if (this.currentStartWidth <= 0f)
				{
					this.canUpdate = false;
					this.currentStartWidth = 0f;
					this.currentEndWidth = 0f;
					this.currentLength = 0f;
				}
				this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.currentLength));
				this.lineRenderer.SetWidth(this.currentStartWidth, this.currentEndWidth);
				if (!this.canUpdate)
				{
					this.lineRenderer.enabled = false;
				}
			}
		}
		else
		{
			if (!this.canUpdate)
			{
				return;
			}
			this.currentStartWidth += Time.deltaTime * (this.StartWidth / this.FadeInSpeed);
			this.currentEndWidth += Time.deltaTime * (this.EndWidth / this.FadeInSpeed);
			this.currentLength += Time.deltaTime * (this.Length / this.FadeInSpeed);
			if (this.currentStartWidth >= this.StartWidth)
			{
				this.canUpdate = false;
				this.currentStartWidth = this.StartWidth;
				this.currentEndWidth = this.EndWidth;
				this.currentLength = this.Length;
			}
			this.lineRenderer.SetPosition(1, new Vector3(0f, 0f, this.currentLength));
			this.lineRenderer.SetWidth(this.currentStartWidth, this.currentEndWidth);
		}
	}

	public EffectSettings EffectSettings;

	public float FadeInSpeed;

	public float FadeOutSpeed;

	public float Length = 2f;

	public float StartWidth = 1f;

	public float EndWidth = 1f;

	private FadeInOutStatus fadeInOutStatus;

	private LineRenderer lineRenderer;

	private float currentStartWidth;

	private float currentEndWidth;

	private float currentLength;

	private bool isInit;

	private bool canUpdate = true;
}
