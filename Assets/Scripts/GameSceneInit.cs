using System;
using I2.Loc;
using UnityEngine;

public class GameSceneInit : BaseBehaviour
{
	private void Awake()
	{
		if (!GameSceneInit._first)
		{
			return;
		}
		QualitySettings.vSyncCount = R.Settings.VSync;
		if (string.IsNullOrEmpty(R.Settings.Language))
		{
			R.Settings.Language = LocalizationManager.CurrentLanguage;
			R.Settings.Save();
		}
		else
		{
			LocalizationManager.CurrentLanguage = R.Settings.Language;
		}
		if (string.IsNullOrEmpty(R.Settings.AudioLanguage))
		{
			R.Settings.AudioLanguage = UILanguage.CurrentLanguage.DefaultAudioLanguage;
			R.Settings.Save();
		}
		Cursor.visible = false;
		Preload x = UnityEngine.Object.FindObjectOfType<Preload>();
		GameObject gameObject = null;
		if (x == null)
		{
			gameObject = Asset.LoadFromResources<GameObject>("Prefab/Core", "Preload");
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }
		
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Core");
		if (gameObject2 == null)
		{
			gameObject2 = Asset.LoadFromResources<GameObject>("Prefab/Core", "Core");
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject2);
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("World");
		if (gameObject3 == null)
		{
			gameObject3 = Asset.LoadFromResources<GameObject>("Prefab/Core", "World");
			gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject3);
		GameObject gameObject4 = GameObject.FindGameObjectWithTag("BattleZoneTrigger");
		if (gameObject4 == null)
		{
			gameObject4 = Asset.LoadFromResources<GameObject>("Prefab/Core", "BattleZoneTrigger");
			gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject4);
		EffectController effectController = UnityEngine.Object.FindObjectOfType<EffectController>();
		GameObject gameObject5;
		if (effectController == null)
		{
			gameObject5 = Asset.LoadFromResources<GameObject>("Prefab/Core", "EffectGenerator");
			gameObject5 = UnityEngine.Object.Instantiate<GameObject>(gameObject5);
		}
		else
		{
			gameObject5 = effectController.gameObject;
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject5);
		if (Camera.main == null)
		{
			GameObject gameObject6 = Asset.LoadFromResources<GameObject>("Prefab/Core", "camera");
			UnityEngine.Object.Instantiate<GameObject>(gameObject6);
		}
		if (Camera.main != null)
		{
			GameObject gameObject6 = Camera.main.transform.parent.gameObject;
			UnityEngine.Object.DontDestroyOnLoad(gameObject6);
		}
	}

	private void Start()
	{
		if (!GameSceneInit._first)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("UI Root");
		if (gameObject == null)
		{
			gameObject = Asset.LoadFromResources<GameObject>("Prefab/Core", "UI Root");
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (Debug.isDebugBuild)
		{
			GameObject gameObject2 = Asset.LoadFromResources<GameObject>("Prefab/Core", "DebugGUI");
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
			UnityEngine.Object.DontDestroyOnLoad(gameObject2);
		}
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("Player");
		if (gameObject3 == null)
		{
			gameObject3 = Asset.LoadFromResources<GameObject>("Prefab/Core", "Player");
			gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject3);
		if (gameObject3 != null)
		{
			Vector3 position = gameObject3.transform.position;
			position.z = LayerManager.ZNum.MMiddle_P;
			gameObject3.transform.position = position;
		}
		if (R.GameData.WindyVisiable)
		{
			GameObject gameObject4 = GameObject.FindGameObjectWithTag("Windy");
			if (gameObject4 == null)
			{
				gameObject4 = Asset.LoadFromResources<GameObject>("Prefab/Core", "Windy");
				gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
			}
			UnityEngine.Object.DontDestroyOnLoad(gameObject4);
		}
		SceneGate sceneGate = SingletonMono<SceneGateManager>.Instance.FindGate(1);
		if (sceneGate != null)
		{
			sceneGate.Exit(0f, SceneGate.OpenType.None);
		}
		GameSceneInit._first = false;
	}

	private static bool _first = true;
}
