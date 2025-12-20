using System;
using UnityEngine;

namespace Xft
{
	public class VortexAffector : Affector
	{
		public VortexAffector(Transform obj, Vector3 dir, bool inhRot, EffectNode node) : base(node, AFFECTORTYPE.VortexAffector)
		{
			this.Direction = dir;
			this.InheritRotation = inhRot;
			this.VortexObj = obj;
			if (node.Owner.IsRandomVortexDir)
			{
				this.Direction.x = UnityEngine.Random.Range(-1f, 1f);
				this.Direction.y = UnityEngine.Random.Range(-1f, 1f);
				this.Direction.z = UnityEngine.Random.Range(-1f, 1f);
			}
			this.Direction.Normalize();
			this.IsFirst = true;
		}

		public override void Reset()
		{
			this.IsFirst = true;
			this.OriginalRadius = 0f;
			if (this.Node.Owner.IsRandomVortexDir)
			{
				this.Direction.x = UnityEngine.Random.Range(-1f, 1f);
				this.Direction.y = UnityEngine.Random.Range(-1f, 1f);
				this.Direction.z = UnityEngine.Random.Range(-1f, 1f);
			}
			this.Direction.Normalize();
		}

		public override void Update(float deltaTime)
		{
			Vector3 vector = this.Node.GetOriginalPos() - this.VortexObj.position;
			if (this.Node.Owner.IsRandomVortexDir && this.IsFirst)
			{
				this.Direction = Vector3.Cross(vector, this.Direction);
			}
			Vector3 vector2 = this.Direction;
			if (this.InheritRotation)
			{
				vector2 = this.Node.Owner.ClientTransform.rotation * vector2;
			}
			if (this.IsFirst)
			{
				this.IsFirst = false;
				this.OriginalRadius = (vector - Vector3.Project(vector, vector2)).magnitude;
			}
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < 1E-06f)
			{
				return;
			}
			if (!this.Node.Owner.UseVortexMaxDistance || sqrMagnitude <= this.Node.Owner.VortexMaxDistance * this.Node.Owner.VortexMaxDistance)
			{
				float d = Vector3.Dot(vector2, vector);
				vector -= d * vector2;
				Vector3 vector3 = Vector3.zero;
				if (vector == Vector3.zero)
				{
					vector3 = vector;
				}
				else
				{
					vector3 = Vector3.Cross(vector2, vector).normalized;
				}
				float elapsedTime = this.Node.GetElapsedTime();
				float num;
				if (this.Node.Owner.VortexMagType == MAGTYPE.Curve_OBSOLETE)
				{
					num = this.Node.Owner.VortexCurve.Evaluate(elapsedTime);
				}
				else if (this.Node.Owner.VortexMagType == MAGTYPE.Fixed)
				{
					num = this.Node.Owner.VortexMag;
				}
				else
				{
					num = this.Node.Owner.VortexCurveX.Evaluate(elapsedTime);
				}
				if (this.Node.Owner.VortexAttenuation < 0.0001f)
				{
					vector3 *= num * deltaTime;
				}
				else
				{
					vector3 *= num * deltaTime / Mathf.Pow(Mathf.Sqrt(sqrMagnitude), this.Node.Owner.VortexAttenuation);
				}
				if (this.Node.Owner.IsVortexAccelerate)
				{
					this.Node.Velocity += vector3;
				}
				else if (this.Node.Owner.IsFixedCircle)
				{
					Vector3 vector4 = this.Node.GetOriginalPos() + vector3 - this.VortexObj.position;
					Vector3 b = Vector3.Project(vector4, vector2);
					Vector3 vector5 = vector4 - b;
					if (this.Node.Owner.SyncClient)
					{
						this.Node.Position = vector5.normalized * this.OriginalRadius + b;
					}
					else
					{
						this.Node.Position = this.Node.GetRealClientPos() + vector5.normalized * this.OriginalRadius + b;
					}
				}
				else
				{
					this.Node.Position += vector3;
				}
			}
		}

		protected Vector3 Direction;

		protected Transform VortexObj;

		protected bool InheritRotation;

		protected bool IsFirst = true;

		protected float OriginalRadius;
	}
}
