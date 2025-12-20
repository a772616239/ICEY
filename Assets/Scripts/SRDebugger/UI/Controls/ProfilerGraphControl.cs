using System;
using System.Collections.Generic;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer))]
	public class ProfilerGraphControl : Graphic
	{
		protected override void Awake()
		{
			base.Awake();
			this._profilerService = SRServiceManager.GetService<IProfilerService>();
		}

		protected override void Start()
		{
			base.Start();
		}

		protected void Update()
		{
			this.SetVerticesDirty();
		}

		[Obsolete]
		protected override void OnPopulateMesh(Mesh m)
		{
			this._meshVertices.Clear();
			this._meshVertexColors.Clear();
			this._meshTriangles.Clear();
			float width = base.rectTransform.rect.width;
			float height = base.rectTransform.rect.height;
			this._clipBounds = new Rect(0f, 0f, width, height);
			int num = this.TargetFps;
			if (Application.isPlaying && this.TargetFpsUseApplication && Application.targetFrameRate > 0)
			{
				num = Application.targetFrameRate;
			}
			float num2 = 1f / (float)num;
			int num3 = -1;
			float num4 = (!this.FloatingScale) ? (1f / (float)num) : this.CalculateMaxFrameTime();
			if (this.FloatingScale)
			{
				for (int i = 0; i < ProfilerGraphControl.ScaleSteps.Length; i++)
				{
					float num5 = ProfilerGraphControl.ScaleSteps[i];
					if (num4 < num5 * 1.1f)
					{
						num2 = num5;
						num3 = i;
						break;
					}
				}
				if (num3 < 0)
				{
					num3 = ProfilerGraphControl.ScaleSteps.Length - 1;
					num2 = ProfilerGraphControl.ScaleSteps[num3];
				}
			}
			else
			{
				for (int j = 0; j < ProfilerGraphControl.ScaleSteps.Length; j++)
				{
					float num6 = ProfilerGraphControl.ScaleSteps[j];
					if (num4 > num6)
					{
						num3 = j;
					}
				}
			}
			float num7 = (height - (float)(this.VerticalPadding * 2)) / num2;
			int num8 = this.CalculateVisibleDataPointCount();
			int frameBufferCurrentSize = this.GetFrameBufferCurrentSize();
			for (int k = 0; k < frameBufferCurrentSize; k++)
			{
				if (k >= num8)
				{
					break;
				}
				ProfilerFrame frame = this.GetFrame(frameBufferCurrentSize - k - 1);
				float xPosition = width - 4f * (float)k - 4f - width / 2f;
				this.DrawDataPoint(xPosition, num7, frame);
			}
			if (this.DrawAxes)
			{
				if (!this.FloatingScale)
				{
					this.DrawAxis(num2, num2 * num7, this.GetAxisLabel(0));
				}
				int num9 = 2;
				int num10 = 0;
				if (!this.FloatingScale)
				{
					num10++;
				}
				for (int l = num3; l >= 0; l--)
				{
					if (num10 >= num9)
					{
						break;
					}
					this.DrawAxis(ProfilerGraphControl.ScaleSteps[l], ProfilerGraphControl.ScaleSteps[l] * num7, this.GetAxisLabel(num10));
					num10++;
				}
			}
			m.Clear();
			m.SetVertices(this._meshVertices);
			m.SetColors(this._meshVertexColors);
			m.SetTriangles(this._meshTriangles, 0);
		}

		protected void DrawDataPoint(float xPosition, float verticalScale, ProfilerFrame frame)
		{
			float x = Mathf.Min(this._clipBounds.width / 2f, xPosition + 4f - 2f);
			float num = 0f;
			for (int i = 0; i < 3; i++)
			{
				int num2 = i;
				float num3 = 0f;
				if (i == 0)
				{
					num3 = (float)frame.UpdateTime;
				}
				else if (i == 1)
				{
					num3 = (float)frame.RenderTime;
				}
				else if (i == 2)
				{
					num3 = (float)frame.OtherTime;
				}
				num3 *= verticalScale;
				if (!num3.ApproxZero() && num3 - 4f >= 0f)
				{
					float num4 = num + 2f - base.rectTransform.rect.height / 2f;
					if (this.VerticalAlignment == ProfilerGraphControl.VerticalAlignments.Top)
					{
						num4 = base.rectTransform.rect.height / 2f - num - 2f;
					}
					float y = num4 + num3 - 2f;
					if (this.VerticalAlignment == ProfilerGraphControl.VerticalAlignments.Top)
					{
						y = num4 - num3 + 2f;
					}
					Color c = this.LineColours[num2];
					this.AddRect(new Vector3(Mathf.Max(-this._clipBounds.width / 2f, xPosition), num4), new Vector3(Mathf.Max(-this._clipBounds.width / 2f, xPosition), y), new Vector3(x, y), new Vector3(x, num4), c);
					num += num3;
				}
			}
		}

		protected void DrawAxis(float frameTime, float yPosition, ProfilerGraphAxisLabel label)
		{
			float num = -base.rectTransform.rect.width * 0.5f;
			float x = -num;
			float y = yPosition - base.rectTransform.rect.height * 0.5f + 0.5f;
			float y2 = yPosition - base.rectTransform.rect.height * 0.5f - 0.5f;
			Color c = new Color(1f, 1f, 1f, 0.4f);
			this.AddRect(new Vector3(num, y2), new Vector3(num, y), new Vector3(x, y), new Vector3(x, y2), c);
			if (label != null)
			{
				label.SetValue(frameTime, yPosition);
			}
		}

		protected void AddRect(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br, Color c)
		{
			this._meshVertices.Add(tl);
			this._meshVertices.Add(tr);
			this._meshVertices.Add(bl);
			this._meshVertices.Add(br);
			this._meshTriangles.Add(this._meshVertices.Count - 4);
			this._meshTriangles.Add(this._meshVertices.Count - 3);
			this._meshTriangles.Add(this._meshVertices.Count - 1);
			this._meshTriangles.Add(this._meshVertices.Count - 2);
			this._meshTriangles.Add(this._meshVertices.Count - 1);
			this._meshTriangles.Add(this._meshVertices.Count - 3);
			this._meshVertexColors.Add(c);
			this._meshVertexColors.Add(c);
			this._meshVertexColors.Add(c);
			this._meshVertexColors.Add(c);
		}

		protected ProfilerFrame GetFrame(int i)
		{
			return this._profilerService.FrameBuffer[i];
		}

		protected int CalculateVisibleDataPointCount()
		{
			return Mathf.RoundToInt(base.rectTransform.rect.width / 4f);
		}

		protected int GetFrameBufferCurrentSize()
		{
			return this._profilerService.FrameBuffer.Count;
		}

		protected int GetFrameBufferMaxSize()
		{
			return this._profilerService.FrameBuffer.Capacity;
		}

		protected float CalculateMaxFrameTime()
		{
			int frameBufferCurrentSize = this.GetFrameBufferCurrentSize();
			int num = Mathf.Min(this.CalculateVisibleDataPointCount(), frameBufferCurrentSize);
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				int i2 = frameBufferCurrentSize - i - 1;
				ProfilerFrame frame = this.GetFrame(i2);
				if (frame.FrameTime > num2)
				{
					num2 = frame.FrameTime;
				}
			}
			return (float)num2;
		}

		private ProfilerGraphAxisLabel GetAxisLabel(int index)
		{
			if (this._axisLabels == null || !Application.isPlaying)
			{
				this._axisLabels = base.GetComponentsInChildren<ProfilerGraphAxisLabel>();
			}
			if (this._axisLabels.Length > index)
			{
				return this._axisLabels[index];
			}
			UnityEngine.Debug.LogWarning("[SRDebugger.Profiler] Not enough axis labels in pool");
			return null;
		}

		public ProfilerGraphControl.VerticalAlignments VerticalAlignment = ProfilerGraphControl.VerticalAlignments.Bottom;

		private static readonly float[] ScaleSteps = new float[]
		{
			0.005f,
			0.00625f,
			0.008333334f,
			0.01f,
			0.0166666675f,
			0.0333333351f,
			0.05f,
			0.0833333358f,
			0.166666672f
		};

		public bool FloatingScale;

		public bool TargetFpsUseApplication;

		public bool DrawAxes = true;

		public int TargetFps = 60;

		public bool Clip = true;

		public const float DataPointMargin = 2f;

		public const float DataPointVerticalMargin = 2f;

		public const float DataPointWidth = 4f;

		public int VerticalPadding = 10;

		public const int LineCount = 3;

		public Color[] LineColours = new Color[0];

		private IProfilerService _profilerService;

		private ProfilerGraphAxisLabel[] _axisLabels;

		private Rect _clipBounds;

		private readonly List<Vector3> _meshVertices = new List<Vector3>();

		private readonly List<Color32> _meshVertexColors = new List<Color32>();

		private readonly List<int> _meshTriangles = new List<int>();

		public enum VerticalAlignments
		{
			Top,
			Bottom
		}
	}
}
