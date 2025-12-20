using System;
using UnityEngine;

public class TrapActiveObject : MonoBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Mathf.Abs(this.player.position.x - base.transform.position.x) < 1f)
		{
			this.target.SetActive(true);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public GameObject target;
}
