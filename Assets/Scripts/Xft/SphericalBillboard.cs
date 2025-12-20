using System;
using UnityEngine;

namespace Xft
{
	public class SphericalBillboard : RenderObject
	{
		public SphericalBillboard(VertexPool.VertexSegment segment)
		{
			this.UVChanged = (this.ColorChanged = false);
			this.Vertexsegment = segment;
			this.ResetSegment();
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
			this.Color = c;
			this.ColorChanged = true;
		}

		public void SetPosition(Vector3 pos)
		{
			this.mCenterPos = pos;
		}

		public void ResetSegment()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int indexStart = this.Vertexsegment.IndexStart;
			int vertStart = this.Vertexsegment.VertStart;
			pool.Indices[indexStart] = vertStart;
			pool.Indices[indexStart + 1] = vertStart + 3;
			pool.Indices[indexStart + 2] = vertStart + 1;
			pool.Indices[indexStart + 3] = vertStart + 3;
			pool.Indices[indexStart + 4] = vertStart + 2;
			pool.Indices[indexStart + 5] = vertStart + 1;
			pool.Vertices[vertStart] = Vector3.zero;
			pool.Vertices[vertStart + 1] = Vector3.zero;
			pool.Vertices[vertStart + 2] = Vector3.zero;
			pool.Vertices[vertStart + 3] = Vector3.zero;
			pool.Colors[vertStart] = Color.white;
			pool.Colors[vertStart + 1] = Color.white;
			pool.Colors[vertStart + 2] = Color.white;
			pool.Colors[vertStart + 3] = Color.white;
			pool.UVs[vertStart] = Vector2.zero;
			pool.UVs[vertStart + 1] = Vector2.zero;
			pool.UVs[vertStart + 2] = Vector2.zero;
			pool.UVs[vertStart + 3] = Vector2.zero;
			pool.UVChanged = (pool.IndiceChanged = (pool.ColorChanged = (pool.VertChanged = true)));
		}

		public void UpdateUV()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			if (this.UVDimensions.y > 0f)
			{
				pool.UVs[vertStart] = this.LowerLeftUV + Vector2.up * this.UVDimensions.y;
				pool.UVs[vertStart + 1] = this.LowerLeftUV;
				pool.UVs[vertStart + 2] = this.LowerLeftUV + Vector2.right * this.UVDimensions.x;
				pool.UVs[vertStart + 3] = this.LowerLeftUV + this.UVDimensions;
			}
			else
			{
				pool.UVs[vertStart] = this.LowerLeftUV;
				pool.UVs[vertStart + 1] = this.LowerLeftUV + Vector2.up * this.UVDimensions.y;
				pool.UVs[vertStart + 2] = this.LowerLeftUV + this.UVDimensions;
				pool.UVs[vertStart + 3] = this.LowerLeftUV + Vector2.right * this.UVDimensions.x;
			}
			this.Vertexsegment.Pool.UVChanged = true;
		}

		public void UpdateColor()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			pool.Colors[vertStart] = this.Color;
			pool.Colors[vertStart + 1] = this.Color;
			pool.Colors[vertStart + 2] = this.Color;
			pool.Colors[vertStart + 3] = this.Color;
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		public void UpdateCenterPos()
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			pool.Vertices[vertStart] = this.mCenterPos;
			pool.Vertices[vertStart + 1] = this.mCenterPos;
			pool.Vertices[vertStart + 2] = this.mCenterPos;
			pool.Vertices[vertStart + 3] = this.mCenterPos;
			this.Vertexsegment.Pool.VertChanged = true;
		}

		public override void Update(float deltaTime)
		{
			this.SetColor(this.Node.Color);
			if (this.Node.Owner.UVAffectorEnable || this.Node.Owner.UVRotAffectorEnable || this.Node.Owner.UVScaleAffectorEnable)
			{
				this.SetUVCoord(this.Node.LowerLeftUV, this.Node.UVDimensions);
			}
			this.SetPosition(this.Node.CurWorldPos);
			if (this.UVChanged)
			{
				this.UpdateUV();
			}
			if (this.ColorChanged)
			{
				this.UpdateColor();
			}
			this.UVChanged = (this.ColorChanged = false);
			this.UpdateCenterPos();
			this.mRadius = this.Node.Scale.x * this.Node.Owner.SpriteWidth * this.Node.OriScaleX;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			Vector2 one = Vector2.one;
			one.x = this.mRadius;
			one.y = ((float)this.Node.OriRotateAngle + this.Node.RotateAngle) * 3.14159274f / 180f;
			pool.UVs2[vertStart] = one;
			pool.UVs2[vertStart + 1] = one;
			pool.UVs2[vertStart + 2] = one;
			pool.UVs2[vertStart + 3] = one;
			this.Vertexsegment.Pool.UV2Changed = true;
		}

		public override void Initialize(EffectNode node)
		{
			base.Initialize(node);
			this.SetUVCoord(node.LowerLeftUV, node.UVDimensions);
			this.SetColor(this.Node.Color);
			if (this.Node.Owner.MyCamera.depthTextureMode == DepthTextureMode.None)
			{
				this.Node.Owner.MyCamera.depthTextureMode = DepthTextureMode.Depth;
			}
		}

		public override void Reset()
		{
			this.SetPosition(this.Node.Position);
			this.SetColor(Color.clear);
			this.mRadius = 0f;
			this.Update(0f);
		}

		protected Vector2 LowerLeftUV;

		protected Vector2 UVDimensions;

		protected Vector3 v1 = Vector3.zero;

		protected Vector3 v2 = Vector3.zero;

		protected Vector3 v3 = Vector3.zero;

		protected Vector3 v4 = Vector3.zero;

		protected VertexPool.VertexSegment Vertexsegment;

		public Color Color;

		protected bool UVChanged;

		protected bool ColorChanged;

		protected Vector3 mCenterPos = Vector3.zero;

		protected float mRadius;
	}
}
