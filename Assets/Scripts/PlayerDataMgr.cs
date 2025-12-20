using System;
using CIS;
using UnityEngine;

public class PlayerDataMgr : MonoBehaviour
{
	public static PlayerDataMgr Instance
	{
		get
		{
			if (PlayerDataMgr._instance == null)
			{
				PlayerDataMgr._instance = UnityEngine.Object.FindObjectOfType<PlayerDataMgr>();
			}
			return PlayerDataMgr._instance;
		}
	}

	public void PersistSectionData()
	{
		GameSection currGameSection = this.logic.currGameSection;
		if (currGameSection == null)
		{
			return;
		}
		string name = currGameSection.name;
		PlayerPrefs.SetString("currSectionName", name);
	}

	public string UnPersistSectionData()
	{
		return PlayerPrefs.GetString("currSectionName");
	}

	private void Start()
	{
		this.logic = SingletonMonoBehaviourClass<GameLogicMgr>.instance;
	}

	private static PlayerDataMgr _instance;

	private GameLogicMgr logic;
}
