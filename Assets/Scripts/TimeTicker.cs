using System;
using CIS;
using UnityEngine;

public class TimeTicker : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		string tag = other.gameObject.tag;
		if (tag == "Player")
		{
			if (this.tickerType == TimeTicker.TickerType.START)
			{
				SingletonMonoBehaviourClass<GameLogicMgr>.instance.StartPlay();
				UnityEngine.Debug.Log("Start to tick!");
			}
			else if (this.tickerType == TimeTicker.TickerType.END)
			{
				UnityEngine.Debug.Log("End tick!");
				SingletonMonoBehaviourClass<GameLogicMgr>.instance.EndPlay();
			}
		}
	}

	public TimeTicker.TickerType tickerType;

	public enum TickerType
	{
		START,
		END
	}
}
