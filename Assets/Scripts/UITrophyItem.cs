using System;
using UnityEngine;

public class UITrophyItem : MonoBehaviour
{
	public int TrophyId
	{
		get
		{
			return this._trophyId;
		}
		set
		{
			this._trophyId = value;
			this.DataBind();
		}
	}

	public string TrophyIconSpriteName
	{
		get
		{
			return this._trophyIcon.spriteName;
		}
		set
		{
			this._trophyIcon.spriteName = value;
		}
	}

	public string TrophyName
	{
		get
		{
			return this._trophyName.text;
		}
		set
		{
			this._trophyName.text = value;
		}
	}

	public string TrophyDetail
	{
		get
		{
			return this._trophyDetail.text;
		}
		set
		{
			this._trophyDetail.text = value;
		}
	}

	private void Awake()
	{
		this._widget = base.GetComponent<UIWidget>();
	}

	private void OnEnable()
	{
		this.DataBind();
	}

	private void DataBind()
	{
		AchievementManager.AchievementInfo achievementInfo = SingletonMono<AchievementManager>.Instance.GetAchievementInfo(this.TrophyId);
		bool achievementUnlockState = SingletonMono<AchievementManager>.Instance.GetAchievementUnlockState(this.TrophyId);
		this.TrophyName = achievementInfo.Name;
		this.TrophyDetail = achievementInfo.Detail;
		this.TrophyIconSpriteName = string.Format("{0}{1}", this.TrophyId, (!achievementUnlockState) ? "_l" : string.Empty);
		this._widget.alpha = ((!achievementUnlockState) ? 0.5f : 1f);
	}

	[SerializeField]
	private UISprite _trophyIcon;

	[SerializeField]
	private UILabel _trophyName;

	[SerializeField]
	private UILabel _trophyDetail;

	private int _trophyId;

	private UIWidget _widget;
}
