using System;

public class BattleAssessmentData
{
	public void ComboClear()
	{
		this.ComboData.SameComboNum = 0;
		this.ComboData.AirComboNum = 0;
		this.ComboData.FlashAttackSuccessNum = 0;
		this.ComboData.CoreBreakNum = 0;
		this.ComboData.AllDamagePercent = 0f;
	}

	public void BattleClear()
	{
		this.FlashAttackSuccessNum = 0;
		this.FlashAttackNum = 0;
	}

	public BattleAssessmentData.BattleComboData ComboData = new BattleAssessmentData.BattleComboData();

	public int FlashAttackSuccessNum;

	public int FlashAttackNum;

	public bool NotHurt = true;

	public int CurrentComboNum;

	public class BattleComboData
	{
		public int SameComboNum;

		public int AirComboNum;

		public int FlashAttackSuccessNum;

		public int CoreBreakNum;

		public float AllDamagePercent;
	}
}
