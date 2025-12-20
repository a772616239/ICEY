using System;
using UnityEngine;

namespace Xft
{
	public class Cone : RenderObject
	{
		public Cone(VertexPool.VertexSegment segment, Vector2 size, int numseg, float angle, Vector3 dir, int uvStretch, float maxFps)
		{
			this.Vertexsegment = segment;
			this.Size = size;
			this.Direction = dir;
			this.SetColor(Color.white);
			this.NumSegment = numseg;
			this.SpreadAngle = angle;
			this.OriSpreadAngle = angle;
			this.InitVerts();
		}

		public override void ApplyShaderParam(float x, float y)
		{
			Vector2 one = Vector2.one;
			one.x = x;
			one.y = y;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.NumVerts; i++)
			{
				pool.UVs2[vertStart + i] = one;
			}
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			this.SetColor(this.Node.Color);
			this.SetRotation((float)this.Node.OriRotateAngle);
		}

		public override void Reset()
		{
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.SetColor(Color.clear);
			this.SetPosition(this.Node.Position);
			this.ResetAngle();
			this.MyUpdate(true, 0f);
		}

		public override void Update(float deltaTime)
		{
			this.SetScale(this.Node.Scale.x * this.Node.OriScaleX, this.Node.Scale.y * this.Node.OriScaleY);
			this.SetColor(this.Node.Color);
			if (this.Node.Owner.UVAffectorEnable || this.Node.Owner.UVRotAffectorEnable || this.Node.Owner.UVScaleAffectorEnable)
			{
				this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			}
			this.SetRotation((float)this.Node.OriRotateAngle + this.Node.RotateAngle);
			this.SetPosition(this.Node.CurWorldPos);
			this.MyUpdate(false, deltaTime);
		}

		public void SetUVCoord(Vector2 topleft, Vector2 dimensions)
		{
			this.LowerLeftUV = topleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.LowerLeftUV.y = this.LowerLeftUV.y - dimensions.y;
			this.UVDimensions.y = -this.UVDimensions.y;
			this.UVChanged = true;
		}

		public void SetColor(Color c)
		{
			this.Color = c;
			this.ColorChanged = true;
		}

		public void SetPosition(Vector3 pos)
		{
			this.Position = pos;
		}

		public void SetScale(float width, float height)
		{
			this.Scale.x = width;
			this.Scale.y = height;
		}

		public void SetRotation(float angle)
		{
			this.OriRotAngle = angle;
		}

		public void ResetAngle()
		{
			this.SpreadAngle = this.OriSpreadAngle;
		}

		protected void UpdateRotAngle(float deltaTime)
		{
			if (!this.UseDeltaAngle)
			{
				return;
			}
			this.SpreadAngle = this.CurveAngle.Evaluate(this.Node.GetElapsedTime());
			for (int i = this.NumVerts / 2; i < this.NumVerts; i++)
			{
				this.Verts[i] = this.Verts[i - this.NumVerts / 2] + Vector3.up * this.Size.y;
				this.Verts[i] = Vector3.RotateTowards(this.Verts[i], this.Verts[i - this.NumVerts / 2], this.SpreadAngle * 0.0174532924f, 0f);
			}
		}

		public void UpdateVerts()
		{
			Vector3 point = Vector3.forward * this.Size.x;
			for (int i = 0; i < this.NumVerts / 2; i++)
			{
				this.Verts[i] = Quaternion.Euler(0f, this.OriRotAngle + this.SegmentAngle * (float)i, 0f) * point;
			}
			for (int j = this.NumVerts / 2; j < this.NumVerts; j++)
			{
				this.Verts[j] = this.Verts[j - this.NumVerts / 2] + Vector3.up * this.Size.y;
				this.Verts[j] = Vector3.RotateTowards(this.Verts[j], this.Verts[j - this.NumVerts / 2], this.SpreadAngle * 0.0174532924f, 0f);
			}
		}

		public void InitVerts()
		{
			this.NumVerts = (this.NumSegment + 1) * 2;
			this.SegmentAngle = 360f / (float)this.NumSegment;
			this.Verts = new Vector3[this.NumVerts];
			this.VertsTemp = new Vector3[this.NumVerts];
			this.UpdateVerts();
			VertexPool pool = this.Vertexsegment.Pool;
			int indexStart = this.Vertexsegment.IndexStart;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.NumSegment; i++)
			{
				int num = indexStart + i * 6;
				int num2 = vertStart + i;
				pool.Indices[num] = num2 + this.NumSegment + 1;
				pool.Indices[num + 1] = num2 + this.NumSegment + 2;
				pool.Indices[num + 2] = num2;
				pool.Indices[num + 3] = num2 + this.NumSegment + 2;
				pool.Indices[num + 4] = num2 + 1;
				pool.Indices[num + 5] = num2;
				pool.Vertices[num2 + this.NumSegment + 1] = Vector3.zero;
				pool.Vertices[num2 + this.NumSegment + 2] = Vector3.zero;
				pool.Vertices[num2] = Vector3.zero;
				pool.Vertices[num2 + 1] = Vector3.zero;
			}
		}

		public void UpdateUV()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			float num = this.UVDimensions.x / (float)this.NumSegment;
			for (int i = 0; i < this.NumSegment + 1; i++)
			{
				pool.UVs[vertStart + i] = this.LowerLeftUV;
				Vector2[] uvs = pool.UVs;
				int num2 = vertStart + i;
				uvs[num2].x = uvs[num2].x + (float)i * num;
			}
			for (int j = this.NumSegment + 1; j < this.NumVerts; j++)
			{
				pool.UVs[vertStart + j] = this.LowerLeftUV + Vector2.up * this.UVDimensions.y;
				Vector2[] uvs2 = pool.UVs;
				int num3 = vertStart + j;
				uvs2[num3].x = uvs2[num3].x + (float)(j - this.NumSegment - 1) * num;
			}
			this.Vertexsegment.Pool.UVChanged = true;
		}

		public void UpdateColor()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.NumVerts; i++)
			{
				pool.Colors[vertStart + i] = this.Color;
			}
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		public void Transform()
		{
			if (this.Node.Owner.RotAffectorEnable || this.OriRotAngle != 0f)
			{
				this.UpdateVerts();
			}
			Quaternion rotation;
			if (this.Node.Owner.AlwaysSyncRotation)
			{
				rotation = Quaternion.FromToRotation(Vector3.up, this.Node.Owner.transform.rotation * this.Direction);
			}
			else
			{
				rotation = Quaternion.FromToRotation(Vector3.up, this.Direction);
			}
			for (int i = 0; i < this.NumSegment + 1; i++)
			{
				this.VertsTemp[i] = this.Verts[i] * this.Scale.x;
				this.VertsTemp[i] = rotation * this.VertsTemp[i];
				this.VertsTemp[i] = this.VertsTemp[i] + this.Position;
			}
			for (int j = this.NumSegment + 1; j < this.NumVerts; j++)
			{
				this.VertsTemp[j] = this.Verts[j] * this.Scale.x;
				this.VertsTemp[j].y = this.Verts[j].y * this.Scale.y;
				this.VertsTemp[j] = rotation * this.VertsTemp[j];
				this.VertsTemp[j] = this.VertsTemp[j] + this.Position;
			}
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int k = 0; k < this.NumVerts; k++)
			{
				pool.Vertices[vertStart + k] = this.VertsTemp[k];
			}
		}

		public void MyUpdate(bool force, float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime > base.Fps || force)
			{
				this.UpdateRotAngle(deltaTime);
				this.Transform();
				if (this.UVChanged)
				{
					this.UpdateUV();
				}
				if (this.ColorChanged)
				{
					this.UpdateColor();
				}
				this.UVChanged = (this.ColorChanged = false);
				if (!force)
				{
					this.ElapsedTime -= base.Fps;
				}
			}
		}

		public Vector2 Size;

		public Vector3 Direction;

		public int NumSegment = 4;

		public float SpreadAngle;

		public float OriSpreadAngle;

		public float OriRotAngle = 45f;

		public bool UseDeltaAngle;

		public AnimationCurve CurveAngle;

		protected int NumVerts;

		protected float SegmentAngle;

		protected VertexPool.VertexSegment Vertexsegment;

		protected Vector3[] Verts;

		protected Vector3[] VertsTemp;

		protected float ElapsedTime;

		protected bool UVChanged = true;

		protected bool ColorChanged = true;

		protected float OriUVX;

		public Vector3 Position = Vector3.zero;

		public Color Color;

		public Vector2 Scale;

		protected Vector2 LowerLeftUV = Vector2.zero;

		protected Vector2 UVDimensions = Vector2.one;
	}
}
