using System;
using UnityEngine;

public class ChangeParticle : BaseBehaviour
{
	private void OnEnable()
	{
		this.ChangeParticles();
	}

	private void Start()
	{
		this.ChangeParticles();
	}

	private void Update()
	{
	}

	public void ChangeParticles()
	{
		if (base.GetComponent<FBParticleType>() != null && !this.useAverageSpeed)
		{
			base.GetComponent<FBParticleType>().speed1.x = UnityEngine.Random.Range(this.minSpeed, this.maxSpeed);
		}
	}

	[SerializeField]
	public float minSpeed;

	[SerializeField]
	public float maxSpeed;

	[SerializeField]
	public bool usePosition;

	[SerializeField]
	public bool useAverageSpeed;

	[SerializeField]
	public float positionY;
}
