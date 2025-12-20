using System;
using System.Collections.Generic;
using System.Diagnostics;
using LitJson;
using UnityEngine;

public class MultiSpineAnimationController : BaseBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<MultiSpineAnimationController.EffectArgs> OnAnimChange;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<MultiSpineAnimationController.EffectArgs> OnAnimSpeedChange;

	public float timeScale
	{
		get
		{
			return this.SwitchSkeletonByWeaponType(this.currentSkeleton).timeScale;
		}
	}

	private void Awake()
	{
		this.timeController = base.GetComponent<PlayerTimeController>();
		this._skeletonMeshRenderers = new List<MeshRenderer>
		{
			this.atkSkeleton.GetComponent<MeshRenderer>(),
			this.restAtkSkeleton.GetComponent<MeshRenderer>(),
			this.normalSkeleton.GetComponent<MeshRenderer>(),
			this.hurtSkeleton.GetComponent<MeshRenderer>(),
			this.upRisingkeleton.GetComponent<MeshRenderer>(),
			this.heavyAtkkeleton.GetComponent<MeshRenderer>()
		};
		this.m_animation = base.GetComponent<Animation>();
		this.currentAnim = string.Empty;
		this.animData = JsonMapper.ToObject(this.animMapping.text);
	}

	public void Play(string animName, PlayerAction.SkeletonType skeletonType, bool loop = false, bool forceChange = false, float animSpeed = 1f)
	{
		if (this.m_animation.GetClip(animName) == null)
		{
			UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
			return;
		}
		this.pauseTimeScale = animSpeed;
		animSpeed = ((!this.timeController.isPause) ? animSpeed : 0f);
		this.SwitchSkeletonByWeaponType(skeletonType).timeScale = animSpeed;
		this.m_animation[animName].speed = animSpeed;
		if (this.OnAnimSpeedChange != null)
		{
			this.OnAnimSpeedChange(this, new MultiSpineAnimationController.EffectArgs(animSpeed));
		}
		if (forceChange)
		{
			this.m_animation.Stop();
			this.m_animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
			if (this.OnAnimChange != null)
			{
				this.OnAnimChange(this, new MultiSpineAnimationController.EffectArgs(this.animData.Get<string>(animName, animName), loop));
			}
			this.m_animation.Play(animName, PlayMode.StopAll);
			this.SwitchSkeletonByWeaponType(skeletonType).state.SetAnimation(0, this.animData.Get<string>(animName, animName), loop);
			this.SwitchSkeletonByWeaponType(skeletonType).skeleton.SetToSetupPose();
			this.SwitchSkeletonByWeaponType(skeletonType).Update(0f);
			this.currentAnim = animName;
		}
		else if (this.currentAnim != animName)
		{
			this.m_animation.Stop();
			this.m_animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
			this.m_animation.Play(animName, PlayMode.StopAll);
			if (this.OnAnimChange != null)
			{
				this.OnAnimChange(this, new MultiSpineAnimationController.EffectArgs(this.animData.Get<string>(animName, animName), loop));
			}
			this.SwitchSkeletonByWeaponType(skeletonType).state.SetAnimation(0, this.animData.Get<string>(animName, animName), loop);
			this.SwitchSkeletonByWeaponType(skeletonType).skeleton.SetToSetupPose();
			this.SwitchSkeletonByWeaponType(skeletonType).Update(0f);
			this.currentAnim = animName;
		}
		this.animLoop = loop;
		this.animForceChange = forceChange;
	}

	public void AddAnim(string animName, PlayerAction.SkeletonType weaponType, bool loop)
	{
		if (this.m_animation[animName] == null)
		{
			UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
			return;
		}
		this.m_animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
		this.m_animation.PlayQueued(animName, QueueMode.CompleteOthers);
		this.SwitchSkeletonByWeaponType(weaponType).state.AddAnimation(0, this.animData.Get<string>(animName, animName), loop, 0f);
		this.SwitchSkeletonByWeaponType(weaponType).Update(0.01f);
		this.currentAnim = animName;
		this.animLoop = loop;
	}

	public void Pause()
	{
		if (this.m_animation[this.currentAnim] != null)
		{
			this.m_animation[this.currentAnim].speed = 0f;
		}
		this.SwitchSkeletonByWeaponType(this.currentSkeleton).timeScale = 0f;
	}

	public void Resume()
	{
		if (this.pauseTimeScale == 0f)
		{
			this.pauseTimeScale = 1f;
		}
		if (this.m_animation[this.currentAnim] != null)
		{
			this.m_animation[this.currentAnim].speed = this.pauseTimeScale;
		}
		this.SwitchSkeletonByWeaponType(this.currentSkeleton).timeScale = this.pauseTimeScale;
	}

	public void Resume(float animSpeed)
	{
		if (this.m_animation[this.currentAnim] != null)
		{
			this.m_animation[this.currentAnim].speed = animSpeed;
		}
		this.SwitchSkeletonByWeaponType(this.currentSkeleton).timeScale = animSpeed;
	}

	public void ChangeTimeScale(float animSpeed)
	{
		if (this.m_animation[this.currentAnim] != null)
		{
			this.m_animation[this.currentAnim].speed = animSpeed;
		}
		if (this.OnAnimSpeedChange != null)
		{
			this.OnAnimSpeedChange(this, new MultiSpineAnimationController.EffectArgs(animSpeed));
		}
		this.SwitchSkeletonByWeaponType(this.currentSkeleton).timeScale = animSpeed;
	}

	public float GetAnimNormalizedTime()
	{
		return this.m_animation[this.currentAnim].normalizedTime;
	}

	public float GetAnimLength()
	{
		return this.m_animation[this.currentAnim].length;
	}

	public string GetCurrentAnimName()
	{
		return this.m_animation[this.currentAnim].name;
	}

	private SkeletonAnimation SwitchSkeletonByWeaponType(PlayerAction.SkeletonType skeleton)
	{
		this.currentSkeleton = skeleton;
		switch (skeleton)
		{
		case PlayerAction.SkeletonType.Normal:
			this.HideSkeleton(2);
			return this.normalSkeleton;
		case PlayerAction.SkeletonType.Attack:
			this.HideSkeleton(0);
			return this.atkSkeleton;
		case PlayerAction.SkeletonType.SpAttack:
			this.HideSkeleton(1);
			return this.restAtkSkeleton;
		case PlayerAction.SkeletonType.Hurt:
			this.HideSkeleton(3);
			return this.hurtSkeleton;
		case PlayerAction.SkeletonType.UpRising:
			this.HideSkeleton(4);
			return this.upRisingkeleton;
		case PlayerAction.SkeletonType.HeavyAttack:
			this.HideSkeleton(5);
			return this.heavyAtkkeleton;
		default:
			return null;
		}
	}

	private void HideSkeleton(int index)
	{
		for (int i = 0; i < this._skeletonMeshRenderers.Count; i++)
		{
			this._skeletonMeshRenderers[i].enabled = (i == index);
		}
	}

	private float pauseTimeScale;

	private PlayerTimeController timeController;

	private List<MeshRenderer> _skeletonMeshRenderers;

	private Animation m_animation;

	private PlayerAction.SkeletonType currentSkeleton;

	private JsonData animData;

	[SerializeField]
	private TextAsset animMapping;

	[SerializeField]
	private SkeletonAnimation atkSkeleton;

	[SerializeField]
	private SkeletonAnimation restAtkSkeleton;

	[SerializeField]
	private SkeletonAnimation normalSkeleton;

	[SerializeField]
	private SkeletonAnimation hurtSkeleton;

	[SerializeField]
	private SkeletonAnimation upRisingkeleton;

	[SerializeField]
	private SkeletonAnimation heavyAtkkeleton;

	public string currentAnim;

	private bool animLoop;

	private bool animForceChange;

	public class EffectArgs : EventArgs
	{
		public EffectArgs(string _effectName, bool _loop)
		{
			this.effectName = _effectName;
			this.loop = _loop;
		}

		public EffectArgs(float _animSpeed)
		{
			this.animSpeed = _animSpeed;
		}

		public string effectName;

		public bool loop;

		public float animSpeed;
	}
}
