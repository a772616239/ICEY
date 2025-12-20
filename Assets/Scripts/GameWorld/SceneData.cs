using System;

namespace GameWorld
{
	public class SceneData
	{
		public bool isPausing
		{
			get
			{
				return WorldTime.IsPausing;
			}
		}

		public void Clear()
		{
			this.assessmentData = new BattleAssessmentData();
		}

		public bool CanAIRun;

		public bool BloodPalaceMode;

		public BloodPalaceTotalScore TotalScore;

		public BloodPalaceLevelScore LevelScore = new BloodPalaceLevelScore();

		public int currentBattleZoneId = -1;

		public BattleAssessmentData assessmentData = new BattleAssessmentData();
	}
}
