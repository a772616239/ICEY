using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/SRText")]
	public class SRText : Text
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<SRText> LayoutDirty;

		public override void SetLayoutDirty()
		{
			base.SetLayoutDirty();
			if (this.LayoutDirty != null)
			{
				this.LayoutDirty(this);
			}
		}
	}
}
