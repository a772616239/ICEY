using System;
using SRDebugger.UI.Other;
using SRF;
using UnityEngine;

namespace SRDebugger.UI.Tabs
{
	public class BugReportTabController : SRMonoBehaviourEx, IEnableTab
	{
		public bool IsEnabled
		{
			get
			{
				return Settings.Instance.EnableBugReporter;
			}
		}

		protected override void Start()
		{
			base.Start();
			BugReportSheetController bugReportSheetController = SRInstantiate.Instantiate<BugReportSheetController>(this.BugReportSheetPrefab);
			bugReportSheetController.IsCancelButtonEnabled = false;
			bugReportSheetController.TakingScreenshot = new Action(this.TakingScreenshot);
			bugReportSheetController.ScreenshotComplete = new Action(this.ScreenshotComplete);
			bugReportSheetController.CachedTransform.SetParent(this.Container, false);
		}

		private void TakingScreenshot()
		{
			SRDebug.Instance.HideDebugPanel();
		}

		private void ScreenshotComplete()
		{
			SRDebug.Instance.ShowDebugPanel(false);
		}

		[RequiredField]
		public BugReportSheetController BugReportSheetPrefab;

		[RequiredField]
		public RectTransform Container;
	}
}
