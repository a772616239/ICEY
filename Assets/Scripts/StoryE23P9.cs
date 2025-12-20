using System;
using System.Collections;
using GameWorld;
using UnityEngine;

public class StoryE23P9 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.P4Coroutine());
	}

	private IEnumerator P4Coroutine()
	{
		this._l.Appear();
		this._r.Appear();
		this.airBot = R.Enemy.Generate(EnemyType.妖姬, new Vector2?(this.airBotPos.position), false, true);
		this.dao = R.Enemy.Generate(EnemyType.斩轮式一型, new Vector2?(this.daoPos.position), false, true);
		this.pao = R.Enemy.Generate(EnemyType.炮击式一型, new Vector2?(this.paoPos.position), false, true);
		this.daoAttribute = this.dao.GetComponent<EnemyAttribute>();
		this.paoAttribute = this.pao.GetComponent<EnemyAttribute>();
		this.airBotAttribute = this.airBot.GetComponent<EnemyAttribute>();
		this.dao.GetComponent<DaoAction>().ChangeFace(1);
		base.StartCoroutine(this.DaoMoe());
		base.StartCoroutine(this.PaoMoe());
		base.StartCoroutine(this.AirBotLookAround());
		this.daoAttribute.currentHp = 1;
		this.paoAttribute.currentHp = 1;
		this.airBotAttribute.currentHp = 1;
		while (this.dao != null || this.pao != null || this.airBot != null)
		{
			yield return null;
		}
		R.Mode.ExitMode(Mode.AllMode.Battle);
		EventManager.PostEvent<StoryE23P9, BattleEventArgs>("Battle", this, BattleEventArgs.End);
		this._l.DisAppear();
		this._r.DisAppear();
		yield break;
	}

	private IEnumerator DaoMoe()
	{
		while (this.dao != null && !this.daoAttribute.isDead)
		{
			int random = UnityEngine.Random.Range(0, 3);
			if (random != 0)
			{
				if (random == 1)
				{
					this.dao.GetComponent<DaoAction>().Idle3();
				}
			}
			else
			{
				this.dao.GetComponent<DaoAction>().Idle2();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
		}
		yield break;
	}

	private IEnumerator PaoMoe()
	{
		while (this.pao != null && !this.paoAttribute.isDead)
		{
			int random = UnityEngine.Random.Range(0, 3);
			if (random != 0)
			{
				if (random == 1)
				{
					this.pao.GetComponent<DaoAction>().Idle3();
				}
			}
			else
			{
				this.pao.GetComponent<DaoAction>().Idle2();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
		}
		yield break;
	}

	private IEnumerator AirBotLookAround()
	{
		while (this.airBot != null && !this.airBotAttribute.isDead)
		{
			int random = UnityEngine.Random.Range(0, 2);
			if (random != 0)
			{
				if (random == 1)
				{
					this.airBot.GetComponent<AirRobotAction>().TurnRound(1);
				}
			}
			else
			{
				this.airBot.GetComponent<AirRobotAction>().TurnRound(-1);
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
		}
		yield break;
	}

	private GameObject airBot;

	private EnemyAttribute airBotAttribute;

	[SerializeField]
	private Transform airBotPos;

	private GameObject dao;

	private EnemyAttribute daoAttribute;

	[SerializeField]
	private Transform daoPos;

	private GameObject pao;

	private EnemyAttribute paoAttribute;

	[SerializeField]
	private Transform paoPos;

	[SerializeField]
	private BattleZoneGate _l;

	[SerializeField]
	private BattleZoneGate _r;
}
