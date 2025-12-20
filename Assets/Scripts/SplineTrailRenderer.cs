using System;
using System.Collections.Generic;
using UnityEngine;

public class SplineTrailRenderer : MonoBehaviour
{
	public void Clear()
	{
		this.Init();
	}

	private void Awake()
	{
		this.spline = new CatmullRomSpline();
	}

	private void Start()
	{
		this.Init();
	}

	public void Launch(Transform target)
	{
		this.spline.Launch(target);
		this.targetTrans = target;
	}

	private void Update()
	{
		if (this.emit)
		{
			List<Knot> knots = this.spline.knots;
			Vector3 position = base.transform.position;
			knots[knots.Count - 1].position = position;
			knots[knots.Count - 2].position = position;
			if (Vector3.Distance(knots[knots.Count - 3].position, position) > this.emissionDistance && Vector3.Distance(knots[knots.Count - 4].position, position) > this.emissionDistance)
			{
				knots.Add(new Knot(position));
				float num = Mathf.Max(this.spline.Length() - 0.1f, 0f);
				if (num > this.maxLength && knots.Count > 0)
				{
					knots.RemoveAt(0);
				}
			}
		}
		this.RenderMesh();
		this.spline.SetLastPos(this.targetTrans.position);
	}

	private float GetNoise(float p1, float p2)
	{
		return 1f - Mathf.PerlinNoise(p1, p2) * 2f;
	}

	private void RenderMesh()
	{
		if (this.advancedParameters.nbSegmentToParametrize == 0)
		{
			this.spline.Parametrize();
		}
		else
		{
			this.spline.Parametrize(this.spline.NbSegments - this.advancedParameters.nbSegmentToParametrize, this.spline.NbSegments);
		}
		float num = Mathf.Max(this.spline.Length() - 0.1f, 0f);
		if (this.noiseAmp > 0f)
		{
		}
		int num2 = (int)(1f / this.width * num) + 1 - this.quadOffset;
		if (this.allocatedNbQuad < num2)
		{
			this.Reallocate(num2);
			num = Mathf.Max(this.spline.Length() - 0.1f, 0f);
			num2 = (int)(1f / this.width * num) + 1 - this.quadOffset;
		}
		if (this.noiseAmp > 0f)
		{
		}
		int num3 = this.lastStartingQuad;
		float num4 = (float)num3 * this.width + (float)this.quadOffset * this.width;
		this.maxInstanciedTriCount = Math.Max(this.maxInstanciedTriCount, (num2 - 1) * 6);
		CatmullRomSpline.Marker marker = new CatmullRomSpline.Marker();
		this.spline.PlaceMarker(marker, num4, null);
		Vector3 a = this.spline.GetPosition(marker);
		Vector3 vector = this.spline.GetTangent(marker);
		Vector3 vector2 = CatmullRomSpline.ComputeBinormal(vector, this.normal);
		int num5 = (this.meshDisposition != SplineTrailRenderer.MeshDisposition.Fragmented) ? (num2 - 1) : (num2 - 1);
		for (int i = num3; i < num5; i++)
		{
			float num6 = num4 + this.width;
			int num7 = i * 4;
			if (num7 > this.vertices.Length - 3)
			{
				break;
			}
			int num8 = i * 6;
			this.spline.MoveMarker(marker, num6);
			Vector3 position = this.spline.GetPosition(marker);
			Vector3 tangent = this.spline.GetTangent(marker);
			Vector3 vector3 = CatmullRomSpline.ComputeBinormal(tangent, this.normal);
			float num9 = this.FadeMultiplier(num4, num);
			float num10 = this.FadeMultiplier(num6, num);
			float num11 = num9 * this.height;
			float num12 = num10 * this.height;
			if (this.fadeType == SplineTrailRenderer.FadeType.Alpha || this.fadeType == SplineTrailRenderer.FadeType.None)
			{
				num11 = ((num9 <= 0f) ? 0f : this.height);
				num12 = ((num10 <= 0f) ? 0f : this.height);
			}
			bool flag = true;
			if (this.noiseAmp <= 0f || num11 == 0f || num12 == 0f || i < num3 + 1)
			{
				flag = false;
			}
			if (this.meshDisposition == SplineTrailRenderer.MeshDisposition.Continuous)
			{
				if (flag)
				{
					float time = Time.time;
					Vector3 vector4 = new Vector3(this.GetNoise((float)num7 * this.noiseFrequence, time) * this.noiseAmp, this.GetNoise((float)num7 * this.noiseFrequence + 0.01f, 0f) * this.noiseAmp, 0f);
					this.vertices[num7] = base.transform.InverseTransformPoint(a - this.origin + vector2 * (num11 * 0.5f) + Vector3.Project(vector4, vector2));
					vector4 = new Vector3(this.GetNoise((float)num7 * this.noiseFrequence + 0.02f, time) * this.noiseAmp, this.GetNoise((float)num7 * this.noiseFrequence + 0.03f, 0f) * this.noiseAmp, 0f);
					this.vertices[num7 + 1] = base.transform.InverseTransformPoint(a - this.origin + -vector2 * (num11 * 0.5f) + Vector3.Project(vector4, vector2));
					vector4 = new Vector3(this.GetNoise((float)num7 * this.noiseFrequence + 0.04f, time) * this.noiseAmp, this.GetNoise((float)num7 * this.noiseFrequence + 0.05f, 0f) * this.noiseAmp, 0f);
					this.vertices[num7 + 2] = base.transform.InverseTransformPoint(position - this.origin + vector3 * (num12 * 0.5f) + Vector3.Project(vector4, vector3));
					vector4 = new Vector3(this.GetNoise((float)num7 * this.noiseFrequence + 0.06f, time) * this.noiseAmp, this.GetNoise((float)num7 * this.noiseFrequence + 0.07f, 0f) * this.noiseAmp, 0f);
					this.vertices[num7 + 3] = base.transform.InverseTransformPoint(position - this.origin + -vector3 * (num12 * 0.5f) + Vector3.Project(vector4, vector3));
				}
				else
				{
					this.vertices[num7] = base.transform.InverseTransformPoint(a - this.origin + vector2 * (num11 * 0.5f));
					this.vertices[num7 + 1] = base.transform.InverseTransformPoint(a - this.origin + -vector2 * (num11 * 0.5f));
					this.vertices[num7 + 2] = base.transform.InverseTransformPoint(position - this.origin + vector3 * (num12 * 0.5f));
					this.vertices[num7 + 3] = base.transform.InverseTransformPoint(position - this.origin + -vector3 * (num12 * 0.5f));
				}
				this.uv[num7] = new Vector2(num4 / this.height, 1f);
				this.uv[num7 + 1] = new Vector2(num4 / this.height, 0f);
				this.uv[num7 + 2] = new Vector2(num6 / this.height, 1f);
				this.uv[num7 + 3] = new Vector2(num6 / this.height, 0f);
			}
			else
			{
				Vector3 a2 = a + vector * this.width * -0.5f - this.origin;
				this.vertices[num7] = base.transform.InverseTransformPoint(a2 + vector2 * (num11 * 0.5f));
				this.vertices[num7 + 1] = base.transform.InverseTransformPoint(a2 + -vector2 * (num11 * 0.5f));
				this.vertices[num7 + 2] = base.transform.InverseTransformPoint(a2 + vector * this.width + vector2 * (num11 * 0.5f));
				this.vertices[num7 + 3] = base.transform.InverseTransformPoint(a2 + vector * this.width + -vector2 * (num11 * 0.5f));
				this.uv[num7] = new Vector2(0f, 1f);
				this.uv[num7 + 1] = new Vector2(0f, 0f);
				this.uv[num7 + 2] = new Vector2(1f, 1f);
				this.uv[num7 + 3] = new Vector2(1f, 0f);
			}
			this.triangles[num8] = num7;
			this.triangles[num8 + 1] = num7 + 1;
			this.triangles[num8 + 2] = num7 + 2;
			this.triangles[num8 + 3] = num7 + 2;
			this.triangles[num8 + 4] = num7 + 1;
			this.triangles[num8 + 5] = num7 + 3;
			this.colors[num7] = this.vertexColor;
			this.colors[num7 + 1] = this.vertexColor;
			this.colors[num7 + 2] = this.vertexColor;
			this.colors[num7 + 3] = this.vertexColor;
			if (this.fadeType == SplineTrailRenderer.FadeType.Alpha || this.fadeType == SplineTrailRenderer.FadeType.Both)
			{
				if (i < num3 + 10)
				{
					this.colors[num7].a = 0f;
					this.colors[num7 + 1].a = 0f;
					this.colors[num7 + 2].a = 0f;
					this.colors[num7 + 3].a = 0f;
				}
				else
				{
					Color[] array = this.colors;
					int num13 = num7;
					array[num13].a = array[num13].a * num9;
					Color[] array2 = this.colors;
					int num14 = num7 + 1;
					array2[num14].a = array2[num14].a * num9;
					Color[] array3 = this.colors;
					int num15 = num7 + 2;
					array3[num15].a = array3[num15].a * num10;
					Color[] array4 = this.colors;
					int num16 = num7 + 3;
					array4[num16].a = array4[num16].a * num10;
				}
			}
			a = position;
			vector = tangent;
			vector2 = vector3;
			num4 = num6;
		}
		for (int j = (num2 - 1) * 6; j < this.maxInstanciedTriCount; j++)
		{
			if (j < this.triangles.Length)
			{
				this.triangles[j] = 0;
			}
		}
		this.lastStartingQuad = ((this.advancedParameters.lengthToRedraw != 0f) ? Math.Max(0, num2 - ((int)(this.advancedParameters.lengthToRedraw / this.width) + 5)) : Math.Max(0, num2 - ((int)(this.maxLength / this.width) + 5)));
		this.mesh.Clear();
		this.mesh.vertices = this.vertices;
		this.mesh.uv = this.uv;
		this.mesh.triangles = this.triangles;
		this.mesh.colors = this.colors;
		this.mesh.normals = this.normals;
	}

	private void OnDrawGizmos()
	{
		if (this.advancedParameters == null || this.spline == null || this.debugDrawSpline)
		{
		}
	}

	private void Init()
	{
		this.origin = Vector3.zero;
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		this.mesh.MarkDynamic();
		this.allocatedNbQuad = this.advancedParameters.baseNbQuad;
		this.maxInstanciedTriCount = 0;
		this.lastStartingQuad = 0;
		this.quadOffset = 0;
		this.vertices = new Vector3[this.advancedParameters.baseNbQuad * 4];
		this.triangles = new int[this.advancedParameters.baseNbQuad * 6];
		this.uv = new Vector2[this.advancedParameters.baseNbQuad * 4];
		this.colors = new Color[this.advancedParameters.baseNbQuad * 4];
		this.normals = new Vector3[this.advancedParameters.baseNbQuad * 4];
		if (this.normal == Vector3.zero)
		{
			this.normal = (base.transform.position - Camera.main.transform.position).normalized;
		}
		for (int i = 0; i < this.normals.Length; i++)
		{
			this.normals[i] = this.normal;
		}
		this.spline.Clear();
		List<Knot> knots = this.spline.knots;
		Vector3 position = base.transform.position;
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
	}

	private void Reallocate(int nbQuad)
	{
		if (this.advancedParameters.shiftMeshData && this.lastStartingQuad > 0)
		{
			int num = 0;
			for (int i = this.lastStartingQuad; i < nbQuad; i++)
			{
				this.vertices[num] = this.vertices[i];
				this.triangles[num] = this.triangles[i];
				this.uv[num] = this.uv[i];
				this.colors[num] = this.colors[i];
				this.normals[num] = this.normals[i];
				num++;
			}
			this.quadOffset += this.lastStartingQuad;
			this.lastStartingQuad = 0;
		}
		if (this.allocatedNbQuad < nbQuad - this.quadOffset)
		{
			if ((this.allocatedNbQuad + this.advancedParameters.nbQuadIncrement) * 4 > 65000)
			{
				this.Clear();
				return;
			}
			this.allocatedNbQuad += this.advancedParameters.nbQuadIncrement;
			Vector3[] array = new Vector3[this.allocatedNbQuad * 4];
			int[] array2 = new int[this.allocatedNbQuad * 6];
			Vector2[] array3 = new Vector2[this.allocatedNbQuad * 4];
			Color[] array4 = new Color[this.allocatedNbQuad * 4];
			Vector3[] array5 = new Vector3[this.allocatedNbQuad * 4];
			this.vertices.CopyTo(array, 0);
			this.triangles.CopyTo(array2, 0);
			this.uv.CopyTo(array3, 0);
			this.colors.CopyTo(array4, 0);
			this.normals.CopyTo(array5, 0);
			this.vertices = array;
			this.triangles = array2;
			this.uv = array3;
			this.colors = array4;
			this.normals = array5;
		}
	}

	private float FadeMultiplier(float distance, float length)
	{
		float a = Mathf.Clamp01((distance - Mathf.Max(length - this.maxLength, 0f)) / this.fadeLengthBegin);
		float b = Mathf.Clamp01((length - distance) / this.fadeLengthEnd);
		return Mathf.Min(a, b);
	}

	public bool emit = true;

	public float emissionDistance = 1f;

	public float height = 1f;

	public float width = 0.2f;

	public Color vertexColor = Color.white;

	public Vector3 normal = new Vector3(0f, 0f, 1f);

	public SplineTrailRenderer.MeshDisposition meshDisposition;

	public SplineTrailRenderer.FadeType fadeType;

	public float fadeLengthBegin = 5f;

	public float fadeLengthEnd = 5f;

	public float maxLength = 50f;

	public float noiseAmp;

	public float noiseFrequence = 0.01f;

	public bool debugDrawSpline;

	private SplineTrailRenderer.AdvancedParameters advancedParameters = new SplineTrailRenderer.AdvancedParameters();

	[HideInInspector]
	public CatmullRomSpline spline;

	private const int NbVertexPerQuad = 4;

	private const int NbTriIndexPerQuad = 6;

	private Vector3[] vertices;

	private int[] triangles;

	private Vector2[] uv;

	private Color[] colors;

	private Vector3[] normals;

	private Vector3 origin;

	private int maxInstanciedTriCount;

	private Mesh mesh;

	private int allocatedNbQuad;

	private int lastStartingQuad;

	private int quadOffset;

	private Transform targetTrans;

	public enum MeshDisposition
	{
		Continuous,
		Fragmented
	}

	public enum FadeType
	{
		None,
		MeshShrinking,
		Alpha,
		Both
	}

	public class AdvancedParameters
	{
		public int baseNbQuad = 500;

		public int nbQuadIncrement = 200;

		public int nbSegmentToParametrize = 3;

		public float lengthToRedraw;

		public bool shiftMeshData;
	}
}
