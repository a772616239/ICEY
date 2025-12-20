using System;
using UnityEngine;

public class ElectricScreen : BaseBehaviour
{
	private void Start()
	{
		this.player_x = R.Player.Transform.position.x;
	}

	private void Update()
	{
		this.player_x = R.Player.Transform.position.x;
		if (Mathf.Abs(this.player_x - base.transform.position.x) < this.distance)
		{
			base.gameObject.GetComponent<Animator>().Play("ElectricScreen1");
		}
		if (Mathf.Abs(this.player_x - base.transform.position.x) >= this.distance)
		{
			base.gameObject.GetComponent<Animator>().Play("ElectricScreen1_Close");
		}
	}

	private float player_x;

	[SerializeField]
	public float distance;
}
