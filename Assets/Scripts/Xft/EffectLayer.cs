using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	public class EffectLayer : MonoBehaviour
	{
		public Plane CollisionPlane
		{
			get
			{
				return this.mCollisionPlane;
			}
		}

		public Camera MyCamera
		{
			get
			{
				return this.Owner.MyCamera;
			}
		}

		protected void InitCollision()
		{
			if (!this.UseCollisionDetection)
			{
				return;
			}
			this.mCollisionPlane = new Plane(this.PlaneDir.normalized, base.transform.position + this.PlaneOffset);
			if (this.CollisionType == COLLITION_TYPE.CollisionLayer || this.CollisionType == COLLITION_TYPE.Plane)
			{
				return;
			}
			if (this.CollisionGoal == null)
			{
			}
		}

		public List<Affector> InitAffectors(EffectNode node)
		{
			List<Affector> list = new List<Affector>();
			if (this.UVAffectorEnable)
			{
				UVAnimation uvanimation = new UVAnimation();
				if (this.UVType == 1)
				{
					float x = this.OriUVDimensions.x / (float)this.Cols;
					float y = Mathf.Abs(this.OriUVDimensions.y / (float)this.Rows);
					Vector2 cellSize = new Vector2(x, y);
					uvanimation.BuildUVAnim(this.OriTopLeftUV, cellSize, this.Cols, this.Rows, this.Cols * this.Rows);
				}
				this.UVDimension = uvanimation.UVDimensions[0];
				this.UVTopLeft = uvanimation.frames[0];
				if (uvanimation.frames.Length != 1)
				{
					uvanimation.loopCycles = this.LoopCircles;
					Affector item = new UVAffector(uvanimation, this.UVTime, node, this.RandomStartFrame);
					list.Add(item);
				}
			}
			else
			{
				this.UVDimension = this.OriUVDimensions;
				this.UVTopLeft = this.OriTopLeftUV;
			}
			if (this.RotAffectorEnable && this.RotateType != RSTYPE.NONE)
			{
				Affector item2;
				if (this.RotateType == RSTYPE.NONE)
				{
					item2 = new RotateAffector(this.DeltaRot, node);
				}
				else
				{
					item2 = new RotateAffector(this.RotateType, node);
				}
				list.Add(item2);
			}
			if (this.ScaleAffectorEnable && this.ScaleType != RSTYPE.NONE)
			{
				Affector item3;
				if (this.ScaleType == RSTYPE.NONE)
				{
					item3 = new ScaleAffector(this.DeltaScaleX, this.DeltaScaleY, node);
				}
				else
				{
					item3 = new ScaleAffector(this.ScaleType, node);
				}
				list.Add(item3);
			}
			if (this.ColorAffectorEnable)
			{
				ColorAffector item4 = new ColorAffector(this, node);
				list.Add(item4);
			}
			if (this.JetAffectorEnable)
			{
				Affector item5 = new JetAffector(this.JetMag, this.JetMagType, this.JetCurve, node);
				list.Add(item5);
			}
			if (this.VortexAffectorEnable)
			{
				Affector item6 = new VortexAffector(this.VortexObj, this.VortexDirection, this.VortexInheritRotation, node);
				list.Add(item6);
			}
			if (this.UVRotAffectorEnable)
			{
				float rotXSpeed = this.UVRotXSpeed;
				float rotYSpeed = this.UVRotYSpeed;
				if (this.RandomUVRotateSpeed)
				{
					rotXSpeed = UnityEngine.Random.Range(this.UVRotXSpeed, this.UVRotXSpeedMax);
					rotYSpeed = UnityEngine.Random.Range(this.UVRotYSpeed, this.UVRotYSpeedMax);
				}
				Affector item7 = new UVRotAffector(rotXSpeed, rotYSpeed, node);
				list.Add(item7);
			}
			if (this.UVScaleAffectorEnable)
			{
				Affector item8 = new UVScaleAffector(node);
				list.Add(item8);
			}
			if (this.GravityAffectorEnable)
			{
				Affector item9 = new GravityAffector(this.GravityObject, this.GravityAftType, this.IsGravityAccelerate, this.GravityDirection, node);
				list.Add(item9);
				if (this.GravityAftType == GAFTTYPE.Spherical && this.GravityObject == null)
				{
					UnityEngine.Debug.LogWarning("Gravity UnityEngine.Object is missing, automatically set to effect layer self:" + base.gameObject.name);
					this.GravityObject = base.transform;
				}
			}
			if (this.AirAffectorEnable)
			{
				Affector item10 = new AirFieldAffector(this.AirObject, this.AirDirection, this.AirAttenuation, this.AirUseMaxDistance, this.AirMaxDistance, this.AirEnableSpread, this.AirSpread, this.AirInheritVelocity, this.AirInheritRotation, node);
				list.Add(item10);
			}
			if (this.BombAffectorEnable)
			{
				Affector item11 = new BombAffector(this.BombObject, this.BombType, this.BombDecayType, this.BombMagnitude, this.BombDecay, this.BombAxis, node);
				list.Add(item11);
			}
			if (this.TurbulenceAffectorEnable)
			{
				Affector item12 = new TurbulenceFieldAffector(this.TurbulenceObject, this.TurbulenceAttenuation, this.TurbulenceUseMaxDistance, this.TurbulenceMaxDistance, node);
				list.Add(item12);
			}
			if (this.DragAffectorEnable)
			{
				Affector item13 = new DragAffector(this.DragObj, this.DragUseDir, this.DragDir, this.DragMag, this.DragUseMaxDist, this.DragMaxDist, this.DragAtten, node);
				list.Add(item13);
			}
			if (this.SineAffectorEnable)
			{
				Affector item14 = new SineAffector(node);
				list.Add(item14);
			}
			return list;
		}

		public void SetClient(Transform client)
		{
			this.ClientTransform = client;
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.ClientTrans = client;
			}
		}

		protected void Init()
		{
			this.InitCollision();
			this.Owner = base.transform.parent.gameObject.GetComponent<XffectComponent>();
			if (this.Owner == null)
			{
				UnityEngine.Debug.LogError("you must set EffectLayer to be XffectComponent's child.");
			}
			if (this.ClientTransform == null)
			{
				UnityEngine.Debug.LogWarning("effect layer: " + base.gameObject.name + " haven't assign a client transform, automaticly set to itself.");
				this.ClientTransform = base.transform;
			}
			this.AvailableENodes = new EffectNode[this.MaxENodes];
			this.ActiveENodes = new EffectNode[this.MaxENodes];
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = new EffectNode(i, this.ClientTransform, this.SyncClient, this);
				List<Affector> affectorList = this.InitAffectors(effectNode);
				effectNode.SetAffectorList(affectorList);
				effectNode.SetRenderType(this.RenderType);
				this.AvailableENodes[i] = effectNode;
			}
			if (this.RenderType == 4)
			{
				this.RopeDatas.Init(this);
			}
			this.AvailableNodeCount = this.MaxENodes;
			this.emitter = new Emitter(this);
			this.mStopped = false;
		}

		public VertexPool GetVertexPool()
		{
			return this.Vertexpool;
		}

		public int GetActiveNodeCount()
		{
			return this.ActiveENodes.Length;
		}

		public void RemoveActiveNode(EffectNode node)
		{
			if (this.AvailableNodeCount == this.MaxENodes)
			{
				UnityEngine.Debug.LogError("something wrong with removing node!");
				return;
			}
			if (this.ActiveENodes[node.Index] == null)
			{
				return;
			}
			this.ActiveENodes[node.Index] = null;
			this.AvailableENodes[node.Index] = node;
			this.AvailableNodeCount++;
		}

		public void AddActiveNode(EffectNode node)
		{
			if (this.AvailableNodeCount == 0)
			{
				UnityEngine.Debug.LogError("out index!");
			}
			if (this.AvailableENodes[node.Index] == null)
			{
				return;
			}
			this.ActiveENodes[node.Index] = node;
			this.AvailableENodes[node.Index] = null;
			this.AvailableNodeCount--;
			ulong totalAddedCount;
			this.TotalAddedCount = (totalAddedCount = this.TotalAddedCount) + 1UL;
			node.TotalIndex = totalAddedCount;
		}

		protected void AddNodes(int num)
		{
			int num2 = 0;
			for (int i = 0; i < this.MaxENodes; i++)
			{
				if (num2 == num)
				{
					break;
				}
				EffectNode effectNode = this.AvailableENodes[i];
				if (effectNode != null)
				{
					this.AddActiveNode(effectNode);
					num2++;
					if (this.UseSubEmitters && !string.IsNullOrEmpty(this.BirthSubEmitter))
					{
						XffectComponent effect = this.SpawnCache.GetEffect(this.BirthSubEmitter);
						if (effect == null)
						{
							return;
						}
						effectNode.SubEmitter = effect;
						effect.Active();
					}
					this.emitter.SetEmitPosition(effectNode);
					float life;
					if (this.IsNodeLifeLoop)
					{
						life = -1f;
					}
					else
					{
						life = UnityEngine.Random.Range(this.NodeLifeMin, this.NodeLifeMax);
					}
					Vector3 emitRotation = this.emitter.GetEmitRotation(effectNode);
					float speed = this.OriSpeed;
					if (this.IsRandomSpeed)
					{
						speed = UnityEngine.Random.Range(this.SpeedMin, this.SpeedMax);
					}
					Color oriColor = this.Color1;
					if (this.IsRandomStartColor)
					{
						oriColor = this.RandomColorGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
					}
					float num3 = UnityEngine.Random.Range(this.OriScaleXMin, this.OriScaleXMax);
					float oriScaleY = UnityEngine.Random.Range(this.OriScaleYMin, this.OriScaleYMax);
					if (this.UniformRandomScale)
					{
						oriScaleY = num3;
					}
					effectNode.Init(emitRotation.normalized, speed, life, UnityEngine.Random.Range(this.OriRotationMin, this.OriRotationMax), num3, oriScaleY, oriColor, this.UVTopLeft, this.UVDimension);
				}
			}
		}

		public void Reset()
		{
			if (this.ActiveENodes == null)
			{
				return;
			}
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode != null)
				{
					effectNode.Reset();
					this.RemoveActiveNode(effectNode);
				}
			}
			this.emitter.Reset();
			this.mStopped = false;
			this.TotalAddedCount = 0UL;
		}

		public void FixedUpdateCustom(float deltaTime)
		{
			int nodes = this.emitter.GetNodes(deltaTime);
			this.AddNodes(nodes);
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode != null)
				{
					effectNode.Update(deltaTime);
				}
			}
			if (this.RenderType == 4)
			{
				this.RopeDatas.Update(deltaTime);
			}
		}

		public void StartCustom()
		{
			this.Init();
			this.LastClientPos = this.ClientTransform.position;
		}

		private void OnDrawGizmos()
		{
			if (this.ClientTransform == null)
			{
				return;
			}
			Gizmos.color = this.DebugColor;
			float num;
			if (this.RenderType == 0)
			{
				num = (this.SpriteWidth + this.SpriteHeight) / 6f;
			}
			else if (this.RenderType == 1)
			{
				num = this.RibbonWidth / 3f;
			}
			else
			{
				num = (this.ConeSize.x + this.ConeSize.y) / 6f;
			}
			num = Mathf.Clamp(num, 0f, 1f);
			if (this.EmitType == 0 || this.EmitType == 3)
			{
				Gizmos.DrawWireSphere(this.ClientTransform.position + this.EmitPoint, num);
			}
			if (this.EmitType == 1)
			{
				Vector3 size = this.BoxSize;
				if (this.Owner != null)
				{
					size = this.BoxSize * this.Owner.Scale;
				}
				Gizmos.DrawWireCube(this.ClientTransform.position + this.EmitPoint, size);
			}
			else if (this.EmitType == 2)
			{
				Gizmos.DrawWireSphere(this.ClientTransform.position + this.EmitPoint, this.Radius);
			}
			else if (this.EmitType == 4)
			{
				if (this.LineStartObj != null && this.LineEndObj != null)
				{
					Vector3 position = this.LineStartObj.position;
					Vector3 position2 = this.LineEndObj.position;
					Gizmos.DrawLine(position, position2);
				}
			}
			else if (this.EmitType == 5)
			{
			}
			if (this.OriVelocityAxis != Vector3.zero)
			{
				Gizmos.DrawLine(this.ClientTransform.position + this.EmitPoint, this.ClientTransform.position + this.EmitPoint + this.ClientTransform.rotation * this.OriVelocityAxis * num * 15f);
			}
			if (this.UseCollisionDetection && this.CollisionType == COLLITION_TYPE.Plane)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(this.ClientTransform.position + this.PlaneOffset, new Vector3(num * 300f, 0f, num * 300f));
				Gizmos.color = Color.white;
			}
		}

		public bool EmitOver(float deltaTime)
		{
			if (this.ActiveENodes == null)
			{
				return false;
			}
			if (this.AvailableNodeCount == this.MaxENodes)
			{
				if (this.EmitWay == EEmitWay.ByRate)
				{
					if (this.emitter.EmitLoop == 0f)
					{
						return true;
					}
				}
				else if (this.EmitWay == EEmitWay.ByCurve)
				{
					if (this.emitter.CurveEmitDone)
					{
						return true;
					}
				}
				else if (this.EmitWay == EEmitWay.ByDistance && this.mStopped)
				{
					return true;
				}
			}
			return false;
		}

		public void StopSmoothly(float fadeTime)
		{
			this.mStopped = true;
			this.emitter.StopEmit();
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode != null)
				{
					if (!this.IsNodeLifeLoop && effectNode.GetLifeTime() < fadeTime)
					{
						fadeTime = effectNode.GetLifeTime() - effectNode.GetElapsedTime();
					}
					effectNode.Fade(fadeTime);
				}
			}
		}

		public void StopEmit()
		{
			this.mStopped = true;
			if (this.IsNodeLifeLoop && this.EmitWay != EEmitWay.ByDistance)
			{
				for (int i = 0; i < this.MaxENodes; i++)
				{
					EffectNode effectNode = this.ActiveENodes[i];
					if (effectNode != null)
					{
						effectNode.Stop();
					}
				}
			}
			this.emitter.StopEmit();
		}

		public void SetCollisionGoalPos(Transform pos)
		{
			if (!this.UseCollisionDetection)
			{
				UnityEngine.Debug.LogWarning(base.gameObject.name + "is not set to collision detect mode, please check it");
				return;
			}
			this.CollisionGoal = pos;
		}

		public void SetArractionAffectorGoal(Transform goal)
		{
			if (!this.GravityAffectorEnable || this.GravityAftType == GAFTTYPE.Planar)
			{
				UnityEngine.Debug.LogWarning(base.gameObject.name + "has no attraction affector, please check it");
				return;
			}
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.AvailableENodes[i];
				if (effectNode == null)
				{
					effectNode = this.ActiveENodes[i];
				}
				List<Affector> affectorList = effectNode.GetAffectorList();
				foreach (Affector affector in affectorList)
				{
					if (affector.Type == AFFECTORTYPE.GravityAffector)
					{
						GravityAffector gravityAffector = (GravityAffector)affector;
						gravityAffector.SetAttraction(goal);
					}
				}
			}
		}

		public void SetScale(Vector2 scale)
		{
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.Scale = scale;
			}
		}

		public void SetColor(Color c)
		{
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.Color = c;
			}
		}

		public void SetRotation(float angle)
		{
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.RotateAngle = angle;
			}
		}

		public EffectNode EmitByPos(Vector3 pos)
		{
			int num = 0;
			EffectNode result = null;
			for (int i = 0; i < this.MaxENodes; i++)
			{
				if (num == 1)
				{
					break;
				}
				EffectNode effectNode = this.AvailableENodes[i];
				if (effectNode != null)
				{
					this.AddActiveNode(effectNode);
					num++;
					effectNode.SetLocalPosition(pos);
					float life;
					if (this.IsNodeLifeLoop)
					{
						life = -1f;
					}
					else
					{
						life = UnityEngine.Random.Range(this.NodeLifeMin, this.NodeLifeMax);
					}
					Vector3 emitRotation = this.emitter.GetEmitRotation(effectNode);
					Color oriColor = this.Color1;
					if (this.IsRandomStartColor)
					{
						oriColor = this.RandomColorGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
					}
					effectNode.Init(emitRotation.normalized, this.OriSpeed, life, UnityEngine.Random.Range(this.OriRotationMin, this.OriRotationMax), UnityEngine.Random.Range(this.OriScaleXMin, this.OriScaleXMax), UnityEngine.Random.Range(this.OriScaleYMin, this.OriScaleYMax), oriColor, this.UVTopLeft, this.UVDimension);
					result = effectNode;
				}
			}
			return result;
		}

		public VertexPool Vertexpool;

		public Transform ClientTransform;

		public bool SyncClient;

		public Material Material;

		public int RenderType;

		public float StartTime;

		public float MaxFps = 60f;

		public Color DebugColor = Color.white;

		public int Depth;

		public int SpriteType;

		public int OriPoint;

		public float SpriteWidth = 1f;

		public float SpriteHeight = 1f;

		public UV_STRETCH SpriteUVStretch;

		public bool RandomOriScale;

		public bool RandomOriRot;

		public int OriRotationMin;

		public int OriRotationMax;

		public bool RotAffectorEnable;

		public RSTYPE RotateType;

		public float DeltaRot;

		public AnimationCurve RotateCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 360f)
		});

		public WRAP_TYPE RotateCurveWrap;

		public float RotateCurveTime = 1f;

		public float RotateCurveMaxValue = 1f;

		public AnimationCurve RotateCurve01 = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		public float RotateSpeedMin;

		public float RotateSpeedMax;

		public bool UniformRandomScale;

		public float OriScaleXMin = 1f;

		public float OriScaleXMax = 1f;

		public float OriScaleYMin = 1f;

		public float OriScaleYMax = 1f;

		public bool ScaleAffectorEnable;

		public RSTYPE ScaleType;

		public float DeltaScaleX;

		public float DeltaScaleY;

		public AnimationCurve ScaleXCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 5f)
		});

		public AnimationCurve ScaleYCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 5f)
		});

		public WRAP_TYPE ScaleWrapMode;

		public float ScaleCurveTime = 1f;

		public float MaxScaleCalue = 1f;

		public float MaxScaleValueY = 1f;

		public AnimationCurve ScaleXCurveNew = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		public AnimationCurve ScaleYCurveNew = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		public bool UseSameScaleCurve;

		public float DeltaScaleXMax;

		public float DeltaScaleYMax;

		public bool ColorAffectorEnable;

		public int ColorAffectType;

		public float ColorGradualTimeLength = 1f;

		public COLOR_GRADUAL_TYPE ColorGradualType;

		public bool IsRandomStartColor;

		public ColorParameter RandomColorParam;

		public Gradient RandomColorGradient;

		public Color Color1 = Color.white;

		public Color Color2;

		public Color Color3;

		public Color Color4;

		public Color Color5;

		public COLOR_CHANGE_TYPE ColorChangeType;

		public ColorParameter ColorParam;

		public Gradient ColorGradient;

		public AnimationCurve ColorGradualCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		public float RibbonWidth = 1f;

		public int MaxRibbonElements = 8;

		public float RibbonLen = 15f;

		public float TailDistance;

		public bool SyncTrailWithClient;

		public UV_STRETCH RibbonUVStretch;

		public bool FaceToObject;

		public Transform FaceObject;

		public bool UseRandomRibbon;

		public float RibbonWidthMin = 1f;

		public float RibbonWidthMax = 1f;

		public float RibbonLenMin = 15f;

		public float RibbonLenMax = 15f;

		public Vector2 ConeSize = new Vector2(1f, 3f);

		public float ConeAngle;

		public int ConeSegment = 12;

		public bool UseConeAngleChange;

		public AnimationCurve ConeDeltaAngle = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 60f)
		});

		public Mesh CMesh;

		public Vector3 MeshRotateAxis = Vector3.up;

		public int EmitType;

		public Vector3 BoxSize;

		public Vector3 EmitPoint;

		public float Radius = 1f;

		public bool UseRandomCircle;

		public float CircleRadiusMin = 1f;

		public float CircleRadiusMax = 10f;

		public Vector3 CircleDir = Vector3.up;

		public bool EmitUniform;

		public Transform LineStartObj;

		public Transform LineEndObj;

		public int MaxENodes = 1;

		public bool IsNodeLifeLoop = true;

		public float NodeLifeMin = 1f;

		public float NodeLifeMax = 1f;

		public EEmitWay EmitWay;

		public XCurveParam EmitterCurveX;

		public float DiffDistance = 0.1f;

		public Mesh EmitMesh;

		public int EmitMeshType;

		public bool IsBurstEmit;

		public float ChanceToEmit = 100f;

		public float EmitDuration = 100f;

		public float EmitRate = 20f;

		public int EmitLoop = -1;

		public DIRECTION_TYPE DirType;

		public Vector3 OriVelocityAxis = Vector3.up;

		public int AngleAroundAxis;

		public bool UseRandomDirAngle;

		public int AngleAroundAxisMax;

		public float OriSpeed;

		public bool AlwaysSyncRotation;

		public bool IsRandomSpeed;

		public float SpeedMin;

		public float SpeedMax;

		public bool JetAffectorEnable;

		public MAGTYPE JetMagType;

		public float JetMag;

		public AnimationCurve JetCurve;

		public XCurveParam JetCurveX;

		public bool VortexAffectorEnable;

		public MAGTYPE VortexMagType;

		public float VortexMag = 1f;

		public XCurveParam VortexCurveX;

		public AnimationCurve VortexCurve;

		public Vector3 VortexDirection = Vector3.up;

		public bool VortexInheritRotation = true;

		public Transform VortexObj;

		public bool IsFixedCircle;

		public bool IsRandomVortexDir;

		public bool IsVortexAccelerate;

		public float VortexAttenuation;

		public bool UseVortexMaxDistance;

		public float VortexMaxDistance;

		public bool UVRotAffectorEnable;

		public bool RandomUVRotateSpeed;

		public float UVRotXSpeed;

		public float UVRotYSpeed;

		public float UVRotXSpeedMax;

		public float UVRotYSpeedMax;

		public Vector2 UVRotStartOffset = Vector2.zero;

		public bool UVScaleAffectorEnable;

		public float UVScaleXSpeed;

		public float UVScaleYSpeed;

		public bool GravityAffectorEnable;

		public GAFTTYPE GravityAftType;

		public MAGTYPE GravityMagType;

		public float GravityMag;

		public AnimationCurve GravityCurve;

		public XCurveParam GravityCurveX;

		public Vector3 GravityDirection = Vector3.up;

		public Transform GravityObject;

		public bool IsGravityAccelerate = true;

		public bool AirAffectorEnable;

		public Transform AirObject;

		public MAGTYPE AirMagType;

		public float AirMagnitude;

		public AnimationCurve AirMagCurve;

		public XCurveParam AirMagCurveX;

		public Vector3 AirDirection = Vector3.up;

		public float AirAttenuation;

		public bool AirUseMaxDistance;

		public float AirMaxDistance;

		public bool AirEnableSpread;

		public float AirSpread;

		public float AirInheritVelocity;

		public bool AirInheritRotation;

		public bool BombAffectorEnable;

		public Transform BombObject;

		public BOMBTYPE BombType = BOMBTYPE.Spherical;

		public BOMBDECAYTYPE BombDecayType;

		public float BombMagnitude;

		public Vector3 BombAxis;

		public float BombDecay;

		public bool TurbulenceAffectorEnable;

		public Transform TurbulenceObject;

		public MAGTYPE TurbulenceMagType;

		public float TurbulenceMagnitude = 1f;

		public XCurveParam TurbulenceMagCurveX;

		public AnimationCurve TurbulenceMagCurve;

		public float TurbulenceAttenuation;

		public bool TurbulenceUseMaxDistance;

		public float TurbulenceMaxDistance;

		public Vector3 TurbulenceForce = Vector3.one;

		public bool DragAffectorEnable;

		public Transform DragObj;

		public bool DragUseDir;

		public Vector3 DragDir = Vector3.up;

		public float DragMag = 10f;

		public bool DragUseMaxDist;

		public float DragMaxDist = 50f;

		public float DragAtten;

		public bool UVAffectorEnable;

		public int UVType;

		public Vector2 OriTopLeftUV = Vector2.zero;

		public Vector2 OriUVDimensions = Vector2.one;

		protected Vector2 UVTopLeft;

		protected Vector2 UVDimension;

		public int Cols = 1;

		public int Rows = 1;

		public int LoopCircles = -1;

		public float UVTime = 1f;

		public string EanPath = "none";

		public int EanIndex;

		public bool RandomStartFrame;

		public bool UseCollisionDetection;

		public float ParticleRadius = 1f;

		public COLLITION_TYPE CollisionType;

		public bool CollisionAutoDestroy = true;

		public Transform EventReceiver;

		public string EventHandleFunctionName = " ";

		public Transform CollisionGoal;

		public float ColliisionPosRange;

		public LayerMask CollisionLayer;

		public Vector3 PlaneDir = Vector3.up;

		public Vector3 PlaneOffset = new Vector3(0f, -10f, 0f);

		protected Plane mCollisionPlane;

		public float RopeWidth = 1f;

		public float RopeUVLen = 5f;

		public bool RopeFixUVLen = true;

		public bool SineAffectorEnable;

		public MAGTYPE SineMagType;

		public float SineMagnitude = 1f;

		public float SineTime = 1f;

		public XCurveParam SineMagCurveX;

		public Vector3 SineForce = Vector3.up;

		public bool SineIsAccelarate;

		public bool UseShaderCurve1;

		public bool UseShaderCurve2;

		public XCurveParam ShaderCurveX1;

		public XCurveParam ShaderCurveX2;

		protected ulong TotalAddedCount;

		public bool UseSubEmitters;

		public XffectCache SpawnCache;

		public string BirthSubEmitter;

		public string CollisionSubEmitter;

		public string DeathSubEmitter;

		public bool SubEmitterAutoStop = true;

		public Emitter emitter;

		public EffectNode[] AvailableENodes;

		public EffectNode[] ActiveENodes;

		public int AvailableNodeCount;

		public Vector3 LastClientPos;

		public XffectComponent Owner;

		public bool mStopped;

		public RopeData RopeDatas = new RopeData();
	}
}
