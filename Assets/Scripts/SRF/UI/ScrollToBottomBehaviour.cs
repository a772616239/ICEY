using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRF.UI
{
	[AddComponentMenu("SRF/UI/Scroll To Bottom Behaviour")]
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class ScrollToBottomBehaviour : MonoBehaviour
	{
		public void Start()
		{
			if (this._scrollRect == null)
			{
				UnityEngine.Debug.LogError("[ScrollToBottomBehaviour] ScrollRect not set");
				return;
			}
			if (this._canvasGroup == null)
			{
				UnityEngine.Debug.LogError("[ScrollToBottomBehaviour] CanvasGroup not set");
				return;
			}
			this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollRectValueChanged));
			this.Refresh();
		}

		private void OnEnable()
		{
			this.Refresh();
		}

		public void Trigger()
		{
			this._scrollRect.normalizedPosition = new Vector2(0f, 0f);
		}

		private void OnScrollRectValueChanged(Vector2 position)
		{
			this.Refresh();
		}

		private void Refresh()
		{
			if (this._scrollRect == null)
			{
				return;
			}
			if (this._scrollRect.normalizedPosition.y < 0.001f)
			{
				this.SetVisible(false);
			}
			else
			{
				this.SetVisible(true);
			}
		}

		private void SetVisible(bool truth)
		{
			if (truth)
			{
				this._canvasGroup.alpha = 1f;
				this._canvasGroup.interactable = true;
				this._canvasGroup.blocksRaycasts = true;
			}
			else
			{
				this._canvasGroup.alpha = 0f;
				this._canvasGroup.interactable = false;
				this._canvasGroup.blocksRaycasts = false;
			}
		}

		[SerializeField]
		private ScrollRect _scrollRect;

		[SerializeField]
		private CanvasGroup _canvasGroup;
	}
}
