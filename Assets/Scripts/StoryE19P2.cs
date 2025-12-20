using System;
using System.Collections;
using Core;
using DG.Tweening;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE19P2 : BaseBehaviour
{
	private bool e19elevator
	{
		get
		{
			return RoundStorage.Get("E19_Elevator", false);
		}
		set
		{
			RoundStorage.Set("E19_Elevator", value);
		}
	}

	private void Start()
	{
		this.battleCount = 1;
		this.rushCount = 0;
		this.battle1Over = false;
		this.battle2Over = false;
		base.StartCoroutine(this.MainCoroutine());
	}

	private void Update()
	{
		if (Core.Input.Game.L2.OnPressed && !this.cheater && this.KillAll() > 0)
		{
			this.cheater = true;
			SingletonMono<MobileInputPlayer>.Instance.Button6Visible = false;
			base.StartCoroutine(this.CheaterCoroutine());
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<BattleRushEventArgs>("BattleRush", new EventManager.FBEventHandler<BattleRushEventArgs>(this.BattleRush), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
		EventManager.UnregisterEvent<BattleRushEventArgs>("BattleRush", new EventManager.FBEventHandler<BattleRushEventArgs>(this.BattleRush), EventManager.ListenerQueue.Game);
	}

	private void OnDestroy()
	{
		R.Ui.Tutorial.Hide(new int?(8));
		SingletonMono<MobileInputPlayer>.Instance.Button6Visible = false;
		CameraFilterUtils.Remove<CameraFilterPack_Blur_Movie>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_VHS>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
		CameraFilterUtils.Remove<CameraFilterPack_VHS_Tracking>(null);
	}

	private bool BattleOver(string eventName, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			if (this.battleCount == 1)
			{
				this.battle1Over = true;
				return true;
			}
			if (this.battleCount == 2)
			{
				this.battle2Over = true;
				return true;
			}
		}
		return true;
	}

	private bool BattleRush(string eventDefine, object sender, BattleRushEventArgs msg)
	{
		this.rushCount = msg.Num;
		int num = this.battleCount;
		if (num != 1)
		{
			if (num == 2)
			{
				this.PlayBattleVoiceTwo();
			}
		}
		else
		{
			this.PlayBattleVoiceOne();
		}
		return true;
	}

	private IEnumerator MainCoroutine()
	{
		yield return R.Audio.PlayVoiceOver("e19t5", null, false);
		while (this.attribute.CurrentHp == this.attribute.MaxHp)
		{
			yield return null;
		}
		yield return R.Audio.PlayVoiceOver("e19t6", null, false);
		yield return this.attribute.GetComponent<SupplyBoxAction>().WaitForBreak();
		yield return new WaitForSeconds(2f);
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		yield return R.Audio.PlayVoiceOver("e19t7", null, false);
		R.Trophy.AwardTrophy(21);
		CameraFilterUtils.Create<CameraFilterPack_Blur_Movie>(null);
		yield return DOTween.To(delegate(float r)
		{
			CameraFilterPack_Blur_Movie.ChangeRadius = r;
		}, 0f, 500f, 1f).WaitForCompletion();
		yield return new WaitForSeconds(3f);
		yield return DOTween.To(delegate(float r)
		{
			CameraFilterPack_Blur_Movie.ChangeRadius = r;
		}, 500f, 0f, 1f).WaitForCompletion();
		CameraFilterUtils.Remove<CameraFilterPack_Blur_Movie>(null);
		R.Ui.Toast.Show(ScriptLocalization.Story.e19s1, 2f, true);
		yield return new WaitForSeconds(4f);
		string e19s2 = ScriptLocalization.mobile.e19s2;
		R.Ui.Toast.Show(e19s2, 2f, true);
		yield return new WaitForSeconds(4f);
		yield return R.Ui.Tutorial.Show(8);
		SingletonMono<MobileInputPlayer>.Instance.Button6Visible = true;
		yield return new WaitForSeconds(4f);
		yield return R.Ui.Tutorial.Hide(new int?(8));
		UnityEngine.Object.Destroy(this.attribute.gameObject);
		yield return R.Audio.PlayVoiceOver("e19t8", null, false);
		this.battle1.SetActive(true);
		while (!this.battle1Over)
		{
			yield return null;
		}
		yield return R.Audio.PlayVoiceOver("e19t11", null, false);
		this.battle2.SetActive(true);
		R.Audio.PlayVoiceOver("e19t12", null, false);
		this.battleCount = 2;
		while (!this.battle2Over)
		{
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.Button6Visible = false;
		CameraFilterUtils.Create<CameraFilterPack_TV_VHS>(null);
		yield return new WaitForSeconds(2f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_VHS>(null);
		yield return R.Audio.PlayVoiceOver("e19t16", null, false);
		this.winSupply.gameObject.SetActive(true);
		yield return R.Audio.PlayVoiceOver("e19t17", null, false);
		yield return this.winSupply.GetComponent<SupplyBoxAction>().WaitForBreak();
		yield return R.Audio.PlayVoiceOver("e19t18", null, false);
		R.Ui.BlackScene.Alpha = 1f;
		yield return new WaitForSeconds(4f);
		this.e19elevator = true;
		this.outGate.Enter(false);
		yield break;
	}

	private IEnumerator CheaterCoroutine()
	{
		UnityEngine.Object.Destroy(this.battle1.gameObject);
		UnityEngine.Object.Destroy(this.battle2.gameObject);
		R.Audio.StopBGM(true);
		R.Audio.StopVoiceOver();
		CameraFilterUtils.Create<CameraFilterPack_TV_PlanetMars>(null);
		this.cheater_text.text = ScriptLocalization.Story.e19s3;
		this.cheater_text.color = Color.red;
		CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
		yield return R.Audio.PlayVoiceOver("e19t20", null, false);
		CameraFilterUtils.Create<CameraFilterPack_VHS_Tracking>(null);
		int start = R.GameData.Equipment.CoinNum;
		for (int i = 0; i < 120; i++)
		{
			R.GameData.Equipment.CoinNum = (int)Mathf.Lerp((float)start, 0f, (float)i / 119f);
		}
		this.cheater_text.text = ScriptLocalization.Story.e19s4;
		R.GameData.Equipment.CoinNum = 0;
		yield return new WaitForSeconds(3f);
		CameraFilterUtils.Remove<CameraFilterPack_VHS_Tracking>(null);
		yield return R.Audio.PlayVoiceOver("e19t21", null, false);
		this.cheaterEffect.SetActive(true);
		yield return new WaitForSeconds(5f);
		R.Ui.BlackScene.Alpha = 1f;
		R.Audio.StopBGM(true);
		InputSetting.Stop(false);
		yield return R.Audio.PlayVoiceOver("e19t22", null, false);
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e19s6, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		InputSetting.Resume(false);
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private void PlayBattleVoiceOne()
	{
		int num = this.rushCount;
		if (num == 1)
		{
			R.Audio.PlayVoiceOver("e19t9", null, false);
			return;
		}
		if (num != 2)
		{
			return;
		}
		R.Audio.PlayVoiceOver("e19t10", null, false);
	}

	private void PlayBattleVoiceTwo()
	{
		int num = this.rushCount;
		if (num == 4)
		{
			R.Audio.PlayVoiceOver("e19t13", null, false);
			return;
		}
		if (num == 5)
		{
			R.Audio.PlayVoiceOver("e19t14", null, false);
			return;
		}
		if (num != 6)
		{
			return;
		}
		R.Audio.PlayVoiceOver("e19t15", null, false);
	}

	private int KillAll()
	{
		int num = 0;
		for (int i = 0; i < R.Enemy.Count; i++)
		{
			EnemyBaseAction component = R.Enemy.EnemyAttributes[i].GetComponent<EnemyBaseAction>();
			component.KillSelf();
			num++;
		}
		return num;
	}

	[SerializeField]
	private SupplyAttribute attribute;

	[SerializeField]
	private SupplyAttribute winSupply;

	[SerializeField]
	private SceneGate outGate;

	[SerializeField]
	private GameObject battle1;

	[SerializeField]
	private GameObject battle2;

	[SerializeField]
	private GameObject cheaterEffect;

	[SerializeField]
	private GameObject L2Effect;

	[SerializeField]
	private TextMesh cheater_text;

	private int battleCount;

	private int rushCount;

	private bool battle1Over;

	private bool battle2Over;

	private bool cheater;
}
