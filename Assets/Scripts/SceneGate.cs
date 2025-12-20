using System;
using Core;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneGate : BaseBehaviour
{
	private void Awake()
	{
		this._trigger = base.GetComponent<BoxCollider2D>();
		if (this._trigger == null)
		{
			Log.Error("No Collider2D in Scene " + LevelManager.SceneName + " Gate " + base.name);
		}
		else
		{
			this.data.TriggerSize = Vector2.Scale(this._trigger.size, base.transform.localScale);
		}
		this.data.SelfPosition = base.transform.position;
		this.data.OpenType = this.openType;
		this.data.InAir = this.InAir;
	}

	public void OnEnable()
	{
		R.SceneGate.GatesInCurrentScene.Add(this);
	}

	public void OnDisable()
	{
		if (!SingletonMono<SceneGateManager>.ApplicationIsQuitting)
		{
			R.SceneGate.GatesInCurrentScene.Remove(this);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (this.AllowEnterGate(collision))
		{
			Vector3 position = R.Player.Transform.position;
			float num = 0.5f;
			bool flag = this.openType == SceneGate.OpenType.All;
			bool flag2 = this.openType == SceneGate.OpenType.Left && position.x < base.transform.position.x + num && R.Player.Attribute.faceDir == 1;
			bool flag3 = this.openType == SceneGate.OpenType.Right && position.x > base.transform.position.x - num && R.Player.Attribute.faceDir == -1;
			bool flag4 = this.openType == SceneGate.OpenType.Top && position.y > base.transform.position.y - num;
			bool flag5 = this.openType == SceneGate.OpenType.Bottom && position.y < base.transform.position.y + num;
			if (flag || flag2 || flag3 || flag4 || flag5)
			{
				R.SceneGate.Enter(this.data, false);
			}
			if (this.openType == SceneGate.OpenType.PressKey && Core.Input.Game.Search.OnClick)
			{
				R.SceneGate.Enter(this.data, false);
			}
		}
	}

	private bool AllowEnterGate(Collider2D collision)
	{
		bool flag = R.SceneGate.IsLocked || R.Player == null || !collision.CompareTag("Player") || this.openType == SceneGate.OpenType.None || R.Mode.IsInBattleMode() || R.Player.Action.NotAllowPassSceneGate || !InputSetting.IsWorking() || (!string.IsNullOrEmpty(this._checkIdInSaveStorage) && SaveStorage.Get(this._checkIdInSaveStorage, false));
		return !flag;
	}

	public Coroutine Enter(bool needProgressBar = false)
	{
		return R.SceneGate.Enter(this.data, needProgressBar);
	}

	public Coroutine Exit(float groundDis = 0f, SceneGate.OpenType lastGateOpenType = SceneGate.OpenType.None)
	{
		return R.SceneGate.Exit(this.data, groundDis, lastGateOpenType);
	}

	public SwitchLevelGateData data;

	[Header("允许进入的方式与方向")]
	[SerializeField]
	public SceneGate.OpenType openType = SceneGate.OpenType.All;

	[SerializeField]
	[Header("允许出现在空中")]
	private bool InAir;

	[SerializeField]
	private string _checkIdInSaveStorage;

	private BoxCollider2D _trigger;

	public enum OpenType
	{
		None = 1,
		All,
		Left,
		Right,
		Top,
		Bottom,
		PressKey
	}
}
