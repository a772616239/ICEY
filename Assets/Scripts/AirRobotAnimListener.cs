using System;
using UnityEngine;

public class AirRobotAnimListener : BaseBehaviour
{
	private void Start()
	{
		this._action = base.GetComponent<AirRobotAction>();
	}

	public void GetUp()
	{
		this._action.AnimChangeState(AirRobotAction.StateEnum.Die, 1f);
	}

	public void ChangeState(AirRobotAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void RandomPlaySound(int index)
	{
		if (UnityEngine.Random.Range(0, 100) < 50)
		{
			return;
		}
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void PlayHitGroundSound()
	{
		R.Audio.PlayEffect(this.hitGroundSound[UnityEngine.Random.Range(0, this.hitGroundSound.Length)], new Vector3?(base.transform.position));
	}

	public void DestroySelf()
	{
		base.Invoke("RealDestroy", 2f);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void DieBlock()
	{
		R.Effect.Generate(92, null, base.transform.position + Vector3.up * 0.5f, Vector3.zero, default(Vector3), true);
	}

	private AirRobotAction _action;

	[SerializeField]
	private int[] hitGroundSound;
}
