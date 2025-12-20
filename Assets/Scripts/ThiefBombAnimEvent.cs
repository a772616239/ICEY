using System;
using LitJson;
using UnityEngine;

public class ThiefBombAnimEvent : BaseBehaviour
{
	private void Awake()
	{
		this._action = base.GetComponent<ThiefBombAction>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.背弹者];
	}

	public void ChangeState(ThiefBombAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void DestroySelf()
	{
		base.Invoke("RealDestroy", 2f);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void PlayBombEffect()
	{
		Transform transform = R.Effect.Generate(162, base.transform, Vector2.up * 1.2f, default(Vector3), default(Vector3), true);
		EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
		componentInChildren.SetAtkData(this._jsonData[this._action.stateMachine.currentState]);
		componentInChildren.damage = base.GetComponentInChildren<EnemyAttribute>().atk;
		componentInChildren.origin = null;
	}

	private ThiefBombAction _action;

	private JsonData _jsonData;

	private EnemyAtk _enemyAtk;
}
