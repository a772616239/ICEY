using System;
using System.Collections;
using System.Collections.Generic;
using SaveDataModel;
using UnityEngine;

public class GameData
{
	public YieldInstruction Save(bool justSaveData = false)
	{
		if (!justSaveData)
		{
			if (LevelManager.SceneName == "ui_start")
			{
				Log.Error("在开始界面存档");
				return null;
			}
			this.PlayerPosition = ((!(R.Player.GameObject != null)) ? Vector3.zero : R.Player.Transform.position);
			this.SceneName = LevelManager.SceneName;
			this.SavePlayerAttribute(R.Player.Attribute);
		}
		return SaveManager.AutoSave(this);
	}

	public YieldInstruction Load()
	{
		if (SaveManager.IsAutoSaveDataExists)
		{
			return R.Coroutine.Start(this.LoadCoroutine());
		}
		return null;
	}

	private IEnumerator LoadCoroutine()
	{
		yield return SaveManager.AutoLoad();
		R.GameData = SaveManager.GameData;
		this.LoadPlayerAttribute(R.Player.Attribute, R.GameData.PlayerAttributeGameData);
		yield break;
	}

	public void Reset()
	{
		R.GameData = new GameData();
		this.LoadPlayerAttribute(R.Player.Attribute, R.GameData.PlayerAttributeGameData);
	}

	private void SavePlayerAttribute(PlayerAttribute attr)
	{
		this.PlayerAttributeGameData.faceDir = attr.faceDir;
		this.PlayerAttributeGameData.moveSpeed = attr.moveSpeed;
		this.PlayerAttributeGameData.maxHP = attr.maxHP;
		this.PlayerAttributeGameData.currentHP = attr.currentHP;
		this.PlayerAttributeGameData.CurrentEnergy = attr.currentEnergy;
		this.PlayerAttributeGameData.flashLevel = attr.flashLevel;
	}

	private void LoadPlayerAttribute(PlayerAttribute attr, PlayerAttributeGameData playerAttributeGameData)
	{
		attr.faceDir = playerAttributeGameData.faceDir;
		attr.moveSpeed = playerAttributeGameData.moveSpeed;
		attr.maxHP = playerAttributeGameData.maxHP;
		attr.currentHP = playerAttributeGameData.currentHP;
		attr.currentEnergy = playerAttributeGameData.CurrentEnergy;
		attr.flashLevel = playerAttributeGameData.flashLevel;
		attr.currentFlashTimes = attr.flashTimes;
	}

	public Vector3 PlayerPosition;

	public string RoleName = "ICEY";

	public string UserName = string.Empty;

	public string SceneName;

	public bool WindyVisiable = true;

	public int Difficulty;

	public readonly Dictionary<string, int> ThisSaveValidStorage = new Dictionary<string, int>();

	public readonly Dictionary<string, int> ThisRoundValidStorage = new Dictionary<string, int>();

	public readonly Dictionary<string, bool> BattleZoneDict = new Dictionary<string, bool>();

	public readonly PlayerAttributeGameData PlayerAttributeGameData = new PlayerAttributeGameData();

	public readonly Enhancement Enhancement = new Enhancement();

	public readonly Equipment Equipment = new Equipment();

	public readonly List<BloodPalaceTotalScore> BloodPalaceRecords = new List<BloodPalaceTotalScore>();
}
