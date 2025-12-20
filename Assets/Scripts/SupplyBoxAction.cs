using System;
using System.Collections;
using ExtensionMethods;
using UnityEngine;

public class SupplyBoxAction : BaseBehaviour
{
	public bool Opened
	{
		get
		{
			return this.attribute.Opened;
		}
	}

	private void Awake()
	{
		this.attribute = base.GetComponent<SupplyAttribute>();
		this.stateMachine = base.GetComponent<StateMachine>();
		this.spineAnim = base.GetComponent<SpineAnimationController>();
	}

	private void Start()
	{
		this.stateMachine.AddStates(typeof(SupplyBoxAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.ChangeState((!this.attribute.Opened) ? SupplyBoxAction.StateEnum.Unknow : SupplyBoxAction.StateEnum.Null);
	}

	private void Update()
	{
		if (this.stateMachine.currentState == "Unknow" && Vector2.Distance(R.Player.Transform.position, base.transform.position) <= this.attribute.ShowDistance)
		{
			this.UnknowToKnow();
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		SupplyBoxAction.StateEnum stateEnum = EnumTools.ToEnum<SupplyBoxAction.StateEnum>(args.state, false);
		switch (stateEnum)
		{
		case SupplyBoxAction.StateEnum.Null:
		case SupplyBoxAction.StateEnum.EnLv0ToLv1:
		case SupplyBoxAction.StateEnum.EnLv1ToLv2:
		case SupplyBoxAction.StateEnum.EnLv2ToLv3:
		case SupplyBoxAction.StateEnum.EnLv3ToLv4:
		case SupplyBoxAction.StateEnum.EnLv4ToNull:
		case SupplyBoxAction.StateEnum.HPLv0ToLv1:
		case SupplyBoxAction.StateEnum.HPLv1ToLv2:
		case SupplyBoxAction.StateEnum.HPLv2ToLv3:
		case SupplyBoxAction.StateEnum.HPLv3ToLv4:
		case SupplyBoxAction.StateEnum.HPLv4ToNull:
		case SupplyBoxAction.StateEnum.KeyLv0ToLv1:
		case SupplyBoxAction.StateEnum.KeyLv1ToLv2:
		case SupplyBoxAction.StateEnum.KeyLv2ToLv3:
		case SupplyBoxAction.StateEnum.KeyLv3ToLv4:
		case SupplyBoxAction.StateEnum.KeyLv4ToNull:
		case SupplyBoxAction.StateEnum.MoneyHit:
		case SupplyBoxAction.StateEnum.MoneyNull:
		case SupplyBoxAction.StateEnum.UnknowToEnergy:
		case SupplyBoxAction.StateEnum.UnknowToHP:
		case SupplyBoxAction.StateEnum.UnknowToKey:
		case SupplyBoxAction.StateEnum.UnknowToMoney:
		case SupplyBoxAction.StateEnum.UnknowToWPower:
		case SupplyBoxAction.StateEnum.WPLv0ToLv1:
		case SupplyBoxAction.StateEnum.WPLv1ToLv2:
		case SupplyBoxAction.StateEnum.WPLv2ToLv3:
		case SupplyBoxAction.StateEnum.WPLv3ToLv4:
		case SupplyBoxAction.StateEnum.WPLv4ToNull:
			this.spineAnim.Play(stateEnum, false, true, 1f);
			break;
		case SupplyBoxAction.StateEnum.Unknow:
			this.spineAnim.Play(stateEnum, true, false, 1f);
			break;
		}
	}

	public void ChangeState(SupplyBoxAction.StateEnum nextSta)
	{
		if (EnumTools.IsInEnum<SupplyBoxAction.DieSta>(this.stateMachine.currentState, false))
		{
			return;
		}
		this.stateMachine.SetState(nextSta);
	}

	private void UnknowToKnow()
	{
		SupplyAttribute.SupplyBoxType supplyBox = this.attribute.SupplyBox;
		if (supplyBox != SupplyAttribute.SupplyBoxType.Flash)
		{
			if (supplyBox != SupplyAttribute.SupplyBoxType.Money)
			{
				if (supplyBox == SupplyAttribute.SupplyBoxType.Cheat)
				{
					this.ChangeState(SupplyBoxAction.StateEnum.UnknowToWPower);
				}
			}
			else
			{
				this.ChangeState(SupplyBoxAction.StateEnum.UnknowToMoney);
			}
		}
		else
		{
			this.ChangeState(SupplyBoxAction.StateEnum.UnknowToEnergy);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 from = base.transform.position + Vector3.up;
		Vector3 vector = base.transform.position.AddY(-Physics2D.Raycast(base.transform.position, Vector2.down, 10f).distance);
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(from, vector);
		Gizmos.DrawLine(vector.AddX(-1f), vector.AddX(1f));
	}

	public Coroutine WaitForBreak()
	{
		return base.StartCoroutine(this.WaitForBreakCoroutine());
	}

	private IEnumerator WaitForBreakCoroutine()
	{
		while (!this.attribute.Opened)
		{
			yield return null;
		}
		yield break;
	}

	private SupplyAttribute attribute;

	private SpineAnimationController spineAnim;

	private StateMachine stateMachine;

	private enum DieSta
	{
		Null,
		EnLv4ToNull,
		HPLv4ToNull,
		KeyLv4ToNull,
		MoneyNull,
		WPLv4ToNull
	}

	public enum StateEnum
	{
		Null,
		EnLv0ToLv1,
		EnLv1ToLv2,
		EnLv2ToLv3,
		EnLv3ToLv4,
		EnLv4ToNull,
		HPLv0ToLv1,
		HPLv1ToLv2,
		HPLv2ToLv3,
		HPLv3ToLv4,
		HPLv4ToNull,
		KeyLv0ToLv1,
		KeyLv1ToLv2,
		KeyLv2ToLv3,
		KeyLv3ToLv4,
		KeyLv4ToNull,
		MoneyHit,
		MoneyNull,
		Unknow,
		UnknowToEnergy,
		UnknowToHP,
		UnknowToKey,
		UnknowToMoney,
		UnknowToWPower,
		WPLv0ToLv1,
		WPLv1ToLv2,
		WPLv2ToLv3,
		WPLv3ToLv4,
		WPLv4ToNull
	}
}
