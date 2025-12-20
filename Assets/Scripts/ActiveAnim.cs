using System;
using UnityEngine;

public class ActiveAnim : BaseBehaviour
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
			base.gameObject.GetComponent<Animator>().enabled = true;
			base.gameObject.GetComponent<Animator>().Play(this.anim1);
		}
		if (Mathf.Abs(this.player_x - base.transform.position.x) >= this.distance)
		{
			base.gameObject.GetComponent<Animator>().enabled = false;
		}
	}

	private float player_x;

	[SerializeField]
	public float distance;

	[SerializeField]
	public string anim1;

	[SerializeField]
	public string anim2;
}
