using System;
using GameWorld;
using UnityEngine;

public class Cup : BaseBehaviour
{
	private void OnEnable()
	{
		this.InitSprite();
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.Hurt), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.Hurt), EventManager.ListenerQueue.Game);
	}

	private void InitSprite()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		Cup.CupType cupType = this.type;
		if (cupType != Cup.CupType.Copper)
		{
			if (cupType != Cup.CupType.Silver)
			{
				if (cupType == Cup.CupType.Gold)
				{
					component.sprite = this.goldCup;
				}
			}
			else
			{
				component.sprite = this.silverCup;
			}
		}
		else
		{
			component.sprite = this.copperCup;
		}
	}

	private bool Hurt(string eventDefine, object sender, EnemyHurtAtkEventArgs msg)
	{
		if (msg.hurted != base.gameObject)
		{
			return false;
		}
		EnemyHurtAtkEventArgs.HurtTypeEnum hurtType = msg.hurtType;
		if (hurtType == EnemyHurtAtkEventArgs.HurtTypeEnum.Normal)
		{
			R.Audio.PlayEffect(401, new Vector3?(base.transform.position));
			this.CupBreak();
		}
		return true;
	}

	private void CupBreak()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	[SerializeField]
	private Sprite copperCup;

	[SerializeField]
	private Sprite silverCup;

	[SerializeField]
	private Sprite goldCup;

	public Cup.CupType type;

	public enum CupType
	{
		Copper,
		Silver,
		Gold
	}
}
