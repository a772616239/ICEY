using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using SRF;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{
	public class PinEntryControl : SRMonoBehaviourEx
	{
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PinEntryControlCallback Complete;

		protected override void Awake()
		{
			base.Awake();
			for (int i = 0; i < this.NumberButtons.Length; i++)
			{
				int number = i;
				this.NumberButtons[i].onClick.AddListener(delegate()
				{
					this.PushNumber(number);
				});
			}
			this.CancelButton.onClick.AddListener(new UnityAction(this.CancelButtonPressed));
			this.RefreshState();
		}

		protected override void Update()
		{
			base.Update();
			if (!this._isVisible)
			{
				return;
			}
			if (this._numbers.Count > 0 && (UnityEngine.Input.GetKeyDown(KeyCode.Backspace) || UnityEngine.Input.GetKeyDown(KeyCode.Delete)))
			{
				this._numbers.PopLast<int>();
				this.RefreshState();
			}
			string inputString = Input.inputString;
			for (int i = 0; i < inputString.Length; i++)
			{
				if (char.IsNumber(inputString, i))
				{
					int num = (int)char.GetNumericValue(inputString, i);
					if (num <= 9 && num >= 0)
					{
						this.PushNumber(num);
					}
				}
			}
		}

		public void Show()
		{
			this.CanvasGroup.alpha = 1f;
			CanvasGroup canvasGroup = this.CanvasGroup;
			bool flag = true;
			this.CanvasGroup.interactable = flag;
			canvasGroup.blocksRaycasts = flag;
			this._isVisible = true;
		}

		public void Hide()
		{
			this.CanvasGroup.alpha = 0f;
			CanvasGroup canvasGroup = this.CanvasGroup;
			bool flag = false;
			this.CanvasGroup.interactable = flag;
			canvasGroup.blocksRaycasts = flag;
			this._isVisible = false;
		}

		public void Clear()
		{
			this._numbers.Clear();
			this.RefreshState();
		}

		public void PlayInvalidCodeAnimation()
		{
			this.DotAnimator.SetTrigger("Invalid");
		}

		protected void OnComplete()
		{
			if (this.Complete != null)
			{
				this.Complete(new ReadOnlyCollection<int>(this._numbers), false);
			}
		}

		protected void OnCancel()
		{
			if (this.Complete != null)
			{
				this.Complete(new int[0], true);
			}
		}

		private void CancelButtonPressed()
		{
			if (this._numbers.Count > 0)
			{
				this._numbers.PopLast<int>();
			}
			else
			{
				this.OnCancel();
			}
			this.RefreshState();
		}

		public void PushNumber(int number)
		{
			if (this._numbers.Count >= 4)
			{
				UnityEngine.Debug.LogWarning("[PinEntry] Expected 4 numbers");
				return;
			}
			this._numbers.Add(number);
			if (this._numbers.Count >= 4)
			{
				this.OnComplete();
			}
			this.RefreshState();
		}

		private void RefreshState()
		{
			for (int i = 0; i < this.NumberDots.Length; i++)
			{
				this.NumberDots[i].isOn = (i < this._numbers.Count);
			}
			if (this._numbers.Count > 0)
			{
				this.CancelButtonText.text = "Delete";
			}
			else
			{
				this.CancelButtonText.text = ((!this.CanCancel) ? string.Empty : "Cancel");
			}
		}

		private bool _isVisible = true;

		private List<int> _numbers = new List<int>(4);

		[RequiredField]
		public Image Background;

		public bool CanCancel = true;

		[RequiredField]
		public Button CancelButton;

		[RequiredField]
		public Text CancelButtonText;

		[RequiredField]
		public CanvasGroup CanvasGroup;

		[RequiredField]
		public Animator DotAnimator;

		public Button[] NumberButtons;

		public Toggle[] NumberDots;

		[RequiredField]
		public Text PromptText;
	}
}
