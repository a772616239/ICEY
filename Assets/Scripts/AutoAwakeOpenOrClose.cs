using System;
using UnityEngine;

public class AutoAwakeOpenOrClose : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.SetActive(this.open);
	}

	public bool open;
}
