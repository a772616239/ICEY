using System;
using System.Collections;
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
		if (SceneGate.DiagEnabled)
		{
			base.StartCoroutine(this.DiagHeartbeat());
		}
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
		if (!this.AllowEnterGate(collision))
		{
			if (SceneGate.DiagEnabled && collision != null && collision.CompareTag("Player") && Time.unscaledTime - this._diagTime > 1f)
			{
				this._diagTime = Time.unscaledTime;
				Log.Info(string.Concat(new object[]
				{
					"[GateDiag] 玩家已在 Gate ",
					this.data.MyId,
					" 触发区内但被拦截：",
					this.BlockReason(collision)
				}));
			}
			return;
		}
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

	private string BlockReason(Collider2D collision)
	{
		string text = string.Empty;
		if (R.SceneGate == null || R.SceneGate.IsLocked)
		{
			text += "IsLocked;";
		}
		if (R.Player == null)
		{
			text += "PlayerNull;";
		}
		if (!collision.CompareTag("Player"))
		{
			text += "ColliderNotPlayerTag(" + collision.tag + ");";
		}
		if (this.openType == SceneGate.OpenType.None)
		{
			text += "OpenTypeNone;";
		}
		if (R.Mode != null && R.Mode.IsInBattleMode())
		{
			text += "BattleMode;";
		}
		if (R.Player != null && R.Player.Action.NotAllowPassSceneGate)
		{
			text += "NotAllowPassSceneGate;";
		}
		if (!InputSetting.IsWorking())
		{
			text += "InputNotWorking;";
		}
		if (!string.IsNullOrEmpty(this._checkIdInSaveStorage) && SaveStorage.Get(this._checkIdInSaveStorage, false))
		{
			text += "SaveStorageBlocked;";
		}
		if (text.Length == 0)
		{
			text = "(无拦截条件命中，需查 AllowEnterGate)";
		}
		if (R.Player != null)
		{
			text += string.Format(" | openType={0} face={1} playerX={2:F2} gateX={3:F2} dx={4:F2}", new object[]
			{
				this.openType,
				R.Player.Attribute.faceDir,
				R.Player.Transform.position.x,
				base.transform.position.x,
				R.Player.Transform.position.x - base.transform.position.x
			});
		}
		return text;
	}

	private IEnumerator DiagHeartbeat()
	{
		WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
		for (;;)
		{
			yield return wait;
			try
			{
				if (R.Player == null)
				{
					Log.Info(string.Format("[GateDiag] scene={0} gate={1} player=NULL", LevelManager.SceneName, this.data.MyId));
				}
				else
				{
					Log.Info(string.Format("[GateDiag] scene={0} gate={1} gateX={2:F2} playerX={3:F2} playerY={4:F2} dx={5:F2} face={6} state={7} mode={8} locked={9} working={10} noPass={11} moveR={12} jump={13} openR={14} uiVisible={15}", new object[]
					{
						LevelManager.SceneName,
						this.data.MyId,
						base.transform.position.x,
						R.Player.Transform.position.x,
						R.Player.Transform.position.y,
						R.Player.Transform.position.x - base.transform.position.x,
						R.Player.Attribute.faceDir,
						R.Player.Action.stateMachine.currentState,
						R.Mode.CurrentMode,
						R.SceneGate.IsLocked,
						InputSetting.IsWorking(),
						R.Player.Action.NotAllowPassSceneGate,
						Core.Input.Game.MoveRight.Pressed,
						Core.Input.Game.Jump.Pressed,
						Core.Input.Game.MoveRight.IsOpen,
						SingletonMono<MobileInputPlayer>.Instance.Visible
					}));
				}
			}
			catch (Exception ex)
			{
				Log.Info("[GateDiag] 心跳异常: " + ex.Message);
			}
		}
	}

	private const bool DiagEnabled = true;

	private float _diagTime;

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
