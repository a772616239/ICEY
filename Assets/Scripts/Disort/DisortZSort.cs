using System;
using Distort;
using UnityEngine;

namespace Disort
{
	public class DisortZSort : MonoBehaviour
	{
		private void Start()
		{
			DisortZSort.DistortCycleGenNum++;
			Vector3 position = base.transform.position;
			position.z -= (float)DisortZSort.DistortCycleGenNum * 0.001f;
			base.transform.position = position;
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
			if (this.controller != null)
			{
				this.controller.Close();
			}
			DisortZSort.DistortCycleDisNum++;
			if (DisortZSort.DistortCycleDisNum == DisortZSort.DistortCycleGenNum)
			{
				DisortZSort.DistortCycleDisNum = 0;
				DisortZSort.DistortCycleGenNum = 0;
			}
		}

		public static int DistortCycleGenNum;

		public static int DistortCycleDisNum;

		public FXCameraController controller;
	}
}
