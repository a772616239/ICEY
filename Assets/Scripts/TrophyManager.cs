using System;

public class TrophyManager
{
	public bool AwardTrophy(int index)
	{
		return SingletonMono<AchievementManager>.Instance.AwardAchievement(index);
	}

	public void AwardAllTrophies()
	{
		SingletonMono<AchievementManager>.Instance.AwardAll();
	}
}
