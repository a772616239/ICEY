using System;
using UnityEngine;

namespace Xft
{
	public class VertexPool
	{
		public VertexPool(Mesh mesh, Material material)
		{
			this.VertexTotal = (this.VertexUsed = 0);
			this.VertCountChanged = false;
			this.Mesh = mesh;
			this.Material = material;
			this.InitArrays();
			this.IndiceChanged = (this.ColorChanged = (this.UVChanged = (this.UV2Changed = (this.VertChanged = true))));
		}

		public void RecalculateBounds()
		{
			this.Mesh.RecalculateBounds();
		}

		public CustomMesh AddCustomMesh(Mesh mesh, Vector3 dir, float maxFps)
		{
			VertexPool.VertexSegment vertices = this.GetVertices(mesh.vertices.Length, mesh.triangles.Length);
			return new CustomMesh(vertices, mesh, dir, maxFps);
		}

		public Cone AddCone(Vector2 size, int numSegment, float angle, Vector3 dir, int uvStretch, float maxFps, bool usedelta, AnimationCurve deltaAngle)
		{
			VertexPool.VertexSegment vertices = this.GetVertices((numSegment + 1) * 2, numSegment * 6);
			return new Cone(vertices, size, numSegment, angle, dir, uvStretch, maxFps)
			{
				UseDeltaAngle = usedelta,
				CurveAngle = deltaAngle
			};
		}

		public XftSprite AddSprite(float width, float height, STYPE type, ORIPOINT ori, float maxFps, bool simple)
		{
			VertexPool.VertexSegment vertices = this.GetVertices(4, 6);
			return new XftSprite(vertices, width, height, type, ori, maxFps, simple);
		}

		public RibbonTrail AddRibbonTrail(bool useFaceObj, Transform faceobj, float width, int maxelemnt, float len, Vector3 pos, float maxFps)
		{
			VertexPool.VertexSegment vertices = this.GetVertices(maxelemnt * 2, (maxelemnt - 1) * 6);
			return new RibbonTrail(vertices, useFaceObj, faceobj, width, maxelemnt, len, pos, maxFps);
		}

		public VertexPool.VertexSegment GetRopeVertexSeg(int maxcount)
		{
			return this.GetVertices(maxcount * 2, (maxcount - 1) * 6);
		}

		public Rope AddRope()
		{
			return new Rope();
		}

		public SphericalBillboard AddSphericalBillboard()
		{
			VertexPool.VertexSegment vertices = this.GetVertices(4, 6);
			return new SphericalBillboard(vertices);
		}

		public Material GetMaterial()
		{
			return this.Material;
		}

		public VertexPool.VertexSegment GetVertices(int vcount, int icount)
		{
			int num = 0;
			int num2 = 0;
			if (this.VertexUsed + vcount >= this.VertexTotal)
			{
				num = (vcount / 108 + 1) * 108;
			}
			if (this.IndexUsed + icount >= this.IndexTotal)
			{
				num2 = (icount / 108 + 1) * 108;
			}
			this.VertexUsed += vcount;
			this.IndexUsed += icount;
			if (num != 0 || num2 != 0)
			{
				this.EnlargeArrays(num, num2);
				this.VertexTotal += num;
				this.IndexTotal += num2;
			}
			return new VertexPool.VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
		}

		private void InitDefaultShaderParam(Vector2[] uv2)
		{
			for (int i = 0; i < uv2.Length; i++)
			{
				uv2[i].x = 1f;
				uv2[i].y = 0f;
			}
		}

		protected void InitArrays()
		{
			this.Vertices = new Vector3[4];
			this.UVs = new Vector2[4];
			this.UVs2 = new Vector2[4];
			this.Colors = new Color[4];
			this.Indices = new int[6];
			this.VertexTotal = 4;
			this.IndexTotal = 6;
			this.InitDefaultShaderParam(this.UVs2);
		}

		public void EnlargeArrays(int count, int icount)
		{
			Vector3[] vertices = this.Vertices;
			this.Vertices = new Vector3[this.Vertices.Length + count];
			vertices.CopyTo(this.Vertices, 0);
			Vector2[] uvs = this.UVs;
			this.UVs = new Vector2[this.UVs.Length + count];
			uvs.CopyTo(this.UVs, 0);
			Vector2[] uvs2 = this.UVs2;
			this.UVs2 = new Vector2[this.UVs2.Length + count];
			uvs2.CopyTo(this.UVs2, 0);
			this.InitDefaultShaderParam(this.UVs2);
			Color[] colors = this.Colors;
			this.Colors = new Color[this.Colors.Length + count];
			colors.CopyTo(this.Colors, 0);
			int[] indices = this.Indices;
			this.Indices = new int[this.Indices.Length + icount];
			indices.CopyTo(this.Indices, 0);
			this.VertCountChanged = true;
			this.IndiceChanged = true;
			this.ColorChanged = true;
			this.UVChanged = true;
			this.VertChanged = true;
			this.UV2Changed = true;
		}

		public void LateUpdate()
		{
			if (this.VertCountChanged)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.UVChanged)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.UV2Changed)
			{
				this.Mesh.uv2 = this.UVs2;
			}
			if (this.ColorChanged)
			{
				this.Mesh.colors = this.Colors;
			}
			if (this.IndiceChanged)
			{
				this.Mesh.triangles = this.Indices;
			}
			this.ElapsedTime += Time.deltaTime;
			if (this.ElapsedTime > this.BoundsScheduleTime || this.FirstUpdate)
			{
				this.RecalculateBounds();
				this.ElapsedTime = 0f;
			}
			if (this.ElapsedTime > this.BoundsScheduleTime)
			{
				this.FirstUpdate = false;
			}
			this.VertCountChanged = false;
			this.IndiceChanged = false;
			this.ColorChanged = false;
			this.UVChanged = false;
			this.UV2Changed = false;
			this.VertChanged = false;
		}

		public Vector3[] Vertices;

		public int[] Indices;

		public Vector2[] UVs;

		public Color[] Colors;

		public Vector2[] UVs2;

		public bool IndiceChanged;

		public bool ColorChanged;

		public bool UVChanged;

		public bool VertChanged;

		public bool UV2Changed;

		public Mesh Mesh;

		public Material Material;

		protected int VertexTotal;

		protected int VertexUsed;

		protected int IndexTotal;

		protected int IndexUsed;

		public bool FirstUpdate = true;

		protected bool VertCountChanged;

		public const int BlockSize = 108;

		public float BoundsScheduleTime = 1f;

		public float ElapsedTime;

		public class VertexSegment
		{
			public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
			{
				this.VertStart = start;
				this.VertCount = count;
				this.IndexCount = icount;
				this.IndexStart = istart;
				this.Pool = pool;
			}

			public void ClearIndices()
			{
				for (int i = this.IndexStart; i < this.IndexStart + this.IndexCount; i++)
				{
					this.Pool.Indices[i] = 0;
				}
				this.Pool.IndiceChanged = true;
			}

			public int VertStart;

			public int IndexStart;

			public int VertCount;

			public int IndexCount;

			public VertexPool Pool;
		}
	}
}
