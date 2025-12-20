using System;
using SRF;
using UnityEngine;

namespace SRDebugger
{
	[AddComponentMenu("SRDebugger Init")]
	public class SRDebuggerInit : SRMonoBehaviourEx
	{
		protected override void Awake()
		{
			base.Awake();
			if (!Settings.Instance.IsEnabled)
			{
				return;
			}
			SRDebug.Init();
		}

		protected override void Start()
		{
			base.Start();
			UnityEngine.Object.Destroy(base.CachedGameObject);
		}
	}
}
