using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBloodPalaceController : MonoBehaviour
{
	[ContextMenu("OpenScoreboard")]
	public YieldInstruction OpenScoreboard(bool withAudio = true)
	{
		this._scoreboard.Animator.gameObject.SetActive(true);
		this._scoreboard.Animator.Play("ScoreboardOpen");
		if (withAudio)
		{
			R.Audio.PlayEffect(485, null);
		}
		List<BloodPalaceTotalScore> bloodPalaceRecords = R.GameData.BloodPalaceRecords;
		for (int i = 0; i < 10; i++)
		{
			UIBloodPalaceScoreboardColumeController uibloodPalaceScoreboardColumeController = this._scoreboard.BloodPalaceScoreboardColumes[i];
			if (i < bloodPalaceRecords.Count)
			{
				BloodPalaceTotalScore bloodPalaceTotalScore = bloodPalaceRecords[i];
				uibloodPalaceScoreboardColumeController.gameObject.SetActive(true);
				uibloodPalaceScoreboardColumeController.Time = (int)bloodPalaceTotalScore.Time;
				uibloodPalaceScoreboardColumeController.NoHurt = bloodPalaceTotalScore.NotHurt;
				uibloodPalaceScoreboardColumeController.Score = bloodPalaceTotalScore.Score;
			}
			else
			{
				uibloodPalaceScoreboardColumeController.gameObject.SetActive(false);
			}
		}
		return new WaitForSeconds(1f);
	}

	[ContextMenu("CloseScoreboard")]
	public YieldInstruction CloseScoreboard(bool withAudio = true)
	{
		if (!this._scoreboard.Animator.gameObject.activeSelf)
		{
			return null;
		}
		this._scoreboard.Animator.Play("ScoreboardClose");
		if (withAudio)
		{
			R.Audio.PlayEffect(486, null);
		}
		return base.StartCoroutine(this.DisableOnEndOfStateCoroutine(this._scoreboard.Animator));
	}

	[ContextMenu("OpenLevelResult")]
	public YieldInstruction OpenLevelResult()
	{
		R.Ui.Pause.Enabled = false;
		this._levelResult.Animator.gameObject.SetActive(true);
		BloodPalaceLevelScore levelScore = R.SceneData.LevelScore;
		this._levelResult.WaveScore = levelScore.WaveScore;
		this._levelResult.BeautyScore = levelScore.BeautyScore;
		this._levelResult.Time = (int)levelScore.Time;
		this._levelResult.FlashPercent = levelScore.FlashPercent;
		this._levelResult.OriginalScore = levelScore.OriginalScore;
		this._levelResult.Score = levelScore.Score;
		this._levelResult.NoHurt = levelScore.NotHurt;
		this._levelResult.Animator.SetBool("noHurt", levelScore.NotHurt);
		this._levelResult.Animator.Play("LevelResultOpen");
		R.Audio.PlayEffect(485, null);
		return new WaitForSeconds(1f);
	}

	[ContextMenu("CloseLevelResult")]
	public YieldInstruction CloseLevelResult(bool withAudio = true)
	{
		if (!this._levelResult.Animator.gameObject.activeSelf)
		{
			return null;
		}
		this._levelResult.Animator.Play("LevelResultClose");
		if (withAudio)
		{
			R.Audio.PlayEffect(486, null);
		}
		return base.StartCoroutine(this.DisableOnEndOfStateCoroutine(this._levelResult.Animator));
	}

	private IEnumerator DisableOnEndOfStateCoroutine(Animator animator)
	{
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		R.Ui.Pause.Enabled = true;
		yield break;
	}

	[SerializeField]
	private UIBloodPalaceController.UIScoreboard _scoreboard;

	[SerializeField]
	private UIBloodPalaceController.UILevelResult _levelResult;

	[Serializable]
	private class UIScoreboard
	{
		public UIBloodPalaceScoreboardColumeController[] BloodPalaceScoreboardColumes;

		public Animator Animator;
	}

	[Serializable]
	private class UILevelResult
	{
		public int WaveScore
		{
			set
			{
				this._waveScore.text = value.ToString();
			}
		}

		public int BeautyScore
		{
			set
			{
				this._beautyScore.text = value.ToString();
			}
		}

		public int Time
		{
			set
			{
				TimeSpan timeSpan = new TimeSpan(0, 0, value);
				this._time.text = string.Format("{0:F0}:{1:D2}", timeSpan.TotalMinutes, timeSpan.Seconds);
			}
		}

		public float FlashPercent
		{
			set
			{
				this._flashPercent.text = string.Format("{0:F0}%", value * 100f);
			}
		}

		public int OriginalScore
		{
			set
			{
				this._originalScore.text = value.ToString();
			}
		}

		public int Score
		{
			set
			{
				this._score.text = value.ToString();
			}
		}

		public bool NoHurt
		{
			set
			{
				this._noHurt.gameObject.SetActive(value);
			}
		}

		[SerializeField]
		private UILabel _beautyScore;

		[SerializeField]
		private UILabel _flashPercent;

		[SerializeField]
		private UISprite _noHurt;

		[SerializeField]
		private UILabel _originalScore;

		[SerializeField]
		private UILabel _score;

		[SerializeField]
		private UILabel _time;

		[SerializeField]
		private UILabel _waveScore;

		public Animator Animator;
	}
}
