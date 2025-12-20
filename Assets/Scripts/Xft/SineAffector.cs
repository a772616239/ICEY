using System;
using UnityEngine;

namespace Xft
{
	public class SineAffector : Affector
	{
		public SineAffector(EffectNode node) : base(node, AFFECTORTYPE.SineAffector)
		{
		}

		public override void Update(float deltaTime)
		{
			float sineTime = this.Node.Owner.SineTime;
			float d = this.Node.Owner.SineMagnitude;
			if (this.Node.Owner.SineMagType != MAGTYPE.Fixed)
			{
				d = this.Node.Owner.SineMagCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			float f = this.Node.GetElapsedTime() / sineTime * 2f * 3.14159274f;
			Vector3 b = Mathf.Sin(f) * this.Node.Owner.SineForce * d;
			if (this.Node.Owner.SineIsAccelarate)
			{
				this.Node.Velocity += b;
			}
			else
			{
				this.Node.Position += b;
			}
		}
	}
}
