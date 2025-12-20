using System;
using CIS;
using UnityEngine;

public class LevelCheckPoint : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			SingletonMonoBehaviourClass<GameLogicMgr>.instance.playerController.SetRespawnPoint(base.transform.position);
			if (this.levelBlock != null && !this.levelBlock.IsEntered())
			{
				SingletonMonoBehaviourClass<GameLogicMgr>.instance.currGameSection.SetCurrentBlock(this.levelBlock);
			}
		}
	}

	public int index;

	public LevelBlock levelBlock;
}
