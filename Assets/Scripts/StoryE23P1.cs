using System;
using System.Collections;
using UnityEngine;

public class StoryE23P1 : BaseBehaviour
{
	private void Start()
	{
		bool flag = R.Player.Transform.position.x < base.transform.position.x;
		if (flag)
		{
			base.StartCoroutine(this.Sequence0());
		}
		else
		{
			this._doorCollider.SetActive(true);
		}
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Tutorial.Hide(new int?(0));
		}
	}

	public IEnumerator Sequence0()
	{
		while (R.Player.ActionController == null)
		{
			yield return null;
		}
		this._outerDoor.state.SetAnimation(0, "CloseToOpen", false);
		this._door.state.SetAnimation(0, "CloseToOpen", false);
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		R.Player.ActionController.TurnRound(1);
		R.Player.ActionController.StartMove();
		InputSetting.Stop(false);
		yield return new WaitForSeconds(1.2f);
		R.Player.ActionController.StopMove();
		yield return new WaitForSeconds(0.3f);
		this._doorCollider.SetActive(true);
		this._outerDoor.state.SetAnimation(0, "OpenToClose", false);
		this._door.state.SetAnimation(0, "OpenToClose", false);
		yield return new WaitForSeconds(1f);
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		R.Ui.Tutorial.Show(0);
		yield return new WaitForSeconds(10f);
		R.Ui.Tutorial.Hide(new int?(0));
		yield break;
	}

	[SerializeField]
	private SkeletonAnimation _door;

	[SerializeField]
	private SkeletonAnimation _outerDoor;

	[SerializeField]
	private GameObject _doorCollider;
}
