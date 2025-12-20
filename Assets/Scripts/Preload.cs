using System;
using UnityEngine;

public class Preload : BaseBehaviour
{
	private void Awake()
	{
		Singleton<ResolutionOption>.Instance.SetResolutionByQualitylevel();
		EnemyGenerator.PreloadEnemyPrefabs();
		Log.Info("Enemy Prefabs are loaded");
		DB.Preload();
		Log.Info("DB is loaded");
		EffectController.AllowPreload = true;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Asset.LoadFromResources<GameObject>("Prefab/Core", "PrefabAnimationPreload"));
		gameObject.transform.parent = base.transform;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
