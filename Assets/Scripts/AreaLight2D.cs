using System;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;


public class AreaLight2D : BaseBehaviour
{
	private void Awake()
	{
		this._light = base.GetComponent<Light>();
		this.InitAreaLight();
	}

	private void Update()
	{
		this.UpdateLight();
	}

	private void InitAreaLight()
	{
		if (base.transform.childCount != this._lightPoints.Count)
		{
			this._lightPoints.Clear();
			foreach (Transform transform in base.transform.GetChildren())
			{
				this._lightPoints.Add(transform.GetComponent<LightPoint>());
			}
			this._lightPoints.Sort((LightPoint l1, LightPoint l2) => l1.transform.position.x.CompareTo(l2.transform.position.x));
		}
	}

	private void UpdateLight()
	{
		if (R.Player != null)
		{
			float x = R.Player.Transform.position.x;
			int num = this.PlayerAt(x);
			if (num == 0)
			{
				LightPoint lightPoint = this._lightPoints[0];
				this._light.color = lightPoint.color;
				this._light.intensity = lightPoint.intensity;
			}
			else if (num == this._lightPoints.Count)
			{
				LightPoint lightPoint2 = this._lightPoints[num - 1];
				this._light.color = lightPoint2.color;
				this._light.intensity = lightPoint2.intensity;
			}
			else
			{
				LightPoint lightPoint3 = this._lightPoints[num - 1];
				LightPoint lightPoint4 = this._lightPoints[num];
				this._light.color = Color.Lerp(lightPoint4.color, lightPoint3.color, this.getLerpPercent(lightPoint4.Position.x, lightPoint3.Position.x, x));
				this._light.intensity = Mathf.Lerp(lightPoint4.intensity, lightPoint3.intensity, this.getLerpPercent(lightPoint4.Position.x, lightPoint3.Position.x, x));
			}
		}
		else
		{
			LightPoint lightPoint5 = this._lightPoints[0];
			this._light.color = lightPoint5.color;
			this._light.intensity = lightPoint5.intensity;
		}
		this._light.intensity -= 1f - this._defaultIntensity;
	}

	private int PlayerAt(float playerPositionX)
	{
		for (int i = 0; i < this._lightPoints.Count; i++)
		{
			if (playerPositionX < this._lightPoints[i].Position.x)
			{
				return i;
			}
		}
		return this._lightPoints.Count;
	}

	private float getLerpPercent(float x1, float x2, float current)
	{
		return (current - x1) / (x2 - x1);
	}

	[SerializeField]
	[Range(0f, 2f)]
	private float _defaultIntensity = 0.6f;

	[SerializeField]
	private List<LightPoint> _lightPoints = new List<LightPoint>();

	private Light _light;
}
