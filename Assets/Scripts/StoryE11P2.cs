using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE11P2 : BaseBehaviour
{
	private int _times
	{
		get
		{
			return RoundStorage.Get("E11_p2_times", 1);
		}
		set
		{
			RoundStorage.Set("E11_p2_times", value);
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		switch (this._times)
		{
		case 1:
			R.Audio.PlayVoiceOver("e11t2", null, false);
			R.Player.SetPosition(this._point1.position);
			break;
		case 2:
			R.Audio.PlayVoiceOver("e11t3", null, false);
			R.Player.SetPosition(this._point1.position);
			break;
		case 3:
			R.Audio.PlayVoiceOver("e11t4", null, false);
			R.Player.SetPosition(this._point1.position);
			break;
		case 4:
			R.Audio.PlayVoiceOver("e11t5", null, false);
			break;
		}
		this._times++;
	}

	[SerializeField]
	private Transform _point1;
}
