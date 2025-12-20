using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;

public class PlayerExecuteAbility : CharacterState
{
	public bool CanExecute
	{
		get
		{
			return this.stateMachine.currentState.IsInArray(PlayerExecuteAbility.CanExecuteSta);
		}
	}

	public override void Update()
	{
		if (this._invincibleRecover > 0)
		{
			this._invincibleRecover--;
			if (this._invincibleRecover <= 0)
			{
				this.pab.hurt.Invincible = false;
			}
		}
	}

	public void Execute()
	{
		if (this.CanExecute)
		{
			base.StartCoroutine((!this.pAttr.isOnGround) ? this.AirMoveToExecuteEnemy() : this.GroundMoveToExecuteEnemy());
		}
	}

	private IEnumerator GroundMoveToExecuteEnemy()
	{
		Transform executeTarget = this.GetExecuteEnemy(false);
		if (executeTarget == null)
		{
			yield break;
		}
		executeTarget.GetComponent<EnemyBaseAction>().hurtBox.gameObject.SetActive(false);
		float distance = Mathf.Abs(this.pac.transform.position.x - executeTarget.position.x) - 1f;
		distance = Mathf.Clamp(distance, 0f, float.PositiveInfinity);
		float deltaLen = distance / 5f;
		for (int i = 0; i < 5; i++)
		{
			Vector3 nextPos = this.pac.transform.position + Vector3.right * (float)this.pAttr.faceDir * deltaLen;
			R.Player.TimeController.NextPosition(nextPos);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	private IEnumerator AirMoveToExecuteEnemy()
	{
		Transform executeTarget = this.GetExecuteEnemy(true);
		if (executeTarget == null)
		{
			yield break;
		}
		EnemyAttribute eAttr = executeTarget.GetComponent<EnemyAttribute>();
		if (eAttr.isOnGround && eAttr.rankType == EnemyAttribute.RankType.Normal)
		{
			eAttr.stiffTime = 1f;
			eAttr.GetComponent<EnemyBaseAction>().AnimReady();
		}
		executeTarget.GetComponent<EnemyBaseAction>().hurtBox.gameObject.SetActive(false);
		eAttr.timeController.SetSpeed(Vector2.zero);
		executeTarget.GetComponent<Rigidbody2D>().gravityScale = 0f;
		Vector3 startPos = this.pac.transform.position;
		Vector3 endPos = executeTarget.transform.position - Vector3.right * (float)this.pAttr.faceDir;
		if (this.pAttr.faceDir == 1)
		{
			endPos.x = Mathf.Clamp(endPos.x, startPos.x, float.MaxValue);
		}
		else if (this.pAttr.faceDir == -1)
		{
			endPos.x = Mathf.Clamp(endPos.x, float.MinValue, startPos.x);
		}
		for (int i = 0; i < 5; i++)
		{
			Vector3 nextPos = Vector3.Lerp(startPos, endPos, (float)i / 4f);
			R.Player.TimeController.NextPosition(nextPos);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	private Transform GetExecuteEnemy(bool inAir)
	{
		GameObject[] array = (from e in this.GetEmenies()
		orderby Mathf.Abs(e.transform.position.x - this.pac.transform.position.x)
		select e).ToArray<GameObject>();
		Transform transform = null;
		int num = 0;
		if (num < array.Length)
		{
			EnemyAttribute component = array[num].GetComponent<EnemyAttribute>();
			component.willBeExecute = true;
			R.Player.TimeController.SetSpeed(Vector2.zero);
			transform = array[num].transform;
			this.pac.TurnRound((this.pac.transform.position.x - transform.position.x <= 0f) ? 1 : -1);
			this.weapon.HandleExecute(inAir, component);
			this.listener.executeEnemyList.Clear();
			this.listener.executeEnemyList.Add(array[num]);
		}
		return transform;
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		for (int i = 0; i < R.Enemy.Count; i++)
		{
			R.Enemy.EnemyAttributes[i].GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		}
		if (args.nextState == "QTECharge1Ready")
		{
			this.listener.charge = true;
		}
		if (args.nextState.IsInArray(PlayerAction.ExecuteSta))
		{
			this.pac.hurtBox.localScale = Vector3.zero;
		}
		if (args.lastState.IsInArray(PlayerAction.ExecuteSta) && !args.nextState.IsInArray(PlayerAction.ExecuteSta))
		{
			this.listener.executeEnemyList.Clear();
			SingletonMono<CameraController>.Instance.CameraZoomFinished();
			this.pab.hurt.Invincible = true;
			this._invincibleRecover = WorldTime.SecondToFrame(0.2f);
		}
		if (args.nextState == "NewExecuteAir1_1" || args.nextState == "NewExecuteAir2_1")
		{
			this.listener.checkFallDown = false;
			this.listener.isFalling = false;
			this.listener.checkHitGround = false;
			this.listener.AirPhysic(0f);
			R.Player.TimeController.SetSpeed(Vector2.zero);
		}
	}

	private List<GameObject> GetEmenies()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < R.Enemy.EnemyAttributes.Count; i++)
		{
			Transform transform = R.Enemy.EnemyAttributes[i].transform;
			EnemyAttribute enemyAttribute = R.Enemy.EnemyAttributes[i];
			if (enemyAttribute.CurrentCanBeExecute)
			{
				list.Add(transform.gameObject);
			}
		}
		return list;
	}

	private int _invincibleRecover;

	private static readonly string[] CanExecuteSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"Atk6",
		"Atk7",
		"Atk8",
		"Atk11",
		"Atk12",
		"Atk13",
		"Atk14",
		"Atk23",
		"Atk15",
		"AtkHv1",
		"AtkHv2",
		"AtkHv3",
		"Atk16",
		"AtkHv1Push",
		"Flash1",
		"FlashDown1",
		"FlashUp1",
		"UpRising",
		"AtkUpRising",
		"HitGround",
		"HitGround2",
		"RollReady",
		"Roll",
		"RollEnd",
		"EndAtk",
		"GetUp",
		"Idle",
		"Ready",
		"Run",
		"RunSlow",
		"Charge1Ready",
		"Charging1",
		"Charge1End",
		"IdleToDefense",
		"Defense",
		"FallToDefenseAir",
		"DefenseAir",
		"AirAtk1",
		"AirAtk2",
		"AirAtk3",
		"AirAtk4",
		"AirAtk6",
		"AirAtkHv1",
		"AirAtkHv2",
		"AirAtkHv3",
		"AirAtkHv4",
		"AirAtkHv5",
		"AirAtkHv1Push",
		"Fall1",
		"Fall2",
		"Flash2",
		"FlashDown2",
		"FlashUp2",
		"Jump",
		"Jump2",
		"RollJump",
		"FlashGround"
	};
}
