using System;
using UnityEngine;

namespace Xft
{
	public class GravityAffector : Affector
	{
		public GravityAffector(Transform obj, GAFTTYPE gtype, bool isacc, Vector3 dir, EffectNode node) : base(node, AFFECTORTYPE.GravityAffector)
		{
			this.GType = gtype;
			this.Dir = dir;
			this.Dir.Normalize();
			this.GravityObj = obj;
			this.IsAccelerate = isacc;
		}

		public void SetAttraction(Transform goal)
		{
			this.GravityObj = goal;
		}

		public override void Update(float deltaTime)
		{
			float d;
			if (this.Node.Owner.GravityMagType == MAGTYPE.Fixed)
			{
				d = this.Node.Owner.GravityMag;
			}
			else if (this.Node.Owner.GravityMagType == MAGTYPE.Curve_OBSOLETE)
			{
				d = this.Node.Owner.GravityCurve.Evaluate(this.Node.GetElapsedTime());
			}
			else
			{
				d = this.Node.Owner.GravityCurveX.Evaluate(this.Node.GetElapsedTime());
			}
			if (this.GType == GAFTTYPE.Planar)
			{
				Vector3 a = this.Node.Owner.ClientTransform.rotation * this.Dir;
				if (this.IsAccelerate)
				{
					this.Node.Velocity += a * d * deltaTime;
				}
				else
				{
					this.Node.Position += a * d * deltaTime;
				}
			}
			else if (this.GType == GAFTTYPE.Spherical)
			{
				Vector3 a2 = this.GravityObj.position - this.Node.GetOriginalPos();
				if (this.IsAccelerate)
				{
					this.Node.Velocity += a2 * d * deltaTime;
				}
				else
				{
					this.Node.Position += a2.normalized * d * deltaTime;
				}
			}
		}

		protected GAFTTYPE GType;

		protected Vector3 Dir;

		protected Transform GravityObj;

		protected bool IsAccelerate = true;
	}
}
