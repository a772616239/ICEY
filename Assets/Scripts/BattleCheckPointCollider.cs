using System;
using GameWorld;
using UnityEngine;

public class BattleCheckPointCollider : BaseBehaviour
{
	public void OnEnable()
	{
		this.parent = base.transform.parent.GetComponent<BattleCheckPoint>();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.CompareTag("Player"))
		{
			return;
		}
		if (!this.parent.isBattleOver && !this.parent.isOpenBattleZone)
		{
			this.parent.isOpenBattleZone = true;
			this.parent.InitGameArea();
			EventManager.PostEvent<BattleCheckPoint, BattleEventArgs>("Battle", this.parent, BattleEventArgs.Begin);
		}
	}

	public BattleCheckPoint parent;
}
