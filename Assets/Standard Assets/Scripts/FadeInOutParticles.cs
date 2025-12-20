using System;
using UnityEngine;

public class FadeInOutParticles : MonoBehaviour
{
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.particles = this.effectSettings.GetComponentsInChildren<ParticleSystem>();
		this.oldVisibleStat = this.effectSettings.IsVisible;
	}

	private void Update()
	{
		if (this.effectSettings.IsVisible != this.oldVisibleStat)
		{
			if (this.effectSettings.IsVisible)
			{
				foreach (ParticleSystem particleSystem in this.particles)
				{
					if (this.effectSettings.IsVisible)
					{
						particleSystem.Play();
						particleSystem.enableEmission = true;
					}
				}
			}
			else
			{
				foreach (ParticleSystem particleSystem2 in this.particles)
				{
					if (!this.effectSettings.IsVisible)
					{
						particleSystem2.Stop();
						particleSystem2.enableEmission = false;
					}
				}
			}
		}
		this.oldVisibleStat = this.effectSettings.IsVisible;
	}

	private EffectSettings effectSettings;

	private ParticleSystem[] particles;

	private bool oldVisibleStat;
}
