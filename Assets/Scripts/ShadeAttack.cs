using System;
using GameWorld;
using UnityEngine;

public class ShadeAttack : BaseBehaviour
{
	private void Start()
	{
		string animationName = this.anim[UnityEngine.Random.Range(0, 3)];
		base.GetComponent<SkeletonAnimation>().state.SetAnimation(0, animationName, false);
		R.Audio.PlayEffect(200, new Vector3?(base.transform.position));
	}

	public void Init(GameObject target)
	{
		this.atkTarget = target;
	}

	public void SendFlashAtkEvent()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(this.atkTarget, EnemyHurtAtkEventArgs.HurtTypeEnum.Flash);
		EventManager.PostEvent<GameObject, EnemyHurtAtkEventArgs>("EnemyHurtAtk", R.Player.GameObject, args);
	}

	private GameObject atkTarget;

	private string[] anim = new string[]
	{
		"Down",
		"ToLeftAir",
		"ToLeftGround"
	};
}
