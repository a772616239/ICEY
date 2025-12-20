using System;
using DatabaseModel;
using UnityEngine;

public class EnemyAutoGetAttribute : BaseBehaviour
{
	private void Awake()
	{
		EnemyAttrData basicData = EnemyAttrData.FindBySceneNameAndType(LevelManager.SceneName, this.type);
		base.GetComponent<EnemyAttribute>().SetBasicData(basicData);
	}

	[Header("怪物类型")]
	public EnemyType type;
}
