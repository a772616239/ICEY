using System;
using UnityEngine;

[ExecuteInEditMode]
public class SupplyAttribute : BaseBehaviour
{
	[HideInInspector]
	public bool Opened
	{
		get
		{
			return (!this.CanTriggerInNewRound) ? SaveStorage.Get("SupplyBox" + LevelManager.SceneName + base.name, false) : RoundStorage.Get("SupplyBox" + LevelManager.SceneName + base.name, false);
		}
		set
		{
			if (this.CanTriggerInNewRound)
			{
				RoundStorage.Set("SupplyBox" + LevelManager.SceneName + base.name, value);
			}
			else
			{
				SaveStorage.Set("SupplyBox" + LevelManager.SceneName + base.name, value);
			}
		}
	}

	private void Awake()
	{
		this.CurrentHp = this.MaxHp;
		this._id = "SupplyBox" + LevelManager.SceneName + base.name;
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			this.MaxHp = (uint)(Mathf.FloorToInt(this.MaxHp / 4f) * 4);
		}
	}

	[SerializeField]
	private string _id;

	[Header("在此距离范围内显示宝箱内容")]
	[SerializeField]
	public float ShowDistance = 5f;

	[SerializeField]
	[Header("宝箱类型")]
	public SupplyAttribute.SupplyBoxType SupplyBox;

	[SerializeField]
	[Header("获得奖励值")]
	public int Bonus;

	[SerializeField]
	[Header("宝箱最大受攻击次数，只能为4的倍数")]
	public uint MaxHp = 4u;

	[SerializeField]
	[Header("是否可以在新周目重复触发")]
	public bool CanTriggerInNewRound;

	[HideInInspector]
	public uint CurrentHp;

	public enum SupplyBoxType
	{
		Money,
		Cheat = 2,
		Flash
	}
}
