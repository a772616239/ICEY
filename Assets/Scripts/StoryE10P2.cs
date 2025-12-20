using System;
using System.Collections;
using UnityEngine;

public class StoryE10P2 : BaseBehaviour
{
	private void Start()
	{
		this.first = true;
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && this.first)
		{
			this.first = false;
			base.StartCoroutine(this.MainCoroutine());
		}
	}

	private IEnumerator MainCoroutine()
	{
		YieldInstruction t2 = R.Audio.PlayVoiceOver("e10t2", null, false);
		this.gate2.openType = SceneGate.OpenType.Left;
		yield return t2;
		yield break;
	}

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	private bool first;

	[SerializeField]
	private SpriteRenderer mainScreen;

	[SerializeField]
	private SpriteRenderer[] screen;
}
