using System;
using SRDebugger.UI.Other;
using SRF;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SRDebugger.UI
{
	public class MobileMenuController : SRMonoBehaviourEx
	{
		public float PeekAmount
		{
			get
			{
				return this._peekAmount;
			}
		}

		public float MaxMenuWidth
		{
			get
			{
				return this._maxMenuWidth;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			RectTransform rectTransform = this.Menu.parent as RectTransform;
			LayoutElement component = this.Menu.GetComponent<LayoutElement>();
			component.ignoreLayout = true;
			this.Menu.pivot = new Vector2(1f, 1f);
			this.Menu.offsetMin = new Vector2(1f, 0f);
			this.Menu.offsetMax = new Vector2(1f, 1f);
			this.Menu.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(rectTransform.rect.width - this.PeekAmount, 0f, this.MaxMenuWidth));
			this.Menu.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
			this.Menu.anchoredPosition = new Vector2(0f, 0f);
			if (this._closeButton == null)
			{
				this.CreateCloseButton();
			}
			this.OpenButton.gameObject.SetActive(true);
			this.TabController.ActiveTabChanged += this.TabControllerOnActiveTabChanged;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			LayoutElement component = this.Menu.GetComponent<LayoutElement>();
			component.ignoreLayout = false;
			this.Content.anchoredPosition = new Vector2(0f, 0f);
			this._closeButton.gameObject.SetActive(false);
			this.OpenButton.gameObject.SetActive(false);
			this.TabController.ActiveTabChanged -= this.TabControllerOnActiveTabChanged;
		}

		private void CreateCloseButton()
		{
			GameObject gameObject = new GameObject("SR_CloseButtonCanvas", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(this.Content, false);
			Canvas canvas = gameObject.AddComponent<Canvas>();
			gameObject.AddComponent<GraphicRaycaster>();
			RectTransform componentOrAdd = gameObject.GetComponentOrAdd<RectTransform>();
			canvas.overrideSorting = true;
			canvas.sortingOrder = 122;
			gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
			this.SetRectSize(componentOrAdd);
			GameObject gameObject2 = new GameObject("SR_CloseButton", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject2.transform.SetParent(componentOrAdd, false);
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			this.SetRectSize(component);
			gameObject2.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			this._closeButton = gameObject2.AddComponent<Button>();
			this._closeButton.transition = Selectable.Transition.None;
			this._closeButton.onClick.AddListener(new UnityAction(this.CloseButtonClicked));
			this._closeButton.gameObject.SetActive(false);
		}

		private void SetRectSize(RectTransform rect)
		{
			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.Content.rect.width);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.Content.rect.height);
		}

		private void CloseButtonClicked()
		{
			this.Close();
		}

		protected override void Update()
		{
			base.Update();
			float x = this.Content.anchoredPosition.x;
			if (Mathf.Abs(this._targetXPosition - x) < 2.5f)
			{
				this.Content.anchoredPosition = new Vector2(this._targetXPosition, this.Content.anchoredPosition.y);
			}
			else
			{
				this.Content.anchoredPosition = new Vector2(SRMath.SpringLerp(x, this._targetXPosition, 15f, Time.unscaledDeltaTime), this.Content.anchoredPosition.y);
			}
		}

		private void TabControllerOnActiveTabChanged(SRTabController srTabController, SRTab srTab)
		{
			this.Close();
		}

		[ContextMenu("Open")]
		public void Open()
		{
			this._targetXPosition = this.Menu.rect.width;
			this._closeButton.gameObject.SetActive(true);
		}

		[ContextMenu("Close")]
		public void Close()
		{
			this._targetXPosition = 0f;
			this._closeButton.gameObject.SetActive(false);
		}

		private Button _closeButton;

		[SerializeField]
		private float _maxMenuWidth = 185f;

		[SerializeField]
		private float _peekAmount = 45f;

		private float _targetXPosition;

		[RequiredField]
		public RectTransform Content;

		[RequiredField]
		public RectTransform Menu;

		[RequiredField]
		public Button OpenButton;

		[RequiredField]
		public SRTabController TabController;
	}
}
