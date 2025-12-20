using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : BaseBehaviour
{
	public BoxCollider2D _boxCollider { get; private set; }

	public void Start()
	{
		if (this.Path == null)
		{
			UnityEngine.Debug.LogError("Path Cannot be null", base.gameObject);
			return;
		}
		if (base.transform.GetComponent<BoxCollider2D>() != null)
		{
			this._boxCollider = base.transform.GetComponent<BoxCollider2D>();
		}
		this._currentPoint = this.Path.GetPathEnumerator();
		this._currentPoint.MoveNext();
		if (this._currentPoint.Current == null)
		{
			return;
		}
		base.transform.position = this._currentPoint.Current.position;
	}

	public void FixedUpdate()
	{
		if (this._currentPoint == null || this._currentPoint.Current == null)
		{
			return;
		}
		Vector3 position = base.transform.position;
		if (this.Type == PathFollow.FollowType.MoveTowards)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this._currentPoint.Current.position, Time.fixedDeltaTime * this.Speed);
		}
		else if (this.Type == PathFollow.FollowType.Lerp)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this._currentPoint.Current.position, Time.fixedDeltaTime * this.Speed);
		}
		float sqrMagnitude = (base.transform.position - this._currentPoint.Current.position).sqrMagnitude;
		if (sqrMagnitude < this.MaxDistanceToGoal * this.MaxDistanceToGoal)
		{
			this._currentPoint.MoveNext();
		}
		Vector3 position2 = base.transform.position;
		this.CurrentSpeed = (position2 - position) / Time.fixedDeltaTime;
	}

	public PathFollow.FollowType Type;

	public PathDefinition Path;

	public float Speed = 1f;

	public float MaxDistanceToGoal = 0.1f;

	public Vector3 CurrentSpeed;

	private IEnumerator<Transform> _currentPoint;

	public enum FollowType
	{
		MoveTowards,
		Lerp
	}
}
