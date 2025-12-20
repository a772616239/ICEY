using System;

public class EnemyAIAttribute : BaseBehaviour
{
	public int facePlayerDir
	{
		get
		{
			return InputSetting.JudgeDir(base.transform.position, R.Player.Transform.position);
		}
	}

	public EnemyAIAttribute.ExpectationParam[] attackExpectations;

	public EnemyAIAttribute.ExpectationParam currentEx;

	public enum ActionEnum
	{
		ATTACK1 = 1,
		ATTACK2,
		ATTACK3,
		ATTACK4,
		ATTACK5,
		ATTACK6,
		ATTACK7,
		ATTACK8,
		ATTACK9,
		ATTACK10,
		ATTACK11,
		ATTACK12,
		ATTACK13,
		ATTACK14,
		ATTACK15
	}

	[Serializable]
	public class ExpectationParam
	{
		public float distance;

		public float range;

		public EnemyAIAttribute.ActionEnum action;
	}
}
