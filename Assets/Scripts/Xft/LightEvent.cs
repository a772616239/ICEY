using System;
using UnityEngine;

namespace Xft
{
	public class LightEvent : XftEvent
	{
		public LightEvent(XftEventComponent owner) : base(XEventType.Light, owner)
		{
		}

		public override void Initialize()
		{
			if (this.m_owner.LightComp == null)
			{
				UnityEngine.Debug.LogWarning("you should assign a light source to Light Event to use it!");
				return;
			}
			this.m_elapsedTime = 0f;
			XffectComponent.SetActive(this.m_owner.LightComp.gameObject, false);
		}

		public override void Reset()
		{
			base.Reset();
			if (this.m_owner.LightComp == null)
			{
				return;
			}
			XffectComponent.SetActive(this.m_owner.LightComp.gameObject, false);
		}

		public override void OnBegin()
		{
			base.OnBegin();
			if (this.m_owner.LightComp != null)
			{
				XffectComponent.SetActive(this.m_owner.LightComp.gameObject, true);
			}
		}

		public override void Update(float deltaTime)
		{
			if (this.m_owner.LightComp == null)
			{
				return;
			}
			this.m_elapsedTime += deltaTime;
			float intensity;
			if (this.m_owner.LightIntensityType == MAGTYPE.Curve_OBSOLETE)
			{
				intensity = this.m_owner.LightIntensityCurve.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			else if (this.m_owner.LightIntensityType == MAGTYPE.Fixed)
			{
				intensity = this.m_owner.LightIntensity;
			}
			else
			{
				intensity = this.m_owner.LightIntensityCurveX.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			this.m_owner.LightComp.intensity = intensity;
			float range;
			if (this.m_owner.LightRangeType == MAGTYPE.Curve_OBSOLETE)
			{
				range = this.m_owner.LightRangeCurve.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			else if (this.m_owner.LightRangeType == MAGTYPE.Fixed)
			{
				range = this.m_owner.LightRange;
			}
			else
			{
				range = this.m_owner.LightRangeCurveX.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			this.m_owner.LightComp.range = range;
		}
	}
}
