using System;
using UnityEngine;

public class TrapInfoSetting : MonoBehaviour
{
	public void SetInfo(TrapAttack.AttackType type, int atkDamage, float atkInterval)
	{
		foreach (TrapAttack trapAttack in this.attack)
		{
			trapAttack.SetInfo(type, atkDamage, atkInterval);
		}
	}

	public TrapAttack[] attack;
}
