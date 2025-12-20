using System;
using UnityEngine;

public class StoryRO : MonoBehaviour
{
	private void OnDestroy()
	{
		SingletonMono<UIROController>.Instance.Hide();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !this._hasShown)
		{
			SingletonMono<UIROController>.Instance.Show();
			this._hasShown = true;
		}
	}

	private bool _hasShown;
}
