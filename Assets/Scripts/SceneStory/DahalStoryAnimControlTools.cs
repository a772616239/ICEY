using System;
using UnityEngine;

namespace SceneStory
{
	public class DahalStoryAnimControlTools : BaseBehaviour
	{
		private void OnEnable()
		{
			GameObject boss = R.Enemy.Boss;
			if (boss == null)
			{
				return;
			}
			this.dac = boss.GetComponent<DahalAction>();
			this.smc = boss.GetComponent<StateMachine>();
		}

		public void DahalHasGravity(BoolEnum open)
		{
			if (open == BoolEnum.True)
			{
				this.dac.GetComponent<TimeController>().SetGravity(1f);
			}
			if (open == BoolEnum.False)
			{
				this.dac.GetComponent<TimeController>().SetGravity(0f);
			}
		}

		public void DahalTurnRound(int dir)
		{
			this.dac.ChangeFace(dir);
		}

		public void DahalChangeState(DahalAction.StateEnum nextState)
		{
			this.smc.SetState(nextState);
		}

		public void DahalStartMove()
		{
			this.dac.AutoMove();
		}

		public void DahalStopMove()
		{
			this.dac.StopMoveToIdle();
		}

		private DahalAction dac;

		private StateMachine smc;
	}
}
