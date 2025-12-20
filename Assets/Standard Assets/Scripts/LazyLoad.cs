using System;
using UnityEngine;

public class LazyLoad : MonoBehaviour
{
	private void Awake()
	{
		this.GO.SetActive(false);
	}

	private void LazyEnable()
	{
		this.GO.SetActive(true);
	}

	private void OnEnable()
	{
		base.Invoke("LazyEnable", this.TimeDelay);
	}

	private void OnDisable()
	{
		base.CancelInvoke("LazyEnable");
		this.GO.SetActive(false);
	}

	public GameObject GO;

	public float TimeDelay = 0.3f;
}
