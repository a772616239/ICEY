using System;
using System.Collections.Generic;
using ExtensionMethods;
using LitJson;
using UnityEngine;

public class EnemyDataPreload : SingletonMono<EnemyDataPreload>
{
	private void Awake()
	{
		this.DataInit(this.attackData, this.attack);
		Log.Info("attackData is loaded");
		this.DataInit(this.hurtData, this.hurt);
		Log.Info("hurtData is loaded");
		this.VibrationData = this.ParseJson<float[][]>("VibrationConfig");
	}

	private void DataInit(EnemyDataPreload.EnemyData[] data, IDictionary<EnemyType, JsonData> dict)
	{
		for (int i = 0; i < data.Length; i++)
		{
			dict.Add(data[i].type, JsonMapper.ToObject(data[i].text.text));
		}
	}

	private T ParseJson<T>(string name)
	{
		return Asset.DeserializeFromFile<T>("Conf/", name);
	}

	[ContextMenu("load")]
	private void Load()
	{
		string[] names = Enum.GetNames(typeof(EnemyType));
		this.attackData = new EnemyDataPreload.EnemyData[names.Length];
		this.hurtData = new EnemyDataPreload.EnemyData[names.Length];
		for (int i = 0; i < names.Length; i++)
		{
			this.attackData[i] = new EnemyDataPreload.EnemyData();
			this.hurtData[i] = new EnemyDataPreload.EnemyData();
			this.attackData[i].type = names[i].ToEnum<EnemyType>(false);
			this.hurtData[i].type = names[i].ToEnum<EnemyType>(false);
		}
	}

	[SerializeField]
	private EnemyDataPreload.EnemyData[] attackData;

	[SerializeField]
	private EnemyDataPreload.EnemyData[] hurtData;

	public Dictionary<EnemyType, JsonData> attack = new Dictionary<EnemyType, JsonData>();

	public Dictionary<EnemyType, JsonData> hurt = new Dictionary<EnemyType, JsonData>();

	public float[][] VibrationData;

	[Serializable]
	public class EnemyData
	{
		public TextAsset text;

		public EnemyType type;
	}
}
