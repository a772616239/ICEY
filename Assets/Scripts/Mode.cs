using System;
using System.Collections.Generic;
using Core;

public class Mode
{
	public Mode()
	{
		Mode.SetInputMode(Mode.AllMode.Normal);
	}

	public Mode.AllMode CurrentMode
	{
		get
		{
			return this._currentMode;
		}
	}

	public bool IsInNormalMode()
	{
		return this._currentMode == Mode.AllMode.Normal;
	}

	public bool IsInBattleMode()
	{
		return this._currentMode == Mode.AllMode.Battle;
	}

	public bool IsInStoryMode()
	{
		return this._currentMode == Mode.AllMode.Story;
	}

	public bool IsInUIMode()
	{
		return this._currentMode == Mode.AllMode.UI;
	}

	public bool IsInShiMode()
	{
		return this._currentMode == Mode.AllMode.Shi;
	}

	public void Reset()
	{
		this._modeStack.Clear();
		this._currentMode = Mode.AllMode.Normal;
		Mode.SetInputMode(Mode.AllMode.Normal);
		InputSetting.Assistant = true;
		InputSetting.Resume(true);
	}

	public void EnterMode(Mode.AllMode nextMode)
	{
		this.LogBefore(nextMode, true);
		this._modeStack.Push(this._currentMode);
		Mode.SetInputMode(nextMode);
		this._currentMode = nextMode;
		this.LogAfter(nextMode, true);
		if (nextMode == Mode.AllMode.UI || nextMode == Mode.AllMode.Story)
		{
			SingletonMono<MobileInputPlayer>.Instance.Visible = false;
		}
	}

	public void ExitMode(Mode.AllMode mode)
	{
		this.LogBefore(mode, false);
		if (this._currentMode != mode)
		{
			Log.Error(string.Concat(new object[]
			{
				"当前模式与退出意图不符，当前模式为 ",
				this._currentMode,
				" 退出模式为",
				mode
			}));
			return;
		}
		if (this._modeStack.Count == 0)
		{
			Log.Error("没有状态可以退出");
			return;
		}
		Mode.AllMode allMode = this._modeStack.Pop();
		Mode.SetInputMode(allMode);
		this._currentMode = allMode;
		this.LogAfter(mode, false);
		if (mode == Mode.AllMode.UI || mode == Mode.AllMode.Story)
		{
			SingletonMono<MobileInputPlayer>.Instance.Visible = true;
		}
	}

	private void LogBefore(Mode.AllMode mode, bool isEnter)
	{
		Log.Info(string.Format("当前模式{0},{2}模式:{1}", this._currentMode, mode, (!isEnter) ? "退出" : "进入"));
	}

	private void LogAfter(Mode.AllMode mode, bool isEnter)
	{
		Log.Info(string.Format("{1}模式成功，当前模式{0}", this._currentMode, (!isEnter) ? "退出" : "进入"));
	}

	private static void SetInputMode(Mode.AllMode mode)
	{
		switch (mode)
		{
		case Mode.AllMode.Normal:
			Mode.SetAllGameInputState(true);
			Input.Game.Search.IsOpen = true;
			Mode.SetAllUIInputState(false);
			Input.UI.Pause.IsOpen = true;
			Input.UI.Debug.IsOpen = true;
			Mode.SetAllStoryInputState(false);
			Mode.SetAllShiInputState(false);
			break;
		case Mode.AllMode.Battle:
			Mode.SetAllGameInputState(true);
			Input.Game.Search.IsOpen = false;
			Mode.SetAllUIInputState(false);
			Input.UI.Pause.IsOpen = true;
			Input.UI.Debug.IsOpen = true;
			Mode.SetAllStoryInputState(false);
			Mode.SetAllShiInputState(false);
			break;
		case Mode.AllMode.Story:
			Mode.SetAllGameInputState(false);
			Mode.SetAllUIInputState(false);
			Mode.SetAllStoryInputState(true);
			Mode.SetAllShiInputState(false);
			break;
		case Mode.AllMode.UI:
			Mode.SetAllGameInputState(false);
			Mode.SetAllUIInputState(true);
			Mode.SetAllStoryInputState(false);
			Mode.SetAllShiInputState(false);
			break;
		case Mode.AllMode.Shi:
			Mode.SetAllGameInputState(false);
			Mode.SetAllUIInputState(false);
			Input.UI.Debug.IsOpen = true;
			Mode.SetAllStoryInputState(false);
			Mode.SetAllShiInputState(true);
			break;
		}
	}

	private static void SetAllGameInputState(bool open)
	{
		Input.Game.MoveDown.IsOpen = open;
		Input.Game.MoveUp.IsOpen = open;
		Input.Game.MoveLeft.IsOpen = open;
		Input.Game.MoveRight.IsOpen = open;
		Input.Game.Atk.IsOpen = open;
		Input.Game.CirtAtk.IsOpen = open;
		Input.Game.Jump.IsOpen = open;
		Input.Game.Flash.Left.IsOpen = open;
		Input.Game.Flash.Right.IsOpen = open;
		Input.Game.Flash.Up.IsOpen = open;
		Input.Game.Flash.Down.IsOpen = open;
		Input.Game.Flash.RightUp.IsOpen = open;
		Input.Game.Flash.LeftUp.IsOpen = open;
		Input.Game.Flash.RightDown.IsOpen = open;
		Input.Game.Flash.LeftDown.IsOpen = open;
		Input.Game.Flash.FaceDirection.IsOpen = open;
		Input.Game.UpRising.IsOpen = open;
		Input.Game.HitGround.IsOpen = open;
		Input.Game.Charging.IsOpen = open;
		Input.Game.Execute.IsOpen = open;
		Input.Game.Defence.IsOpen = open;
		Input.Game.JumpDown.IsOpen = open;
		Input.Game.Chase.IsOpen = open;
		Input.Game.FlashAttack.IsOpen = open;
		Input.Game.BladeStorm.IsOpen = open;
		Input.Game.ShadeAtk.IsOpen = open;
		Input.Game.Search.IsOpen = open;
		Input.Game.L2.IsOpen = open;
		Input.Game.R2.IsOpen = open;
	}

	private static void SetAllUIInputState(bool open)
	{
		Input.UI.Down.IsOpen = open;
		Input.UI.Up.IsOpen = open;
		Input.UI.Left.IsOpen = open;
		Input.UI.Right.IsOpen = open;
		Input.UI.Confirm.IsOpen = open;
		Input.UI.Cancel.IsOpen = open;
		Input.UI.Pause.IsOpen = open;
		Input.UI.Debug.IsOpen = open;
	}

	private static void SetAllStoryInputState(bool open)
	{
		Input.Story.Skip.IsOpen = open;
		Input.Story.BackGame.IsOpen = open;
	}

	private static void SetAllShiInputState(bool open)
	{
		Input.Shi.Down.IsOpen = open;
		Input.Shi.Up.IsOpen = open;
		Input.Shi.Left.IsOpen = open;
		Input.Shi.Right.IsOpen = open;
		Input.Shi.Jump.IsOpen = open;
		Input.Shi.Pause.IsOpen = open;
	}

	private readonly Stack<Mode.AllMode> _modeStack = new Stack<Mode.AllMode>();

	private Mode.AllMode _currentMode;

	public enum AllMode
	{
		Normal,
		Battle,
		Story,
		UI,
		Shi
	}
}
