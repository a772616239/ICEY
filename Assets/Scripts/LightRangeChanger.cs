using System;
using CIS;
using UnityEngine;

public class LightRangeChanger : MonoBehaviour
{
	private void Awake()
	{
		this.myLight = base.GetComponent<Light>();
	}

	private void Update()
	{
		this.timer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime * this.speed;
		this.myLight.range = this.min + Mathf.PingPong(this.timer, this.max - this.min);
	}

	public float speed;

	public float max;

	public float min;

	private float timer;

	private Light myLight;
}
