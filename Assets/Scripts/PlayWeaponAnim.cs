using System;
using UnityEngine;

public class PlayWeaponAnim : BaseBehaviour
{
	public void OnEnable()
	{
		this.animController = R.Player.Action;
		this.multiSpineAnimController = R.Player.GetComponent<MultiSpineAnimationController>();
		this.animController.OnPlayerTurnRound += this.stopFollow;
		this.multiSpineAnimController.OnAnimChange += new EventHandler<MultiSpineAnimationController.EffectArgs>(this.stopFollow);
	}

	public void stopFollow(object obj, EventArgs e)
	{
		if (this.collider != null && !this.hitGround)
		{
			this.collider.enabled = false;
		}
		base.transform.parent = null;
	}

	private void OnDestroy()
	{
		this.animController.OnPlayerTurnRound -= this.stopFollow;
		this.multiSpineAnimController.OnAnimChange -= new EventHandler<MultiSpineAnimationController.EffectArgs>(this.stopFollow);
	}

	[SerializeField]
	private PlayerAction animController;

	[SerializeField]
	private MultiSpineAnimationController multiSpineAnimController;

	[SerializeField]
	private BoxCollider2D collider;

	[SerializeField]
	private bool hitGround;
}
