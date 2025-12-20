using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	public class EffectNode : IComparable<EffectNode>
	{
		public EffectNode(int index, Transform clienttrans, bool sync, EffectLayer owner)
		{
			this.Index = index;
			this.ClientTrans = clienttrans;
			this.SyncClient = sync;
			this.Owner = owner;
			this.LowerLeftUV = Vector2.zero;
			this.UVDimensions = Vector2.one;
			this.Scale = Vector2.one;
			this.RotateAngle = 0f;
			this.Color = Color.white;
		}

		public Camera MyCamera
		{
			get
			{
				if (this.Owner == null)
				{
					UnityEngine.Debug.LogError("something wrong with camera init!");
					return null;
				}
				return this.Owner.MyCamera;
			}
		}

		public int CompareTo(EffectNode other)
		{
			return this.TotalIndex.CompareTo(other.TotalIndex);
		}

		public void SetAffectorList(List<Affector> afts)
		{
			this.AffectorList = afts;
		}

		public List<Affector> GetAffectorList()
		{
			return this.AffectorList;
		}

		public void Init(Vector3 oriDir, float speed, float life, int oriRot, float oriScaleX, float oriScaleY, Color oriColor, Vector2 oriLowerUv, Vector2 oriUVDimension)
		{
			this.OriDirection = oriDir;
			this.LifeTime = life;
			this.OriRotateAngle = oriRot;
			this.OriScaleX = oriScaleX;
			this.OriScaleY = oriScaleY;
			this.StartColor = oriColor;
			this.Color = oriColor;
			this.ElapsedTime = 0f;
			if (this.Owner.DirType != DIRECTION_TYPE.Sphere)
			{
				this.Velocity = this.Owner.ClientTransform.rotation * this.OriDirection * speed;
			}
			else
			{
				this.Velocity = this.OriDirection * speed;
			}
			this.LowerLeftUV = oriLowerUv;
			this.UVDimensions = oriUVDimension;
			this.IsCollisionEventSended = false;
			this.RenderObj.Initialize(this);
		}

		public float GetElapsedTime()
		{
			return this.ElapsedTime;
		}

		public float GetLifeTime()
		{
			return this.LifeTime;
		}

		public void SetLocalPosition(Vector3 pos)
		{
			if (this.Type == 1)
			{
				RibbonTrail ribbonTrail = this.RenderObj as RibbonTrail;
				if (!this.SyncClient)
				{
					ribbonTrail.OriHeadPos = pos;
				}
				else
				{
					ribbonTrail.OriHeadPos = this.GetRealClientPos() + pos;
				}
			}
			this.Position = pos;
		}

		public Vector3 GetLocalPosition()
		{
			return this.Position;
		}

		public Vector3 GetRealClientPos()
		{
			Vector3 vector = Vector3.one * this.Owner.Owner.Scale;
			Vector3 zero = Vector3.zero;
			zero.x = this.ClientTrans.position.x / vector.x;
			zero.y = this.ClientTrans.position.y / vector.y;
			zero.z = this.ClientTrans.position.z / vector.z;
			return zero;
		}

		public Vector3 GetOriginalPos()
		{
			Vector3 realClientPos = this.GetRealClientPos();
			Vector3 result;
			if (!this.SyncClient)
			{
				result = this.Position - realClientPos + this.ClientTrans.position;
			}
			else
			{
				result = this.Position + this.ClientTrans.position;
			}
			return result;
		}

		public Vector3 GetWorldPos()
		{
			return this.CurWorldPos;
		}

		protected bool IsSimpleSprite()
		{
			bool result = false;
			if (this.Owner.SpriteType == 2 && this.Owner.OriVelocityAxis == Vector3.zero && !this.Owner.ScaleAffectorEnable && !this.Owner.RotAffectorEnable && (double)this.Owner.OriSpeed < 0.0001 && !this.Owner.GravityAffectorEnable && !this.Owner.AirAffectorEnable && !this.Owner.TurbulenceAffectorEnable && !this.Owner.BombAffectorEnable && !this.Owner.UVRotAffectorEnable && !this.Owner.UVScaleAffectorEnable && (double)Mathf.Abs(this.Owner.OriRotationMax - this.Owner.OriRotationMin) < 0.0001 && (double)Mathf.Abs(this.Owner.OriScaleXMin - this.Owner.OriScaleXMax) < 0.0001 && (double)Mathf.Abs(this.Owner.OriScaleYMin - this.Owner.OriScaleYMax) < 0.0001 && (double)this.Owner.SpeedMin < 0.0001)
			{
				result = true;
			}
			return result;
		}

		public void SetRenderType(int type)
		{
			this.Type = type;
			if (type == 0)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddSprite(this.Owner.SpriteWidth, this.Owner.SpriteHeight, (STYPE)this.Owner.SpriteType, (ORIPOINT)this.Owner.OriPoint, 60f, this.IsSimpleSprite());
			}
			else if (type == 1)
			{
				float width = this.Owner.RibbonWidth;
				float len = this.Owner.RibbonLen;
				if (this.Owner.UseRandomRibbon)
				{
					width = UnityEngine.Random.Range(this.Owner.RibbonWidthMin, this.Owner.RibbonWidthMax);
					len = UnityEngine.Random.Range(this.Owner.RibbonLenMin, this.Owner.RibbonLenMax);
				}
				this.RenderObj = this.Owner.GetVertexPool().AddRibbonTrail(this.Owner.FaceToObject, this.Owner.FaceObject, width, this.Owner.MaxRibbonElements, len, this.Owner.ClientTransform.position + this.Owner.EmitPoint, 60f);
			}
			else if (type == 2)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddCone(this.Owner.ConeSize, this.Owner.ConeSegment, this.Owner.ConeAngle, this.Owner.OriVelocityAxis, 0, 60f, this.Owner.UseConeAngleChange, this.Owner.ConeDeltaAngle);
			}
			else if (type == 3)
			{
				if (this.Owner.CMesh == null)
				{
					UnityEngine.Debug.LogError("custom mesh layer has no mesh to display!", this.Owner.gameObject);
				}
				Vector3 dir = Vector3.zero;
				if (this.Owner.OriVelocityAxis == Vector3.zero)
				{
					this.Owner.OriVelocityAxis = Vector3.up;
				}
				dir = this.Owner.OriVelocityAxis;
				this.RenderObj = this.Owner.GetVertexPool().AddCustomMesh(this.Owner.CMesh, dir, 60f);
			}
			else if (type == 4)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddRope();
			}
			else if (type == 5)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddSphericalBillboard();
			}
			this.RenderObj.Node = this;
		}

		public void Reset()
		{
			if (this.Owner.UseSubEmitters && !string.IsNullOrEmpty(this.Owner.DeathSubEmitter))
			{
				XffectComponent effect = this.Owner.SpawnCache.GetEffect(this.Owner.DeathSubEmitter);
				if (effect == null)
				{
					return;
				}
				effect.transform.position = this.CurWorldPos;
				effect.Active();
			}
			this.Position = this.Owner.ClientTransform.position;
			this.Velocity = Vector3.zero;
			this.ElapsedTime = 0f;
			this.CurWorldPos = this.Owner.transform.position;
			this.LastWorldPos = this.CurWorldPos;
			this.IsCollisionEventSended = false;
			if (this.Owner.IsRandomStartColor)
			{
				this.StartColor = this.Owner.RandomColorGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
			}
			for (int i = 0; i < this.AffectorList.Count; i++)
			{
				Affector affector = this.AffectorList[i];
				affector.Reset();
			}
			this.Scale = Vector3.one;
			this.mIsFade = false;
			this.RenderObj.Reset();
			if (this.Owner.UseSubEmitters && this.SubEmitter != null && XffectComponent.IsActive(this.SubEmitter.gameObject) && this.Owner.SubEmitterAutoStop)
			{
				this.SubEmitter.StopEmit();
			}
		}

		public void Remove()
		{
			this.Owner.RemoveActiveNode(this);
		}

		public void Stop()
		{
			this.Reset();
			this.Remove();
		}

		public void Fade(float time)
		{
			ColorAffector colorAffector = null;
			for (int i = 0; i < this.AffectorList.Count; i++)
			{
				if (this.AffectorList[i].Type == AFFECTORTYPE.ColorAffector)
				{
					colorAffector = (ColorAffector)this.AffectorList[i];
					break;
				}
			}
			if (colorAffector == null)
			{
				colorAffector = new ColorAffector(this.Owner, this);
				this.AffectorList.Add(colorAffector);
			}
			this.mIsFade = true;
			colorAffector.Fade(time);
		}

		public void CollisionDetection()
		{
			if (!this.Owner.UseCollisionDetection || this.IsCollisionEventSended)
			{
				return;
			}
			bool flag = false;
			GameObject obj = null;
			if (this.Owner.CollisionType == COLLITION_TYPE.Sphere && this.Owner.CollisionGoal != null)
			{
				Vector3 lastCollisionDetectDir = this.CurWorldPos - this.Owner.CollisionGoal.position;
				float num = this.Owner.ColliisionPosRange + this.Owner.ParticleRadius;
				if (lastCollisionDetectDir.sqrMagnitude <= num * num)
				{
					flag = true;
					obj = this.Owner.CollisionGoal.gameObject;
				}
				this.LastCollisionDetectDir = lastCollisionDetectDir;
			}
			else if (this.Owner.CollisionType == COLLITION_TYPE.CollisionLayer)
			{
				int layerMask = 1 << this.Owner.CollisionLayer.value;
				Vector3 originalPos = this.GetOriginalPos();
				RaycastHit raycastHit;
				if (Physics.SphereCast(originalPos, this.Owner.ParticleRadius, this.Velocity.normalized, out raycastHit, this.Owner.ParticleRadius, layerMask))
				{
					flag = true;
					obj = raycastHit.collider.gameObject;
				}
			}
			else if (this.Owner.CollisionType == COLLITION_TYPE.Plane)
			{
				if (!this.Owner.CollisionPlane.GetSide(this.CurWorldPos - this.Owner.PlaneDir.normalized * this.Owner.ParticleRadius))
				{
					flag = true;
					obj = this.Owner.gameObject;
				}
			}
			if (flag)
			{
				if (this.Owner.EventHandleFunctionName != string.Empty && this.Owner.EventReceiver != null)
				{
					this.Owner.EventReceiver.SendMessage(this.Owner.EventHandleFunctionName, new CollisionParam(obj, this.GetOriginalPos(), this.Velocity.normalized));
				}
				this.IsCollisionEventSended = true;
				if (this.Owner.CollisionAutoDestroy)
				{
					this.ElapsedTime = float.PositiveInfinity;
				}
				if (this.Owner.UseSubEmitters && !string.IsNullOrEmpty(this.Owner.CollisionSubEmitter))
				{
					XffectComponent effect = this.Owner.SpawnCache.GetEffect(this.Owner.CollisionSubEmitter);
					if (effect == null)
					{
						return;
					}
					effect.transform.position = this.CurWorldPos;
					effect.Active();
				}
			}
		}

		public void Update(float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			for (int i = 0; i < this.AffectorList.Count; i++)
			{
				Affector affector = this.AffectorList[i];
				affector.Update(deltaTime);
			}
			this.Position += this.Velocity * deltaTime;
			if (this.SyncClient)
			{
				this.CurWorldPos = this.Position + this.GetRealClientPos();
			}
			else
			{
				this.CurWorldPos = this.Position;
			}
			this.CollisionDetection();
			if (this.Owner.UseSubEmitters && this.SubEmitter != null && XffectComponent.IsActive(this.SubEmitter.gameObject))
			{
				this.SubEmitter.transform.position = this.CurWorldPos;
			}
			this.RenderObj.Update(deltaTime);
			if (this.Owner.UseShaderCurve2 || this.Owner.UseShaderCurve1)
			{
				float x = (!this.Owner.UseShaderCurve1) ? 1f : this.Owner.ShaderCurveX1.Evaluate(this.GetElapsedTime(), this);
				float y = (!this.Owner.UseShaderCurve2) ? 0f : this.Owner.ShaderCurveX2.Evaluate(this.GetElapsedTime(), this);
				this.RenderObj.ApplyShaderParam(x, y);
			}
			this.LastWorldPos = this.CurWorldPos;
			if (this.ElapsedTime > this.LifeTime && this.LifeTime > 0f)
			{
				this.Reset();
				this.Remove();
			}
		}

		public RenderObject RenderObj;

		protected int Type;

		public int Index;

		public ulong TotalIndex;

		public Transform ClientTrans;

		public bool SyncClient;

		public EffectLayer Owner;

		protected Vector3 CurDirection;

		protected Vector3 LastWorldPos = Vector3.zero;

		public Vector3 CurWorldPos;

		protected float ElapsedTime;

		public Vector3 Position;

		public Vector2 LowerLeftUV;

		public Vector2 UVDimensions;

		public Vector3 Velocity;

		public Vector2 Scale;

		public float RotateAngle;

		public Color Color;

		public XffectComponent SubEmitter;

		public List<Affector> AffectorList;

		public Vector3 OriDirection;

		public float LifeTime;

		public int OriRotateAngle;

		public float OriScaleX;

		public float OriScaleY;

		public bool SimpleSprite;

		public Color StartColor;

		protected bool IsCollisionEventSended;

		protected Vector3 LastCollisionDetectDir = Vector3.zero;

		public bool mIsFade;
	}
}
