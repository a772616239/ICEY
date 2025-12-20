using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Graphic))]
	[AddComponentMenu("SRF/UI/Inherit Colour")]
	public class InheritColour : SRMonoBehaviour
	{
		private Graphic Graphic
		{
			get
			{
				if (this._graphic == null)
				{
					this._graphic = base.GetComponent<Graphic>();
				}
				return this._graphic;
			}
		}

		private void Refresh()
		{
			if (this.From == null)
			{
				return;
			}
			this.Graphic.color = this.From.canvasRenderer.GetColor();
		}

		private void Update()
		{
			this.Refresh();
		}

		private void Start()
		{
			this.Refresh();
		}

		private Graphic _graphic;

		public Graphic From;
	}
}
