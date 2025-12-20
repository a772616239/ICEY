using System;
using UnityEngine;

public class UIMoneyController : MonoBehaviour
{
	public int CoinNum
	{
		get
		{
			return this._coinNum;
		}
		set
		{
			if (this._coinNum != value)
			{
				this._coinNum = value;
				this.moneyLabel.text = value.ToString();
			}
		}
	}

	private void Update()
	{
		this.CoinNum = R.Equipment.CoinNum;
	}

	private int _coinNum;

	[SerializeField]
	private UILabel moneyLabel;
}
