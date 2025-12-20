using System;
using BehaviorDesigner.Runtime.Tasks;

public class StickerAIConditional
{
	[TaskDescription("巨柱血量判断")]
	[TaskCategory("Enemy/Sticker")]
	public class StickerHPJudge : Conditional
	{
		public override void OnAwake()
		{
			this.eAttr = base.GetComponent<EnemyAttribute>();
		}

		public override TaskStatus OnUpdate()
		{
			bool flag = this.eAttr.currentHp < this.eAttr.maxHp / 2;
			if (flag)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		private EnemyAttribute eAttr;
	}
}
