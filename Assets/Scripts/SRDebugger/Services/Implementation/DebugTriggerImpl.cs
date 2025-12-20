using System;
using System.Runtime.CompilerServices;
using SRDebugger.Internal;
using SRDebugger.UI.Other;
using SRF;
using SRF.Service;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IDebugTriggerService))]
	public class DebugTriggerImpl : SRServiceBase<IDebugTriggerService>, IDebugTriggerService
	{
		public bool IsEnabled
		{
			get
			{
				return this._trigger != null && this._trigger.CachedGameObject.activeSelf;
			}
			set
			{
				if (value && this._trigger == null)
				{
					this.CreateTrigger();
				}
				if (this._trigger != null)
				{
					this._trigger.CachedGameObject.SetActive(value);
				}
			}
		}

		public PinAlignment Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._trigger != null)
				{
					DebugTriggerImpl.SetTriggerPosition(this._trigger.TriggerTransform, value);
				}
				this._position = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			UnityEngine.Object.DontDestroyOnLoad(base.CachedGameObject);
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"), true);
			base.name = "Trigger";
		}

		private void CreateTrigger()
		{
			TriggerRoot triggerRoot = Resources.Load<TriggerRoot>("SRDebugger/UI/Prefabs/Trigger");
			if (triggerRoot == null)
			{
				UnityEngine.Debug.LogError("[SRDebugger] Error loading trigger prefab");
				return;
			}
			this._trigger = SRInstantiate.Instantiate<TriggerRoot>(triggerRoot);
			this._trigger.CachedTransform.SetParent(base.CachedTransform, true);
			DebugTriggerImpl.SetTriggerPosition(this._trigger.TriggerTransform, this._position);
			switch (Settings.Instance.TriggerBehaviour)
			{
			case Settings.TriggerBehaviours.TripleTap:
				this._trigger.TripleTapButton.onClick.AddListener(new UnityAction(this.OnTriggerButtonClick));
				this._trigger.TapHoldButton.gameObject.SetActive(false);
				break;
			case Settings.TriggerBehaviours.TapAndHold:
				this._trigger.TapHoldButton.onLongPress.AddListener(new UnityAction(this.OnTriggerButtonClick));
				this._trigger.TripleTapButton.gameObject.SetActive(false);
				break;
			case Settings.TriggerBehaviours.DoubleTap:
				this._trigger.TripleTapButton.RequiredTapCount = 2;
				this._trigger.TripleTapButton.onClick.AddListener(new UnityAction(this.OnTriggerButtonClick));
				this._trigger.TapHoldButton.gameObject.SetActive(false);
				break;
			default:
				throw new Exception("Unhandled TriggerBehaviour");
			}
			SRDebuggerUtil.EnsureEventSystemExists();
			if (DebugTriggerImpl._003C_003Ef__mg_0024cache0 == null)
			{
				DebugTriggerImpl._003C_003Ef__mg_0024cache0 = new UnityAction<Scene, Scene>(DebugTriggerImpl.OnActiveSceneChanged);
			}
			SceneManager.activeSceneChanged += DebugTriggerImpl._003C_003Ef__mg_0024cache0;
		}

		protected override void OnDestroy()
		{
			if (DebugTriggerImpl._003C_003Ef__mg_0024cache1 == null)
			{
				DebugTriggerImpl._003C_003Ef__mg_0024cache1 = new UnityAction<Scene, Scene>(DebugTriggerImpl.OnActiveSceneChanged);
			}
			SceneManager.activeSceneChanged -= DebugTriggerImpl._003C_003Ef__mg_0024cache1;
			base.OnDestroy();
		}

		private static void OnActiveSceneChanged(Scene s1, Scene s2)
		{
			SRDebuggerUtil.EnsureEventSystemExists();
		}

		private void OnTriggerButtonClick()
		{
			SRDebug.Instance.ShowDebugPanel(true);
		}

		private static void SetTriggerPosition(RectTransform t, PinAlignment position)
		{
			float x = 0f;
			float y = 0f;
			float x2 = 0f;
			float y2 = 0f;
			if (position == PinAlignment.TopLeft || position == PinAlignment.TopRight || position == PinAlignment.TopCenter)
			{
				y = 1f;
				y2 = 1f;
			}
			else if (position == PinAlignment.BottomLeft || position == PinAlignment.BottomRight || position == PinAlignment.BottomCenter)
			{
				y = 0f;
				y2 = 0f;
			}
			else if (position == PinAlignment.CenterLeft || position == PinAlignment.CenterRight)
			{
				y = 0.5f;
				y2 = 0.5f;
			}
			if (position == PinAlignment.TopLeft || position == PinAlignment.BottomLeft || position == PinAlignment.CenterLeft)
			{
				x = 0f;
				x2 = 0f;
			}
			else if (position == PinAlignment.TopRight || position == PinAlignment.BottomRight || position == PinAlignment.CenterRight)
			{
				x = 1f;
				x2 = 1f;
			}
			else if (position == PinAlignment.TopCenter || position == PinAlignment.BottomCenter)
			{
				x = 0.5f;
				x2 = 0.5f;
			}
			t.pivot = new Vector2(x, y);
			Vector2 vector = new Vector2(x2, y2);
			t.anchorMin = vector;
			t.anchorMax = vector;
		}

		private PinAlignment _position;

		private TriggerRoot _trigger;

		[CompilerGenerated]
		private static UnityAction<Scene, Scene> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static UnityAction<Scene, Scene> _003C_003Ef__mg_0024cache1;
	}
}
