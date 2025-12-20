using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseModel;
using ExtensionMethods;
using UnityEngine;

public class EnemyGenerator : Singleton<EnemyGenerator>
{
	private static IDictionary<EnemyType, EnemyGenerator.EnemyPrefab> EnemyPrefabs
	{
		get
		{
			IDictionary<EnemyType, EnemyGenerator.EnemyPrefab> result;
			if ((result = EnemyGenerator._enemyPrefabs) == null)
			{
				result = (EnemyGenerator._enemyPrefabs = EnemyGenerator.LoadEnemyPrefabs());
			}
			return result;
		}
	}

	public static void PreloadEnemyPrefabs()
	{
		EnemyGenerator._enemyPrefabs = EnemyGenerator.LoadEnemyPrefabs();
	}

	private static IDictionary<EnemyType, EnemyGenerator.EnemyPrefab> LoadEnemyPrefabs()
	{
		return (from e in Asset.DeserializeFromFile<EnemyGenerator.EnemyGeneratorJsonObject[]>("Conf/", "EnemyGeneratorConfig")
		select new EnemyGenerator.EnemyPrefab
		{
			type = e.typeString.ToEnum<EnemyType>(false),
			prefab = Asset.LoadFromResources<GameObject>("Prefab/Enemys", e.prefabName),
			inUse = e.inUse
		}).ToDictionary((EnemyGenerator.EnemyPrefab e) => e.type);
	}

	public static EnemyType GetEnemyType(string name)
	{
		name = name.Replace("(Clone)", string.Empty);
		if (EnemyGenerator._enemyNameToType == null)
		{
			EnemyGenerator._enemyNameToType = EnemyGenerator.EnemyPrefabs.ToDictionary((KeyValuePair<EnemyType, EnemyGenerator.EnemyPrefab> e) => e.Value.prefab.name, (KeyValuePair<EnemyType, EnemyGenerator.EnemyPrefab> e) => e.Key);
		}
		return EnemyGenerator._enemyNameToType[name];
	}

	public GameObject GenerateEnemy(string name, Vector2? pos = null, bool withEffect = true, bool enemyPoint = true)
	{
		return this.GenerateEnemy(EnemyGenerator.GetEnemyType(name), pos, withEffect, enemyPoint);
	}

	public GameObject GenerateEnemy(EnemyType type, Vector2? pos = null, bool withEffect = true, bool enemyPoint = true)
	{
		return this.GenerateEnemy(EnemyGenerator.EnemyPrefabs[type], (pos == null) ? (Vector2)EnemyGenerator.EnemyPrefabs[type].prefab.transform.position : pos.Value, withEffect, enemyPoint);
	}

	private GameObject GenerateEnemy(EnemyGenerator.EnemyPrefab enemyPrefab, Vector2 pos, bool withEffect, bool enemyPoint)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyPrefab.prefab);
		EnemyAttribute component = gameObject.GetComponent<EnemyAttribute>();
		if (component != null)
		{
			gameObject.transform.position = new Vector3(pos.x, pos.y, LayerManager.ZNum.MMiddleE(component.rankType));
			gameObject.transform.localScale = Vector3.one;
			if (Application.isPlaying)
			{
				component.SetBasicData(EnemyAttrData.FindBySceneNameAndType(LevelManager.SceneName, enemyPrefab.type));
				pos.y = this.ClampOverGround(pos);
				if (enemyPoint || component.InTheWorld)
				{
					R.Ui.CreateEnemyPoint(component);
				}
				EnemyBaseAction component2 = component.GetComponent<EnemyBaseAction>();
				component2.AppearAtPosition(pos);
				if (withEffect)
				{
					component2.AppearEffect(pos);
				}
			}
		}
		else
		{
			gameObject.transform.position = new Vector3(pos.x, pos.y, LayerManager.ZNum.MMiddleE(EnemyAttribute.RankType.Normal));
			gameObject.transform.localScale = Vector3.one;
		}
		return gameObject;
	}

	private float ClampOverGround(Vector3 pos)
	{
		BoxCollider2D boxCollider2D = Physics2D.OverlapPoint(pos, LayerManager.GroundMask) as BoxCollider2D;
		if (boxCollider2D == null)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.down, GameArea.MapRange.height, LayerManager.GroundMask);
			pos.y -= raycastHit2D.distance;
			return pos.y;
		}
		return boxCollider2D.transform.position.y + boxCollider2D.size.y * boxCollider2D.transform.localScale.y / 2f;
	}

	private const string ConfigFileName = "EnemyGeneratorConfig";

	private const string EnemysPrefabPath = "Prefab/Enemys";

	private static IDictionary<string, EnemyType> _enemyNameToType;

	private static IDictionary<EnemyType, EnemyGenerator.EnemyPrefab> _enemyPrefabs;

	[Serializable]
	public class EnemyPrefab
	{
		public GameObject prefab;

		public EnemyType type;

		public bool inUse;
	}

	[Serializable]
	public class EnemyGeneratorJsonObject
	{
		public string prefabName;

		public string typeString;

		public bool inUse;
	}
}
