using System;
using UnityEngine;

namespace Xft
{
	public class AirFieldAffector : Affector
	{
		public AirFieldAffector(Transform airObj, Vector3 dir, float atten, bool useMaxdist, float maxDist, bool enableSpread, float spread, float inhV, bool inhRot, EffectNode node) : base(node, AFFECTORTYPE.AirFieldAffector)
		{
			this.AirObj = airObj;
			this.Direction = dir.normalized;
			this.Attenuation = atten;
			this.UseMaxDistance = useMaxdist;
			this.MaxDistance = maxDist;
			this.MaxDistanceSqr = this.MaxDistance * this.MaxDistance;
			this.EnableSpread = enableSpread;
			this.Spread = spread;
			this.InheritVelocity = inhV;
			this.InheritRotation = inhRot;
			this.LastFieldPos = this.AirObj.position;
		}

		public override void Reset()
		{
			this.LastFieldPos = this.AirObj.position;
		}

		public override void Update(float deltaTime)
		{
			Vector3 a;
			if (this.InheritRotation)
			{
				a = this.AirObj.rotation * this.Direction;
			}
			else
			{
				a = this.Direction;
			}
			Vector3 vector = Vector3.zero;
			vector = (this.AirObj.position - this.LastFieldPos) * this.InheritVelocity / deltaTime;
			this.LastFieldPos = this.AirObj.position;
			float d;
			if (this.Node.Owner.AirMagType == MAGTYPE.Fixed)
			{
				d = this.Node.Owner.AirMagnitude;
			}
			else if (this.Node.Owner.AirMagType == MAGTYPE.Curve_OBSOLETE)
			{
				d = this.Node.Owner.AirMagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				d = this.Node.Owner.AirMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			vector += a * d;
			float magnitude = vector.magnitude;
			float num = (!this.EnableSpread) ? 0f : Mathf.Cos(1.57079637f * this.Spread);
			Vector3 vector2 = this.Node.GetOriginalPos() - this.AirObj.position;
			float sqrMagnitude = vector2.sqrMagnitude;
			if (!this.UseMaxDistance || sqrMagnitude < this.MaxDistanceSqr)
			{
				Vector3 vector3 = vector;
				if (this.EnableSpread)
				{
					vector3 = vector2.normalized;
					if (Vector3.Dot(vector, vector3) < num)
					{
						return;
					}
					vector3 *= magnitude;
				}
				Vector3 vector4 = this.Node.Velocity;
				if (Vector3.Dot(vector3, vector4 - vector3) < 0f)
				{
					float num2 = deltaTime;
					if (this.UseMaxDistance && this.Attenuation > 1E-06f)
					{
						num2 *= Mathf.Pow(1f - Mathf.Sqrt(sqrMagnitude) / this.MaxDistance, this.Attenuation);
					}
					vector4 += vector3 * num2;
					this.Node.Velocity = vector4;
				}
			}
		}

		protected Transform AirObj;

		protected Vector3 Direction;

		protected float Attenuation;

		protected bool UseMaxDistance;

		protected float MaxDistance;

		protected float MaxDistanceSqr;

		protected bool EnableSpread;

		protected float Spread;

		protected float InheritVelocity;

		protected bool InheritRotation;

		protected Vector3 LastFieldPos;
	}
}
