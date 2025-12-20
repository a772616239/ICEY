using System;

namespace GameWorld
{
	public class BattleRushEventArgs : EventArgs
	{
		public BattleRushEventArgs(int num, int wave)
		{
			this.Num = num;
			this.Wave = wave;
		}

		public readonly int Num;

		public readonly int Wave;
	}
}
