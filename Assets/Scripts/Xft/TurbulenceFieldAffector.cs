using System;
using UnityEngine;

namespace Xft
{
	public class TurbulenceFieldAffector : Affector
	{
		public TurbulenceFieldAffector(Transform obj, float atten, bool useMax, float maxDist, EffectNode node) : base(node, AFFECTORTYPE.TurbulenceAffector)
		{
			this.TurbulenceObj = obj;
			this.UseMaxDistance = useMax;
			this.MaxDistance = maxDist;
			this.MaxDistanceSqr = this.MaxDistance * this.MaxDistance;
		}

		protected void UpateNoAttenuation(float deltaTime)
		{
			float sqrMagnitude = (this.Node.GetOriginalPos() - this.TurbulenceObj.position).sqrMagnitude;
			float d;
			if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Fixed)
			{
				d = this.Node.Owner.TurbulenceMagnitude;
			}
			else if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Curve_OBSOLETE)
			{
				d = this.Node.Owner.TurbulenceMagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				d = this.Node.Owner.TurbulenceMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			if (!this.UseMaxDistance || sqrMagnitude <= this.MaxDistanceSqr)
			{
				Vector3 vector;
				vector.x = UnityEngine.Random.Range(-1f, 1f);
				vector.y = UnityEngine.Random.Range(-1f, 1f);
				vector.z = UnityEngine.Random.Range(-1f, 1f);
				vector *= d;
				vector = Vector3.Scale(vector, this.Node.Owner.TurbulenceForce);
				this.Node.Velocity += vector;
			}
		}

		public override void Update(float deltaTime)
		{
			if ((double)this.Attenuation < 1E-06)
			{
				this.UpateNoAttenuation(deltaTime);
				return;
			}
			float magnitude = (this.Node.GetOriginalPos() - this.TurbulenceObj.position).magnitude;
			float num;
			if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Fixed)
			{
				num = this.Node.Owner.TurbulenceMagnitude;
			}
			else if (this.Node.Owner.TurbulenceMagType == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.Node.Owner.TurbulenceMagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				num = this.Node.Owner.TurbulenceMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			if (!this.UseMaxDistance || magnitude <= this.MaxDistance)
			{
				Vector3 vector;
				vector.x = UnityEngine.Random.Range(-1f, 1f);
				vector.y = UnityEngine.Random.Range(-1f, 1f);
				vector.z = UnityEngine.Random.Range(-1f, 1f);
				vector *= num / (1f + magnitude * this.Attenuation);
				this.Node.Velocity += vector;
			}
		}

		protected Transform TurbulenceObj;

		protected float Attenuation;

		protected bool UseMaxDistance;

		protected float MaxDistance;

		protected float MaxDistanceSqr;
	}
}
