using System;
using System.Collections;
using GameWorld;
using UnityEngine;

public class EnemyQueue : MonoBehaviour
{
	private void OnEnable()
	{
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private bool EnemyDie(string eventName, object sender, EventArgs msg)
	{
		EnemyAttribute enemyAttribute = sender as EnemyAttribute;
		base.StartCoroutine(this.CheckEnemy(enemyAttribute.baseData.enemyType));
		return true;
	}

	public int GetEnemyCount()
	{
		return this.queueData.EnemyType.Count;
	}

	private bool CheckEnemyExist(EnemyType type)
	{
		foreach (EnemyType enemyType in this.queueData.EnemyType)
		{
			if (type == enemyType)
			{
				return true;
			}
		}
		return false;
	}

	private void CheckEnemyDie(object sender)
	{
		EnemyAttribute eAttr = sender as EnemyAttribute;
		base.StartCoroutine(this.GenerateEnemyDelay(eAttr));
	}

	private IEnumerator GenerateEnemyDelay(EnemyAttribute eAttr)
	{
		GameObject e = R.Enemy.GetEnemyByType(this.queueData.CheckEnemy);
		yield return new WaitForSeconds(this.queueData.DelayTime);
		if (this.queueData.CheckEnemy != EnemyType.未定)
		{
			if (e != null && !e.GetComponent<EnemyAttribute>().isDead)
			{
				this.GenerateEnemy(eAttr.baseData.enemyType);
			}
		}
		else
		{
			this.GenerateEnemy(eAttr.baseData.enemyType);
		}
		yield break;
	}

	private void GenerateEnemy()
	{
		int index = UnityEngine.Random.Range(0, this.queueData.EnemyType.Count);
		if (!this.queueData.Random)
		{
			index = 0;
		}
		Rect rect = this.queueData.enemyQueueArea.ToRect();
		Vector2 value = new Vector2(UnityEngine.Random.Range(rect.x - rect.width / 2f, rect.x + rect.width / 2f), UnityEngine.Random.Range(rect.y - rect.height / 2f, rect.y + rect.height / 2f));
		Singleton<EnemyGenerator>.Instance.GenerateEnemy(this.queueData.EnemyType[index], new Vector2?(value), true, true);
		this.queueData.EnemyType.RemoveAt(index);
	}

	private void GenerateEnemy(EnemyType type)
	{
		int index = UnityEngine.Random.Range(0, this.queueData.EnemyType.Count);
		if (this.queueData.EnemyType.Count == 1)
		{
			index = 0;
		}
		Rect rect = this.queueData.enemyQueueArea.ToRect();
		Vector2 value = new Vector2(UnityEngine.Random.Range(rect.x - rect.width / 2f, rect.x + rect.width / 2f), UnityEngine.Random.Range(rect.y - rect.height / 2f, rect.y + rect.height / 2f));
		if (this.queueData.Random && this.queueData.EnemyType.Count > 0)
		{
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(this.queueData.EnemyType[index], new Vector2?(value), true, true);
			if (this.queueData.Limited)
			{
				foreach (EnemyType enemyType in this.queueData.EnemyType)
				{
					if (this.queueData.EnemyType[index] == enemyType)
					{
						this.queueData.EnemyType.Remove(enemyType);
						break;
					}
				}
			}
		}
		else
		{
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(type, new Vector2?(value), true, true);
		}
	}

	private IEnumerator CheckEnemy(EnemyType type)
	{
		yield return new WaitForSeconds(this.queueData.DelayTime);
		if (this.queueData.CheckEnemy == EnemyType.未定)
		{
			this.CheckSuppleType(type);
		}
		else
		{
			GameObject enemyByType = R.Enemy.GetEnemyByType(this.queueData.CheckEnemy);
			if (enemyByType != null && !enemyByType.GetComponent<EnemyAttribute>().isDead)
			{
				this.CheckSuppleType(type);
			}
		}
		yield break;
	}

	private void CheckSuppleType(EnemyType type)
	{
		if (this.queueData.Random)
		{
			this.RandomSuppleEnemy();
		}
		else
		{
			this.GivenTypeSuppleEnemy(type);
		}
	}

	private void RandomSuppleEnemy()
	{
		int index = UnityEngine.Random.Range(0, this.queueData.EnemyType.Count);
		if (this.queueData.EnemyType.Count == 1)
		{
			index = 0;
		}
		Vector2 spwanPosition = this.GetSpwanPosition();
		if (this.queueData.Limited)
		{
			if (this.queueData.EnemyType.Count > 0)
			{
				Singleton<EnemyGenerator>.Instance.GenerateEnemy(this.queueData.EnemyType[index], new Vector2?(spwanPosition), true, true);
				this.queueData.EnemyType.RemoveAt(index);
			}
		}
		else if (this.queueData.EnemyType.Count > 0)
		{
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(this.queueData.EnemyType[index], new Vector2?(spwanPosition), true, true);
		}
		else
		{
			Log.Alert("怪物池为空！！！" + LevelManager.SceneName);
		}
	}

	private void GivenTypeSuppleEnemy(EnemyType type)
	{
		if (!this.CheckEnemyExist(type))
		{
			return;
		}
		Vector2 spwanPosition = this.GetSpwanPosition();
		if (this.queueData.Limited)
		{
			if (this.queueData.EnemyType.Count > 0)
			{
				Singleton<EnemyGenerator>.Instance.GenerateEnemy(type, new Vector2?(spwanPosition), true, true);
				this.RemoveEnemyFromPool(type);
			}
		}
		else
		{
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(type, new Vector2?(spwanPosition), true, true);
		}
	}

	private Vector2 GetSpwanPosition()
	{
		Rect rect = this.queueData.enemyQueueArea.ToRect();
		return new Vector2(UnityEngine.Random.Range(rect.x - rect.width / 2f, rect.x + rect.width / 2f), UnityEngine.Random.Range(rect.y - rect.height / 2f, rect.y + rect.height / 2f));
	}

	private void RemoveEnemyFromPool(EnemyType type)
	{
		foreach (EnemyType enemyType in this.queueData.EnemyType)
		{
			if (type == enemyType)
			{
				this.queueData.EnemyType.Remove(enemyType);
				break;
			}
		}
	}

	[SerializeField]
	public EnemyQueueData queueData;
}
