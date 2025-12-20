using System;
using UnityEngine;

namespace SRDebugger.Profiler
{
	public class ProfilerLateUpdateListener : MonoBehaviour
	{
		private void LateUpdate()
		{
			if (this.OnLateUpdate != null)
			{
				this.OnLateUpdate();
			}
		}

		public Action OnLateUpdate;
	}
}
