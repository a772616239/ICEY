using System;
using UnityEngine;

namespace Distort
{
	public class HeatWaveAnimation : MonoBehaviour
	{
		private void Awake()
		{
			this.renderer = base.GetComponent<Renderer>();
		}

		private void OnEnable()
		{
			this.time = Time.time;
		}

		private void Update()
		{
			float num;
			if (this.loop)
			{
				num = Mathf.PingPong((Time.time - this.time) / this.animTime, 1f);
			}
			else
			{
				num = Mathf.Clamp((Time.time - this.time) / this.animTime, 0f, 1f);
			}
			float num2 = this.curve.Evaluate(num);
			base.transform.localScale = Vector3.one * this.maxsize * num2;
			this.renderer.material.SetFloat("_StrengthX", (1f - num2) * 0.4f);
			this.renderer.material.SetFloat("_StrengthY", (1f - num2) * 0.4f);
		}

		public float maxsize = 10f;

		public AnimationCurve curve;

		public float animTime;

		private float time;

		public bool loop;

		private Renderer renderer;
	}
}
