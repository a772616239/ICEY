using System;
using UnityEngine;

public class UIBloodPalaceScoreboardColumeController : MonoBehaviour
{
	public int Time
	{
		set
		{
			TimeSpan timeSpan = new TimeSpan(0, 0, value);
			this._time.text = string.Format("{0:F0}:{1:D2}", timeSpan.TotalMinutes, timeSpan.Seconds);
		}
	}

	public bool NoHurt
	{
		set
		{
			this._noHurt.gameObject.SetActive(value);
		}
	}

	public int Score
	{
		set
		{
			this._score.text = value.ToString();
		}
	}

	private void Awake()
	{
		this._time = base.transform.Find("Time").GetComponent<UILabel>();
		this._score = base.transform.Find("Score").GetComponent<UILabel>();
		this._noHurt = this._score.transform.Find("NoHurt").GetComponent<UISprite>();
	}

	private UISprite _noHurt;

	private UILabel _score;

	private UILabel _time;
}
