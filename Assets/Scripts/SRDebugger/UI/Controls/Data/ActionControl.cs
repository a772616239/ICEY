using System;
using SRF;
using SRF.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{
	public class ActionControl : OptionsControlBase
	{
		public MethodReference Method
		{
			get
			{
				return this._method;
			}
		}

		protected override void Start()
		{
			base.Start();
			this.Button.onClick.AddListener(new UnityAction(this.ButtonOnClick));
		}

		private void ButtonOnClick()
		{
			if (this._method == null)
			{
				UnityEngine.Debug.LogWarning("[SRDebugger.Options] No method set for action control", this);
				return;
			}
			try
			{
				this._method.Invoke(null);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError("[SRDebugger] Exception thrown while executing action.");
				UnityEngine.Debug.LogException(exception);
			}
		}

		public void SetMethod(string methodName, MethodReference method)
		{
			this._method = method;
			this.Title.text = methodName;
		}

		private MethodReference _method;

		[RequiredField]
		public Button Button;

		[RequiredField]
		public Text Title;
	}
}
