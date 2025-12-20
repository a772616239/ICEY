using System;
using UnityEngine;

namespace Xft
{
	public class CustomMesh : RenderObject
	{
		public CustomMesh(VertexPool.VertexSegment segment, Mesh mesh, Vector3 dir, float maxFps)
		{
			this.MyMesh = mesh;
			this.MeshVerts = new Vector3[mesh.vertices.Length];
			mesh.vertices.CopyTo(this.MeshVerts, 0);
			this.Vertexsegment = segment;
			this.MyDirection = dir;
			this.SetPosition(Vector3.zero);
			this.InitVerts();
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
			this.SetColor(this.Node.Color);
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.SetScale(this.Node.OriScaleX, this.Node.OriScaleY);
			this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			this.SetDirection(this.Node.OriDirection);
		}

		public override void Reset()
		{
			this.SetColor(Color.clear);
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.Update(true, 0f);
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
			this.Update(false, deltaTime);
		}

		public void SetDirection(Vector3 dir)
		{
			this.MyDirection = dir;
		}

		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.UVChanged = true;
		}

		public void SetColor(Color c)
		{
			this.MyColor = c;
			this.ColorChanged = true;
		}

		public void SetPosition(Vector3 pos)
		{
			this.MyPosition = pos;
		}

		public void SetScale(float width, float height)
		{
			this.MyScale.x = width;
			this.MyScale.y = height;
		}

		public void SetRotation(float angle)
		{
			this.MyRotation = Quaternion.AngleAxis(angle, this.Node.Owner.MeshRotateAxis);
		}

		public void InitVerts()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int indexStart = this.Vertexsegment.IndexStart;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.MeshVerts.Length; i++)
			{
				pool.Vertices[vertStart + i] = Vector3.zero;
			}
			int[] triangles = this.MyMesh.triangles;
			for (int j = 0; j < this.Vertexsegment.IndexCount; j++)
			{
				pool.Indices[j + indexStart] = triangles[j] + vertStart;
			}
			this.m_oriUvs = this.MyMesh.uv;
			for (int k = 0; k < this.m_oriUvs.Length; k++)
			{
				pool.UVs[k + vertStart] = this.m_oriUvs[k];
			}
			Color[] colors = this.MyMesh.colors;
			for (int l = 0; l < colors.Length; l++)
			{
				pool.Colors[l + vertStart] = Color.clear;
			}
		}

		public void UpdateUV()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.m_oriUvs.Length; i++)
			{
				Vector2 vector = this.LowerLeftUV + Vector2.Scale(this.m_oriUvs[i], this.UVDimensions);
				if (vector.x > this.UVDimensions.x + this.LowerLeftUV.x)
				{
					float num = vector.x - this.UVDimensions.x - this.LowerLeftUV.x;
					num = Mathf.Repeat(num, this.UVDimensions.x + this.LowerLeftUV.x);
					vector.x = this.LowerLeftUV.x + num * this.UVDimensions.x;
				}
				pool.UVs[i + vertStart] = vector;
			}
			this.Vertexsegment.Pool.UVChanged = true;
		}

		public void UpdateColor()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			for (int i = 0; i < this.Vertexsegment.VertCount; i++)
			{
				pool.Colors[vertStart + i] = this.MyColor;
			}
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		public void Transform()
		{
			Quaternion lhs;
			if (this.Node.Owner.AlwaysSyncRotation)
			{
				lhs = Quaternion.FromToRotation(Vector3.up, this.Node.Owner.transform.rotation * this.MyDirection);
			}
			else
			{
				lhs = Quaternion.FromToRotation(Vector3.up, this.MyDirection);
			}
			Vector3 one = Vector3.one;
			one.x = (one.z = this.MyScale.x);
			one.y = this.MyScale.y;
			this.LocalMat.SetTRS(Vector3.zero, lhs * this.MyRotation, one);
			this.WorldMat.SetTRS(this.MyPosition, Quaternion.identity, Vector3.one);
			Matrix4x4 matrix4x = this.WorldMat * this.LocalMat;
			VertexPool pool = this.Vertexsegment.Pool;
			for (int i = this.Vertexsegment.VertStart; i < this.Vertexsegment.VertStart + this.Vertexsegment.VertCount; i++)
			{
				pool.Vertices[i] = matrix4x.MultiplyPoint3x4(this.MeshVerts[i - this.Vertexsegment.VertStart]);
			}
		}

		public void Update(bool force, float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime > base.Fps || force)
			{
				this.Transform();
				if (this.ColorChanged)
				{
					this.UpdateColor();
				}
				if (this.UVChanged)
				{
					this.UpdateUV();
				}
				this.ColorChanged = (this.UVChanged = false);
				if (!force)
				{
					this.ElapsedTime -= base.Fps;
				}
			}
		}

		protected VertexPool.VertexSegment Vertexsegment;

		public Mesh MyMesh;

		public Vector3[] MeshVerts;

		public Color MyColor;

		public Vector3 MyPosition = Vector3.zero;

		public Vector2 MyScale = Vector2.one;

		public Quaternion MyRotation = Quaternion.identity;

		public Vector3 MyDirection;

		private Matrix4x4 LocalMat;

		private Matrix4x4 WorldMat;

		private float ElapsedTime;

		public bool ColorChanged;

		public bool UVChanged;

		protected Vector2 LowerLeftUV;

		protected Vector2 UVDimensions;

		protected Vector2[] m_oriUvs;
	}
}
