using System;
using System.Diagnostics;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Profiler
{
	[Service(typeof(IProfilerService))]
	public class ProfilerServiceImpl : SRServiceBase<IProfilerService>, IProfilerService
	{
		public float AverageFrameTime { get; private set; }

		public float LastFrameTime { get; private set; }

		public CircularBuffer<ProfilerFrame> FrameBuffer
		{
			get
			{
				return this._frameBuffer;
			}
		}

		protected void PushFrame(double totalTime, double updateTime, double renderTime)
		{
			this._frameBuffer.PushBack(new ProfilerFrame
			{
				OtherTime = totalTime - updateTime - renderTime,
				UpdateTime = updateTime,
				RenderTime = renderTime
			});
		}

		protected override void Awake()
		{
			base.Awake();
			this._lateUpdateListener = base.gameObject.AddComponent<ProfilerLateUpdateListener>();
			this._lateUpdateListener.OnLateUpdate = new Action(this.OnLateUpdate);
			base.CachedGameObject.hideFlags = HideFlags.NotEditable;
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"), true);
		}

		protected override void Update()
		{
			base.Update();
			if (this.FrameBuffer.Count > 0)
			{
				ProfilerFrame value = this.FrameBuffer.Back();
				value.FrameTime = (double)Time.deltaTime;
				this.FrameBuffer[this.FrameBuffer.Count - 1] = value;
			}
			this.LastFrameTime = Time.deltaTime;
			int num = Mathf.Min(20, this.FrameBuffer.Count);
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				num2 += this.FrameBuffer[i].FrameTime;
			}
			this.AverageFrameTime = (float)num2 / (float)num;
			if (this._reportedCameras != this._cameraListeners.Count)
			{
			}
			if (this._stopwatch.IsRunning)
			{
				this._stopwatch.Stop();
				this._stopwatch.Reset();
			}
			this._updateDuration = (this._renderDuration = (this._updateToRenderDuration = 0.0));
			this._reportedCameras = 0;
			this.CameraCheck();
			this._stopwatch.Start();
		}

		private void OnLateUpdate()
		{
			this._updateDuration = this._stopwatch.Elapsed.TotalSeconds;
		}

		private void EndFrame()
		{
			if (this._stopwatch.IsRunning)
			{
				this.PushFrame(this._stopwatch.Elapsed.TotalSeconds, this._updateDuration, this._renderDuration);
				this._stopwatch.Reset();
				this._stopwatch.Start();
			}
		}

		private void CameraDurationCallback(ProfilerCameraListener listener, double duration)
		{
			this._reportedCameras++;
			this._renderDuration = this._stopwatch.Elapsed.TotalSeconds - this._updateDuration - this._updateToRenderDuration;
			if (this._reportedCameras >= this.GetExpectedCameraCount())
			{
				this.EndFrame();
			}
		}

		private int GetExpectedCameraCount()
		{
			int num = 0;
			for (int i = 0; i < this._cameraListeners.Count; i++)
			{
				if (!(this._cameraListeners[i] != null) || (this._cameraListeners[i].isActiveAndEnabled && this._cameraListeners[i].Camera.isActiveAndEnabled))
				{
					num++;
				}
			}
			return num;
		}

		private void CameraCheck()
		{
			for (int i = this._cameraListeners.Count - 1; i >= 0; i--)
			{
				if (this._cameraListeners[i] == null)
				{
					this._cameraListeners.RemoveAt(i);
				}
			}
			if (Camera.allCamerasCount == this._cameraListeners.Count)
			{
				return;
			}
			if (Camera.allCamerasCount > this._cameraCache.Length)
			{
				this._cameraCache = new Camera[Camera.allCamerasCount];
			}
			int allCameras = Camera.GetAllCameras(this._cameraCache);
			for (int j = 0; j < allCameras; j++)
			{
				Camera camera = this._cameraCache[j];
				bool flag = false;
				for (int k = 0; k < this._cameraListeners.Count; k++)
				{
					if (this._cameraListeners[k].Camera == camera)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ProfilerCameraListener profilerCameraListener = camera.gameObject.AddComponent<ProfilerCameraListener>();
					profilerCameraListener.hideFlags = (HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
					profilerCameraListener.RenderDurationCallback = new Action<ProfilerCameraListener, double>(this.CameraDurationCallback);
					this._cameraListeners.Add(profilerCameraListener);
				}
			}
		}

		private const int FrameBufferSize = 400;

		private readonly SRList<ProfilerCameraListener> _cameraListeners = new SRList<ProfilerCameraListener>();

		private readonly CircularBuffer<ProfilerFrame> _frameBuffer = new CircularBuffer<ProfilerFrame>(400);

		private Camera[] _cameraCache = new Camera[6];

		private ProfilerLateUpdateListener _lateUpdateListener;

		private double _renderDuration;

		private int _reportedCameras;

		private Stopwatch _stopwatch = new Stopwatch();

		private double _updateDuration;

		private double _updateToRenderDuration;
	}
}
