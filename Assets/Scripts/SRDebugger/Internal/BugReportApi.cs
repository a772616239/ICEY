using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SRDebugger.Services;
using SRF;
using UnityEngine;

namespace SRDebugger.Internal
{
	public class BugReportApi
	{
		public BugReportApi(BugReport report, string apiKey)
		{
			this._bugReport = report;
			this._apiKey = apiKey;
		}

		public bool IsComplete { get; private set; }

		public bool WasSuccessful { get; private set; }

		public string ErrorMessage { get; private set; }

		public float Progress
		{
			get
			{
				if (this._www == null)
				{
					return 0f;
				}
				return Mathf.Clamp01(this._www.progress + this._www.uploadProgress);
			}
		}

		public IEnumerator Submit()
		{
			if (this._isBusy)
			{
				throw new InvalidOperationException("BugReportApi is already sending a bug report");
			}
			this._isBusy = true;
			this.ErrorMessage = string.Empty;
			this.IsComplete = false;
			this.WasSuccessful = false;
			this._www = null;
			try
			{
				string s = BugReportApi.BuildJsonRequest(this._bugReport);
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary["Content-Type"] = "application/json";
				dictionary["Accept"] = "application/json";
				dictionary["Method"] = "POST";
				dictionary["X-ApiKey"] = this._apiKey;
				this._www = new WWW("http://srdebugger.stompyrobot.uk/report/submit", bytes, dictionary);
			}
			catch (Exception ex)
			{
				this.ErrorMessage = ex.Message;
			}
			if (this._www == null)
			{
				this.SetCompletionState(false);
				yield break;
			}
			yield return this._www;
			if (!string.IsNullOrEmpty(this._www.error))
			{
				this.ErrorMessage = this._www.error;
				this.SetCompletionState(false);
				yield break;
			}
			if (!this._www.responseHeaders.ContainsKey("X-STATUS"))
			{
				this.ErrorMessage = "Completion State Unknown";
				this.SetCompletionState(false);
				yield break;
			}
			string status = this._www.responseHeaders["X-STATUS"];
			if (!status.Contains("200"))
			{
				this.ErrorMessage = SRDebugApiUtil.ParseErrorResponse(this._www.text, status);
				this.SetCompletionState(false);
				yield break;
			}
			this.SetCompletionState(true);
			yield break;
		}

		private void SetCompletionState(bool wasSuccessful)
		{
			this._bugReport.ScreenshotData = null;
			this.WasSuccessful = wasSuccessful;
			this.IsComplete = true;
			this._isBusy = false;
			if (!wasSuccessful)
			{
				UnityEngine.Debug.LogError("Bug Reporter Error: " + this.ErrorMessage);
			}
		}

		private static string BuildJsonRequest(BugReport report)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("userEmail", report.Email);
			hashtable.Add("userDescription", report.UserDescription);
			hashtable.Add("console", BugReportApi.CreateConsoleDump());
			hashtable.Add("systemInformation", report.SystemInformation);
			if (report.ScreenshotData != null)
			{
				hashtable.Add("screenshot", Convert.ToBase64String(report.ScreenshotData));
			}
			return Json.Serialize(hashtable);
		}

		private static IList<IList<string>> CreateConsoleDump()
		{
			List<IList<string>> list = new List<IList<string>>();
			IReadOnlyList<ConsoleEntry> entries = Service.Console.Entries;
			foreach (ConsoleEntry consoleEntry in entries)
			{
				List<string> list2 = new List<string>();
				list2.Add(consoleEntry.LogType.ToString());
				list2.Add(consoleEntry.Message);
				list2.Add(consoleEntry.StackTrace);
				if (consoleEntry.Count > 1)
				{
					list2.Add(consoleEntry.Count.ToString());
				}
				list.Add(list2);
			}
			return list;
		}

		private readonly string _apiKey;

		private readonly BugReport _bugReport;

		private bool _isBusy;

		private WWW _www;
	}
}
