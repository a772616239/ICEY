using System;
using System.Collections.Generic;
using CIS;
using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
	private void Awake()
	{
		this.nextIndex = UnityEngine.Random.Range(1, this.colors.Length);
		this.myLight = base.GetComponent<Light>();
		this.canditate = new List<int>();
	}

	private void Process()
	{
		if (this.wait)
		{
			this.timer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
			if (this.timer <= this.waitTime)
			{
				return;
			}
			this.wait = false;
			this.timer = 0f;
		}
		Color a = this.colors[this.index];
		Color b = this.colors[this.nextIndex];
		this.timer += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		Color color = Color.Lerp(a, b, this.timer / this.time);
		this.myLight.color = color;
		if (this.timer > this.time)
		{
			this.wait = true;
			this.waitTime = UnityEngine.Random.Range(this.time / 2f, this.time);
			this.timer = 0f;
			this.index = this.nextIndex;
			this.canditate.Clear();
			for (int i = 0; i < this.colors.Length; i++)
			{
				if (i != this.index)
				{
					this.canditate.Add(i);
				}
			}
			this.nextIndex = this.canditate[UnityEngine.Random.Range(0, this.canditate.Count)];
		}
	}

	private void Update()
	{
		if (this.colors.Length < 2)
		{
			return;
		}
		this.Process();
	}

	public Color[] colors;

	public float time;

	private int index;

	private int nextIndex = 1;

	private float timer;

	private List<int> canditate;

	private Light myLight;

	private bool wait;

	private float waitTime;
}
