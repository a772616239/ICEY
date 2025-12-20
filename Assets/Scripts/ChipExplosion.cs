using System;
using UnityEngine;

public class ChipExplosion : BaseBehaviour
{
	private void OnEnable()
	{
		for (int i = 0; i < this.EffectCount; i++)
		{
			Transform transform = R.Effect.Generate(this.EffectId, base.transform, new Vector3(0f, 0.5f, 0f), default(Vector3), default(Vector3), true);
			transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			transform.GetComponent<EnemyChipMove>().canPlay = true;
		}
	}

	public int EffectId;

	public int EffectCount = 3;
}
