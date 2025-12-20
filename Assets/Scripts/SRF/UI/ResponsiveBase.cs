using System;
using UnityEngine;

namespace SRF.UI
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public abstract class ResponsiveBase : SRMonoBehaviour
	{
		protected RectTransform RectTransform
		{
			get
			{
				return (RectTransform)base.CachedTransform;
			}
		}

		protected void OnEnable()
		{
			this._queueRefresh = true;
		}

		protected void OnRectTransformDimensionsChange()
		{
			this._queueRefresh = true;
		}

		protected void Update()
		{
			if (this._queueRefresh)
			{
				this.Refresh();
				this._queueRefresh = false;
			}
		}

		protected abstract void Refresh();

		[ContextMenu("Refresh")]
		private void DoRefresh()
		{
			this.Refresh();
		}

		private bool _queueRefresh;
	}
}
