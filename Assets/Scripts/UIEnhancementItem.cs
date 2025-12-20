using System;
using GameWorld;
using UnityEngine;

public class UIEnhancementItem : MonoBehaviour
{
	private string LocalizeName
	{
		get
		{
			return this._name.text;
		}
		set
		{
			this._name.text = value;
		}
	}

	private int Level
	{
		get
		{
			return this._intLevel;
		}
		set
		{
			this._intLevel = value;
			this._level.text = "Lv " + this._intLevel;
		}
	}

	private void Start()
	{
		this.DataBind();
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
		EventDelegate.Add(this._toggle.onChange, new EventDelegate.Callback(this.OnToggleSelect));
		this.DataBind();
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
		EventDelegate.Remove(this._toggle.onChange, new EventDelegate.Callback(this.OnToggleSelect));
	}

	private void OnToggleSelect()
	{
		if (UIToggle.current.value && UIToggle.current == this._toggle)
		{
			UIEnhancementItem.Current = this.DataBind();
		}
	}

	private bool OnEnhancementLevelUp(string eventDefine, object sender, EnhanceArgs msg)
	{
		if (msg.Name == this.Name)
		{
			UIEnhancementItem.Current = this.DataBind();
		}
		else
		{
			this.DataBind();
		}
		return true;
	}

	private EnhancementItem DataBind()
	{
		EnhancementItem enhancementItem = new EnhancementItem(this.Name);
		this.LocalizeName = enhancementItem.LocalizeName;
		this.Level = enhancementItem.Level;
		if (enhancementItem.CoinEnough)
		{
			this._name.color = UIEnhancementItem.LightYellow;
			this._level.color = UIEnhancementItem.LightYellow;
		}
		else if (enhancementItem.Level == 0)
		{
			this._name.color = UIEnhancementItem.LightGray;
			this._level.color = UIEnhancementItem.LightGray;
		}
		else
		{
			this._name.color = Color.white;
			this._level.color = Color.white;
		}
		return enhancementItem;
	}

	public static EnhancementItem Current;

	public string Name;

	[SerializeField]
	private UIToggle _toggle;

	[SerializeField]
	private UILabel _name;

	[SerializeField]
	private UILabel _level;

	private int _intLevel;

	private static readonly Color LightYellow = new Color(0.647f, 1f, 0.31f);

	private static readonly Color LightGray = new Color(0.25f, 0.25f, 0.25f);
}
