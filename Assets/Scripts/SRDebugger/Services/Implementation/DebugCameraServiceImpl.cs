using System;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IDebugCameraService))]
	public class DebugCameraServiceImpl : IDebugCameraService
	{
		public DebugCameraServiceImpl()
		{
			if (Settings.Instance.UseDebugCamera)
			{
				this._debugCamera = new GameObject("SRDebugCamera").AddComponent<Camera>();
				this._debugCamera.cullingMask = 1 << Settings.Instance.DebugLayer;
				this._debugCamera.depth = Settings.Instance.DebugCameraDepth;
				this._debugCamera.clearFlags = CameraClearFlags.Depth;
				this._debugCamera.transform.SetParent(Hierarchy.Get("SRDebugger"));
			}
		}

		public Camera Camera
		{
			get
			{
				return this._debugCamera;
			}
		}

		private Camera _debugCamera;
	}
}
