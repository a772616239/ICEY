using System;
using UnityEngine;

namespace Distort
{
	[RequireComponent(typeof(Camera))]
	public class FXCameraController : MonoBehaviour
	{
		private void Start()
		{
			this._fxCamera = base.GetComponent<Camera>();
		}

		private void Update()
		{
		}

		public void Open()
		{
			if (this._openCount == 0)
			{
				this._fxCamera.enabled = true;
			}
			this._openCount++;
		}

		public void Close()
		{
			this._openCount = Mathf.Clamp(this._openCount - 1, 0, this._openCount);
			if (this._openCount == 0)
			{
				this._fxCamera.enabled = false;
			}
		}

		private Camera _fxCamera;

		private int _openCount;
	}
}
