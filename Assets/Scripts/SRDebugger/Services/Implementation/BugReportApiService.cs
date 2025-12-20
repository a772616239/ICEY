using System;
using SRDebugger.Internal;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IBugReportService))]
	public class BugReportApiService : SRServiceBase<IBugReportService>, IBugReportService
	{
		public void SendBugReport(BugReport report, BugReportCompleteCallback completeHandler, BugReportProgressCallback progressCallback = null)
		{
			if (report == null)
			{
				throw new ArgumentNullException("report");
			}
			if (completeHandler == null)
			{
				throw new ArgumentNullException("completeHandler");
			}
			if (this._isBusy)
			{
				completeHandler(false, "BugReportApiService is already sending a bug report");
				return;
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				completeHandler(false, "No Internet Connection");
				return;
			}
			this._errorMessage = string.Empty;
			base.enabled = true;
			this._isBusy = true;
			this._completeCallback = completeHandler;
			this._progressCallback = progressCallback;
			this._reportApi = new BugReportApi(report, Settings.Instance.ApiKey);
			base.StartCoroutine(this._reportApi.Submit());
		}

		protected override void Awake()
		{
			base.Awake();
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));
		}

		private void OnProgress(float progress)
		{
			if (this._progressCallback != null)
			{
				this._progressCallback(progress);
			}
		}

		private void OnComplete()
		{
			this._isBusy = false;
			base.enabled = false;
			this._completeCallback(this._reportApi.WasSuccessful, (!string.IsNullOrEmpty(this._reportApi.ErrorMessage)) ? this._reportApi.ErrorMessage : this._errorMessage);
			this._completeCallback = null;
		}

		protected override void Update()
		{
			base.Update();
			if (!this._isBusy)
			{
				return;
			}
			if (this._reportApi == null)
			{
				this._isBusy = false;
			}
			if (this._reportApi.IsComplete)
			{
				this.OnComplete();
				return;
			}
			if (this._previousProgress != this._reportApi.Progress)
			{
				this.OnProgress(this._reportApi.Progress);
				this._previousProgress = this._reportApi.Progress;
			}
		}

		public const float Timeout = 12f;

		private BugReportCompleteCallback _completeCallback;

		private string _errorMessage;

		private bool _isBusy;

		private float _previousProgress;

		private BugReportProgressCallback _progressCallback;

		private BugReportApi _reportApi;
	}
}
