using System;
using BasicTools;
using DG.Tweening;
using ExtensionMethods;
using GameWorld;
using UnityEngine;

public class SupplyBoxHurt : BaseBehaviour
{
	private void Awake()
	{
		this.action = base.GetComponent<SupplyBoxAction>();
		this.attribute = base.GetComponent<SupplyAttribute>();
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EventHurt), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EventHurt), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
	}

	private bool EventHurt(string eventName, object sender, EnemyHurtAtkEventArgs args)
	{
		if (args.hurted != base.gameObject)
		{
			return false;
		}
		EnemyHurtAtkEventArgs.HurtTypeEnum hurtType = args.hurtType;
		if (hurtType != EnemyHurtAtkEventArgs.HurtTypeEnum.Normal)
		{
			return false;
		}
		this.Hurt();
		return true;
	}

	private void Hurt()
	{
		if (this.attribute.Opened || this.attribute.CurrentHp == 0u)
		{
			return;
		}
		if (!DOTween.IsTweening(base.transform, false))
		{
			base.transform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0f), 20, 90f, false, true);
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
		}
		this.attribute.CurrentHp -= 1u;
		switch (this.attribute.SupplyBox)
		{
		case SupplyAttribute.SupplyBoxType.Money:
			this.action.ChangeState(SupplyBoxAction.StateEnum.MoneyHit);
			return;
		case SupplyAttribute.SupplyBoxType.Cheat:
			switch (this.attribute.CurrentHp / (this.attribute.MaxHp / 4u))
			{
			case 0u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.WPLv3ToLv4);
				break;
			case 1u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.WPLv2ToLv3);
				break;
			case 2u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.WPLv1ToLv2);
				break;
			case 3u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.WPLv0ToLv1);
				break;
			}
			return;
		case SupplyAttribute.SupplyBoxType.Flash:
			switch (this.attribute.CurrentHp / (this.attribute.MaxHp / 4u))
			{
			case 0u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.EnLv3ToLv4);
				break;
			case 1u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.EnLv2ToLv3);
				break;
			case 2u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.EnLv1ToLv2);
				break;
			case 3u:
				this.action.ChangeState(SupplyBoxAction.StateEnum.EnLv0ToLv1);
				break;
			}
			return;
		}
		throw new ArgumentOutOfRangeException();
	}

	private void SupplyBroken()
	{
		SupplyAttribute.SupplyBoxType supplyBox = this.attribute.SupplyBox;
		if (supplyBox != SupplyAttribute.SupplyBoxType.Cheat)
		{
			if (supplyBox != SupplyAttribute.SupplyBoxType.Flash)
			{
				if (supplyBox == SupplyAttribute.SupplyBoxType.Money)
				{
					this.action.ChangeState(SupplyBoxAction.StateEnum.MoneyNull);
					Transform transform = R.Effect.Generate(192, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
					transform.GetComponent<Coin>().GenerateCoin(1.5f, 8, this.attribute.Bonus);
					R.Audio.PlayEffect(464, new Vector3?(base.transform.position));
				}
			}
			else
			{
				this.action.ChangeState(SupplyBoxAction.StateEnum.EnLv4ToNull);
				this.SupplyBoxAnim(189);
			}
		}
		else
		{
			this.action.ChangeState(SupplyBoxAction.StateEnum.WPLv4ToNull);
			this.SupplyBoxAnim(188);
		}
	}

	private Transform SupplyBoxAnim(int effectId)
	{
		R.Audio.PlayEffect(265, new Vector3?(base.transform.position));
		Transform transform = R.Effect.Generate(effectId, null, Camera.main.transform.position.AddY(-2.4f).SetZ(-5f), Vector3.zero, Vector3.zero, false);
		transform.gameObject.AddComponent<FollowGameObject>().Init(Camera.main.transform, new Vector3(0f, -2.4f, -5f), FollowGameObject.IgnoreParam.Z);
		return transform;
	}

	public void OpenDetect()
	{
		if (!this.attribute.Opened && this.attribute.CurrentHp == 0u)
		{
			this.attribute.Opened = true;
			this.SupplyBroken();
		}
	}

	private SupplyBoxAction action;

	private SupplyAttribute attribute;
}
