using System;
using UnityEngine;

public class UIEnhancementDescController : MonoBehaviour
{
	private int Coin
	{
		get
		{
			return int.Parse(this._coin.text);
		}
		set
		{
			this._coin.text = value.ToString();
		}
	}

	private int TotalCoin
	{
		get
		{
			return this._totalCoinCache;
		}
		set
		{
			if (this._totalCoinCache != value)
			{
				this._totalCoinCache = value;
				this._totalCoin.text = value.ToString();
			}
		}
	}

	private string Desc
	{
		get
		{
			return this._desc.text;
		}
		set
		{
			this._desc.text = value;
		}
	}

	private string NextLevelDesc
	{
		get
		{
			return this._nextLevelDesc.text;
		}
		set
		{
			this._nextLevelDesc.text = value;
		}
	}

	private string Controlkeys
	{
		get
		{
			return this.control_keys.text;
		}
		set
		{
			this.control_keys.text = value;
		}
	}

	private void Awake()
	{
		this._deadlyDiscIcon = this.control_keys.transform.GetChild(0).gameObject;
	}

	private void OnEnable()
	{
        if(UIEnhancementItem.Current!=null)
		this._coin.color = ((!UIEnhancementItem.Current.CoinEnough) ? UIEnhancementDescController.ColorRed : Color.white);
	}

	private void Update()
	{
		this.TotalCoin = R.Equipment.CoinNum;
		EnhancementItem current = UIEnhancementItem.Current;
		if (current != null && !current.Equals(this._currentItem))
		{
			this._currentItem = current;
			this._level = this._currentItem.Level;
			if (this._level == 3)
			{
				this._coin.gameObject.SetActive(false);
				this._nextLevelDesc.gameObject.SetActive(false);
				this._upGradeGameObject.SetActive(false);
			}
			else
			{
				this._coin.gameObject.SetActive(true);
				this._nextLevelDesc.gameObject.SetActive(true);
				this._upGradeGameObject.SetActive(true);
				this.Coin = this._currentItem.Cost;
				this.NextLevelDesc = this._currentItem.NextLevelDesc;
			}
			this.Desc = this._currentItem.Desc;
			string controlkeys = this._currentItem.Controlkeys;
			if (controlkeys == "流")
			{
				this.Controlkeys = string.Empty;
				this._deadlyDiscIcon.SetActive(true);
			}
			else
			{
				if (this._deadlyDiscIcon.activeSelf)
				{
					this._deadlyDiscIcon.SetActive(false);
				}
				this.Controlkeys = controlkeys;
			}
			this._coin.color = ((!current.CoinEnough) ? UIEnhancementDescController.ColorRed : Color.white);
		}
	}

	private static readonly Color ColorRed = new Color(1f, 0.333333343f, 0.333333343f);

	private EnhancementItem _currentItem;

	[SerializeField]
	private UILabel _desc;

	[SerializeField]
	private UILabel _coin;

	[SerializeField]
	private UILabel _totalCoin;

	[SerializeField]
	private UILabel _nextLevelDesc;

	[SerializeField]
	private UILabel control_keys;

	private GameObject _deadlyDiscIcon;

	[SerializeField]
	private GameObject _upGradeGameObject;

	private int _level;

	private int _totalCoinCache;
}
