using System;

public class PlayerEffectSync : BaseBehaviour
{
	private void Awake()
	{
		this.m_skeletonAnimation = base.GetComponent<SkeletonAnimation>();
		this.animController = R.Player.Action;
		this.multiSpineAnimController = R.Player.GetComponent<MultiSpineAnimationController>();
	}

	private void OnEnable()
	{
		this.animController.OnPlayerTurnRound += this.StopFollow;
		this.multiSpineAnimController.OnAnimChange += new EventHandler<MultiSpineAnimationController.EffectArgs>(this.StopFollow);
	}

	private void Update()
	{
		this.m_skeletonAnimation.timeScale = this.multiSpineAnimController.timeScale;
	}

	private void OnDisable()
	{
		this.animController.OnPlayerTurnRound -= this.StopFollow;
		this.multiSpineAnimController.OnAnimChange -= new EventHandler<MultiSpineAnimationController.EffectArgs>(this.StopFollow);
		this.m_skeletonAnimation.Reset();
	}

	private void StopFollow(object obj, EventArgs e)
	{
		base.transform.parent = R.Effect.transform;
		if (this.isLoop)
		{
			EffectController.TerminateEffect(base.gameObject);
		}
	}

	private SkeletonAnimation m_skeletonAnimation;

	private PlayerAction animController;

	private MultiSpineAnimationController multiSpineAnimController;

	public bool isLoop;
}
