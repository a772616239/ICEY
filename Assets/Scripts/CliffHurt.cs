using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CliffHurt : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			base.StartCoroutine(this.PlayerReset());
		}
	}

	private IEnumerator PlayerReset()
	{
		yield return new WaitForSeconds(this.delayTime);
		PlayerAttribute pAttr = R.Player.Attribute;
		CliffHurt.DamgeType damgeType = this.damageType;
		if (damgeType != CliffHurt.DamgeType.Constant)
		{
			if (damgeType == CliffHurt.DamgeType.Percent)
			{
				pAttr.currentHP -= (int)((float)(pAttr.maxHP * this.percent) / 100f);
			}
		}
		else
		{
			pAttr.currentHP -= this.damage;
		}
		if (!pAttr.isDead)
		{
			R.Player.GetComponent<Rigidbody2D>().gravityScale = 0f;
			R.Player.Action.ChangeState(PlayerAction.StateEnum.UnderAtk1, 1f);
			yield return new WaitForSeconds(0.2f);
			R.Player.Transform.position = Cliff.RebornPoint.position + Vector3.up * 3f;
		}
		yield break;
	}

	[SerializeField]
	private CliffHurt.DamgeType damageType;

	[SerializeField]
	private float delayTime;

	[SerializeField]
	private int damage;

	[SerializeField]
	private int percent;

	public enum DamgeType
	{
		Constant,
		Percent
	}
}
