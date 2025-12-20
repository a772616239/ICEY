using System;
using System.Collections;
using SRDebugger.Internal;
using SRDebugger.UI.Other;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(BugReportPopoverService))]
	public class BugReportPopoverService : SRServiceBase<BugReportPopoverService>
	{
		public bool IsShowingPopover
		{
			get
			{
				return this._isVisible;
			}
		}

		public void ShowBugReporter(BugReportCompleteCallback callback, bool takeScreenshotFirst = true, string descriptionText = null)
		{
			if (this._isVisible)
			{
				throw new InvalidOperationException("Bug report popover is already visible.");
			}
			if (this._popover == null)
			{
				this.Load();
			}
			if (this._popover == null)
			{
				UnityEngine.Debug.LogWarning("[SRDebugger] Bug report popover failed loading, executing callback with fail result");
				callback(false, "Resource load failed");
				return;
			}
			this._callback = callback;
			this._isVisible = true;
			SRDebuggerUtil.EnsureEventSystemExists();
			base.StartCoroutine(this.OpenCo(takeScreenshotFirst, descriptionText));
		}

		private IEnumerator OpenCo(bool takeScreenshot, string descriptionText)
		{
			if (takeScreenshot)
			{
				yield return base.StartCoroutine(BugReportScreenshotUtil.ScreenshotCaptureCo());
			}
			this._popover.CachedGameObject.SetActive(true);
			yield return new WaitForEndOfFrame();
			if (!string.IsNullOrEmpty(descriptionText))
			{
				this._sheet.DescriptionField.text = descriptionText;
			}
			yield break;
		}

		private void SubmitComplete(bool didSucceed, string errorMessage)
		{
			this.OnComplete(didSucceed, errorMessage, false);
		}

		private void CancelPressed()
		{
			this.OnComplete(false, "User Cancelled", true);
		}

		private void OnComplete(bool success, string errorMessage, bool close)
		{
			if (!this._isVisible)
			{
				UnityEngine.Debug.LogWarning("[SRDebugger] Received callback at unexpected time. ???");
				return;
			}
			if (!success && !close)
			{
				return;
			}
			this._isVisible = false;
			this._popover.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(this._popover.gameObject);
			this._popover = null;
			this._sheet = null;
			BugReportScreenshotUtil.ScreenshotData = null;
			this._callback(success, errorMessage);
		}

		private void TakingScreenshot()
		{
			if (!this.IsShowingPopover)
			{
				UnityEngine.Debug.LogWarning("[SRDebugger] Received callback at unexpected time. ???");
				return;
			}
			this._popover.CanvasGroup.alpha = 0f;
		}

		private void ScreenshotComplete()
		{
			if (!this.IsShowingPopover)
			{
				UnityEngine.Debug.LogWarning("[SRDebugger] Received callback at unexpected time. ???");
				return;
			}
			this._popover.CanvasGroup.alpha = 1f;
		}

		protected override void Awake()
		{
			base.Awake();
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));
		}

		private void Load()
		{
			BugReportPopoverRoot bugReportPopoverRoot = Resources.Load<BugReportPopoverRoot>("SRDebugger/UI/Prefabs/BugReportPopover");
			BugReportSheetController bugReportSheetController = Resources.Load<BugReportSheetController>("SRDebugger/UI/Prefabs/BugReportSheet");
			if (bugReportPopoverRoot == null)
			{
				UnityEngine.Debug.LogError("[SRDebugger] Unable to load bug report popover prefab");
				return;
			}
			if (bugReportSheetController == null)
			{
				UnityEngine.Debug.LogError("[SRDebugger] Unable to load bug report sheet prefab");
				return;
			}
			this._popover = SRInstantiate.Instantiate<BugReportPopoverRoot>(bugReportPopoverRoot);
			this._popover.CachedTransform.SetParent(base.CachedTransform, false);
			this._sheet = SRInstantiate.Instantiate<BugReportSheetController>(bugReportSheetController);
			this._sheet.CachedTransform.SetParent(this._popover.Container, false);
			this._sheet.SubmitComplete = new Action<bool, string>(this.SubmitComplete);
			this._sheet.CancelPressed = new Action(this.CancelPressed);
			this._sheet.TakingScreenshot = new Action(this.TakingScreenshot);
			this._sheet.ScreenshotComplete = new Action(this.ScreenshotComplete);
			this._popover.CachedGameObject.SetActive(false);
		}

		private BugReportCompleteCallback _callback;

		private bool _isVisible;

		private BugReportPopoverRoot _popover;

		private BugReportSheetController _sheet;
	}
}
