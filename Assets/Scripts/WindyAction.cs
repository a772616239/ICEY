using System;
using UnityEngine;

public class WindyAction : BaseBehaviour
{
	private void Awake()
	{
		this._anim = base.GetComponent<SpineAnimationController>();
		this._windyLocalPosition = base.transform.localPosition;
		this._windyLocalScale = base.transform.localScale;
	}

	private void Start()
	{
		this._anim.Play("Idle", true, false, 1f);
	}

	private void Update()
	{
		if (this._currentState == "Move" && this._currentV.magnitude < 0.3f)
		{
			this._anim.Play("Idle", true, false, 1f);
		}
		else if (this._currentState == "Idle" && this._currentV.magnitude >= 0.3f)
		{
			this._anim.Play("Move", true, false, 1f);
		}
		this.FollowPlayer();
	}

	private void FollowPlayer()
	{
		Vector3 localPosition = R.Player.Transform.localPosition;
		Vector3 a = localPosition + this.playerAnchorOffset;
		Vector3 vector = a - this._windyLocalPosition;
		if (Mathf.Abs(vector.x) > 0.5f || Mathf.Abs(vector.y) > 0.5f)
		{
			Vector3 target = a - new Vector3((float)this._faceDir * 0.5f * 1.2f, 0f, 0f);
			this._windyLocalPosition = Vector3.SmoothDamp(this._windyLocalPosition, target, ref this._currentV, 0.4f);
			base.transform.position = this._windyLocalPosition;
		}
		this.TurnRound(InputSetting.JudgeDir(this._windyLocalPosition, localPosition));
	}

	private void TurnRound(int dir)
	{
		if (this._faceDir != dir)
		{
			this._faceDir = dir;
			this._windyLocalScale.x = (float)(-(float)dir) * Mathf.Abs(this._windyLocalScale.x);
			base.transform.localScale = this._windyLocalScale;
		}
	}

	private const float SmoothTime = 0.4f;

	private const float FollowJudgeDistance = 0.5f;

	private readonly string _currentState = "Idle";

	private SpineAnimationController _anim;

	private Vector3 _currentV;

	private int _faceDir = -1;

	private Vector3 _windyLocalPosition;

	private Vector3 _windyLocalScale;

	[SerializeField]
	private Vector3 playerAnchorOffset;

	private static class State
	{
		public const string Move = "Move";

		public const string Idle = "Idle";

		public static readonly string[] StateArray = new string[]
		{
			"Move",
			"Idle"
		};
	}
}
