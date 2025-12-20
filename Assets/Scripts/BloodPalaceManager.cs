using System;
using System.Collections;
using System.Text.RegularExpressions;
using GameWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodPalaceManager : BaseBehaviour
{
	private BloodPalaceManager()
	{
	}

	private void Awake()
	{
		if (BloodPalaceManager.Instance == null)
		{
			BloodPalaceManager.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		this._listener = R.Player.GetComponent<PlayerAnimEventListener>();
		this._listener.OnPlayerDead += this.OnPlayerDead;
		SingletonMono<BattleAssessmentManager>.Instance.EnterBloodPalace();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.SceneManagerOnSceneLoaded;
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<PassGateEventArgs>("PassGate", new EventManager.FBEventHandler<PassGateEventArgs>(this.OnPassGate), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		this._listener.OnPlayerDead -= this.OnPlayerDead;
		SceneManager.sceneLoaded -= this.SceneManagerOnSceneLoaded;
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.UnregisterEvent<PassGateEventArgs>("PassGate", new EventManager.FBEventHandler<PassGateEventArgs>(this.OnPassGate), EventManager.ListenerQueue.Game);
		BloodPalaceManager.Instance = null;
	}

	private void OnPlayerDead(object sender, EventArgs e)
	{
		if (!R.SceneData.BloodPalaceMode)
		{
			return;
		}
		R.SceneData.CanAIRun = false;
		base.StartCoroutine(this.LevelResult(true));
	}

	private bool BattleEventReceiver(string eventDefine, object sender, BattleEventArgs msg)
	{
		if (msg.Status == BattleEventArgs.BattleStatus.End)
		{
			this._coroutine = base.StartCoroutine(this.LevelResult(false));
		}
		return true;
	}

	private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
	{
		if (!Regex.IsMatch(arg0.name, "palace[1-5]"))
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
			}
			R.Ui.BloodPalace.CloseLevelResult(false);
			SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
			SingletonMono<BattleAssessmentManager>.Instance.ExitBloodPalace();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private bool OnPassGate(string eventDefine, object sender, PassGateEventArgs msg)
	{
		if (msg.Status == PassGateEventArgs.PassGateStatus.Enter)
		{
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
			}
			R.Ui.BloodPalace.CloseLevelResult(false);
			SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		}
		return true;
	}

	private IEnumerator LevelResult(bool load)
	{
		yield return new WaitForSeconds(0.1f);
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = false;
		yield return R.Ui.BloodPalace.OpenLevelResult();
		yield return new WaitForSeconds(5f);
		yield return R.Ui.BloodPalace.CloseLevelResult(true);
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		if (load)
		{
			LevelManager.OnPlayerDie(true);
		}
		yield break;
	}

	public static BloodPalaceManager Instance;

	private PlayerAnimEventListener _listener;

	private Coroutine _coroutine;
}
