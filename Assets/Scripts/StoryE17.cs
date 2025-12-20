using System;
using System.Collections;
using UnityEngine;

public class StoryE17 : BaseBehaviour
{
	private bool E17P1
	{
		get
		{
			return RoundStorage.Get("E17P1", true);
		}
		set
		{
			RoundStorage.Set("E17P1", value);
		}
	}

	private bool E17T6
	{
		get
		{
			return RoundStorage.Get("E17T6", true);
		}
		set
		{
			RoundStorage.Set("E17T6", value);
		}
	}

	private int E17Process
	{
		get
		{
			return RoundStorage.Get("E17Process", 0);
		}
		set
		{
			RoundStorage.Set("E17Process", value);
		}
	}

	private int E17MoveIn
	{
		get
		{
			return RoundStorage.Get("E17_MoveIn", 0);
		}
		set
		{
			RoundStorage.Set("E17_MoveIn", value);
		}
	}

	private void Start()
	{
		this._gate.openType = SceneGate.OpenType.None;
		this.canOpenDoor = ((this.E17MoveIn != 2 && !this.E17T6) || this.E17P1);
		this.moveRight = (R.Player.Transform.position.x < base.transform.position.x);
		if (this.canOpenDoor)
		{
			this.OuterDoor.state.SetAnimation(0, "CloseToOpen", false);
			this.Door.state.SetAnimation(0, "CloseToOpen", false);
		}
		else if (this.E17MoveIn != 1)
		{
			this.DoorCollider.SetActive(true);
		}
	}

	private void Update()
	{
		if (this.moveRight && R.Player != null)
		{
			this.E17P1 = false;
			this.moveRight = false;
			base.StartCoroutine(this.SceneStart());
		}
		else if (this._isInCollision && !this._isPlayingVoiceOver && this.E17Process < 5)
		{
			this._stayTime += Time.deltaTime;
			if (this._stayTime > (float)this._waitTimes[this.E17Process])
			{
				this._stayTime = 0f;
				this.E17Process++;
				this._isPlayingVoiceOver = true;
				R.Audio.PlayVoiceOver("e17t" + this.E17Process, delegate
				{
					this._isPlayingVoiceOver = false;
					if (this.E17Process == 5)
					{
						base.StartCoroutine(this.WaitForDoor());
					}
				}, false);
			}
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			this._isInCollision = false;
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			this._isInCollision = true;
		}
	}

	public IEnumerator WaitForDoor()
	{
		R.Audio.PlayEffect(353, null);
		this._stayTime = Time.time;
		yield return new WaitForSeconds(5f);
		InputSetting.Stop(false);
		this.OuterDoor.state.SetAnimation(0, "CloseToOpen", false);
		this.Door.state.SetAnimation(0, "CloseToOpen", false);
		yield return new WaitForSeconds(4f);
		this.E17T6 = false;
		R.Audio.PlayVoiceOver("e17t6", null, false);
		this.DoorCollider.SetActive(false);
		this._gate.openType = SceneGate.OpenType.Right;
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		yield break;
	}

	public IEnumerator SceneStart()
	{
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		R.Player.ActionController.TurnRound(1);
		R.Player.ActionController.StartMove();
		InputSetting.Stop(false);
		yield return new WaitForSeconds(1.2f);
		R.Player.ActionController.StopMove();
		yield return new WaitForSeconds(0.3f);
		this.DoorCollider.SetActive(true);
		this.OuterDoor.state.SetAnimation(0, "OpenToClose", false);
		this.Door.state.SetAnimation(0, "OpenToClose", false);
		yield return new WaitForSeconds(1f);
		if (!InputSetting.IsWorking())
		{
			InputSetting.Resume(false);
		}
		if (this.E17MoveIn == 1)
		{
			this.E17MoveIn = 2;
		}
		yield break;
	}

	public SkeletonAnimation Door;

	public SkeletonAnimation OuterDoor;

	public GameObject DoorCollider;

	[SerializeField]
	private SceneGate _gate;

	private bool _isInCollision;

	private bool _isPlayingVoiceOver;

	private int[] _waitTimes = new int[]
	{
		10,
		10,
		15,
		15,
		10
	};

	private float _stayTime;

	private bool canOpenDoor;

	private bool moveRight;
}
