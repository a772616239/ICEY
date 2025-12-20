using System;

public class NoArmor : EnemyArmor
{
	private void Start()
	{
		this.eAttr.currentSp = 0;
	}

	public override void HitArmor(int damage, string data)
	{
	}

	public override void Break()
	{
	}
}
