using System;
using System.Collections.Generic;
using System.Xml;
using I2.Loc;
using UnityEngine;

public class AchievementManager : SingletonMono<AchievementManager>
{
	private void Awake()
	{
		this._originalLanguageInfoDictionary = new Dictionary<string, string>
		{
			{
				"English (United States)",
				this._defaultLanguageInfo.text
			},
			{
				"Chinese (Simplified)",
				this._simpChineseLanguageInfo.text
			},
			{
				"Chinese (Traditional)",
				this._tradChineseLanguageInfo.text
			}
		};
		foreach (KeyValuePair<string, string> keyValuePair in this._originalLanguageInfoDictionary)
		{
			XmlNodeList xmlInfo = this.GetXmlInfo(keyValuePair.Value);
			this._achievementInfoDictionary.Add(keyValuePair.Key, new Dictionary<int, AchievementManager.AchievementInfo>());
			this.SetDictionaryInfo(xmlInfo, this._achievementInfoDictionary[keyValuePair.Key]);
		}
	}

	public bool AwardAchievement(int index)
	{
		if (this.GetAchievementUnlockState(index))
		{
			return false;
		}
		this.UnlockAchievement(index);
		R.Ui.TrophyNotification.AwardTrophy(this.GetAchievementInfo(index).Name, index.ToString());
		this.UnlockHastur();
		return true;
	}

	public void AwardAll()
	{
		for (int i = 1; i < AchievementManager.TrophyMaxCount; i++)
		{
			this.AwardAchievement(i);
		}
	}

	public AchievementManager.AchievementInfo GetAchievementInfo(int index)
	{
		bool flag = this._achievementInfoDictionary.ContainsKey(LocalizationManager.CurrentLanguage);
		return this._achievementInfoDictionary[(!flag) ? "English (United States)" : LocalizationManager.CurrentLanguage][index];
	}

	public bool GetAchievementUnlockState(int index)
	{
		return R.Settings.AchievementInfo.Contains(index);
	}

	private void UnlockAchievement(int index)
	{
		if (!this.GetAchievementUnlockState(index))
		{
			R.Settings.AchievementInfo.Add(index);
		}
		R.Settings.Save();
	}

	private void UnlockHastur()
	{
		if (R.Settings.AchievementInfo.Count == AchievementManager.TrophyMaxCount - 1)
		{
			this.AwardAchievement(0);
		}
	}

	private XmlNodeList GetXmlInfo(string text)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(text);
		return xmlDocument.SelectNodes("trophyconf/trophy");
	}

	private void SetDictionaryInfo(XmlNodeList nodeList, Dictionary<int, AchievementManager.AchievementInfo> dictionary)
	{
		for (int i = 0; i < nodeList.Count; i++)
		{
			XmlNode xmlNode = nodeList.Item(i);
			if (xmlNode != null)
			{
				dictionary.Add(i, new AchievementManager.AchievementInfo
				{
					Id = i,
					Name = xmlNode.ChildNodes[0].InnerText,
					Detail = xmlNode.ChildNodes[1].InnerText
				});
			}
		}
	}

	private static readonly int TrophyMaxCount = 34;

	private readonly Dictionary<string, Dictionary<int, AchievementManager.AchievementInfo>> _achievementInfoDictionary = new Dictionary<string, Dictionary<int, AchievementManager.AchievementInfo>>();

	[SerializeField]
	private TextAsset _defaultLanguageInfo;

	private Dictionary<string, string> _originalLanguageInfoDictionary = new Dictionary<string, string>();

	[SerializeField]
	private TextAsset _simpChineseLanguageInfo;

	[SerializeField]
	private TextAsset _tradChineseLanguageInfo;

	public struct AchievementInfo
	{
		public int Id;

		public string Name;

		public string Detail;
	}
}
