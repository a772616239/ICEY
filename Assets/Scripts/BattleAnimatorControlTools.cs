using System;
using GameWorld;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BattleAnimatorControlTools : BaseBehaviour
{
	private void Awake()
	{
		this._animator = base.GetComponent<Animator>();
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private bool OnBattle(string eventName, object sender, BattleEventArgs battleEventArgs)
	{
		BattleEventArgs.BattleStatus status = battleEventArgs.Status;
		if (status != BattleEventArgs.BattleStatus.Begin)
		{
			if (status != BattleEventArgs.BattleStatus.End)
			{
				throw new ArgumentOutOfRangeException();
			}
			this._animator.SetTrigger(this._battleEndTriggerName);
			this._animator.SetBool(this._isInBattle, false);
		}
		else
		{
			this._animator.SetTrigger(this._battleBeginTriggerName);
			this._animator.SetBool(this._isInBattle, true);
		}
		return true;
	}

	[SerializeField]
	private string _battleBeginTriggerName = "BattleBegin";

	[SerializeField]
	private string _battleEndTriggerName = "BattleEnd";

	[SerializeField]
	private string _isInBattle = "IsInBattle";

	private Animator _animator;
}
