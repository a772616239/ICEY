using System;
using System.Collections;
using Core;
using GameWorld;
using UnityEngine;

public class Level56DahalDieTrigger : BaseBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Start()
	{
		this.dahalDieFlag = false;
		this.dahalShotFlag = false;
		this.startCheckDahalShot = false;
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.DieEvent), EventManager.ListenerQueue.Game);
	}

	public void OnDestroy()
	{
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.DieEvent), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		if (!R.Player.Attribute.isDead && R.Player.Action.IsInNormalState() && this.player.position.x > 40f && this.player.position.x < 53f && !this.dahalShotFlag && this.startCheckDahalShot)
		{
			this.dahalShotFlag = true;
			this.DahalShot();
		}
	}

	private bool DieEvent(string eventName, object sender, EventArgs msg)
	{
		if (!this.dahalDieFlag)
		{
			this.dahalDieFlag = true;
			DahalAction component = R.Enemy.Boss.GetComponent<DahalAction>();
			component.DahalDie = true;
			EventManager.PostEvent<Level56DahalDieTrigger, BattleEventArgs>("Battle", this, BattleEventArgs.End);
			base.StartCoroutine(this.DieEventStart(component));
		}
		return true;
	}

	private IEnumerator DieEventStart(DahalAction dAction)
	{
		while (dAction.stateMachine.currentState == "QTEDie")
		{
			yield return null;
		}
		this.InputSet(false);
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		yield return new WaitForSeconds(0.5f);
		if (this.dahalDieEvent)
		{
			this.dahalDieEvent.SetActive(true);
		}
		yield return new WaitForSeconds(2f);
		this.startCheckDahalShot = true;
		yield break;
	}

	private void DahalShot()
	{
		if (this.dahalShotEvent)
		{
			this.dahalShotEvent.SetActive(true);
			this.InputSet(true);
		}
	}

	private void InputSet(bool open)
	{
		Core.Input.Game.Jump.IsOpen = open;
		Core.Input.Game.Flash.Down.IsOpen = open;
		Core.Input.Game.Flash.FaceDirection.IsOpen = open;
		Core.Input.Game.Flash.Left.IsOpen = open;
		Core.Input.Game.Flash.LeftDown.IsOpen = open;
		Core.Input.Game.Flash.LeftUp.IsOpen = open;
		Core.Input.Game.Flash.Right.IsOpen = open;
		Core.Input.Game.Flash.RightDown.IsOpen = open;
		Core.Input.Game.Flash.RightUp.IsOpen = open;
		Core.Input.Game.Flash.Up.IsOpen = open;
	}

	[Header("死亡飞走事件")]
	public GameObject dahalDieEvent;

	[Header("炸桥事件")]
	public GameObject dahalShotEvent;

	private bool dahalDieFlag;

	private bool dahalShotFlag;

	private bool startCheckDahalShot;
}
