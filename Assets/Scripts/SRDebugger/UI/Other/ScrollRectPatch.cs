using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	[RequireComponent(typeof(ScrollRect))]
	[ExecuteInEditMode]
	public class ScrollRectPatch : MonoBehaviour
	{
		private void Awake()
		{
			ScrollRect component = base.GetComponent<ScrollRect>();
			component.content = this.Content;
			component.viewport = this.Viewport;
			if (this.ReplaceMask != null)
			{
				GameObject gameObject = this.ReplaceMask.gameObject;
				UnityEngine.Object.Destroy(gameObject.GetComponent<Graphic>());
				UnityEngine.Object.Destroy(gameObject.GetComponent<CanvasRenderer>());
				UnityEngine.Object.Destroy(this.ReplaceMask);
				gameObject.AddComponent<RectMask2D>();
			}
		}

		public RectTransform Content;

		public Mask ReplaceMask;

		public RectTransform Viewport;
	}
}
