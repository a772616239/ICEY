using System;
using UnityEngine;

public class PlayerAttribute
{
	public int currentHP
	{
		get
		{
			return this._currentHP;
		}
		set
		{
			this._currentHP = Mathf.Clamp(value, 0, this.maxHP);
		}
	}

	public int currentEnergy
	{
		get
		{
			return this._currentEnergy;
		}
		set
		{
			this._currentEnergy = Mathf.Clamp(value, 0, this.maxEnergy);
		}
	}

	public int flashTimes
	{
		get
		{
			return (this.flashLevel < 2) ? 3 : 5;
		}
	}

	public bool isInCharging
	{
		get
		{
			return R.Player.Action.stateMachine.currentState == "Charging1" || R.Player.Action.stateMachine.currentState == "Charge1Ready" || R.Player.Action.stateMachine.currentState == "AirCharging";
		}
	}

	public bool isOnGround
	{
		get
		{
			return this.platform.State.IsDetectedGround;
		}
	}

	public bool isDead
	{
		get
		{
			return this.currentHP <= 0;
		}
	}

	public Bounds bounds
	{
		get
		{
			return R.Player.GetComponent<Collider2D>().bounds;
		}
	}

	private PlatformMovement platform
	{
		get
		{
			PlatformMovement result;
			if ((result = this._platform) == null)
			{
				result = (this._platform = R.Player.GetComponent<PlatformMovement>());
			}
			return result;
		}
	}

	public void ResetData()
	{
		this.SetBaseLevelData();
		this.AllAttributeRecovery();
	}

	private void SetBaseLevelData()
	{
		this.maxHP = DB.Enhancements["maxHP"].GetEnhanceEffect(R.Player.Enhancement.MaxHp);
		this.baseAtk = ((!Debug.isDebugBuild) ? 40 : ((!R.Settings.CheatMode) ? 40 : 9999));
		this.maxEnergy = ((R.GameData.Difficulty != 3) ? 10 : 1);
		this.moveSpeed = 9f;
		this.currentFlashTimes = this.flashTimes;
		this.maxChargeTime = 2.5f;
	}

	public void AllAttributeRecovery()
	{
		this.maxHP = DB.Enhancements["maxHP"].GetEnhanceEffect(R.Player.Enhancement.MaxHp);
		this.currentHP = this.maxHP;
		this.currentEnergy = this.maxEnergy;
		this.currentFlashTimes = this.flashTimes;
		R.Ui.Flash.RecoverAll(this.flashTimes);
	}

	public const int maxExecuteNum = 1;

	public const float ChargeTime = 2.5f;

	private const float BaseMoveSpeed = 9f;

	public int faceDir;

	public int maxHP;

	[SerializeField]
	private int _currentHP;

	public int maxEnergy;

	[SerializeField]
	private int _currentEnergy;

	public float moveSpeed;

	public int flashLevel = 1;

	public int currentFlashTimes;

	public int FlashCd;

	public bool flashFlag;

	[HideInInspector]
	public float maxChargeTime;

	public int maxChargeEndureDamage;

	public int baseAtk;

	[NonSerialized]
	private PlatformMovement _platform;
}
