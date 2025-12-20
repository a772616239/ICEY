using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/Style Component")]
	[ExecuteInEditMode]
	public class StyleComponent : SRMonoBehaviour
	{
		public string StyleKey
		{
			get
			{
				return this._styleKey;
			}
			set
			{
				this._styleKey = value;
				this.Refresh(false);
			}
		}

		private void Start()
		{
			this.Refresh(true);
			this._hasStarted = true;
		}

		private void OnEnable()
		{
			if (this._hasStarted)
			{
				this.Refresh(false);
			}
		}

		public void Refresh(bool invalidateCache)
		{
			if (string.IsNullOrEmpty(this.StyleKey))
			{
				this._activeStyle = null;
				return;
			}
			if (this._cachedRoot == null || invalidateCache)
			{
				this._cachedRoot = this.GetStyleRoot();
			}
			if (this._cachedRoot == null)
			{
				UnityEngine.Debug.LogWarning("[StyleComponent] No active StyleRoot object found in parents.", this);
				this._activeStyle = null;
				return;
			}
			Style style = this._cachedRoot.GetStyle(this.StyleKey);
			if (style == null)
			{
				UnityEngine.Debug.LogWarning("[StyleComponent] Style not found ({0})".Fmt(new object[]
				{
					this.StyleKey
				}), this);
				this._activeStyle = null;
				return;
			}
			this._activeStyle = style;
			this.ApplyStyle();
		}

		private StyleRoot GetStyleRoot()
		{
			Transform transform = base.CachedTransform;
			int num = 0;
			StyleRoot componentInParent;
			for (;;)
			{
				componentInParent = transform.GetComponentInParent<StyleRoot>();
				if (componentInParent != null)
				{
					transform = componentInParent.transform.parent;
				}
				num++;
				if (num > 100)
				{
					break;
				}
				if (!(componentInParent != null) || componentInParent.enabled || !(transform != null))
				{
					return componentInParent;
				}
			}
			UnityEngine.Debug.LogWarning("Breaking Loop");
			return componentInParent;
		}

		private void ApplyStyle()
		{
			if (this._activeStyle == null)
			{
				return;
			}
			if (this._graphic == null)
			{
				this._graphic = base.GetComponent<Graphic>();
			}
			if (this._selectable == null)
			{
				this._selectable = base.GetComponent<Selectable>();
			}
			if (this._image == null)
			{
				this._image = base.GetComponent<Image>();
			}
			if (!this.IgnoreImage && this._image != null)
			{
				this._image.sprite = this._activeStyle.Image;
			}
			if (this._selectable != null)
			{
				ColorBlock colors = this._selectable.colors;
				colors.normalColor = this._activeStyle.NormalColor;
				colors.highlightedColor = this._activeStyle.HoverColor;
				colors.pressedColor = this._activeStyle.ActiveColor;
				colors.disabledColor = this._activeStyle.DisabledColor;
				colors.colorMultiplier = 1f;
				this._selectable.colors = colors;
				if (this._graphic != null)
				{
					this._graphic.color = Color.white;
				}
			}
			else if (this._graphic != null)
			{
				this._graphic.color = this._activeStyle.NormalColor;
			}
		}

		private void SRStyleDirty()
		{
			if (!base.CachedGameObject.activeInHierarchy)
			{
				this._cachedRoot = null;
				return;
			}
			this.Refresh(true);
		}

		private Style _activeStyle;

		private StyleRoot _cachedRoot;

		private Graphic _graphic;

		private bool _hasStarted;

		private Image _image;

		private Selectable _selectable;

		[HideInInspector]
		[FormerlySerializedAs("StyleKey")]
		[SerializeField]
		private string _styleKey;

		public bool IgnoreImage;
	}
}
