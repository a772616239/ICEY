using System;
using CIS;
using UnityEngine;

public class WindWavable : MonoBehaviour
{
	private void Awake()
	{
		this.animator = base.GetComponent<Animator>();
	}

	private void Start()
	{
		this.wind = SingletonMonoBehaviourClass<GlobalWindGenerator>.instance;
		if (this.random)
		{
			this.randomFactor = UnityEngine.Random.Range(-1f, 0f);
		}
		else
		{
			this.randomFactor = 0f;
		}
	}

	private void Update()
	{
		this.animator.speed = this.wind.power / this.wind.maxPower;
		this.animator.SetFloat("windPower", this.wind.power + this.randomFactor);
	}

	public bool random;

	private Animator animator;

	private GlobalWindGenerator wind;

	private float randomFactor;
}
