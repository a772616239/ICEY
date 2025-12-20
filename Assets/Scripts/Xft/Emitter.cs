using System;
using UnityEngine;

namespace Xft
{
	public class Emitter
	{
		public Emitter(EffectLayer owner)
		{
			this.Layer = owner;
			this.EmitLoop = (float)this.Layer.EmitLoop;
			this.EmitDist = this.Layer.DiffDistance;
			this.LastClientPos = this.Layer.ClientTransform.position;
		}

		public void Reset()
		{
			this.EmitterElapsedTime = 0f;
			this.IsFirstEmit = true;
			this.EmitLoop = (float)this.Layer.EmitLoop;
			this.EmitDist = this.Layer.DiffDistance;
			this.m_emitCount = 0;
			this.CurveEmitDone = false;
			this.m_curveCountTime = 0f;
		}

		public void StopEmit()
		{
			this.EmitLoop = 0f;
			this.EmitterElapsedTime = 999999f;
			this.EmitDist = 9999999f;
			this.mElapsedTime = 0f;
		}

		protected int EmitByCurve(float deltaTime)
		{
			XCurveParam emitterCurveX = this.Layer.EmitterCurveX;
			if (emitterCurveX == null)
			{
				UnityEngine.Debug.LogWarning("emitter hasn't set a curve yet!");
				return 0;
			}
			this.EmitterElapsedTime += deltaTime;
			int num = (int)emitterCurveX.Evaluate(this.EmitterElapsedTime) - this.m_emitCount;
			int num2;
			if (num > this.Layer.AvailableNodeCount)
			{
				num2 = this.Layer.AvailableNodeCount;
			}
			else
			{
				num2 = num;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.m_emitCount += num2;
			if (num2 == 0)
			{
				this.m_curveCountTime += deltaTime;
				if (this.m_curveCountTime > 1f)
				{
					this.CurveEmitDone = true;
				}
			}
			else
			{
				this.m_curveCountTime = 0f;
			}
			if (this.CurveEmitDone)
			{
				return 0;
			}
			return num2;
		}

		protected int EmitByDistance()
		{
			if (this.Layer.mStopped)
			{
				return 0;
			}
			if ((this.Layer.ClientTransform.position - this.LastClientPos).magnitude >= this.EmitDist)
			{
				this.LastClientPos = this.Layer.ClientTransform.position;
				return 1;
			}
			return 0;
		}

		protected int EmitByBurst(float deltaTime)
		{
			if (!this.IsFirstEmit)
			{
				this.EmitLoop = 0f;
				return 0;
			}
			this.IsFirstEmit = false;
			int num = (int)this.Layer.EmitRate;
			int num2;
			if (num > this.Layer.AvailableNodeCount)
			{
				num2 = this.Layer.AvailableNodeCount;
			}
			else
			{
				num2 = num;
			}
			if (num2 <= 0)
			{
				return 0;
			}
			return num2;
		}

		protected int EmitByRate(float deltaTime)
		{
			if (this.Layer.IsBurstEmit)
			{
				return this.EmitByBurst(deltaTime);
			}
			this.EmitterElapsedTime += deltaTime;
			if (this.Layer.EmitDuration > 0f && this.EmitterElapsedTime >= this.Layer.EmitDuration)
			{
				if (this.EmitLoop > 0f)
				{
					this.EmitLoop -= 1f;
				}
				this.m_emitCount = 0;
				this.EmitterElapsedTime = 0f;
				this.IsFirstEmit = false;
				this.mElapsedTime = 0f;
			}
			if (this.EmitLoop == 0f)
			{
				return 0;
			}
			if (this.Layer.AvailableNodeCount == 0)
			{
				return 0;
			}
			this.mElapsedTime += deltaTime;
			int num = (int)(this.mElapsedTime * this.Layer.EmitRate);
			int num2;
			if (num > this.Layer.AvailableNodeCount)
			{
				num2 = this.Layer.AvailableNodeCount;
			}
			else
			{
				num2 = num;
			}
			if (num2 <= 0)
			{
				return 0;
			}
			this.mElapsedTime = 0f;
			this.m_emitCount += num2;
			return num2;
		}

		public Vector3 GetEmitRotation(EffectNode node)
		{
			Vector3 vector = Vector3.zero;
			if (this.Layer.DirType == DIRECTION_TYPE.Sphere)
			{
				vector = node.GetOriginalPos() - this.Layer.ClientTransform.position;
				if (vector == Vector3.zero)
				{
					Vector3 up = Vector3.up;
					Quaternion rotation = Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
					vector = rotation * up;
				}
			}
			else if (this.Layer.DirType == DIRECTION_TYPE.Planar)
			{
				vector = this.Layer.OriVelocityAxis;
			}
			else if (this.Layer.DirType == DIRECTION_TYPE.Cone)
			{
				if (this.Layer.EmitType == 3 && this.Layer.EmitUniform)
				{
					Vector3 vector2;
					if (!this.Layer.SyncClient)
					{
						vector2 = node.Position - (node.GetRealClientPos() + this.Layer.EmitPoint);
					}
					else
					{
						vector2 = node.Position - this.Layer.EmitPoint;
					}
					int num = this.Layer.AngleAroundAxis;
					if (this.Layer.UseRandomDirAngle)
					{
						num = UnityEngine.Random.Range(this.Layer.AngleAroundAxis, this.Layer.AngleAroundAxisMax);
					}
					Vector3 toDirection = Vector3.RotateTowards(vector2, this.Layer.CircleDir, (float)(90 - num) * 0.0174532924f, 1f);
					Quaternion rotation2 = Quaternion.FromToRotation(vector2, toDirection);
					vector = rotation2 * vector2;
				}
				else
				{
					Quaternion rhs = Quaternion.Euler(0f, 0f, (float)this.Layer.AngleAroundAxis);
					Quaternion rhs2 = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
					Quaternion lhs = Quaternion.FromToRotation(Vector3.up, this.Layer.OriVelocityAxis);
					vector = lhs * rhs2 * rhs * Vector3.up;
				}
			}
			else if (this.Layer.DirType == DIRECTION_TYPE.Cylindrical)
			{
				Vector3 vector3 = node.GetOriginalPos() - this.Layer.ClientTransform.position;
				if (vector3 == Vector3.zero)
				{
					Vector3 up2 = Vector3.up;
					Quaternion rotation3 = Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
					vector3 = rotation3 * up2;
				}
				float d = Vector3.Dot(this.Layer.OriVelocityAxis, vector3);
				vector = vector3 - d * this.Layer.OriVelocityAxis.normalized;
			}
			return vector;
		}

		public void SetEmitPosition(EffectNode node)
		{
			Vector3 vector = Vector3.zero;
			Vector3 realClientPos = node.GetRealClientPos();
			if (this.Layer.EmitType == 1)
			{
				Vector3 emitPoint = this.Layer.EmitPoint;
				float x = UnityEngine.Random.Range(emitPoint.x - this.Layer.BoxSize.x / 2f, emitPoint.x + this.Layer.BoxSize.x / 2f);
				float y = UnityEngine.Random.Range(emitPoint.y - this.Layer.BoxSize.y / 2f, emitPoint.y + this.Layer.BoxSize.y / 2f);
				float z = UnityEngine.Random.Range(emitPoint.z - this.Layer.BoxSize.z / 2f, emitPoint.z + this.Layer.BoxSize.z / 2f);
				vector.x = x;
				vector.y = y;
				vector.z = z;
				if (!this.Layer.SyncClient)
				{
					vector = this.Layer.ClientTransform.rotation * vector + realClientPos;
				}
				else
				{
					vector = this.Layer.ClientTransform.rotation * vector;
				}
			}
			else if (this.Layer.EmitType == 0)
			{
				vector = this.Layer.EmitPoint;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + this.Layer.EmitPoint;
				}
			}
			else if (this.Layer.EmitType == 2)
			{
				vector = this.Layer.EmitPoint;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + this.Layer.EmitPoint;
				}
				Vector3 point = Vector3.up * this.Layer.Radius;
				Quaternion rotation = Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360));
				vector = rotation * point + vector;
			}
			else if (this.Layer.EmitType == 4)
			{
				Vector3 position = this.Layer.LineStartObj.position;
				Vector3 position2 = this.Layer.LineEndObj.position;
				Vector3 vector2 = position2 - position;
				float d;
				if (this.Layer.EmitUniform)
				{
					float num = (float)(node.Index + 1) / (float)this.Layer.MaxENodes;
					d = vector2.magnitude * num;
				}
				else
				{
					d = UnityEngine.Random.Range(0f, vector2.magnitude);
				}
				vector = position + vector2.normalized * d - realClientPos;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + vector;
				}
			}
			else if (this.Layer.EmitType == 3)
			{
				float y2;
				if (this.Layer.EmitUniform)
				{
					int index = node.Index;
					float num2 = (float)(index + 1) / (float)this.Layer.MaxENodes;
					y2 = 360f * num2;
				}
				else
				{
					y2 = (float)UnityEngine.Random.Range(0, 360);
				}
				float d2 = this.Layer.Radius;
				if (this.Layer.UseRandomCircle)
				{
					d2 = UnityEngine.Random.Range(this.Layer.CircleRadiusMin, this.Layer.CircleRadiusMax);
				}
				Quaternion rotation2 = Quaternion.Euler(0f, y2, 0f);
				Vector3 point2 = rotation2 * (Vector3.right * d2);
				Quaternion rotation3 = Quaternion.FromToRotation(Vector3.up, this.Layer.ClientTransform.rotation * this.Layer.CircleDir);
				vector = rotation3 * point2;
				if (!this.Layer.SyncClient)
				{
					vector = realClientPos + vector + this.Layer.EmitPoint;
				}
				else
				{
					vector += this.Layer.EmitPoint;
				}
			}
			else if (this.Layer.EmitType == 5)
			{
				if (this.Layer.EmitMesh == null)
				{
					UnityEngine.Debug.LogWarning("please set a mesh to the emitter.");
					return;
				}
				if (this.Layer.EmitMeshType == 0)
				{
					int vertexCount = this.Layer.EmitMesh.vertexCount;
					int num3;
					if (this.Layer.EmitUniform)
					{
						num3 = node.Index % (vertexCount - 1);
					}
					else
					{
						num3 = UnityEngine.Random.Range(0, vertexCount - 1);
					}
					vector = this.Layer.EmitMesh.vertices[num3];
					if (!this.Layer.SyncClient)
					{
						vector = realClientPos + vector + this.Layer.EmitPoint;
					}
					else
					{
						vector += this.Layer.EmitPoint;
					}
				}
				else if (this.Layer.EmitMeshType == 1)
				{
					Vector3[] vertices = this.Layer.EmitMesh.vertices;
					int num4 = this.Layer.EmitMesh.triangles.Length / 3;
					int num3;
					if (this.Layer.EmitUniform)
					{
						num3 = node.Index % (num4 - 1);
					}
					else
					{
						num3 = UnityEngine.Random.Range(0, num4 - 1);
					}
					int num5 = this.Layer.EmitMesh.triangles[num3 * 3];
					int num6 = this.Layer.EmitMesh.triangles[num3 * 3 + 1];
					int num7 = this.Layer.EmitMesh.triangles[num3 * 3 + 2];
					vector = (vertices[num5] + vertices[num6] + vertices[num7]) / 3f;
					if (!this.Layer.SyncClient)
					{
						vector = realClientPos + vector + this.Layer.EmitPoint;
					}
					else
					{
						vector += this.Layer.EmitPoint;
					}
				}
			}
			node.SetLocalPosition(vector);
		}

		public int GetNodes(float deltaTime)
		{
			if (this.Layer.EmitWay == EEmitWay.ByRate)
			{
				return this.EmitByRate(deltaTime);
			}
			if (this.Layer.EmitWay == EEmitWay.ByCurve)
			{
				return this.EmitByCurve(deltaTime);
			}
			return this.EmitByDistance();
		}

		public EffectLayer Layer;

		public float EmitterElapsedTime;

		protected float mElapsedTime;

		private bool IsFirstEmit = true;

		public float EmitLoop;

		public float EmitDist = 1f;

		public bool CurveEmitDone;

		private Vector3 LastClientPos = Vector3.zero;

		protected int m_emitCount;

		protected float m_curveCountTime;
	}
}
