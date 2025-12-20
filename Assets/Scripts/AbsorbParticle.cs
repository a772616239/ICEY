using System;
using UnityEngine;

public class AbsorbParticle : BaseBehaviour
{
	private void Start()
	{
		this._particleSystem = base.GetComponent<ParticleSystem>();
	}

	private void LateUpdate()
	{
		int num = this._particleSystem.GetParticles(this.particles);
		for (int i = 0; i < num; i++)
		{
			this.particles[0].velocity = new Vector3(10f, 10f, 0f);
		}
		this._particleSystem.SetParticles(this.particles, num);
	}

	private ParticleSystem.Particle[] particles;

	private ParticleSystem _particleSystem;
}
