using System;
using DG.Tweening;
using GameWorld;
using UnityEngine;

public class MusicPlayerHurt : BaseBehaviour
{
	private MusicPlayerAction action
	{
		get
		{
			return base.GetComponent<MusicPlayerAction>();
		}
	}

	private void Start()
	{
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
		if (this.action.currentHP == 0 && !this.broken)
		{
			this.broken = true;
			this.MusicPlayerBroken();
		}
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
		if (this.action.currentHP == 0)
		{
			return;
		}
		if (!DOTween.IsTweening(base.transform, false))
		{
			base.transform.DOShakePosition(0.2f, new Vector3(0.2f, 0.2f, 0f), 20, 90f, false, true);
		}
		this.action.currentHP--;
		R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
	}

	private void MusicPlayerBroken()
	{
		UnityEngine.Object.Destroy(this._hurtBox);
		this.thunder.SetActive(false);
		R.Audio.PlayEffect(269, new Vector3?(base.transform.position));
		R.Effect.Generate(206, base.transform, Vector2.up * 1.2f, default(Vector3), default(Vector3), true);
		this.action.StopShake();
		this.action.ChangeState(MusicPlayerAction.State.Blast);
	}

	private bool broken;

	[SerializeField]
	private GameObject thunder;

	[SerializeField]
	private GameObject _hurtBox;
}
