using System;
using UnityEngine;

public class CursorOscillation : MonoBehaviour
{
	private void Awake()
	{
		this.Oscillate();
	}

	private void Start()
	{
		this.Oscillate();
	}

	private void Update()
	{
		this.Oscillate();
	}

	private void Oscillate()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.offset + Mathf.Sin(Time.time * this.speed) * this.amplitude, base.transform.localPosition.z);
	}

	public float speed = 1f;

	public float amplitude = 1f;

	public float offset = 1f;
}
