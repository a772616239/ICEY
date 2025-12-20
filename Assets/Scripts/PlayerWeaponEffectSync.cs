using System;
using LitJson;
using UnityEngine;

public class PlayerWeaponEffectSync : BaseBehaviour
{
	private void Awake()
	{
		this.animData = JsonMapper.ToObject(this.animMapping.text);
	}

	private void OnEnable()
	{
		this.animController.OnAnimChange += this.PlayEffect;
	}

	private void OnDisable()
	{
		if (this.animController)
		{
			this.animController.OnAnimChange -= this.PlayEffect;
		}
	}

	private void OnDestroy()
	{
		if (this.animController)
		{
			this.animController.OnAnimChange -= this.PlayEffect;
		}
	}

	private void Update()
	{
		this.m_skeletonAnimation.timeScale = this.animController.timeScale;
	}

	private void PlayEffect(object obj, MultiSpineAnimationController.EffectArgs e)
	{
		string effectName = e.effectName;
		if (this.animData.Contains(effectName))
		{
			this.m_skeletonAnimation.state.SetAnimation(0, effectName, e.loop);
		}
		else
		{
			this.m_skeletonAnimation.state.SetAnimation(0, "Null", true);
		}
		this.m_skeletonAnimation.skeleton.SetToSetupPose();
	}

	[SerializeField]
	private SkeletonAnimation m_skeletonAnimation;

	[SerializeField]
	private MultiSpineAnimationController animController;

	[SerializeField]
	private TextAsset animMapping;

	private JsonData animData;
}
