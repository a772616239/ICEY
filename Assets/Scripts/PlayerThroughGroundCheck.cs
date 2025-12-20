using System;
using UnityEngine;

public class PlayerThroughGroundCheck : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		if (!this.isOpen)
		{
			return;
		}
		GameObject gameObject = R.Player.GameObject;
		if (gameObject && gameObject.transform.position.y < base.transform.position.y)
		{
			Log.Warning("目前玩家掉落到比我低的位置了");
			UnityEngine.Debug.Break();
		}
	}

	public bool isOpen = true;
}
