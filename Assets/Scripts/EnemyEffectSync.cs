using System;
using LitJson;
using UnityEngine;

public class EnemyEffectSync : BaseBehaviour
{
	private void Awake()
	{
		this.data = JsonMapper.ToObject(this.effectConfigure.text);
	}

	private void Update()
	{
		this.m_skeletonAnimation.timeScale = this.animController.TimeScale;
	}

	private void OnEnable()
	{
		this.animController.OnAnimChange += this.SyncEffect;
	}

	private void OnDisable()
	{
		if (this.animController)
		{
			this.animController.OnAnimChange -= this.SyncEffect;
		}
	}

	private void OnDestroy()
	{
		if (this.animController)
		{
			this.animController.OnAnimChange -= this.SyncEffect;
		}
	}

	private void SyncEffect(object obj, SpineAnimationController.EffectArgs e)
	{
		string effectName = e.EffectName;
		if (this.data.Contains(effectName))
		{
			this.m_skeletonAnimation.state.SetAnimation(0, this.data[effectName].ToString(), e.Loop);
			this.m_skeletonAnimation.skeleton.SetToSetupPose();
			this.m_skeletonAnimation.Update(0f);
		}
		else
		{
			this.m_skeletonAnimation.state.SetAnimation(0, "Null", true);
			this.m_skeletonAnimation.skeleton.SetToSetupPose();
			this.m_skeletonAnimation.Update(0f);
		}
	}

	[SerializeField]
	private SkeletonAnimation m_skeletonAnimation;

	[SerializeField]
	private SpineAnimationController animController;

	[SerializeField]
	private TextAsset effectConfigure;

	private JsonData data;
}
