using System;
using UnityEngine;

public class JackAimReal : BaseBehaviour
{
	public Transform Jack
	{
		private get
		{
			return this._jack;
		}
		set
		{
			this._jack = value;
			this._jackAction = value.GetComponent<JackAction>();
		}
	}

	private void Awake()
	{
		this._skeletonAnimation = base.GetComponentInChildren<SkeletonAnimation>();
		this._animation = base.GetComponent<Animation>();
	}

	private void Update()
	{
		if (this.Jack == null)
		{
			return;
		}
		if (this._jackAction.IsInWeakSta())
		{
			this._animation.Play("ShowDisappear");
		}
	}

	private void PlaySpine(string name)
	{
		this._skeletonAnimation.skeleton.SetToSetupPose();
		this._skeletonAnimation.state.SetAnimation(0, name, false);
		this._skeletonAnimation.Update(0f);
	}

	public void PlayShoot()
	{
		this.PlaySpine("ShowShoot");
		this._animation.Play("ShowShoot");
	}

	public void PlayShootEnd()
	{
		this.PlaySpine("ShowShootEnd");
		this._animation.Play("ShowShootEnd");
	}

	public void PlayDisappear()
	{
		this.PlaySpine("ShowDisappear");
		this._animation.Play("ShowDisappear");
	}

	public void PlayDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void PlayShootEffect()
	{
		Transform transform = R.Effect.Generate(202, null, base.transform.position, default(Vector3), default(Vector3), true);
		R.Audio.PlayEffect(321, new Vector3?(transform.position));
		EnemyBullet component = transform.GetComponent<EnemyBullet>();
		component.SetAtkData(this.Jack.GetComponent<JackAnimEvent>().jsonData["Atk5Ready"]);
		component.origin = this.Jack.gameObject;
	}

	private Transform _jack;

	private SkeletonAnimation _skeletonAnimation;

	private Animation _animation;

	private JackAction _jackAction;
}
