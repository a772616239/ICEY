using System;
using System.Collections.Generic;
using GameWorld;
using LitJson;
using UnityEngine;

public class BattleAssessmentManager : SingletonMono<BattleAssessmentManager>
{
	private BattleAssessmentManager()
	{
	}

	private static float DeltaTime
	{
		get
		{
			return Time.time - BattleAssessmentManager._lastComboTime;
		}
	}

	private void OnEnable()
	{
		this._bloodData = JsonMapper.ToObject(this._text.text);
		EventManager.RegisterEvent<EventArgs>("PlayerHurt", new EventManager.FBEventHandler<EventArgs>(this.PlayerHurtEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnemyHurtEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<AssessmentEventArgs>("Assessment", new EventManager.FBEventHandler<AssessmentEventArgs>(this.AssessmentEventReceiver), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EventArgs>("PlayerHurt", new EventManager.FBEventHandler<EventArgs>(this.PlayerHurtEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.UnregisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnemyHurtEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.UnregisterEvent<AssessmentEventArgs>("Assessment", new EventManager.FBEventHandler<AssessmentEventArgs>(this.AssessmentEventReceiver), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		if (BattleAssessmentManager.DeltaTime >= this.AllowComboTime)
		{
			this.ClearCurrentComboNum();
		}
		else
		{
			R.Ui.HitsGrade.Hitbar = (this.AllowComboTime - BattleAssessmentManager.DeltaTime) / this.AllowComboTime;
		}
	}

	private float AllowComboTime
	{
		get
		{
			int currentComboNum = R.SceneData.assessmentData.CurrentComboNum;
			if (currentComboNum >= 30)
			{
				return 2f;
			}
			if (currentComboNum >= 20)
			{
				return 3f;
			}
			return (float)((currentComboNum < 10) ? 5 : 4);
		}
	}

	public static void Reset()
	{
		BattleAssessmentManager._lastComboTime = 0f;
	}

	private void AddCurrentComboNum()
	{
		BattleAssessmentManager._lastComboTime = Time.time;
		R.Ui.HitsGrade.BeHited();
		R.SceneData.assessmentData.CurrentComboNum++;
	}

	private void ClearCurrentComboNum()
	{
		BattleAssessmentManager._lastComboTime = this.AllowComboTime;
		if (R.SceneData.assessmentData.CurrentComboNum == 0)
		{
			return;
		}
		if (R.SceneData.BloodPalaceMode)
		{
			this.CalculateBattleScore();
		}
		R.SceneData.assessmentData.CurrentComboNum = 0;
		if (R.Ui != null)
		{
			R.Ui.HitsGrade.HideHitNumAndBar();
		}
	}

	private bool PlayerHurtEventReceiver(string eventName, object sender, EventArgs msg)
	{
		this.ClearCurrentComboNum();
		if (!R.SceneData.BloodPalaceMode)
		{
			return true;
		}
		R.SceneData.assessmentData.NotHurt = false;
		this.StartBattleWave();
		return true;
	}

	private bool EnemyHurtEventReceiver(string eventName, object sender, EnemyHurtAtkEventArgs msg)
	{
		this.AddCurrentComboNum();
		if (R.SceneData.BloodPalaceMode)
		{
			this.BloodPalaceEnemyHurt(msg);
		}
		return true;
	}

	private bool AssessmentEventReceiver(string eventName, object sender, AssessmentEventArgs msg)
	{
		if (msg.Type == AssessmentEventArgs.EventType.ContinueGame)
		{
			this.ClearCurrentComboNum();
		}
		if (R.SceneData.BloodPalaceMode)
		{
			this.BloodPalaceAssessment(msg.Type);
		}
		return true;
	}

	private bool BattleRushEventReceiver(string eventdefine, object sender, BattleRushEventArgs msg)
	{
		this._battleWave = msg.Wave;
		if (this._battleWave == 0)
		{
			return true;
		}
		R.SceneData.LevelScore.WaveScore += this._battleWave * 300;
		this.FinishBattleWave(this._battleWave - 1);
		return true;
	}

	private bool BattleEventReceiver(string eventdefine, object sender, BattleEventArgs msg)
	{
		if (msg.Status == BattleEventArgs.BattleStatus.Begin)
		{
			this._battleScore.Clear();
			this._timeScore.Clear();
			this._currentCombo.Clear();
			this._combo.Clear();
			this._battleWave = 0;
			R.SceneData.assessmentData.NotHurt = true;
			R.SceneData.LevelScore.Clear();
		}
		if (msg.Status == BattleEventArgs.BattleStatus.End)
		{
			R.SceneData.LevelScore.WaveScore += this._battleWave * 300;
			this.FinishBattleWave(this._battleWave);
			this.ClearCurrentComboNum();
			this.CalculateFinalScore();
		}
		return true;
	}

	public Coroutine SaveTotalScore()
	{
		List<BloodPalaceTotalScore> records = R.GameData.BloodPalaceRecords;
		BloodPalaceTotalScore totalScore = R.SceneData.TotalScore;
		if (!records.Contains(R.SceneData.TotalScore))
		{
			records.Add(totalScore);
		}
		records.Sort();
		if (records.Count > 10)
		{
			records.RemoveAt(10);
		}
		return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			gameData.BloodPalaceRecords.Clear();
			for (int i = 0; i < records.Count; i++)
			{
				gameData.BloodPalaceRecords.Add(records[i]);
			}
		});
	}

	public void EnterBloodPalace()
	{
		R.SceneData.BloodPalaceMode = true;
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<BattleRushEventArgs>("BattleRush", new EventManager.FBEventHandler<BattleRushEventArgs>(this.BattleRushEventReceiver), EventManager.ListenerQueue.Game);
		this._listener = R.Player.GetComponent<PlayerAnimEventListener>();
		this._listener.OnPlayerDead += this.OnPlayerDead;
		R.SceneData.TotalScore = new BloodPalaceTotalScore();
	}

	public void ExitBloodPalace()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleEventReceiver), EventManager.ListenerQueue.Game);
		EventManager.UnregisterEvent<BattleRushEventArgs>("BattleRush", new EventManager.FBEventHandler<BattleRushEventArgs>(this.BattleRushEventReceiver), EventManager.ListenerQueue.Game);
		this._listener.OnPlayerDead -= this.OnPlayerDead;
		R.SceneData.BloodPalaceMode = false;
	}

	private void BloodPalaceEnemyHurt(EnemyHurtAtkEventArgs msg)
	{
		this.StartBattleWave();
		switch (msg.hurtType)
		{
		case EnemyHurtAtkEventArgs.HurtTypeEnum.Normal:
			this.AddAirComboNum();
			this.AddToCurrentCombo(msg.attackData.atkName);
			R.SceneData.assessmentData.ComboData.AllDamagePercent += msg.attackData.damagePercent;
			break;
		case EnemyHurtAtkEventArgs.HurtTypeEnum.Execute:
		case EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt:
			R.SceneData.assessmentData.ComboData.CoreBreakNum++;
			break;
		case EnemyHurtAtkEventArgs.HurtTypeEnum.Flash:
			R.SceneData.assessmentData.FlashAttackSuccessNum++;
			R.SceneData.assessmentData.ComboData.FlashAttackSuccessNum++;
			break;
		}
	}

	private void BloodPalaceAssessment(AssessmentEventArgs.EventType type)
	{
		if (type != AssessmentEventArgs.EventType.AddFlashTime)
		{
			if (type == AssessmentEventArgs.EventType.CurrentComboFinish)
			{
				this.AddToCombo();
			}
		}
		else
		{
			R.SceneData.assessmentData.FlashAttackNum++;
		}
	}

	private void OnPlayerDead(object sender, EventArgs e)
	{
		this.FinishBattleWave(this._battleWave);
		this.ClearCurrentComboNum();
		this.CalculateFinalScore();
	}

	private void StartBattleWave()
	{
		if (!this._battleStart)
		{
			this._battleStart = true;
			this._battleStartTime = Time.time;
		}
	}

	private void FinishBattleWave(int waveNum)
	{
		this._battleStart = false;
		this._timeScore.Add(this.CalculateTimeScore(Time.time - this._battleStartTime, waveNum));
	}

	private void CalculateBattleScore()
	{
		this.AddToCombo();
		this.CalculateSameCombo();
		BattleAssessmentData.BattleComboData comboData = R.SceneData.assessmentData.ComboData;
		float item = ((float)(R.SceneData.assessmentData.CurrentComboNum - comboData.SameComboNum) + (float)comboData.AirComboNum * 0.5f + (float)(comboData.FlashAttackSuccessNum * 5) + (float)(comboData.CoreBreakNum * 2)) * (comboData.AllDamagePercent / (float)R.SceneData.assessmentData.CurrentComboNum) * 20f;
		this._battleScore.Add(item);
		R.SceneData.assessmentData.ComboClear();
	}

	private void CalculateFinalScore()
	{
		BloodPalaceLevelScore levelScore = R.SceneData.LevelScore;
		levelScore.BeautyScore = (int)this.AverageBattleScore();
		levelScore.NotHurt = R.SceneData.assessmentData.NotHurt;
		float num = ((float)levelScore.WaveScore + this.AverageBattleScore()) * this.AverageBattleTime() * this.FlashAttackScore();
		levelScore.OriginalScore = (int)num;
		R.SceneData.TotalScore.AddLevelScore(levelScore);
		this.SaveTotalScore();
		this._battleScore.Clear();
		this._timeScore.Clear();
		R.SceneData.assessmentData.BattleClear();
	}

	private void AddToCombo()
	{
		if (this._currentCombo.Count <= 0)
		{
			return;
		}
		this._combo.Add(this._currentCombo);
		this._currentCombo.Clear();
	}

	private void AddToCurrentCombo(string comboName)
	{
		if (comboName == "AirAtk7")
		{
			return;
		}
		if (this._currentCombo.Count <= 0 || this._currentCombo[this._currentCombo.Count - 1] != comboName)
		{
			this._currentCombo.Add(comboName);
		}
	}

	private void AddAirComboNum()
	{
		if (!R.Player.Attribute.isOnGround)
		{
			R.SceneData.assessmentData.ComboData.AirComboNum++;
		}
	}

	private void CalculateSameCombo()
	{
		BattleAssessmentData.BattleComboData comboData = R.SceneData.assessmentData.ComboData;
		if (this._combo.Count <= 1)
		{
			comboData.SameComboNum = 0;
			this._combo.Clear();
			return;
		}
		for (int i = 1; i < this._combo.Count; i++)
		{
			List<string> oldCombo = this._combo[i - 1];
			List<string> newCombo = this._combo[i];
			this.AddSameComboNum(oldCombo, newCombo);
		}
		this._combo.Clear();
	}

	private void AddSameComboNum(List<string> oldCombo, List<string> newCombo)
	{
		for (int i = 0; i < newCombo.Count; i++)
		{
			if (oldCombo.Contains(newCombo[i]))
			{
				R.SceneData.assessmentData.ComboData.SameComboNum++;
			}
		}
	}

	private float CalculateTimeScore(float time, int waveNum)
	{
		R.SceneData.LevelScore.Time += time;
		float num = this._bloodData[LevelManager.SceneName].Get<float>(waveNum, 0f);
		if (time <= num)
		{
			return 2.5f;
		}
		if (time <= num * 1.2f)
		{
			return 2f;
		}
		if (time <= num * 1.6f)
		{
			return 1.5f;
		}
		return (time > num * 1.8f) ? 1f : 1.2f;
	}

	private float AverageBattleTime()
	{
		float num = 0f;
		for (int i = 0; i < this._timeScore.Count; i++)
		{
			num += this._timeScore[i];
		}
		return num / (float)this._timeScore.Count;
	}

	private float AverageBattleScore()
	{
		float num = 0f;
		if (this._battleScore.Count == 0)
		{
			return 0f;
		}
		for (int i = 0; i < this._battleScore.Count; i++)
		{
			num += this._battleScore[i];
		}
		return num / (float)this._battleScore.Count;
	}

	private float FlashAttackScore()
	{
		BattleAssessmentData assessmentData = R.SceneData.assessmentData;
		BloodPalaceLevelScore levelScore = R.SceneData.LevelScore;
		if (assessmentData.FlashAttackNum <= 0)
		{
			levelScore.FlashPercent = 0f;
			return 0.7f;
		}
		float num = (float)assessmentData.FlashAttackSuccessNum * 1f / (float)assessmentData.FlashAttackNum;
		levelScore.FlashPercent = num;
		if (num >= 0.95f)
		{
			return 2f;
		}
		if (num >= 0.8f)
		{
			return 1.8f;
		}
		if (num >= 0.7f)
		{
			return 1.5f;
		}
		if (num >= 0.5f)
		{
			return 1.3f;
		}
		return (num < 0.2f) ? 0.7f : 1f;
	}

	[SerializeField]
	private TextAsset _text;

	private JsonData _bloodData;

	private static float _lastComboTime;

	private bool _battleStart;

	private float _battleStartTime;

	private int _battleWave;

	private readonly List<float> _battleScore = new List<float>();

	private readonly List<float> _timeScore = new List<float>();

	private readonly List<string> _currentCombo = new List<string>();

	private readonly List<List<string>> _combo = new List<List<string>>();

	private PlayerAnimEventListener _listener;
}
