using System;
using UnityEngine;

public class TrailFade : MonoBehaviour
{
	private void OnEnable()
	{
		this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		if (this.fadeInTime < 0.01f)
		{
			this.fadeInTime = 0.01f;
		}
		this.percent = this.timeElapsed / this.fadeInTime;
	}

	private void Update()
	{
		this.timeElapsed += Time.deltaTime;
		if (this.timeElapsed <= this.fadeInTime)
		{
			this.percent = this.timeElapsed / this.fadeInTime;
			this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, this.percent));
		}
		if (this.timeElapsed > this.fadeInTime && this.timeElapsed < this.fadeInTime + this.stayTime)
		{
			this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
		}
		if (this.timeElapsed >= this.fadeInTime + this.stayTime && this.timeElapsed < this.fadeInTime + this.stayTime + this.fadeOutTime)
		{
			this.timeElapsedLast += Time.deltaTime;
			this.percent = 1f - this.timeElapsedLast / this.fadeOutTime;
			this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, this.percent));
		}
	}

	private void OnDisable()
	{
		this.thisTrail.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0f));
	}

	private float timeElapsed;

	private float timeElapsedLast;

	private float percent;

	[SerializeField]
	public float fadeInTime = 0.1f;

	[SerializeField]
	public float stayTime = 1f;

	[SerializeField]
	public float fadeOutTime = 0.7f;

	[SerializeField]
	public TrailRenderer thisTrail;
}
