using System;
using System.Collections;
using System.Collections.Generic;
using GameWorld;
using UnityEngine;

public class BattleCheckPoint : BaseBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.fileId.Count; i++)
		{
			string sceneName = LevelManager.SceneName;
			string path = string.Format("Level/{0}", sceneName);
			string name = string.Format("{0}.{1}", sceneName, this.fileId[i]);
			LevelEnemyData value = Asset.DeserializeFromFile<LevelEnemyData>(path, name);
			this._levelEnemyData.Add(this.fileId[i], value);
		}
	}

	private void OnEnable()
	{
		this.queue = base.GetComponent<EnemyQueue>();
	}

	private void Start()
	{
		if (!this.PointData.canMultiTrigger && R.GameData.BattleZoneDict.ContainsKey(base.gameObject.name))
		{
			this.isBattleOver = R.GameData.BattleZoneDict[base.gameObject.name];
		}
	}

	private void Update()
	{
		if (!this.isBattleOver)
		{
			if (this.isOpenBattleZone && this.noEnemyTime > 0.5f && this.fileId.Count > this.currentId)
			{
				this.noEnemyTime = 0f;
				base.StartCoroutine(this.GenerateEnemyRushOneAfterOne(this.fileId[this.currentId], this.PointData.HasWall));
				this.currentId++;
			}
			if (this.isOpenBattleZone && this.noEnemyTime > 1f && this.PointData.HasWall)
			{
				this.isBattleOver = true;
				this.LeftGate.GetComponent<BattleZoneGate>().DisAppear();
				this.RightGate.GetComponent<BattleZoneGate>().DisAppear();
				base.StartCoroutine(this.OpenArea());
			}
			else if (!this.PointData.HasWall && ((this.isOpenBattleZone && this.noEnemyTime > 0.3f) || R.Player.IsNearSceneGate()))
			{
				base.StartCoroutine(this.OpenArea());
			}
			if (this.useNoEnemyTime)
			{
				if (this.PointData.HasWall)
				{
					this.rect.xMin = this.LeftGate.position.x - 5f;
					this.rect.xMax = this.RightGate.position.x + 5f;
					this.rect.yMin = Mathf.Min(this.LeftGate.position.y, this.RightGate.position.y) - 5f;
					this.rect.height = 50f;
				}
				else
				{
					this.rect.x = GameArea.MapRange.x;
					this.rect.y = GameArea.MapRange.y;
					this.rect.width = GameArea.MapRange.width;
					this.rect.height = GameArea.MapRange.height;
				}
				if (!R.Enemy.HasEnemyInRect(this.rect) && (this.queue.GetEnemyCount() <= 0 || this.queue.queueData.CheckEnemy != EnemyType.未定))
				{
					this.noEnemyTime += Time.deltaTime;
				}
				else
				{
					this.noEnemyTime = 0f;
				}
			}
		}
	}

	private IEnumerator OpenArea()
	{
		this.isOpenBattleZone = false;
		this.ExitBattleMode();
		if (!R.GameData.BattleZoneDict.ContainsKey(base.gameObject.name))
		{
			R.GameData.BattleZoneDict.Add(base.gameObject.name, true);
		}
		yield return new WaitForSeconds(0.5f);
		GameArea.CameraRange.xMin = GameArea.MapRange.min.x;
		GameArea.CameraRange.xMax = GameArea.MapRange.max.x;
		GameArea.PlayerRange.xMin = GameArea.MapRange.min.x - 1f;
		GameArea.PlayerRange.xMax = GameArea.MapRange.max.x + 1f;
		EventManager.PostEvent<BattleCheckPoint, BattleEventArgs>("Battle", this, BattleEventArgs.End);
		yield break;
	}

	private void ExitBattleMode()
	{
		if (R.Mode.IsInBattleMode())
		{
			R.Mode.ExitMode(Mode.AllMode.Battle);
		}
	}

	public void InitGameArea()
	{
		if (this.PointData.HasWall)
		{
			this.LeftGate.GetComponent<BattleZoneGate>().Appear();
			this.RightGate.GetComponent<BattleZoneGate>().Appear();
			GameArea.CameraRange.xMin = this.LeftGate.position.x;
			GameArea.CameraRange.xMax = this.RightGate.position.x;
			GameArea.EnemyRange.xMin = this.LeftGate.position.x;
			GameArea.EnemyRange.xMax = this.RightGate.position.x;
			GameArea.PlayerRange.xMin = this.LeftGate.Find("PlayerLimitPosLX").transform.position.x;
			GameArea.PlayerRange.xMax = this.RightGate.Find("PlayerLimitPosRX").transform.position.x;
		}
		else
		{
			GameArea.EnemyRange.xMin = GameArea.MapRange.min.x + 15f;
			GameArea.EnemyRange.xMax = GameArea.MapRange.max.x - 15f;
		}
	}

	private IEnumerator GenerateEnemyRushOneAfterOne(int num, bool canShake)
	{
		if (this._levelEnemyData.ContainsKey(num))
		{
			EventManager.PostEvent<BattleCheckPoint, BattleRushEventArgs>("BattleRush", this, new BattleRushEventArgs(num, this.currentId));
			LevelEnemyData levelEnemyData = this._levelEnemyData[num];
			foreach (EnemyJsonObject enemyJsonObject in levelEnemyData.enemyJsonObject)
			{
				GameObject e = R.Enemy.Generate(enemyJsonObject.name, new Vector2?(enemyJsonObject.position), this.PointData.HasWall, true);
				e.GetComponent<EnemyAttribute>().id = enemyJsonObject.guid;
				if (canShake)
				{
					SingletonMono<CameraController>.Instance.CameraShake(0.13333334f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
				}
				yield return new WaitForSeconds(0.5f);
			}
			R.SceneData.CanAIRun = true;
			this.useNoEnemyTime = true;
			this.queue.queueData = levelEnemyData.enemyQueueData;
		}
		else
		{
			Log.Warning(string.Format("关卡{0}第{1}波敌人尚未设置", LevelManager.SceneName, num));
		}
		yield break;
	}

	[SerializeField]
	public CheckPointData PointData;

	public List<int> fileId = new List<int>();

	public Transform LeftGate;

	public Transform RightGate;

	public bool isOpenBattleZone;

	public bool isBattleOver;

	private readonly Dictionary<int, LevelEnemyData> _levelEnemyData = new Dictionary<int, LevelEnemyData>();

	private float noEnemyTime = 0.6f;

	private bool useNoEnemyTime;

	private int currentId;

	private Rect rect;

	private EnemyQueue queue;
}
