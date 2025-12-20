using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIEnergyController : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.EnergyValue > this.EnergyMaxValue)
		{
			this.EnergyValue = this.EnergyMaxValue;
		}
		for (int i = 0; i < this.energyItems.Length; i++)
		{
			this.energyItems[i].enabled = (i < this.EnergyValue);
		}
		for (int j = 0; j < this.energyGroup.Length; j++)
		{
			this.energyGroup[j].enabled = (j < this.EnergyMaxValue / 2);
		}
	}

	[Range(0f, 20f)]
	[SerializeField]
	public int EnergyValue;

	[SerializeField]
	[Range(6f, 20f)]
	public int EnergyMaxValue;

	[SerializeField]
	private UISprite[] energyItems;

	[SerializeField]
	private UISprite[] energyGroup;
}
