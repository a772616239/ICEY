using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

public class UICharactorController : MonoBehaviour
{
	private PlayerAttribute playerAttribute
	{
		get
		{
			return R.Player.Attribute;
		}
	}

	private void Awake()
	{
		this._roleNameLocalize = this._roleNameLabel.GetComponent<Localize>();
	}

	private void Start()
	{
		Transform child = this.hp2Bar.transform.GetChild(0);
		this.cursorTweenAlpha = child.GetComponent<TweenAlpha>();
		this.cursorSprite = child.GetComponent<UISprite>();
		base.StartCoroutine(this.HPValueHistoryRecorder());
		base.StartCoroutine(this.HPChangeAnim());
	}

	private void Update()
	{
		this.DataBind();
	}

	private void DataBind()
	{
		if (this.playerAttribute == null)
		{
			return;
		}
		this.UpdateHpBarShape();
		this.UpdateCharInfo();
	}

	private void UpdateHpBarShape()
	{
		if (this._lastPlayerMaxHP != R.Player.Attribute.maxHP)
		{
			this._lastPlayerMaxHP = R.Player.Attribute.maxHP;
			int num = (int)((double)this._lastPlayerMaxHP * 2.48);
			this._hp1Sprite.width = num;
			this._hp2Sprite.width = num;
			this._hpBgSprite.width = num + 56;
		}
	}

	private void UpdateCharInfo()
	{
		float num = (float)this.playerAttribute.currentHP / (float)Mathf.Max(this.playerAttribute.maxHP, 1);
		if (Math.Abs(this.hp1Bar.value - num) > 1.401298E-45f)
		{
			this.hp1Bar.value = num;
		}
		if (this.hpValueHistory.Count >= 10 && (double)Mathf.Abs(this.hp1Bar.value - this.hpValueHistory.Peek()) < 0.0001 && (double)Mathf.Abs(this.hp1Bar.value - this.hpValueWhenLastPlayAnim) > 0.0001)
		{
			this.hpValueWhenLastPlayAnim = this.hp1Bar.value;
			this.play = true;
		}
		if (this.playerAttribute.currentHP == 0)
		{
			this.hpValueWhenLastPlayAnim = this.hp1Bar.value;
			this.play = true;
		}
		this.energyController.EnergyMaxValue = this.playerAttribute.maxEnergy * 2;
		this.energyController.EnergyValue = this.playerAttribute.currentEnergy * 2;
		if (this._roleName != R.Player.RoleName)
		{
			this._roleName = R.Player.RoleName;
			string roleName = R.Player.RoleName;
			if (roleName != null)
			{
				if (roleName == "ICEY")
				{
					this._roleNameLocalize.enabled = true;
					this._roleNameLocalize.Term = "ui/ICEY";
					return;
				}
				if (roleName == "UCEY")
				{
					this._roleNameLocalize.enabled = true;
					this._roleNameLocalize.Term = "ui/UCEY";
					return;
				}
				if (roleName == "anonymous")
				{
					this._roleNameLocalize.enabled = true;
					this._roleNameLocalize.Term = "ui/anonymous";
					return;
				}
			}
			this._roleNameLocalize.enabled = false;
			this._roleNameLabel.text = R.Player.RoleName;
		}
	}

	private IEnumerator HPChangeAnim()
	{
		for (;;)
		{
			if (this.play)
			{
				this.play = false;
				this.cursorTweenAlpha.enabled = true;
				this.cursorSprite.enabled = true;
				float start = this.hp2Bar.value;
				float targe = this.hp1Bar.value;
				for (float x = 0f; x < 1f; x += 0.02f)
				{
					float current = start + (targe - start) * x;
					this.hp2Bar.value = current;
					yield return new WaitForSeconds(0.02f);
				}
				this.cursorTweenAlpha.enabled = false;
				this.cursorSprite.alpha = 0f;
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator HPValueHistoryRecorder()
	{
		for (;;)
		{
			this.hpValueHistory.Enqueue(this.hp1Bar.value);
			if (this.hpValueHistory.Count > 10)
			{
				this.hpValueHistory.Dequeue();
			}
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	public void OnRoleNameLocalized()
	{
		this._roleNameLabel.spacingX = ((!UILanguage.IsSimplifiedChinese) ? 0 : 8);
	}

	private bool play;

	[SerializeField]
	private UISprite _hp1Sprite;

	[SerializeField]
	private UISprite _hp2Sprite;

	[SerializeField]
	private UISprite _hpBgSprite;

	[SerializeField]
	private UIProgressBar hp1Bar;

	[SerializeField]
	private UIProgressBar hp2Bar;

	private TweenAlpha cursorTweenAlpha;

	private UISprite cursorSprite;

	private readonly Queue<float> hpValueHistory = new Queue<float>();

	private float hpValueWhenLastPlayAnim = 1f;

	private int _lastPlayerMaxHP;

	[SerializeField]
	private UIEnergyController energyController;

	[SerializeField]
	private UILabel _roleNameLabel;

	private Localize _roleNameLocalize;

	private string _roleName;
}
