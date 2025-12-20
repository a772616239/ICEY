using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class StoryE4 : BaseBehaviour
{
	private void Awake()
	{
		this._boxCollider2D = base.GetComponent<PolygonCollider2D>();
		this._boxCollider2D.isTrigger = true;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			int jumpTimes = this._jumpTimes;
			if (jumpTimes != 1)
			{
				if (jumpTimes != 2)
				{
					if (jumpTimes == 3)
					{
						this._jumpTimes++;
						base.StartCoroutine(this.Sequence0());
					}
				}
				else
				{
					this._jumpTimes++;
					R.Audio.PlayVoiceOver("e4t2", null, false);
				}
			}
			else
			{
				this._jumpTimes++;
				R.Audio.PlayVoiceOver("e4t1", null, false);
			}
		}
	}

	private IEnumerator Sequence0()
	{
		R.Audio.PlayVoiceOver("e4t3", null, false);
		CameraFilterUtils.Create<CameraFilterPack_FX_Drunk>(null);
		R.Trophy.AwardTrophy(4);
		yield return new WaitForSeconds(10f);
		CameraFilterUtils.Remove<CameraFilterPack_FX_Drunk>(null);
		yield break;
	}

	private void OnDestroy()
	{
		CameraFilterUtils.Remove<CameraFilterPack_FX_Drunk>(null);
	}

	private PolygonCollider2D _boxCollider2D;

	private int _jumpTimes = 1;
}
