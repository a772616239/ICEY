using System;
using UnityEngine;

public class ParticleSystemAttractor : MonoBehaviour
{
	public void Attract(ParticleSystem ps, Transform target, Vector3 offset, float speed)
	{
		Vector3 vector = ps.transform.InverseTransformPoint(target.position + offset);
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[100];
		int particles = ps.GetParticles(array);
		for (int i = 0; i < particles; i++)
		{
			float num = Vector2.Distance(array[i].position, vector);
			array[i].velocity = (vector - array[i].position) * speed * UnityEngine.Random.Range(1.5f, 1.8f);
			array[i].velocity = new Vector2(array[i].velocity.x, array[i].velocity.y);
			array[i].remainingLifetime = num / array[i].velocity.magnitude;
			array[i].size = 70f;
		}
		ps.SetParticles(array, particles);
	}
}
