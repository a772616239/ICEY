using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	public class RibbonTrail : RenderObject
	{
		public RibbonTrail(VertexPool.VertexSegment segment, bool useFaceObj, Transform faceobj, float width, int maxelemnt, float len, Vector3 pos, float maxFps)
		{
			if (maxelemnt <= 2)
			{
				UnityEngine.Debug.LogError("ribbon trail's maxelement should > 2!");
			}
			this.MaxElements = maxelemnt;
			this.Vertexsegment = segment;
			this.ElementArray = new RibbonTrail.Element[this.MaxElements];
			this.Head = (this.Tail = 99999);
			this.OriHeadPos = pos;
			this.SetTrailLen(len);
			this.UnitWidth = width;
			this.HeadPosition = pos;
			Vector3 vector;
			if (this.UseFaceObject)
			{
				vector = faceobj.position - this.HeadPosition;
			}
			else
			{
				vector = Vector3.zero;
			}
			RibbonTrail.Element element = new RibbonTrail.Element(this.HeadPosition, this.UnitWidth);
			element.Normal = vector.normalized;
			this.IndexDirty = false;
			this.AddElememt(element);
			this.AddElememt(new RibbonTrail.Element(this.HeadPosition, this.UnitWidth)
			{
				Normal = vector.normalized
			});
			for (int i = 0; i < this.MaxElements; i++)
			{
				this.ElementPool.Add(new RibbonTrail.Element(this.HeadPosition, this.UnitWidth));
			}
			this.UseFaceObject = useFaceObj;
			this.FaceObject = faceobj;
		}

		public override void ApplyShaderParam(float x, float y)
		{
			Vector2 one = Vector2.one;
			one.x = x;
			one.y = y;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.Vertexsegment.VertCount; i++)
			{
				pool.UVs2[vertStart + i] = one;
			}
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(node.LowerLeftUV, node.UVDimensions);
			this.SetColor(node.Color);
			this.SetHeadPosition(node.GetRealClientPos() + node.Position);
			this.ResetElementsPos();
			this.mLastClientPos = node.Owner.ClientTransform.position;
		}

		public override void Reset()
		{
			Vector3 headPosition;
			if (this.Node.Owner.AlwaysSyncRotation)
			{
				headPosition = this.Node.ClientTrans.rotation * (this.Node.GetRealClientPos() + this.Node.Owner.EmitPoint);
			}
			else
			{
				headPosition = this.Node.GetRealClientPos() + this.Node.Owner.EmitPoint;
			}
			this.SetHeadPosition(headPosition);
			this.ResetElementsPos();
			this.StretchCount = 0;
			this.SetColor(Color.clear);
			this.UpdateVertices(Vector3.zero);
			this.mLastClientPos = this.Node.Owner.ClientTransform.position;
		}

		public override void Update(float deltaTime)
		{
			this.SetHeadPosition(this.Node.CurWorldPos);
			if (this.Node.Owner.UVAffectorEnable || this.Node.Owner.UVRotAffectorEnable || this.Node.Owner.UVScaleAffectorEnable)
			{
				this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			}
			if (this.Node.Owner.SyncTrailWithClient)
			{
				this.CheckUpdateSync();
			}
			this.SetColor(this.Node.Color);
			this.MyUpdate(deltaTime);
		}

		private void CheckUpdateSync()
		{
			Vector3 b = this.Node.Owner.ClientTransform.position - this.mLastClientPos;
			this.mLastClientPos = this.Node.Owner.ClientTransform.position;
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num = this.Head + 1;
				for (;;)
				{
					int num2 = num;
					if (num2 == this.MaxElements)
					{
						num2 = 0;
					}
					this.ElementArray[num2].Position += b;
					if (num2 == this.Tail)
					{
						break;
					}
					num = num2 + 1;
				}
			}
		}

		public void ResetElementsPos()
		{
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num = this.Head;
				for (;;)
				{
					int num2 = num;
					if (num2 == this.MaxElements)
					{
						num2 = 0;
					}
					this.ElementArray[num2].Position = this.OriHeadPos;
					if (num2 == this.Tail)
					{
						break;
					}
					num = num2 + 1;
				}
			}
		}

		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
		}

		public void SetColor(Color color)
		{
			this.Color = color;
		}

		public void SetTrailLen(float len)
		{
			this.TrailLength = len;
			this.ElemLength = this.TrailLength / (float)(this.MaxElements - 2);
			this.SquaredElemLength = this.ElemLength * this.ElemLength;
		}

		public void SetHeadPosition(Vector3 pos)
		{
			this.HeadPosition = pos;
		}

		public int GetStretchCount()
		{
			return this.StretchCount;
		}

		public void Smooth()
		{
			if (this.ElemCount <= 3)
			{
				return;
			}
			RibbonTrail.Element element = this.ElementArray[this.Head];
			int num = this.Head + 1;
			if (num == this.MaxElements)
			{
				num = 0;
			}
			int num2 = num + 1;
			if (num2 == this.MaxElements)
			{
				num2 = 0;
			}
			RibbonTrail.Element element2 = this.ElementArray[num];
			RibbonTrail.Element element3 = this.ElementArray[num2];
			Vector3 from = element.Position - element2.Position;
			Vector3 to = element2.Position - element3.Position;
			float num3 = Vector3.Angle(from, to);
			if (num3 > 60f)
			{
				Vector3 a = (element.Position + element3.Position) / 2f;
				Vector3 vector = a - element2.Position;
				Vector3 zero = Vector3.zero;
				float smoothTime = 0.1f / (num3 / 60f);
				element2.Position = Vector3.SmoothDamp(element2.Position, element2.Position + vector.normalized * element2.Width, ref zero, smoothTime);
			}
		}

		private void ScaleLength()
		{
			float num = this.Node.Scale.y;
			num = Mathf.Clamp(num, 0.1f, num);
			this.SetTrailLen(this.Node.Owner.RibbonLen * num);
		}

		public void MyUpdate(float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime < base.Fps)
			{
				return;
			}
			this.ElapsedTime -= base.Fps;
			if (this.Node.Owner.ScaleAffectorEnable)
			{
				this.ScaleLength();
			}
			bool flag = false;
			Vector3 b = Vector3.one * this.Node.Owner.Owner.Scale;
			while (!flag)
			{
				RibbonTrail.Element element = this.ElementArray[this.Head];
				int num = this.Head + 1;
				if (num == this.MaxElements)
				{
					num = 0;
				}
				RibbonTrail.Element element2 = this.ElementArray[num];
				Vector3 headPosition = this.HeadPosition;
				Vector3 a = headPosition - element2.Position;
				float sqrMagnitude = a.sqrMagnitude;
				if (sqrMagnitude >= this.SquaredElemLength)
				{
					this.StretchCount++;
					Vector3 b2 = a * (this.ElemLength / a.magnitude);
					element.Position = element2.Position + b2;
					RibbonTrail.Element element3 = this.ElementPool[0];
					this.ElementPool.RemoveAt(0);
					element3.Position = headPosition;
					element3.Width = this.UnitWidth;
					if (this.UseFaceObject)
					{
						element3.Normal = this.FaceObject.position - Vector3.Scale(headPosition, b);
					}
					else
					{
						element3.Normal = Vector3.zero;
					}
					this.AddElememt(element3);
					a = headPosition - element.Position;
					if (a.sqrMagnitude <= this.SquaredElemLength)
					{
						flag = true;
					}
				}
				else
				{
					element.Position = headPosition;
					flag = true;
				}
				if ((this.Tail + 1) % this.MaxElements == this.Head)
				{
					RibbonTrail.Element element4 = this.ElementArray[this.Tail];
					int num2;
					if (this.Tail == 0)
					{
						num2 = this.MaxElements - 1;
					}
					else
					{
						num2 = this.Tail - 1;
					}
					RibbonTrail.Element element5 = this.ElementArray[num2];
					Vector3 vector = element4.Position - element5.Position;
					float magnitude = vector.magnitude;
					if ((double)magnitude > 1E-06)
					{
						float num3 = this.ElemLength - a.magnitude;
						vector *= num3 / magnitude;
						element4.Position = element5.Position + vector;
					}
				}
			}
			Vector3 position = this.Node.MyCamera.transform.position;
			this.UpdateVertices(position);
			this.UpdateIndices();
		}

		public void UpdateIndices()
		{
			if (!this.IndexDirty)
			{
				return;
			}
			VertexPool pool = this.Vertexsegment.Pool;
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num = this.Head;
				int num2 = 0;
				for (;;)
				{
					int num3 = num + 1;
					if (num3 == this.MaxElements)
					{
						num3 = 0;
					}
					if (num3 * 2 >= 65536)
					{
						UnityEngine.Debug.LogError("Too many elements!");
					}
					int num4 = this.Vertexsegment.VertStart + num3 * 2;
					int num5 = this.Vertexsegment.VertStart + num * 2;
					int num6 = this.Vertexsegment.IndexStart + num2 * 6;
					pool.Indices[num6] = num5;
					pool.Indices[num6 + 1] = num5 + 1;
					pool.Indices[num6 + 2] = num4;
					pool.Indices[num6 + 3] = num5 + 1;
					pool.Indices[num6 + 4] = num4 + 1;
					pool.Indices[num6 + 5] = num4;
					if (num3 == this.Tail)
					{
						break;
					}
					num = num3;
					num2++;
				}
				pool.IndiceChanged = true;
			}
			this.IndexDirty = false;
		}

		public void UpdateVertices(Vector3 eyePos)
		{
			float num = 0f;
			float num2 = this.ElemLength * (float)(this.MaxElements - 2);
			Vector3 b = Vector3.one * this.Node.Owner.Owner.Scale;
			if (this.Head != 99999 && this.Head != this.Tail)
			{
				int num3 = this.Head;
				int num4 = this.Head;
				for (;;)
				{
					if (num4 == this.MaxElements)
					{
						num4 = 0;
					}
					RibbonTrail.Element element = this.ElementArray[num4];
					if (num4 * 2 >= 65536)
					{
						UnityEngine.Debug.LogError("Too many elements!");
					}
					int num5 = this.Vertexsegment.VertStart + num4 * 2;
					int num6 = num4 + 1;
					if (num6 == this.MaxElements)
					{
						num6 = 0;
					}
					Vector3 lhs;
					if (num4 == this.Head)
					{
						lhs = Vector3.Scale(this.ElementArray[num6].Position, b) - Vector3.Scale(element.Position, b);
					}
					else if (num4 == this.Tail)
					{
						lhs = Vector3.Scale(element.Position, b) - Vector3.Scale(this.ElementArray[num3].Position, b);
					}
					else
					{
						lhs = Vector3.Scale(this.ElementArray[num6].Position, b) - Vector3.Scale(this.ElementArray[num3].Position, b);
					}
					Vector3 rhs;
					if (!this.UseFaceObject)
					{
						rhs = eyePos - Vector3.Scale(element.Position, b);
					}
					else
					{
						rhs = element.Normal;
					}
					Vector3 vector = Vector3.Cross(lhs, rhs);
					vector.Normalize();
					vector *= element.Width * 0.5f * this.Node.Scale.x;
					Vector3 vector2 = element.Position - vector;
					Vector3 vector3 = element.Position + vector;
					VertexPool pool = this.Vertexsegment.Pool;
					float num7;
					if (this.Node.Owner.RibbonUVStretch == UV_STRETCH.Vertical)
					{
						num7 = num / num2 * Mathf.Abs(this.UVDimensions.y);
					}
					else
					{
						num7 = num / num2 * Mathf.Abs(this.UVDimensions.x);
					}
					Vector2 zero = Vector2.zero;
					pool.Vertices[num5] = vector2;
					pool.Colors[num5] = this.Color;
					if (this.Node.Owner.RibbonUVStretch == UV_STRETCH.Vertical)
					{
						zero.x = this.LowerLeftUV.x + this.UVDimensions.x;
						zero.y = this.LowerLeftUV.y - num7;
					}
					else
					{
						zero.x = this.LowerLeftUV.x + num7;
						zero.y = this.LowerLeftUV.y;
					}
					pool.UVs[num5] = zero;
					pool.Vertices[num5 + 1] = vector3;
					pool.Colors[num5 + 1] = this.Color;
					if (this.Node.Owner.RibbonUVStretch == UV_STRETCH.Vertical)
					{
						zero.x = this.LowerLeftUV.x;
						zero.y = this.LowerLeftUV.y - num7;
					}
					else
					{
						zero.x = this.LowerLeftUV.x + num7;
						zero.y = this.LowerLeftUV.y - Mathf.Abs(this.UVDimensions.y);
					}
					pool.UVs[num5 + 1] = zero;
					if (num4 == this.Tail)
					{
						break;
					}
					num3 = num4;
					num += (this.ElementArray[num6].Position - element.Position).magnitude;
					num4++;
				}
				this.Vertexsegment.Pool.UVChanged = true;
				this.Vertexsegment.Pool.VertChanged = true;
				this.Vertexsegment.Pool.ColorChanged = true;
			}
		}

		public void AddElememt(RibbonTrail.Element dtls)
		{
			if (this.Head == 99999)
			{
				this.Tail = this.MaxElements - 1;
				this.Head = this.Tail;
				this.IndexDirty = true;
				this.ElemCount++;
			}
			else
			{
				if (this.Head == 0)
				{
					this.Head = this.MaxElements - 1;
				}
				else
				{
					this.Head--;
				}
				if (this.Head == this.Tail)
				{
					if (this.Tail == 0)
					{
						this.Tail = this.MaxElements - 1;
					}
					else
					{
						this.Tail--;
					}
				}
				else
				{
					this.ElemCount++;
				}
			}
			if (this.ElementArray[this.Head] != null)
			{
				this.ElementPool.Add(this.ElementArray[this.Head]);
			}
			this.ElementArray[this.Head] = dtls;
			this.IndexDirty = true;
		}

		protected List<RibbonTrail.Element> ElementPool = new List<RibbonTrail.Element>();

		protected VertexPool.VertexSegment Vertexsegment;

		public int MaxElements;

		public RibbonTrail.Element[] ElementArray;

		public const int CHAIN_EMPTY = 99999;

		public int Head;

		public int Tail;

		protected Vector3 HeadPosition;

		protected float TrailLength;

		protected float ElemLength;

		public float SquaredElemLength;

		protected float UnitWidth;

		protected bool IndexDirty;

		protected Vector2 LowerLeftUV;

		protected Vector2 UVDimensions;

		protected Color Color = Color.white;

		public int ElemCount;

		protected float ElapsedTime;

		protected int StretchCount;

		public Vector3 OriHeadPos;

		private bool UseFaceObject;

		private Transform FaceObject;

		protected Vector3 mLastClientPos;

		public class Element
		{
			public Element(Vector3 position, float width)
			{
				this.Position = position;
				this.Width = width;
			}

			public Vector3 Position;

			public Vector3 Normal;

			public float Width;
		}
	}
}
