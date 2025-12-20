using System;

public abstract class EnemyArmor : BaseBehaviour
{
	private void Awake()
	{
		this.eAttr = base.GetComponent<EnemyAttribute>();
	}

	public abstract void HitArmor(int damage, string data);

	public abstract void Break();

	protected EnemyAttribute eAttr;
}
