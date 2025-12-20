using System;
using ExtensionMethods;
using UnityEngine;

public class PlayerChaseAbility : CharacterState
{
	public void Chase()
	{
		if (R.Player.Enhancement.Chase == 0)
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAction.SpHurtSta) || this.stateMachine.currentState.IsInArray(PlayerAction.HurtSta))
		{
			return;
		}
		this._enemy = this.GetChaseEnemy();
		if (this._enemy != null)
		{
			Vector3 position = this._enemy.position;
			this.listener.PlayFlashSound();
			position.y = Mathf.Clamp(position.y, LayerManager.YNum.GetGroundHeight(this._enemy.gameObject), float.MaxValue);
			Transform transform = R.Effect.Generate(182, null, position, default(Vector3), default(Vector3), true);
			Vector3 localScale = transform.localScale;
			localScale.x = (float)((UnityEngine.Random.Range(0, 2) != 0) ? -1 : 1);
			transform.localScale = localScale;
			EnemyBaseHurt component = this._enemy.GetComponent<EnemyBaseHurt>();
			if (R.Player.Enhancement.Chase != 3)
			{
				component.ChaseEnd();
			}
			else if (component.currentChaseTime < 2)
			{
				component.ChaseStart();
			}
			else
			{
				component.ChaseEnd();
			}
		}
	}

	private Transform GetChaseEnemy()
	{
		for (int i = 0; i < R.Enemy.Count; i++)
		{
			EnemyAttribute enemyAttribute = R.Enemy.EnemyAttributes[i];
			if (enemyAttribute.canBeChased)
			{
				return enemyAttribute.transform;
			}
		}
		return null;
	}

	private Transform _enemy;
}
