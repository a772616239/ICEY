using System;
using CIS;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		string tag = other.gameObject.tag;
		if (tag == "Player")
		{
			SingletonMonoBehaviourClass<GameLogicMgr>.instance.GameEnd();
		}
	}
}
