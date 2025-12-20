using System;
using UnityEngine;

namespace BasicTools
{
	public class CameraFacingBillboard : MonoBehaviour
	{
		private void Awake()
		{
			if (this.autoInit)
			{
				if (this.m_Camera == null)
				{
					this.m_Camera = Camera.main;
				}
				this.amActive = true;
			}
		}

		private void Update()
		{
			if (this.amActive)
			{
				base.transform.LookAt(this.m_Camera.transform);
			}
		}

		public Camera m_Camera;

		public bool amActive;

		public bool autoInit;

		private GameObject myContainer;
	}
}
