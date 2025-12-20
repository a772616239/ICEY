using System;

namespace GameWorld
{
	public static class EventDefine
	{
		public const string PlayerHurtAtk = "PlayerHurtAtk";

		public const string PlayerHurt = "PlayerHurt";

		public const string EnemyHurtAtk = "EnemyHurtAtk";

		public const string EnemyKilled = "EnemyKilled";

		[Obsolete]
		public const string BossPartBreak = "BossPartBreak";

		public const string HitWall = "HitWall";

		public const string Assessment = "Assessment";

		public const string EnemyFindPlayer = "EnemyFindPlayer";

		public const string DestroyEffect = "DestroyEffect";

		public const string QTE = "QTE";

		public const string PassGate = "PassGate";

		public const string Battle = "Battle";

		public const string BattleRush = "BattleRush";

		public const string EnhanceLevelup = "EnhanceLevelup";

		public const string PauseGame = "PauseGame";

		public const string QualityChange = "QualityChange";
	}
}
