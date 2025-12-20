using System;

public class BloodPalaceLevelScore
{
	public int Score
	{
		get
		{
			return this.OriginalScore * ((!this.NotHurt) ? 1 : 5);
		}
	}

	public void Clear()
	{
		this.WaveScore = 0;
		this.BeautyScore = 0;
		this.Time = 0f;
		this.FlashPercent = 0f;
		this.OriginalScore = 0;
		this.NotHurt = true;
	}

	public int WaveScore;

	public int BeautyScore;

	public float Time;

	public float FlashPercent;

	public int OriginalScore;

	public bool NotHurt = true;
}
