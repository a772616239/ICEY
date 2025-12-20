using System;
using UnityEngine;

namespace Xft
{
	public class JetAffector : Affector
	{
		public JetAffector(float mag, MAGTYPE type, AnimationCurve curve, EffectNode node) : base(node, AFFECTORTYPE.JetAffector)
		{
			this.Mag = mag;
			this.MType = type;
			this.MagCurve = curve;
		}

		public override void Update(float deltaTime)
		{
			Vector3 b = Vector3.zero;
			if (this.MType == MAGTYPE.Fixed)
			{
				b = this.Node.Velocity.normalized * this.Mag * deltaTime;
			}
			else if (this.MType == MAGTYPE.Curve_OBSOLETE)
			{
				b = this.Node.Velocity.normalized * this.MagCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				b = this.Node.Velocity.normalized * this.Node.Owner.JetCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			Vector3 vector = this.Node.Velocity + b;
			if (Vector3.Dot(vector, this.Node.Velocity) <= 0f)
			{
				this.Node.Velocity = Vector3.zero;
				return;
			}
			this.Node.Velocity = vector;
		}

		protected float Mag;

		protected MAGTYPE MType;

		protected AnimationCurve MagCurve;
	}
}
