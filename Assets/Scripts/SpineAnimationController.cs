using System;
using System.Diagnostics;
using LitJson;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animation))]
public class SpineAnimationController : BaseBehaviour
{
	public float TimeScale
	{
		get
		{
			return this._skeletonAnimation.timeScale;
		}
	}

	public string Skin
	{
		get
		{
			return this._skeletonAnimation.skeleton.skin.name;
		}
		set
		{
			this._skeletonAnimation.skeleton.SetSkin(value);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<SpineAnimationController.EffectArgs> OnAnimChange;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<SpineAnimationController.EffectArgs> OnAnimSpeedChange;

	private void Awake()
	{
		this._timeController = base.GetComponent<TimeController>();
		SkeletonAnimation skeletonAnimation;
		if ((skeletonAnimation = this._skeletonAnimation) == null)
		{
			skeletonAnimation = (this._skeletonAnimation = base.GetComponent<SkeletonAnimation>());
		}
		this._skeletonAnimation = skeletonAnimation;
		this._animation = base.GetComponent<Animation>();
		this.CurrentUnityAnim = string.Empty;
		this.CurrentSpineAnim = string.Empty;
		this._mappingData = ((!(this._animMapping != null)) ? null : JsonMapper.ToObject(this._animMapping.text));
	}

	public void Play(string animName, bool loop = false, bool forceChange = false, float animSpeed = 1f)
	{
		this._lastAnimSpeed = animSpeed;
		if (loop && this.CurrentUnityAnim == animName && MathfX.isInMiddleRange(this._lastAnimSpeed, animSpeed, 0.1f))
		{
			return;
		}
		try
		{
			this._resumeScale = animSpeed;
			if (SingletonMono<WorldTime>.Instance.IsFrozen)
			{
				animSpeed = 0f;
				if (this._timeController != null)
				{
					this._timeController.isPause = true;
				}
			}
			this._skeletonAnimation.timeScale = animSpeed;
			this._animation[animName].speed = animSpeed;
			if (this.OnAnimSpeedChange != null)
			{
				this.OnAnimSpeedChange(this, new SpineAnimationController.EffectArgs(animSpeed));
			}
			if (forceChange || this.CurrentUnityAnim != animName)
			{
				this._animation.Stop();
				this._animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
				this._animation.Play(animName, PlayMode.StopAll);
				this.CurrentUnityAnim = animName;
				animName = ((this._mappingData != null) ? this._mappingData.Get<string>(animName, animName) : animName);
				this._skeletonAnimation.state.SetAnimation(0, animName, loop);
				this._skeletonAnimation.skeleton.SetToSetupPose();
				this._skeletonAnimation.Update(0f);
				if (this.OnAnimChange != null)
				{
					this.OnAnimChange(this, new SpineAnimationController.EffectArgs(animName, loop));
				}
				this.CurrentSpineAnim = animName;
			}
			this.AnimLoop = loop;
			this.AnimForceChange = forceChange;
		}
		catch (IndexOutOfRangeException)
		{
			UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
			throw;
		}
		catch (NullReferenceException)
		{
			UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
			throw;
		}
	}

	public void AddAnim(string animName, bool loop)
	{
		if (this._animation[animName] == null)
		{
			UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
			return;
		}
		this._animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
		this._animation.PlayQueued(animName, QueueMode.CompleteOthers);
		animName = ((this._mappingData != null) ? this._mappingData.Get<string>(animName, animName) : animName);
		this._skeletonAnimation.state.AddAnimation(0, animName, loop, 0f);
		this._skeletonAnimation.Update(0.01f);
		this.CurrentUnityAnim = animName;
		this.AnimLoop = loop;
	}

	public void Pause()
	{
		this.ChangeTimeScale(0f);
	}

	public void Resume()
	{
		if (Math.Abs(this._resumeScale) < 1.401298E-45f)
		{
			this._resumeScale = 1f;
		}
		this.ChangeTimeScale(this._resumeScale);
	}

	public void Resume(float animSpeed)
	{
		if (this._animation[this.CurrentUnityAnim] != null)
		{
			this._animation[this.CurrentUnityAnim].speed = animSpeed;
		}
		this._skeletonAnimation.timeScale = animSpeed;
	}

	private void ChangeTimeScale(float animSpeed)
	{
		if (this._animation[this.CurrentUnityAnim] != null)
		{
			this._animation[this.CurrentUnityAnim].speed = animSpeed;
		}
		if (this.OnAnimSpeedChange != null)
		{
			this.OnAnimSpeedChange(this, new SpineAnimationController.EffectArgs(animSpeed));
		}
		this._skeletonAnimation.timeScale = animSpeed;
	}

	public bool IsPlaying(string animName)
	{
		return this._animation.IsPlaying(animName);
	}

	public void Play(Enum stateEnum, bool loop = false, bool forceChange = false, float animSpeed = 1f)
	{
		this.Play(stateEnum.ToString(), loop, forceChange, animSpeed);
	}

	[SerializeField]
	[FormerlySerializedAs("animMapping")]
	private TextAsset _animMapping;

	private float _lastAnimSpeed;

	private Animation _animation;

	private JsonData _mappingData;

	private float _resumeScale;

	[SerializeField]
	[FormerlySerializedAs("m_skeletonAnimation")]
	private SkeletonAnimation _skeletonAnimation;

	private TimeController _timeController;

	public bool AnimForceChange;

	public bool AnimLoop;

	public string CurrentSpineAnim;

	public string CurrentUnityAnim;

	public class EffectArgs : EventArgs
	{
		public EffectArgs(string effectName, bool _loop)
		{
			this.EffectName = effectName;
			this.Loop = _loop;
		}

		public EffectArgs(float _animSpeed)
		{
			this.AnimSpeed = _animSpeed;
		}

		public readonly string EffectName;

		public readonly bool Loop;

		public float AnimSpeed;
	}
}
