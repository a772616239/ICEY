using System;
using UnityEngine;

namespace Xft
{
	public class XftSprite : RenderObject
	{
		public XftSprite(VertexPool.VertexSegment segment, float width, float height, STYPE type, ORIPOINT oripoint, float maxFps, bool simple)
		{
			this.UVChanged = (this.ColorChanged = false);
			this.MyTransform.position = Vector3.zero;
			this.MyTransform.rotation = Quaternion.identity;
			this.LocalMat = (this.WorldMat = Matrix4x4.identity);
			this.Vertexsegment = segment;
			this.LastMat = Matrix4x4.identity;
			this.ElapsedTime = 0f;
			this.Simple = simple;
			this.OriPoint = oripoint;
			this.RotateAxis = Vector3.zero;
			this.SetSizeXZ(width, height);
			this.RotateAxis.y = 1f;
			this.Type = type;
			this.ResetSegment();
		}

		public override void ApplyShaderParam(float x, float y)
		{
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			Vector2 one = Vector2.one;
			one.x = x;
			one.y = y;
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
			if (this.Simple)
			{
				node.Update(0f);
				this.Transform();
			}
			if (node.Owner.DirType != DIRECTION_TYPE.Sphere)
			{
				this.SetRotationTo(node.Owner.ClientTransform.rotation * node.OriDirection);
			}
			else
			{
				this.SetRotationTo(node.OriDirection);
			}
		}

		public override void Reset()
		{
			this.SetRotation((float)this.Node.OriRotateAngle);
			this.SetPosition(this.Node.Position);
			this.SetColor(Color.clear);
			this.Update(true, 0f);
		}

		public override void Update(float deltaTime)
		{
			if (this.Node.Owner.AlwaysSyncRotation && this.Node.Owner.DirType != DIRECTION_TYPE.Sphere)
			{
				this.SetRotationTo(this.Node.Owner.ClientTransform.rotation * this.Node.OriDirection);
			}
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

		public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
		{
			this.LowerLeftUV = lowerleft;
			this.UVDimensions = dimensions;
			XftTools.TopLeftUVToLowerLeft(ref this.LowerLeftUV, ref this.UVDimensions);
			this.UVChanged = true;
		}

		public void SetPosition(Vector3 pos)
		{
			this.MyTransform.position = pos;
		}

		public void SetRotation(Quaternion q)
		{
			this.MyTransform.rotation = q;
		}

		public void SetRotationFaceTo(Vector3 dir)
		{
			this.MyTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
		}

		public void SetRotationTo(Vector3 dir)
		{
			if (dir == Vector3.zero)
			{
				return;
			}
			Quaternion rotation = Quaternion.identity;
			Vector3 vector = dir;
			vector.y = 0f;
			if (vector == Vector3.zero)
			{
				vector = Vector3.up;
			}
			if (this.OriPoint == ORIPOINT.CENTER)
			{
				Quaternion rhs = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), vector);
				Quaternion lhs = Quaternion.FromToRotation(vector, dir);
				rotation = lhs * rhs;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_UP)
			{
				Quaternion rhs2 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v3), vector);
				Quaternion lhs2 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs2 * rhs2;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
			{
				Quaternion rhs3 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v4), vector);
				Quaternion lhs3 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs3 * rhs3;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
			{
				Quaternion rhs4 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v1), vector);
				Quaternion lhs4 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs4 * rhs4;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_UP)
			{
				Quaternion rhs5 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v2), vector);
				Quaternion lhs5 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs5 * rhs5;
			}
			else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
			{
				Quaternion rhs6 = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), vector);
				Quaternion lhs6 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs6 * rhs6;
			}
			else if (this.OriPoint == ORIPOINT.TOP_CENTER)
			{
				Quaternion rhs7 = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), vector);
				Quaternion lhs7 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs7 * rhs7;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
			{
				Quaternion rhs8 = Quaternion.FromToRotation(new Vector3(-1f, 0f, 0f), vector);
				Quaternion lhs8 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs8 * rhs8;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
			{
				Quaternion rhs9 = Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), vector);
				Quaternion lhs9 = Quaternion.FromToRotation(vector, dir);
				rotation = lhs9 * rhs9;
			}
			this.MyTransform.rotation = rotation;
		}

		public void SetSizeXZ(float width, float height)
		{
			this.v1 = new Vector3(-width / 2f, 0f, height / 2f);
			this.v2 = new Vector3(-width / 2f, 0f, -height / 2f);
			this.v3 = new Vector3(width / 2f, 0f, -height / 2f);
			this.v4 = new Vector3(width / 2f, 0f, height / 2f);
			Vector3 zero = Vector3.zero;
			if (this.OriPoint == ORIPOINT.LEFT_UP)
			{
				zero = this.v3;
			}
			else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
			{
				zero = this.v4;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
			{
				zero = this.v1;
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_UP)
			{
				zero = this.v2;
			}
			else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
			{
				zero = new Vector3(0f, 0f, height / 2f);
			}
			else if (this.OriPoint == ORIPOINT.TOP_CENTER)
			{
				zero = new Vector3(0f, 0f, -height / 2f);
			}
			else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
			{
				zero = new Vector3(width / 2f, 0f, 0f);
			}
			else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
			{
				zero = new Vector3(-width / 2f, 0f, 0f);
			}
			this.v1 += zero;
			this.v2 += zero;
			this.v3 += zero;
			this.v4 += zero;
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

		public void SetCustomHeight(float[] h)
		{
			this.UseCustomHeight = true;
			this.h1 = h[0];
			this.h2 = h[1];
			this.h3 = h[2];
			this.h4 = h[3];
			this.Transform();
		}

		public void Transform()
		{
			this.LocalMat.SetTRS(Vector3.zero, this.Rotation, this.ScaleVector);
			Transform transform = this.Node.MyCamera.transform;
			if (this.Type == STYPE.BILLBOARD)
			{
				this.MyTransform.LookAt(transform.rotation * Vector3.up, transform.rotation * Vector3.back);
			}
			else if (this.Type == STYPE.BILLBOARD_Y)
			{
				Vector3 up = transform.position - this.MyTransform.position;
				up.y = 0f;
				this.MyTransform.LookAt(Vector3.up, up);
			}
			this.WorldMat.SetTRS(this.MyTransform.position, this.MyTransform.rotation, Vector3.one);
			Matrix4x4 matrix4x = this.WorldMat * this.LocalMat;
			VertexPool pool = this.Vertexsegment.Pool;
			int vertStart = this.Vertexsegment.VertStart;
			Vector3 vector = matrix4x.MultiplyPoint3x4(this.v1);
			Vector3 vector2 = matrix4x.MultiplyPoint3x4(this.v2);
			Vector3 vector3 = matrix4x.MultiplyPoint3x4(this.v3);
			Vector3 vector4 = matrix4x.MultiplyPoint3x4(this.v4);
			if (this.Type == STYPE.BILLBOARD_SELF)
			{
				Vector3 a = Vector3.zero;
				Vector3 vector5 = Vector3.zero;
				Vector3 b = Vector3.one * this.Node.Owner.Owner.Scale;
				float magnitude;
				if (this.Node.Owner.SpriteUVStretch == UV_STRETCH.Vertical)
				{
					a = (vector + vector4) / 2f;
					vector5 = (vector2 + vector3) / 2f;
					magnitude = (vector4 - vector).magnitude;
				}
				else
				{
					a = (vector + vector2) / 2f;
					vector5 = (vector4 + vector3) / 2f;
					magnitude = (vector2 - vector).magnitude;
				}
				Vector3 lhs = a - vector5;
				Vector3 rhs = this.Node.MyCamera.transform.position - Vector3.Scale(a, b);
				Vector3 vector6 = Vector3.Cross(lhs, rhs);
				vector6.Normalize();
				vector6 *= magnitude * 0.5f;
				Vector3 rhs2 = this.Node.MyCamera.transform.position - Vector3.Scale(vector5, b);
				Vector3 vector7 = Vector3.Cross(lhs, rhs2);
				vector7.Normalize();
				vector7 *= magnitude * 0.5f;
				if (this.Node.Owner.SpriteUVStretch == UV_STRETCH.Vertical)
				{
					vector = a - vector6;
					vector4 = a + vector6;
					vector2 = vector5 - vector7;
					vector3 = vector5 + vector7;
				}
				else
				{
					vector = a - vector6;
					vector2 = a + vector6;
					vector4 = vector5 - vector7;
					vector3 = vector5 + vector7;
				}
			}
			pool.Vertices[vertStart] = vector;
			pool.Vertices[vertStart + 1] = vector2;
			pool.Vertices[vertStart + 2] = vector3;
			pool.Vertices[vertStart + 3] = vector4;
			if (this.UseCustomHeight)
			{
				pool.Vertices[vertStart].y = this.h1;
				pool.Vertices[vertStart + 1].y = this.h2;
				pool.Vertices[vertStart + 2].y = this.h3;
				pool.Vertices[vertStart + 3].y = this.h4;
			}
		}

		public void SetRotation(float angle)
		{
			this.Rotation = Quaternion.AngleAxis(angle, this.RotateAxis);
		}

		public void SetScale(float width, float height)
		{
			this.ScaleVector.x = width;
			this.ScaleVector.z = height;
		}

		public void Update(bool force, float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			if (this.ElapsedTime > base.Fps || force)
			{
				if (!this.Simple || force)
				{
					this.Transform();
				}
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

		public void SetColor(Color c)
		{
			this.Color = c;
			this.ColorChanged = true;
		}

		protected Vector2 LowerLeftUV;

		protected Vector2 UVDimensions;

		public XTransform MyTransform;

		public Vector3 v1 = Vector3.zero;

		public Vector3 v2 = Vector3.zero;

		public Vector3 v3 = Vector3.zero;

		public Vector3 v4 = Vector3.zero;

		protected VertexPool.VertexSegment Vertexsegment;

		public Color Color;

		private Vector3 ScaleVector;

		private Quaternion Rotation;

		private Matrix4x4 LocalMat;

		private Matrix4x4 WorldMat;

		protected float ElapsedTime;

		protected bool UVChanged;

		protected bool ColorChanged;

		protected Matrix4x4 LastMat;

		protected Vector3 RotateAxis;

		private STYPE Type;

		private ORIPOINT OriPoint;

		private bool Simple;

		public bool UseCustomHeight;

		public float h1;

		public float h2;

		public float h3;

		public float h4;
	}
}
