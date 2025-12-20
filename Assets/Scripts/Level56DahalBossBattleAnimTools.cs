using System;
using UnityEngine;

public class Level56DahalBossBattleAnimTools : BaseBehaviour
{
	public void SwitchBridge()
	{
		this.bridgeOld.SetActive(false);
		this.bridgeNew.SetActive(true);
	}

	public GameObject bridgeOld;

	public GameObject bridgeNew;
}
