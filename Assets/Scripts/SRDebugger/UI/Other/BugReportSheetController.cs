using System;
using System.Collections;
using System.Linq;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRDebugger.UI.Other
{
	public class BugReportSheetController : SRMonoBehaviourEx
	{
		public bool IsCancelButtonEnabled
		{
			get
			{
				return this.CancelButton.gameObject.activeSelf;
			}
			set
			{
				this.CancelButton.gameObject.SetActive(value);
			}
		}

		protected override void Start()
		{
			base.Start();
			this.SetLoadingSpinnerVisible(false);
			this.ClearErrorMessage();
			this.ClearForm();
		}

		public void Submit()
		{
			EventSystem.current.SetSelectedGameObject(null);
			this.ProgressBar.value = 0f;
			this.ClearErrorMessage();
			this.SetLoadingSpinnerVisible(true);
			this.SetFormEnabled(false);
			if (!string.IsNullOrEmpty(this.EmailField.text))
			{
				this.SetDefaultEmailFieldContents(this.EmailField.text);
			}
			base.StartCoroutine(this.SubmitCo());
		}

		public void Cancel()
		{
			if (this.CancelPressed != null)
			{
				this.CancelPressed();
			}
		}

		private IEnumerator SubmitCo()
		{
			if (BugReportScreenshotUtil.ScreenshotData == null)
			{
				if (this.TakingScreenshot != null)
				{
					this.TakingScreenshot();
				}
				yield return new WaitForEndOfFrame();
				yield return base.StartCoroutine(BugReportScreenshotUtil.ScreenshotCaptureCo());
				if (this.ScreenshotComplete != null)
				{
					this.ScreenshotComplete();
				}
			}
			IBugReportService s = SRServiceManager.GetService<IBugReportService>();
			BugReport r = new BugReport();
			r.Email = this.EmailField.text;
			r.UserDescription = this.DescriptionField.text;
			r.ConsoleLog = Service.Console.AllEntries.ToList<ConsoleEntry>();
			r.SystemInformation = SRServiceManager.GetService<ISystemInformationService>().CreateReport(false);
			r.ScreenshotData = BugReportScreenshotUtil.ScreenshotData;
			BugReportScreenshotUtil.ScreenshotData = null;
			s.SendBugReport(r, new BugReportCompleteCallback(this.OnBugReportComplete), new BugReportProgressCallback(this.OnBugReportProgress));
			yield break;
		}

		private void OnBugReportProgress(float progress)
		{
			this.ProgressBar.value = progress;
		}

		private void OnBugReportComplete(bool didSucceed, string errorMessage)
		{
			if (!didSucceed)
			{
				this.ShowErrorMessage("Error sending bug report", errorMessage);
			}
			else
			{
				this.ClearForm();
				this.ShowErrorMessage("Bug report submitted successfully", null);
			}
			this.SetLoadingSpinnerVisible(false);
			this.SetFormEnabled(true);
			if (this.SubmitComplete != null)
			{
				this.SubmitComplete(didSucceed, errorMessage);
			}
		}

		protected void SetLoadingSpinnerVisible(bool visible)
		{
			this.ProgressBar.gameObject.SetActive(visible);
			this.ButtonContainer.SetActive(!visible);
		}

		protected void ClearForm()
		{
			this.EmailField.text = this.GetDefaultEmailFieldContents();
			this.DescriptionField.text = string.Empty;
		}

		protected void ShowErrorMessage(string userMessage, string serverMessage = null)
		{
			string text = userMessage;
			if (!string.IsNullOrEmpty(serverMessage))
			{
				text += " (<b>{0}</b>)".Fmt(new object[]
				{
					serverMessage
				});
			}
			this.ResultMessageText.text = text;
			this.ResultMessageText.gameObject.SetActive(true);
		}

		protected void ClearErrorMessage()
		{
			this.ResultMessageText.text = string.Empty;
			this.ResultMessageText.gameObject.SetActive(false);
		}

		protected void SetFormEnabled(bool e)
		{
			this.SubmitButton.interactable = e;
			this.CancelButton.interactable = e;
			this.EmailField.interactable = e;
			this.DescriptionField.interactable = e;
		}

		private string GetDefaultEmailFieldContents()
		{
			return PlayerPrefs.GetString("SRDEBUGGER_BUG_REPORT_LAST_EMAIL", string.Empty);
		}

		private void SetDefaultEmailFieldContents(string value)
		{
			PlayerPrefs.SetString("SRDEBUGGER_BUG_REPORT_LAST_EMAIL", value);
			PlayerPrefs.Save();
		}

		[RequiredField]
		public GameObject ButtonContainer;

		[RequiredField]
		public Text ButtonText;

		[RequiredField]
		public Button CancelButton;

		public Action CancelPressed;

		[RequiredField]
		public InputField DescriptionField;

		[RequiredField]
		public InputField EmailField;

		[RequiredField]
		public Slider ProgressBar;

		[RequiredField]
		public Text ResultMessageText;

		public Action ScreenshotComplete;

		[RequiredField]
		public Button SubmitButton;

		public Action<bool, string> SubmitComplete;

		public Action TakingScreenshot;
	}
}
