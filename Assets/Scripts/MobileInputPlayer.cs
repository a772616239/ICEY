using System;
using FBInput;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class MobileInputPlayer : SingletonMono<MobileInputPlayer>, IInputPlayer
{
	public bool Visible
	{
		get
		{
			return this._panel.gameObject.activeSelf;
		}
		set
		{
			this._panel.gameObject.SetActive(value);
		}
	}

	public bool MainControllerVisiable
	{
		get
		{
			return this._mainControllerVisiable;
		}
		set
		{
			this._mainControllerVisiable = value;
			this._mainController.alpha = (float)((!value) ? 0 : 1);
		}
	}

	public bool OptionsVisible
	{
		get
		{
			return this._options.IsActive;
		}
		set
		{
			this._options.IsActive = true;
		}
	}

	public bool Button6Visible
	{
		get
		{
			return this._button6.IsActive;
		}
		set
		{
			this._button6.IsActive = value;
		}
	}

	public bool L2R2Visiable
	{
		get
		{
			return this._l2R2Widget.gameObject.activeSelf;
		}
		private set
		{
			this._l2R2Widget.gameObject.SetActive(value);
		}
	}

	public void Awake()
	{
		this.Visible = true;
		this.MainControllerVisiable = true;
	}

	public void VisiableBladeStorm()
	{
		this._button1.gameObject.SetActive(R.Player.Enhancement.BladeStorm > 0);
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		if (R.Mode.CurrentMode != Mode.AllMode.Battle)
		{
			return;
		}
		this._button3Sprite.Term = ((!R.Enemy.CanBeExecutedEnemyExist() || !R.Player.CanExecute()) ? "mobile/uisprite/button_02_1" : "mobile/uisprite/button_02_2");
	}

	public void EnterSHI()
	{
		this._button1.gameObject.SetActive(false);
		this._button2.gameObject.SetActive(false);
		this._button3.gameObject.SetActive(false);
		this._button4.gameObject.SetActive(false);
		this._button5SHIMainSprite.spriteName = "Button_03";
		this._button5SHINumSprite.spriteName = "Num03";
	}

	public void ExitSHI()
	{
		this.VisiableBladeStorm();
		this._button2.gameObject.SetActive(true);
		this._button3.gameObject.SetActive(true);
		this._button4.gameObject.SetActive(true);
		this._button5SHIMainSprite.spriteName = "Button_01";
		this._button5SHINumSprite.spriteName = "Num01";
	}

	public Vector2 GetJoystick(string axis)
	{
		if (axis != null)
		{
			if (axis == "Joystick")
			{
				return (!this.MainControllerVisiable) ? Vector2.zero : this._joystick.Axis;
			}
			if (axis == "Swipe")
			{
				return (!this.MainControllerVisiable) ? Vector2.zero : this._swipe.Direction;
			}
		}
		throw new ArgumentOutOfRangeException("axis", axis);
	}

	public Vector2 GetJoystickRaw(string axis)
	{
		if (axis != null)
		{
		}
		throw new ArgumentOutOfRangeException("axis", axis);
	}

	public bool GetButton(string buttonName)
	{
		switch (buttonName)
		{
		case "Button1":
			return this.MainControllerVisiable && this._button1.IsPressed;
		case "Button2":
			return this.MainControllerVisiable && this._button2.IsPressed;
		case "Button3":
			return this.MainControllerVisiable && this._button3.IsPressed;
		case "Button4":
			return this.MainControllerVisiable && this._button4.IsPressed;
		case "Button5":
			return this.MainControllerVisiable && this._button5.IsPressed;
		case "Button6":
			return this._button6.IsPressed;
		case "Options":
			return this._options.IsPressed;
		case "L2":
			return this._l2.IsPressed;
		case "R2":
			return this._r2.IsPressed;
		}
		throw new ArgumentOutOfRangeException("buttonName", buttonName, string.Format("Button \"{0}\" is not exist.", buttonName));
	}

	public void SetVibration(float leftMotorValue, float rightMotorValue)
	{
	}

	public void ShowL2R2(string l2, string r2)
	{
		if (!this.L2R2Visiable)
		{
			this.L2R2Visiable = true;
			this.MainControllerVisiable = false;
		}
		this._l2.Text = l2;
		this._r2.Text = r2;
	}

	public void HideL2R2(bool showMainController = true)
	{
		this.L2R2Visiable = false;
		this.MainControllerVisiable = showMainController;
	}

	private bool OnEnhancementLevelUp(string eventDefine, object sender, EnhanceArgs msg)
	{
		if (!this._button1.gameObject.activeSelf && msg.Name == "bladeStorm")
		{
			this._button1.gameObject.SetActive(true);
		}
		return true;
	}

	public void UpdateRadiusAndPosition()
	{
		this._joystick.UpdateRadiusAndPosition();
	}

	[SerializeField]
	private UIPanel _panel;

	[SerializeField]
	private UIWidget _mainController;

	[SerializeField]
	private UIWidget _l2R2Widget;

	private bool _mainControllerVisiable;

	[SerializeField]
	private FBButton _button1;

	[SerializeField]
	private FBButton _button2;

	[SerializeField]
	private FBButton _button3;

	[SerializeField]
	private FBButton _button4;

	[SerializeField]
	private FBButton _button5;

	[SerializeField]
	private FBButton _button6;

	[SerializeField]
	private FBButton _options;

	[SerializeField]
	private FBButton _l2;

	[SerializeField]
	private FBButton _r2;

	[SerializeField]
	private FBTouchPad _joystick;

	[SerializeField]
	private FBSwipe _swipe;

	[SerializeField]
	private Localize _button3Sprite;

	[SerializeField]
	private UISprite _button5SHIMainSprite;

	[SerializeField]
	private UISprite _button5SHINumSprite;
}
