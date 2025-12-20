using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRF;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class DockConsoleController : SRMonoBehaviourEx, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		public bool IsVisible
		{
			get
			{
				return base.CachedGameObject.activeSelf;
			}
			set
			{
				base.CachedGameObject.SetActive(value);
			}
		}

		protected override void Start()
		{
			base.Start();
			Service.Console.Updated += this.ConsoleOnUpdated;
			this.Refresh();
			this.RefreshAlpha();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (Service.Console != null)
			{
				Service.Console.Updated -= this.ConsoleOnUpdated;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this._pointersOver = 0;
			this._isDragging = false;
			this.RefreshAlpha();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			this._pointersOver = 0;
		}

		protected override void Update()
		{
			base.Update();
			if (this._isDirty)
			{
				this.Refresh();
			}
		}

		private void ConsoleOnUpdated(IConsoleService console)
		{
			this._isDirty = true;
		}

		public void SetDropdownVisibility(bool visible)
		{
			this.Dropdown.SetActive(visible);
			this.DropdownToggleSprite.rectTransform.localRotation = Quaternion.Euler(0f, 0f, (!visible) ? 180f : 0f);
		}

		public void SetAlignmentMode(ConsoleAlignment alignment)
		{
			if (alignment != ConsoleAlignment.Top)
			{
				if (alignment == ConsoleAlignment.Bottom)
				{
					this.Dropdown.transform.SetSiblingIndex(0);
					this.TopBar.transform.SetSiblingIndex(2);
					this.TopHandle.SetActive(true);
					this.BottomHandle.SetActive(false);
					base.transform.SetSiblingIndex(1);
					this.DropdownToggleSprite.rectTransform.parent.localRotation = Quaternion.Euler(0f, 0f, 180f);
				}
			}
			else
			{
				this.TopBar.transform.SetSiblingIndex(0);
				this.Dropdown.transform.SetSiblingIndex(2);
				this.TopHandle.SetActive(false);
				this.BottomHandle.SetActive(true);
				base.transform.SetSiblingIndex(0);
				this.DropdownToggleSprite.rectTransform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}

		private void Refresh()
		{
			this.TextInfo.text = SRDebuggerUtil.GetNumberString(Service.Console.InfoCount, 999, "999+");
			this.TextWarnings.text = SRDebuggerUtil.GetNumberString(Service.Console.WarningCount, 999, "999+");
			this.TextErrors.text = SRDebuggerUtil.GetNumberString(Service.Console.ErrorCount, 999, "999+");
			this._isDirty = false;
		}

		private void RefreshAlpha()
		{
			if (this._isDragging || this._pointersOver > 0)
			{
				this.CanvasGroup.alpha = 1f;
			}
			else
			{
				this.CanvasGroup.alpha = 0.65f;
			}
		}

		public void ToggleDropdownVisible()
		{
			this.SetDropdownVisibility(!this.Dropdown.activeSelf);
		}

		public void MenuButtonPressed()
		{
			SRDebug.Instance.ShowDebugPanel(DefaultTabs.Console, true);
		}

		public void ClearButtonPressed()
		{
			Service.Console.Clear();
		}

		public void TogglesUpdated()
		{
			this.Console.ShowErrors = this.ToggleErrors.isOn;
			this.Console.ShowWarnings = this.ToggleWarnings.isOn;
			this.Console.ShowInfo = this.ToggleInfo.isOn;
			this.SetDropdownVisibility(true);
		}

		public void OnPointerEnter(PointerEventData e)
		{
			this._pointersOver = 1;
			this.RefreshAlpha();
		}

		public void OnPointerExit(PointerEventData e)
		{
			this._pointersOver = 0;
			this.RefreshAlpha();
		}

		public void OnBeginDrag()
		{
			this._isDragging = true;
			this.RefreshAlpha();
		}

		public void OnEndDrag()
		{
			this._isDragging = false;
			this._pointersOver = 0;
			this.RefreshAlpha();
		}

		public const float NonFocusOpacity = 0.65f;

		private bool _isDirty;

		private bool _isDragging;

		private int _pointersOver;

		[RequiredField]
		public GameObject BottomHandle;

		[RequiredField]
		public CanvasGroup CanvasGroup;

		[RequiredField]
		public ConsoleLogControl Console;

		[RequiredField]
		public GameObject Dropdown;

		[RequiredField]
		public Image DropdownToggleSprite;

		[RequiredField]
		public Text TextErrors;

		[RequiredField]
		public Text TextInfo;

		[RequiredField]
		public Text TextWarnings;

		[RequiredField]
		public Toggle ToggleErrors;

		[RequiredField]
		public Toggle ToggleInfo;

		[RequiredField]
		public Toggle ToggleWarnings;

		[RequiredField]
		public GameObject TopBar;

		[RequiredField]
		public GameObject TopHandle;
	}
}
