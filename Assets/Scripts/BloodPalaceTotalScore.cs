using System;

public class BloodPalaceTotalScore : IComparable<BloodPalaceTotalScore>
{
	public void AddLevelScore(BloodPalaceLevelScore levelScore)
	{
		this.Score += levelScore.Score;
		this.NotHurt &= levelScore.NotHurt;
		this.Time += levelScore.Time;
	}

	public int CompareTo(BloodPalaceTotalScore other)
	{
		return -1 * this.Score.CompareTo(other.Score);
	}

	public int Score;

	public bool NotHurt = true;

	public float Time;
}
